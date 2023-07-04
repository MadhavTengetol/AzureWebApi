using AutoMapper;
using KeyVaultWebApi.Data;
using KeyVaultWebApi.Data.Repository;
using KeyVaultWebApi.Dto;

namespace KeyVaultWebApi.Services
{
    public class BatchService : IService
    {
        private readonly BatchRepository repository;
        private readonly IMapper mapper;

        
        public BatchService(AppDbContext context,IMapper mapper) {
            this.repository = new(context);
            this.mapper = mapper;
        }

        public async Task<BatchDto> Create(BatchDto batch)
        {
           
            var result = await repository.CreateBatch(mapper.Map<Batch>(batch));

            return mapper.Map<BatchDto>(result);
        }

        public async Task<BatchResponseDto> GetBatchById(Guid id)
        {
            var result = await repository.GetBatchById(id);
            return mapper.Map<BatchResponseDto>(result);
        }


        public async Task<bool> SaveFileMetadata(Files file)
        {
            return await repository.SaveFileMetaData(file);
        }

    }
}
