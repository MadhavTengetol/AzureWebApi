using KeyVaultWebApi.Dto;

namespace KeyVaultWebApi.Services
{
    public interface IService
    {
        public Task<BatchDto> Create(BatchDto batch);
        public Task<BatchResponseDto> GetBatchById(Guid id);
    }
}
