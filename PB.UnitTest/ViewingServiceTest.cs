using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PB.Viewings;

namespace PB.UnitTest
{
    [TestClass]
    public class ViewingServiceTest
    {
        [TestMethod]
        public async Task TestBookViewingMethodReturnsNotFound()
        {
            var advertId = 123;
            var customerId = 456;
            var viewingDate = new DateTime(2019, 1, 2, 12, 0, 0);

            var mockHandler = new Mock<IAdvertHandler>();
            mockHandler.Setup(h => h.GetAsync(advertId)).Returns(() => Task.FromResult((Advert)null));

            var viewingService = new ViewingService();
            viewingService.AdvertHandler = mockHandler.Object;
            var result = await viewingService.BookViewing(advertId, customerId, viewingDate);

            Assert.AreEqual(result, BookViewingResult.FailAdvertNotFound);
        }

        [TestMethod]
        public async Task TestBookViewingMethodReturnsIsOffMarket()
        {
            var advertId = 123;
            var customerId = 456;
            var viewingDate = new DateTime(2019, 1, 2, 12, 0, 0);
            var advert = new Advert()
            {
                id = 123,
                isOnMarket = false,
                hasAccompaniedViewings = false
            };

            var mockHandler = new Mock<IAdvertHandler>();
            mockHandler.Setup(h => h.GetAsync(advertId)).Returns(() => Task.FromResult(advert));

            var viewingService = new ViewingService();
            viewingService.AdvertHandler = mockHandler.Object;
            var result = await viewingService.BookViewing(advertId, customerId, viewingDate);

            Assert.AreEqual(result, BookViewingResult.FailAdvertIsOffMarket);
        }

        [TestMethod]
        public async Task TestBookViewingMethodWithAccompaniedReturnsViewingRequested()
        {
            var advertId = 123;
            var customerId = 456;
            var viewingDate = new DateTime(2019, 1, 2, 12, 0, 0);
            var advert = new Advert()
            {
                id = 123,
                isOnMarket = true,
                hasAccompaniedViewings = true
            };
            var agentDiary = new Diary();
            var slots = new List<Slot>();
            slots.Add(new Slot()
            {
                StartTime = viewingDate,
                IsBooked = true
            });
            agentDiary.Slots = slots;

            var mockHandler = new Mock<IAdvertHandler>();
            mockHandler.Setup(h => h.GetAsync(advertId)).Returns(() => Task.FromResult(advert));
            mockHandler.Setup(h => h.FindDiary(advert, viewingDate)).Returns(agentDiary);

            var viewingService = new ViewingService();
            viewingService.AdvertHandler = mockHandler.Object;
            var result = await viewingService.BookViewing(advertId, customerId, viewingDate);

            Assert.AreEqual(result, BookViewingResult.ViewingRequested);
        }

        [TestMethod]
        public async Task TestBookViewingMethodWithoutAccompaniedReturnsViewingRequested()
        {
            var advertId = 123;
            var customerId = 456;
            var viewingDate = new DateTime(2019, 1, 2, 12, 0, 0);
            var advert = new Advert()
            {
                id = 123,
                isOnMarket = true,
                hasAccompaniedViewings = false
            };
            var customerDiary = new Diary();
            var slots = new List<Slot>();
            slots.Add(new Slot()
            {
                StartTime = viewingDate,
                IsBooked = true
            });
            customerDiary.Slots = slots;

            var mockHandler = new Mock<IAdvertHandler>();
            mockHandler.Setup(h => h.GetAsync(advertId)).Returns(() => Task.FromResult(advert));
            mockHandler.Setup(h => h.FindDiary(advert, viewingDate)).Returns(customerDiary);

            var viewingService = new ViewingService();
            viewingService.AdvertHandler = mockHandler.Object;
            var result = await viewingService.BookViewing(advertId, customerId, viewingDate);

            Assert.AreEqual(result, BookViewingResult.ViewingRequested);
        }
    }
}
