using System.Threading.Tasks;

namespace PB.ServiceBus
{
    // Please do not change
    public interface IEventPublisher
    {
        Task PublishEvent<T>(T @event);
    }
}