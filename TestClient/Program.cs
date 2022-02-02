using Dapper;
using Newtonsoft.Json;
using Npgsql;
using RestSharp;

var random = new Random();

JsonConvert.DefaultSettings = () => new JsonSerializerSettings
{
    DateTimeZoneHandling = DateTimeZoneHandling.Utc
};

Console.WriteLine("Waiting for other services");
await Task.Delay(20000);

while (true)
{
    var client = new RestClient("http://webapi:80/Departure/ChangeTime");
    var request = new RestRequest();
    request.AddHeader("Content-Type", "application/json");
    var body = new DepartureTimeChangeDto
    {
        DepartureDate = RandomDay(),
        FlightNumber = random.Next(1, 500).ToString()
    };
    request.AddStringBody(JsonConvert.SerializeObject(body), DataFormat.Json);
    request.AddParameter("application/json", body, ParameterType.RequestBody);
    var response = await client.ExecutePutAsync(request);

    Console.WriteLine($"Get status code = {response.StatusCode} for DepartureDate = {body.DepartureDate}, FlightNumber = {body.FlightNumber}");
    await Task.Delay(2000);
    
    await using var conn = new NpgsqlConnection("Server=postgres;Port=5432;Database=test_kafka_service;User Id=postgres;Password=admin;");
    var param = new { Flight_number = body.FlightNumber, Departure_date = body.DepartureDate };
    const string querySql = @"SELECT * FROM departure_change_times where departure_date = @Departure_date and flight_number = @Flight_number;";
    var entity = await conn.QueryFirstOrDefaultAsync<DepartureChangeTime>(querySql, param);
    if (entity is not null)
    {
        Console.WriteLine($"Entity with DepartureDate = {body.DepartureDate}, FlightNumber = {body.FlightNumber} found in Db with id = {entity.Id}");
        continue;
    }
    
    Console.WriteLine($"Entity with DepartureDate = {body.DepartureDate}, FlightNumber = {body.FlightNumber} not found in Db");
}


DateTime RandomDay()
{
    var start = new DateTime(2022, 1, 1);
    var range = (DateTime.Today - start).Days;
    return start.AddDays(random.Next(range));
}

public class DepartureChangeTime
{
    public int Id { get; set; }
    public DateTime? DepartureDate { get; set; }
    public string? FlightNumber { get; set; }
}
public record DepartureTimeChangeDto
{
    public DateTime? DepartureDate { get; set; }
    public string? FlightNumber { get; set; }
}