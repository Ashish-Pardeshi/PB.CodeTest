using System.Configuration;
using System.Net.Http;
using System.Runtime.Serialization.Json;

namespace PB.Viewings.AdvertClients
{
    public class LegacyAdvertClient
    {
        public Advert Get(int advertId)
        {
            using (var httpClient = new HttpClient())
            {
                var response = httpClient.GetAsync(
                    ConfigurationManager.AppSettings["LegacyAdvertApi"]
                    + "/"
                    + advertId).GetAwaiter().GetResult();
                
                var stream1 = response.Content.ReadAsStreamAsync().GetAwaiter().GetResult();
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(LegacyAdvert));  
            
                 stream1.Position = 0;  
                var advert = (LegacyAdvert)ser.ReadObject(stream1);
                
                return MapLegacyAdvertToAdvert(advert);
            };

            Advert MapLegacyAdvertToAdvert(LegacyAdvert la)
            {
                var advert = new Advert();
                advert.id = la.id;
                advert.hasAccompaniedViewings = la.accompaniedViewings;
                
                if (la.status == LegacyAdvertStatus.OffMarket)
                {
                    advert.isOnMarket = false;
                }
                else
                {
                    advert.isOnMarket = true;
                }

                return advert;
            }
        }
    }
}