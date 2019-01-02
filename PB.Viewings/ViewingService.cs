namespace PB.Viewings
{
    using System.Net.Http;
    using System;
    using System.Collections;
    using System.Configuration;
    using System.Runtime.Remoting.Metadata.W3cXsd2001;
    using System.Threading.Tasks;
    using PB.ServiceBus;
    using System.Linq;

    public class ViewingService
    {
        public IAdvertHandler AdvertHandler { get; set; }

        public async Task<BookViewingResult> BookViewing(int advertId, int customerId, DateTime ViewingStartTime)
        {
            BookViewingResult bookViewingResult = BookViewingResult.Failed;
            Advert property = null;
            Diary diary = null;

            // Get Property Advert
            property = await AdvertHandler.GetAsync(advertId);

            // Property not found
            if (property == null)
                return BookViewingResult.FailAdvertNotFound;

            // Property not available on market
            if (!property.isOnMarket)
                return BookViewingResult.FailAdvertIsOffMarket;

            // Get diary
            diary = AdvertHandler.FindDiary(property, ViewingStartTime);
     
            bool slotIsAvailable = diary.Slots.Any(s => s.StartTime == ViewingStartTime && !s.IsBooked);   
            if (slotIsAvailable)
            {
                BookSlot(advertId, customerId, ViewingStartTime, diary, property);
                bookViewingResult = BookViewingResult.Success;
            }
            else
            {
                // Request booking
                var requestBookingService = new RequestBookingService();
                await requestBookingService.RequestBooking(advertId, customerId, ViewingStartTime, diary);
                bookViewingResult = BookViewingResult.ViewingRequested;
            }

            return bookViewingResult;
        }

        private void BookSlot(int advertId, int customerId, DateTime ViewingStartTime, Diary diary, Advert advert)
        {
            if (ViewingStartTime != null && diary != null && advert != null)
            {
                Slot slot = diary.Slots.FirstOrDefault(s => s.StartTime == ViewingStartTime);

                DiaryStore.BookViewing(customerId, advertId, slot, advert.hasAccompaniedViewings);

                // Publish event
                var p = new EventPublisher();
                p.PublishEvent(new ViewingBookedV1()
                {
                    Slot = slot.StartTime,
                    AdvertId = advertId,
                    CustomerId = customerId,
                    TimeStampUtc = DateTime.UtcNow
                }).GetAwaiter().GetResult();
            }
        }
    }
}