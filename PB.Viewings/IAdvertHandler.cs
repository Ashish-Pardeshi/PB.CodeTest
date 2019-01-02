using System;
using System.Threading.Tasks;

namespace PB.Viewings
{
    public interface IAdvertHandler
    {
        Task<Advert> GetAsync(int advertId);

        Diary FindDiary(Advert property, DateTime viewingStartTime);
    }
}