using ConsoleApp1;
using System.Globalization;

namespace HotelApp
{
    public class HotelRoom
    {
        public int roomNumber = 0;
        private RoomType roomType;
        private bool isAvailable;
        private double pricePerNight;
        private int bedCount;
        private string currency = "грн";

        private static int totalRooms;
        private static double maxPricePerNight;

        // лічильник
        public static int TotalRooms
        {
            get { return totalRooms; }
            private set { totalRooms = value; }
        }

        public static double MaxPricePerNight
        {
            get { return maxPricePerNight; }
            private set { maxPricePerNight = value; }
        }

        public int RoomNumber { get => roomNumber; set => roomNumber = value; }

        public RoomType RoomType
        {
            get { return roomType; }
            set { roomType = value; }
        }

        public bool IsAvailable
        {
            get { return isAvailable; }
            private set { isAvailable = value; }
        }

        public double PricePerNight
        {
            get { return pricePerNight; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Вартість доби не може бути від'ємною");
                pricePerNight = value;
            }
        }

        public int BedCount
        {
            get { return bedCount; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Кількість ліжок не може бути від'ємною");

                bedCount = value;
            }
        }

        public double CountTotalCostForNights(int nights)
        {
            if (nights < 0)
                throw new ArgumentException("Кількість ночей не може бути від'ємною");

            return PricePerNight * nights;
        }

        public HotelRoom()
        {
            roomNumber = 0;
            roomType = RoomType.Standard;
            isAvailable = true;
            pricePerNight = 100.0;
            bedCount = 1;
            TotalRooms++;
        }

        public HotelRoom(int roomNumber,
                         RoomType roomType) : this()
        {
            this.roomNumber = roomNumber;
            this.roomType = roomType;
        }

        public HotelRoom(int roomNumber,
                         RoomType roomType,
                         bool isAvailable,
                         double pricePerNight,
                         int bedCount) : this(roomNumber, roomType)
        {
            this.isAvailable = isAvailable;
            this.pricePerNight = pricePerNight;
            this.bedCount = bedCount;
            if (pricePerNight > MaxPricePerNight)
            {
                MaxPricePerNight = pricePerNight;
            }
        }

        public void DisplayInfo()
        {
            ServicesManager.Writer.Log($"Номер кімнати: {roomNumber}");
            ServicesManager.Writer.Log($"Тип кімнати: {roomType}");
            ServicesManager.Writer.Log($"Доступність: {(isAvailable ? "Доступна" : "Зайнята")}");
            ServicesManager.Writer.Log($"Ціна за ніч: {pricePerNight} {currency}.");
            ServicesManager.Writer.Log($"Кількість ліжок: {bedCount}");
        }

        public void UpdatePrice(double newPrice)
        {
            if (newPrice <= 30000)
            {
                PricePerNight = newPrice;

                ServicesManager.Writer.Log("");
                ServicesManager.Writer.Log($"Ціна за номер {roomNumber} оновлена до {newPrice} {currency}.");

                if (newPrice > MaxPricePerNight)
                {
                    MaxPricePerNight = newPrice;
                }
            }
            else
            {
                ServicesManager.Writer.Log("Невірна ціна. Ціна повинна бути додатною і не перевищувати 30000.");
            }
        }

        public void UpdatePrice(double newPrice, string currency)
        {
            if (newPrice >= 0 && newPrice <= 30000 && !string.IsNullOrEmpty(currency))
            {
                PricePerNight = newPrice;
                this.currency = currency;
                ServicesManager.Writer.Log("");
                ServicesManager.Writer.Log($"Ціна за номер {roomNumber} оновлена до {newPrice} {currency}.");
                if (newPrice > MaxPricePerNight)
                {
                    MaxPricePerNight = newPrice;
                }
            }
            else
            {
                ServicesManager.Writer.Log("Невірна ціна. Ціна повинна бути додатною і не перевищувати 30000.");
            }
        }

        public void ChangeAvailability()
        {
            IsAvailable = !IsAvailable;
            ServicesManager.Writer.Log($"Статус доступності номера {roomNumber} змінено на {(IsAvailable ? "Доступний" : "Зайнятий")}.");
        }

        public void UpdateBedCount(int newBedCount)
        {
            BedCount = newBedCount;
        }

        public static double CalculateAveragePricePerNight(List<HotelRoom> rooms)
        {
            if (rooms == null || rooms.Count == 0)
                return 0;

            double total = 0;
            foreach (var room in rooms)
            {
                total += room.PricePerNight;
            }

            return total / rooms.Count;
        }

        public static HotelRoom Parse(string input)
        {
            var parts = input.Split(',');

            if (parts.Length != 6)
            {
                throw new Exception("Невірний формат вхідних даних. Частин повинно бути 6");
            }

            if (!int.TryParse(parts[0].Trim(), out int roomNumber))
            {
                throw new Exception("Невірний формат номера кімнати");
            }
            if (!Enum.TryParse(parts[1].Trim(), out RoomType roomType))
            {
                throw new Exception("Невірний формат типу кімнати");
            }
            if (!bool.TryParse(parts[2].Trim(), out bool isAvailable))
            {
                throw new Exception("Невірний формат доступності");
            }
            if (!double.TryParse(parts[3].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out double pricePerNight))
            {
                throw new Exception("Невірний формат ціни за ніч");
            }
            if (!int.TryParse(parts[4].Trim(), out int bedCount))
            {
                throw new Exception("Невірний формат кількості ліжок");
            }
            string currency = parts[5].Trim();

            return new HotelRoom(roomNumber, roomType, isAvailable, pricePerNight, bedCount) { currency = currency };
        }

        public static bool TryParse(string input, out HotelRoom hotelRoom)
        {
            try
            {
                hotelRoom = Parse(input);
                return true;
            }
            catch (Exception e)
            {
                hotelRoom = null;
                ServicesManager.Writer.Log(e.Message);
                return false;
            }
        }

        public override string ToString()
        {
            return $"{roomNumber},{roomType},{isAvailable},{pricePerNight},{bedCount},{currency}";
        }


        public static HotelRoom? FromCsv(string csvLine)
        {
            var values = csvLine.Split(',');

            if (values.Length != 6)
            {
                ServicesManager.Writer.Log("Неправильний формат рядка. Очікується 6 частин.");
                ServicesManager.Writer.Log("Помилка у рядку: " + string.Join(',', values));
                return null;
            }

            try
            {
                if (!int.TryParse(values[0].Trim(), out int roomNumber))
                {
                    ServicesManager.Writer.Log("Невірний формат номера кімнати");
                    return null;
                }
                if (!Enum.TryParse(values[1].Trim(), out RoomType roomType))
                {
                    ServicesManager.Writer.Log("Невірний формат типу кімнати");
                    return null;
                }
                if (!bool.TryParse(values[2].Trim(), out bool isAvailable))
                {
                    ServicesManager.Writer.Log("Невірний формат доступності");
                    return null;
                }
                if (!double.TryParse(values[3].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out double pricePerNight))
                {
                    ServicesManager.Writer.Log("Невірний формат ціни за ніч");
                    return null;
                }
                if (!int.TryParse(values[4].Trim(), out int bedCount))
                {
                    ServicesManager.Writer.Log("Невірний формат кількості ліжок");
                    return null;
                }
                string currency = values[5].Trim();

                return new HotelRoom(roomNumber, roomType, isAvailable, pricePerNight, bedCount) { currency = currency };
            }
            catch (Exception e)
            {
                ServicesManager.Writer.Log($"Помилка у: {csvLine}");
                ServicesManager.Writer.Log(e.Message);
                return null;
            }
        }

    }

}

