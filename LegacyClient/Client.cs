using System;
using System.Threading.Tasks;
using PB.Viewings;

namespace LegacyClient
{
    // This is a legacy client.
    // This client shouldn't break.
    // Please do not make any changes to this code.
    public class Client
    {
        public async Task BookViewing()
        {
            var viewingService = new ViewingService();
            viewingService.AdvertHandler = AdvertHandler.GetHandler();
            var result = await viewingService.BookViewing(123, 456, new DateTime(2019, 1, 2, 12, 0, 0));
            
            // Do stuff
        }
    }
}