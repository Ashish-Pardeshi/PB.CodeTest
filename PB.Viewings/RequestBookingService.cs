using System;
using System.Linq;
using System.Threading.Tasks;
using PB.ServiceBus;
using PB.Viewings;

namespace PB.Viewings
{
    public class RequestBookingService
    {
        public async Task RequestBooking(int advertId, int customerId, DateTime viewingStartTime, Diary diary)
        {
            var slot = diary.Slots.FirstOrDefault(s => s.StartTime == viewingStartTime);

            // Publish event
            var p = new EventPublisher();
            await p.PublishEvent(new ViewingRequestedV1()
            {
                Slot = slot.StartTime,
                AdvertId = advertId,
                CustomerId = customerId,
                TimeStampUtc = DateTime.UtcNow
            });
        }
    }
}