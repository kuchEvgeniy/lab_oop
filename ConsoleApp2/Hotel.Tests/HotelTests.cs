using ConsoleApp1;
using ConsoleApp1.ConsoleInterface;
using HotelApp;
using System.Text.Json;

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


        [TestMethod]
        public void Test_SaveRoomsToCsv()
        {
            var rooms = new List<HotelRoom>
            {
                new HotelRoom(101, RoomType.Deluxe, true, 150.0, 2),
                new HotelRoom(102, RoomType.Standard, false, 100.0, 1)
            };

            string filePath = "test_rooms.csv";
            SaveRoomsToCsv(filePath, rooms);

            Assert.IsTrue(File.Exists(filePath));

            var lines = File.ReadAllLines(filePath);
            Assert.AreEqual(2, lines.Length);
            Assert.AreEqual("101,Deluxe,True,150,2,грн", lines[0]);
            Assert.AreEqual("102,Standard,False,100,1,грн", lines[1]);

            File.Delete(filePath);
        }

        [TestMethod]
        public void Test_LoadRoomsFromCsv()
        {
            string filePath = "test_rooms.csv";
            File.WriteAllLines(filePath, new[]
            {
                "101,Deluxe,True,150,2,грн",
                "102,Standard,False,100,1,грн"
            });

            var rooms = LoadRoomsFromCsv(filePath);

            Assert.AreEqual(2, rooms.Count);
            Assert.AreEqual(101, rooms[0].RoomNumber);
            Assert.AreEqual(RoomType.Deluxe, rooms[0].RoomType);
            Assert.IsTrue(rooms[0].IsAvailable);
            Assert.AreEqual(150.0, rooms[0].PricePerNight);
            Assert.AreEqual(2, rooms[0].BedCount);

            Assert.AreEqual(102, rooms[1].RoomNumber);
            Assert.AreEqual(RoomType.Standard, rooms[1].RoomType);
            Assert.IsFalse(rooms[1].IsAvailable);
            Assert.AreEqual(100.0, rooms[1].PricePerNight);
            Assert.AreEqual(1, rooms[1].BedCount);

            File.Delete(filePath);
        }

        [TestMethod]
        public void Test_SaveRoomsToJson()
        {
            var rooms = new List<HotelRoom>
            {
                new HotelRoom(101, RoomType.Deluxe, true, 150.0, 2),
                new HotelRoom(102, RoomType.Standard, false, 100.0, 1)
            };

            string filePath = "test_rooms.json";
            SaveRoomsToJson(filePath, rooms);

            Assert.IsTrue(File.Exists(filePath));

            string jsonString = File.ReadAllText(filePath);
            var loadedRooms = JsonSerializer.Deserialize<List<HotelRoom>>(jsonString);

            Assert.AreEqual(2, loadedRooms.Count);
            Assert.AreEqual(101, loadedRooms[0].RoomNumber);
            Assert.AreEqual(RoomType.Deluxe, loadedRooms[0].RoomType);
            Assert.IsTrue(loadedRooms[0].IsAvailable);
            Assert.AreEqual(150.0, loadedRooms[0].PricePerNight);
            Assert.AreEqual(2, loadedRooms[0].BedCount);

            Assert.AreEqual(102, loadedRooms[1].RoomNumber);
            Assert.AreEqual(RoomType.Standard, loadedRooms[1].RoomType);
            Assert.IsTrue(loadedRooms[1].IsAvailable);
            Assert.AreEqual(100.0, loadedRooms[1].PricePerNight);
            Assert.AreEqual(1, loadedRooms[1].BedCount);
        }

        [TestMethod]
        public void Test_LoadRoomsFromJson()
        {
            string filePath = "test_rooms.json";
            var rooms = new List<HotelRoom>
            {
                new HotelRoom(101, RoomType.Deluxe, true, 150.0, 2),
                new HotelRoom(102, RoomType.Standard, false, 100.0, 1)
            };

            string jsonString = JsonSerializer.Serialize(rooms);
            File.WriteAllText(filePath, jsonString);

            var loadedRooms = LoadRoomsFromJson(filePath);

            Assert.AreEqual(2, loadedRooms.Count);
            Assert.AreEqual(101, loadedRooms[0].RoomNumber);
            Assert.AreEqual(RoomType.Deluxe, loadedRooms[0].RoomType);
            Assert.IsTrue(loadedRooms[0].IsAvailable);
            Assert.AreEqual(150.0, loadedRooms[0].PricePerNight);
            Assert.AreEqual(2, loadedRooms[0].BedCount);

            Assert.AreEqual(102, loadedRooms[1].RoomNumber);
            Assert.AreEqual(RoomType.Standard, loadedRooms[1].RoomType);
            Assert.IsTrue(loadedRooms[1].IsAvailable);
            Assert.AreEqual(100.0, loadedRooms[1].PricePerNight);
            Assert.AreEqual(1, loadedRooms[1].BedCount);

            File.Delete(filePath);
        }

        private void SaveRoomsToCsv(string filePath, List<HotelRoom> rooms)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var room in rooms)
                {
                    writer.WriteLine(room.ToString());
                }
            }
        }

        private List<HotelRoom> LoadRoomsFromCsv(string filePath)
        {
            var rooms = new List<HotelRoom>();

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var room = HotelRoom.Parse(line);
                    if (room != null)
                    {
                        rooms.Add(room);
                    }
                }
            }

            return rooms;
        }

        private void SaveRoomsToJson(string filePath, List<HotelRoom> rooms)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string jsonString = JsonSerializer.Serialize(rooms, options);
            File.WriteAllText(filePath, jsonString);
        }

        private List<HotelRoom> LoadRoomsFromJson(string filePath)
        {
            string jsonString = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<HotelRoom>>(jsonString);
        }
    }
}