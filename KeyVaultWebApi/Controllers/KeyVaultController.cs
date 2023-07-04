using KeyVaultWebApi.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Azure.Security.KeyVault.Secrets;
using Azure.Identity;

namespace KeyVaultWebApi.Controllers
{
    [ApiController]
    public class KeyVaultController : ControllerBase
    {
       
        private readonly IConfiguration configuration;
        private readonly KeyVaultManager keyVaultManager;

        public KeyVaultController(IConfiguration configuration)
        {
            this.configuration = configuration;
           keyVaultManager = new KeyVaultManager(new SecretClient(new Uri(configuration["AzureKeyVault:url"]),new DefaultAzureCredential()));
        }
         

        [HttpGet("get/{connectionString}")]
        public IActionResult GetConnectionString([FromRoute]string connectionString)
        {

            var result = keyVaultManager.GetSecret(connectionString);

            return Ok(new {ConnectionString = result.Result});
        }

    }
}
