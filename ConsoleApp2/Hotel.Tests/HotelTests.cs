using ConsoleApp1;
using ConsoleApp1.ConsoleInterface;
using HotelApp;

namespace Hotel.Tests
{
    [TestClass]
    public sealed class HotelTests
    {
        [TestInitialize]
        public void Initialize()
        {
            ServicesManager.Writer = new ConsoleLogger();
            ServicesManager.Reader = new ConsoleReader();
        }

        [TestMethod]
        public void Test_RoomNumber_Property()
        {
            var room = new HotelRoom();
            room.RoomNumber = 101;
            Assert.AreEqual(101, room.RoomNumber);
        }

        [TestMethod]
        public void Test_RoomType_Property()
        {
            var room = new HotelRoom();
            room.RoomType = RoomType.Deluxe;
            Assert.AreEqual(RoomType.Deluxe, room.RoomType);
        }

        [TestMethod]
        public void Test_IsAvailable_Property()
        {
            var room = new HotelRoom();
            room.ChangeAvailability();
            Assert.IsFalse(room.IsAvailable);
        }

        [TestMethod]
        public void Test_PricePerNight_Property()
        {
            var room = new HotelRoom();
            Assert.AreEqual(100.0, room.PricePerNight);
        }

        [TestMethod]
        public void Test_BedCount_Property()
        {
            var room = new HotelRoom();
            room.BedCount = 2;
            Assert.AreEqual(2, room.BedCount);
        }

        [TestMethod]
        public void Test_CountTotalCostForNights()
        {
            var room = new HotelRoom();
            double totalCost = room.CountTotalCostForNights(3);
            Assert.AreEqual(300.0, totalCost);
        }

        [TestMethod]
        public void Test_UpdatePrice()
        {
            var room = new HotelRoom();
            room.UpdatePrice(200.0);
            Assert.AreEqual(200.0, room.PricePerNight);
        }

        [TestMethod]
        public void Test_UpdatePriceWithCurrency()
        {
            var room = new HotelRoom();
            room.UpdatePrice(250.0, "USD");
            Assert.AreEqual(250.0, room.PricePerNight);
        }

        [TestMethod]
        public void Test_ChangeAvailability()
        {
            var room = new HotelRoom();
            room.ChangeAvailability();
            Assert.IsFalse(room.IsAvailable);
        }

        [TestMethod]
        public void Test_UpdateBedCount()
        {
            var room = new HotelRoom();
            room.UpdateBedCount(3);
            Assert.AreEqual(3, room.BedCount);
        }

        [TestMethod]
        public void Test_CalculateAveragePricePerNight()
        {
            var rooms = new List<HotelRoom>
            {
                new HotelRoom { PricePerNight = 100.0 },
                new HotelRoom { PricePerNight = 200.0 },
                new HotelRoom { PricePerNight = 300.0 }
            };
            double averagePrice = HotelRoom.CalculateAveragePricePerNight(rooms);
            Assert.AreEqual(200.0, averagePrice);
        }

        [TestMethod]
        public void Test_Parse()
        {
            string input = "101, Deluxe, True, 150.0, 2, USD";
            var room = HotelRoom.Parse(input);
            Assert.AreEqual(101, room.RoomNumber);
            Assert.AreEqual(RoomType.Deluxe, room.RoomType);
            Assert.IsTrue(room.IsAvailable);
            Assert.AreEqual(150.0, room.PricePerNight);
            Assert.AreEqual(2, room.BedCount);
        }

        [TestMethod]
        public void Test_TryParse()
        {
            string input = "101, Deluxe, True, 150.0, 2, USD";
            bool result = HotelRoom.TryParse(input, out var room);
            Assert.IsTrue(result);
            Assert.AreEqual(101, room.RoomNumber);
            Assert.AreEqual(RoomType.Deluxe, room.RoomType);
            Assert.IsTrue(room.IsAvailable);
            Assert.AreEqual(150.0, room.PricePerNight);
            Assert.AreEqual(2, room.BedCount);
        }

        [TestMethod]
        public void Test_ToString()
        {
            var room = new HotelRoom(101, RoomType.Deluxe, true, 150.0, 2);
            string result = room.ToString();
            Assert.AreEqual("101,Deluxe,True,150,2,грн", result);
        }

        [TestMethod]
        public void Test_UpdatePrice_InvalidValue()
        {
            var room = new HotelRoom();
            Assert.ThrowsException<ArgumentException>(() => room.UpdatePrice(-50.0));
        }

        [TestMethod]
        public void Test_UpdateBedCount_InvalidValue()
        {
            var room = new HotelRoom();
            Assert.ThrowsException<ArgumentException>(() => room.UpdateBedCount(-1));
        }

        [TestMethod]
        public void Test_CountTotalCostForNights_InvalidValue()
        {
            var room = new HotelRoom();
            Assert.ThrowsException<ArgumentException>(() => room.CountTotalCostForNights(-1));
        }

        [TestMethod]
        public void Test_TryParse_InvalidInput()
        {
            string input = "invalid, data, for, parsing";
            bool result = HotelRoom.TryParse(input, out var room);
            Assert.IsFalse(result);
            Assert.IsNull(room);
        }



    }
}