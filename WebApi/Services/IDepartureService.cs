using FluentValidation.Results;
using WebApi.Dto;

namespace WebApi.Services;

public interface IDepartureService
{
    Task<ValidationResult> ChangeTimeAsync(DepartureTimeChangeDto request);
}