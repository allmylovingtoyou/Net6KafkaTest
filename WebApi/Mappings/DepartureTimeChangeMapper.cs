using AutoMapper;
using KafkaServiceApi;
using WebApi.Dto;
using WebApi.Mappings.Base;

namespace WebApi.Mappings;

public class DepartureTimeChangeMapper : BaseCommandMapper<DepartureTimeChangeDto, DepartureTimeChangeCommand>
{
    public DepartureTimeChangeMapper(IMapper mapper) : base(mapper)
    {
    }
}