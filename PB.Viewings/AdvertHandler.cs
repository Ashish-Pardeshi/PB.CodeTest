using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System;

namespace PB.Viewings
{
    public class AdvertHandler : IAdvertHandler
    {
        private static IAdvertHandler _handler;
        public static IAdvertHandler GetHandler()
        {
            if (_handler == null)
                _handler = new AdvertHandler();
            return _handler;
        }

        private AdvertHandler()
        {
        }

        public async Task<Advert> GetAsync(int advertId)
        {
            Advert advert = null;
            if (ConfigurationManager.AppSettings["FeatureToggle.UseNewAdvertApi"] == "true")
            {
                advert = await AdvertClient.GetById(advertId);
            }
            else
            {
                var legacyAdvertClient = new AdvertClients.LegacyAdvertClient();
                advert = legacyAdvertClient.Get(advertId);
            }

            return advert;
        }

        public Diary FindDiary(Advert property, DateTime viewingStartTime)
        {
            Diary diary = new Diary();
            if (property.hasAccompaniedViewings)
            {
                // get AVLPE diary
                diary = DiaryStore.FindAVLPEDiary(property.id, viewingStartTime.Year, viewingStartTime.Month, viewingStartTime.Day);
            }
            else
            {
                // get customer diary
                diary = DiaryStore.FindCustomerDiary(property.id, viewingStartTime.Year, viewingStartTime.Month, viewingStartTime.Day);
            }
            return diary;
        }
    }
}