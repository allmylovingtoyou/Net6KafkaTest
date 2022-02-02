using FluentValidation;
using WebApi.Dto;

namespace WebApi.Validators;

public class DepartureTimeChangeDtoValidator : AbstractValidator<DepartureTimeChangeDto>
{
    public DepartureTimeChangeDtoValidator() 
    {
        RuleFor(x => x.FlightNumber).NotNull();
        RuleFor(x => x.DepartureDate).NotNull();
    }
}