using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;

namespace PB.Viewings
{
    public static class AdvertClient
    {
        public static async Task<Advert> GetById(int advertId)
        {
            var advertApi = ConfigurationManager.AppSettings["AdvertAPI"];
            var httpClient = new HttpClient();

            var response = await httpClient.GetAsync(advertApi + "/" + advertId);
            var stream1 = await response.Content.ReadAsStreamAsync();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Advert));  
            
            stream1.Position = 0;  
            var advert = (Advert)ser.ReadObject(stream1);
            
            stream1.Dispose();
            httpClient.Dispose();
            return advert;
        }
    }
}