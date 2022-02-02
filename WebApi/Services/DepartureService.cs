using FluentValidation;
using FluentValidation.Results;
using KafkaServiceApi;
using WebApi.Clients;
using WebApi.Dto;
using WebApi.Mappings;
using WebApi.Validators;

namespace WebApi.Services;

public class DepartureService : IDepartureService
{
    private readonly ILogger<DepartureService> _logger;
    private readonly IKafkaClient _client;
    private readonly DepartureTimeChangeMapper _mapper;
    private readonly DepartureTimeChangeDtoValidator _validator;

    public DepartureService(
        ILogger<DepartureService> logger,
        IKafkaClient client,
        DepartureTimeChangeMapper mapper,
        DepartureTimeChangeDtoValidator validator)
    {
        _logger = logger;
        _client = client;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<ValidationResult> ChangeTimeAsync(DepartureTimeChangeDto request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return validationResult;
        }
        var command = _mapper.ToCommand(request);
        await _client.SendDepartureTimeChangeAsync(command);

        return new ValidationResult();
    }
}