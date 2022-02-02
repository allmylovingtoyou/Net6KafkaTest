namespace KafkaService.Domain;

public class DepartureChangeTime
{
    public int Id { get; set; }
    public DateTime? DepartureDate { get; set; }
    public string? FlightNumber { get; set; }
}