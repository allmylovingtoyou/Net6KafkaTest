using WebApi.Dto.Base;

namespace WebApi.Dto;

public record DepartureTimeChangeDto : IBaseDto
{
    public DateTime? DepartureDate { get; set; }
    public string? FlightNumber { get; set; }
}