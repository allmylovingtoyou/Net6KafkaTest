using AutoMapper;
using KafkaServiceApi;
using WebApi.Dto;

namespace WebApi.Mappings;

internal class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<DepartureTimeChangeDto, DepartureTimeChangeCommand>();
    }
}