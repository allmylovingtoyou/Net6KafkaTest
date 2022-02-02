using AutoMapper;
using KafkaServiceApi.Base;
using WebApi.Dto.Base;

namespace WebApi.Mappings.Base;

public abstract class BaseCommandMapper<TD, TC> where TC : IBaseKafkaCommand where TD : IBaseDto
{
    // For custom child mappings
    protected readonly IMapper Mapper;

    protected BaseCommandMapper(IMapper mapper)
    {
        Mapper = mapper;
    }

    public TC ToCommand(TD dto)
    {
        return Mapper.Map<TD, TC>(dto);
    }
}