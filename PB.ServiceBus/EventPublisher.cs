using System.Threading.Tasks;

namespace PB.ServiceBus
{
    // Please do not change
    public class EventPublisher : IEventPublisher
    {
        public async Task PublishEvent<T>(T @event)
        {
            await Task.Delay(50);
        }
    }
}