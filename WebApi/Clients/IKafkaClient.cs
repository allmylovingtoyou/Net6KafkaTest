using KafkaServiceApi;

namespace WebApi.Clients;

public interface IKafkaClient
{
    Task SendDepartureTimeChangeAsync(DepartureTimeChangeCommand command);
}