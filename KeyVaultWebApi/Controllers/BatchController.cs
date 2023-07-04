using Assignment_UKHO.Services;
using AutoMapper;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Storage.Blobs;
using FluentValidation;
using KeyVaultWebApi.Data;
using KeyVaultWebApi.Dto;
using KeyVaultWebApi.ErrorHandling;
using KeyVaultWebApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace KeyVaultWebApi.Controllers
{

    [ApiController]
    public class BatchController : ControllerBase
    {
        private readonly BatchService _service;

        private readonly IValidator<BatchDto> _validator;
        private readonly ILogger<BatchController> _logger;
        private readonly BusinessUnitServices _unitServices;
        private readonly IConfiguration _configuration;
        public BatchController(AppDbContext repo, IValidator<BatchDto> validator, IMapper mapper, ILogger<BatchController> logger, IConfiguration configuration)
        {
            this._service = new(repo, mapper);
            this._validator = validator;
            this._logger = logger;
            this._unitServices = new(repo);
            _configuration = configuration;

        }

        [HttpGet("batch/{batchId}")]
        public async Task<IActionResult> GetBatchById([FromRoute] Guid batchId)
        {
            if (batchId != Guid.Empty)
            {
                var result = await _service.GetBatchById(batchId);
                _logger.LogInformation("Get Batch By Id is called");
                if (result == null)
                {
                    var errorRes = new ErrorModel
                    {
                        CorrelationId = Guid.NewGuid().ToString(),
                        Errors = new List<Errors>
                        {
                            new Errors{Source="Batch Id",Description="Batch Id does not exists."}
                        }
                    };
                    _logger.LogInformation("Batch Id does not exists.");
                    return NotFound(errorRes);
                }
                _logger.LogInformation("Batch id found.");
                return Ok(result);
            }
            var errorResponse = new ErrorModel
            {
                CorrelationId = Guid.NewGuid().ToString(),
                Errors = new List<Errors>
                {
                    new Errors{Source="Batch Id",Description="Batch Id is not valid."}
                }
            };
            _logger.LogInformation("Batch Id is not valid.");
            return BadRequest(errorResponse);
        }


        [HttpPost]
        [Route("/batch")]
        public async Task<IActionResult> CreateBatch(BatchDto batch)
        {
            var result = _validator.Validate(batch);
            var errorResponse = new ErrorModel();
            if (!result.IsValid)
            {
                var errors = result.ToDictionary();
                List<Errors> errors1 = new();
                foreach (var error in errors)
                {
                    errors1.Add(new Errors { Source = error.Key, Description = error.Value.FirstOrDefault()! });
                }
                _logger.LogInformation("Found Errors : " + result.ToString());
                errorResponse.CorrelationId = Guid.NewGuid().ToString();
                errorResponse.Errors = errors1;
                return BadRequest(errorResponse);
            }

            if (_unitServices.IsExists(batch.BusinessUnit).Result)
            {
                var createdBatch = await _service.Create(batch);
                _logger.LogInformation("New Batch is Created");
                string connectionStringName = "blobConnectionString";
                var client = GetKeyVaultClient();
                var blobConnectionString = await client.GetSecretAsync(connectionStringName);
                var blobContainerName = createdBatch.BatchId.ToString();
                var blobServiceClient = new BlobServiceClient(blobConnectionString.Value.Value);
                blobServiceClient.CreateBlobContainer(blobContainerName);
                UploadFile(createdBatch.BatchId, createdBatch.Files);
                return Created("", new { BatchId = createdBatch.BatchId });
            }
            _logger.LogInformation("Business unit does not exists");
            errorResponse.CorrelationId = Guid.NewGuid().ToString();
            errorResponse.Errors = new List<Errors> { new Errors() { Source = "Business Unit", Description = "Business Unit Does Not Exists" } };
            return BadRequest(errorResponse);


        }


        [HttpPost("/batch/{batchId}/{filename}")]
        public async Task<IActionResult> AddFileToBatch([FromRoute] Guid batchId,
                                  [FromRoute] string filename, [FromHeader(Name = "X-MIME-Type")] string type,
                                  [FromHeader(Name = "X-Content-Size"), Required] long contentSize)
        {

            if (filename != null)
            {
                // creating metadata of file and inserting into table
                var file = new Files
                {
                    FileName = filename,
                    FileSize = contentSize,
                    MimeType = type,
                    Hash = Guid.NewGuid().ToString(),
                    Attributes = new List<Data.FileAttributes>
                    {
                        new Data.FileAttributes
                        {
                            Key="key1",
                            Value= "value1"
                        }
                    },
                    BatchId = batchId
                };

                if (await _service.SaveFileMetadata(file))
                {

                    UploadFile(batchId, new List<Files> { file });
                    return Created("File Uploaded", new { File = "File Uploaded :" + filename });
                }

                var response = new ErrorModel
                {
                    CorrelationId = Guid.NewGuid().ToString(),
                    Errors = new List<Errors>
                    {
                        new Errors
                        {
                            Source = "Batch Id",
                            Description = "Could be a bad batch Id,a batch ID doesn't exists"
                        }
                    }
                };
                return BadRequest(response);
            }

            var badRes = new ErrorModel
            {
                CorrelationId = Guid.NewGuid().ToString(),
                Errors = new List<Errors>
                    {
                        new Errors
                        {
                            Source = "Internal Server Error",
                            Description = "something went wrong"
                        }
                    }
            };

            return BadRequest(badRes);

        }



        private async void UploadFile(Guid batchId, List<Files> files)
        {
            //var blobConnectionString = _configuration["AzureConfig:blobConnectionString"];
            string connectionStringName = "blobConnectionString";
            var client = GetKeyVaultClient();
            var blobConnectionString = await client.GetSecretAsync(connectionStringName);
            var blobContainerName = batchId.ToString();
            var container = new BlobContainerClient(blobConnectionString.Value.Value, blobContainerName);

            foreach (var file in files)
            {
                var blob = container.GetBlobClient(file.FileName + "." + file.MimeType);
                var stream = System.IO.File.OpenRead(file.FileName + "." + file.MimeType);
                await blob.UploadAsync(stream);
            }
        }

        private SecretClient GetKeyVaultClient()
        {
            var keyVaultName = _configuration["AzureKeyVault:keyVaultName"];
            var url = _configuration["AzureKeyVault:url"]!;

            var client = new SecretClient(new Uri(url), new DefaultAzureCredential());
            _logger.LogInformation("Key Vault Client Created.");
            return client;
        }

    }
}
