using AutoMapper;
using KeyVaultWebApi.Data;

namespace KeyVaultWebApi.Dto
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Batch, BatchDto>().ReverseMap();
            CreateMap<Batch, BatchResponseDto>().ReverseMap();
            CreateMap<BatchDto, BatchResponseDto>().ReverseMap();
        }
    }
}
