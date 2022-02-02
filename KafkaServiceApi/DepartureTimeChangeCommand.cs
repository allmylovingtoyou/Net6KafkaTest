using System.Text.Json;
using Confluent.Kafka;
using KafkaServiceApi.Base;

namespace KafkaServiceApi;

public class DepartureTimeChangeCommand : IBaseKafkaCommand, ISerializer<DepartureTimeChangeCommand>, IDeserializer<DepartureTimeChangeCommand>
{
    public DateTime? DepartureDate { get; set; }
    public string? FlightNumber { get; set; }
    
    public byte[] Serialize(DepartureTimeChangeCommand data, SerializationContext context)
    {
        return JsonSerializer.SerializeToUtf8Bytes(data);

    }

    public DepartureTimeChangeCommand Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        return JsonSerializer.Deserialize<DepartureTimeChangeCommand>(data)!;
    }
}