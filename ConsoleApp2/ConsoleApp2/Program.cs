using ConsoleApp1;
using ConsoleApp1.ConsoleInterface;
using HotelApp;

ServicesManager.Writer = new ConsoleLogger();
ServicesManager.Reader = new ConsoleReader();

List<HotelRoom> rooms = new List<HotelRoom>();

int maxRooms = 0;

Console.OutputEncoding = System.Text.Encoding.UTF8;

ServicesManager.Writer.Log("Введіть максимальну кількість номерів (N): ");

while (!int.TryParse(ServicesManager.Reader.Read(), out maxRooms) || maxRooms <= 1)
{
    ServicesManager.Writer.Log("Невірне значення. Введіть число більше 1.");
}

bool running = true;
while (running)
{
    ServicesManager.Writer.Log("\nМеню:");
    ServicesManager.Writer.Log("1. Додати номер");
    ServicesManager.Writer.Log("2. Вивести всі номери");
    ServicesManager.Writer.Log("3. Пошук номера");
    ServicesManager.Writer.Log("4. Видалити номер");
    ServicesManager.Writer.Log("5. Демонстрація поведінки");
    ServicesManager.Writer.Log("6. Показати середню ціну за ніч для всіх номерів(static)");
    ServicesManager.Writer.Log("0. Вийти з програми");
    ServicesManager.Writer.Log("");

    ServicesManager.Writer.Log("Оберіть пункт меню: \n");

    string choice = ServicesManager.Reader.Read();

    switch (choice)
    {
        case "1":
            AddRoom();
            break;
        case "2":
            DisplayRooms();
            break;
        case "3":
            SearchRoom();
            break;
        case "4":
            RemoveRoom();
            break;
        case "5":
            DemonstrateBehavior();
            break;
        case "6":
            ShowAveragePricePerNight();
            break;
        case "0":
            running = false;
            break;
        default:
            ServicesManager.Writer.Log("Невірний вибір, спробуйте ще раз.");
            break;
    }
}

void AddRoom()
{
    if (rooms.Count >= maxRooms)
    {
        ServicesManager.Writer.Log("Досягнуто максимальної кількості номерів.");
        return;
    }

    ServicesManager.Writer.Log("");
    int roomNumber;
    while (true)
    {
        ServicesManager.Writer.Log("Введіть номер кімнати: ");
        if (int.TryParse(ServicesManager.Reader.Read(), out roomNumber) && roomNumber > 0)
            break;
        ServicesManager.Writer.Log("Номер кімнати повинен бути додатнім цілим числом.");
    }

    RoomType roomType;
    while (true)
    {
        ServicesManager.Writer.Log("Введіть тип кімнати ( Standard (0), Superior (1), Deluxe (2), Suite (3) ): ");
        if (Enum.TryParse<RoomType>(ServicesManager.Reader.Read(), out roomType) && Enum.IsDefined(typeof(RoomType), roomType))
            break;
        ServicesManager.Writer.Log("Невірний тип кімнати.");
    }

    bool isAvailable;
    while (true)
    {
        ServicesManager.Writer.Log("Чи доступна кімната? (+/-): ");
        string input = ServicesManager.Reader.Read();
        if (input == "+")
        {
            isAvailable = true;
            break;
        }
        else if (input == "-")
        {
            isAvailable = false;
            break;
        }
        else
        {
            ServicesManager.Writer.Log("Невірний ввід. Введіть + або -");
        }
    }

    double pricePerNight;
    while (true)
    {
        ServicesManager.Writer.Log("Введіть ціну за ніч: ");
        if (double.TryParse(ServicesManager.Reader.Read(), out pricePerNight) && pricePerNight >= 0 && pricePerNight <= 30000)
            break;
        ServicesManager.Writer.Log("Ціна повинна бути додатною і не перевищувати 30000.");
    }

    int bedCount;
    while (true)
    {
        ServicesManager.Writer.Log("Введіть кількість ліжок: ");
        if (int.TryParse(ServicesManager.Reader.Read(), out bedCount) && bedCount > 0 && bedCount <= 5)
            break;
        ServicesManager.Writer.Log("Кількість ліжок повинна бути додатнім цілим числом і не більше 5.");
    }

    rooms.Add(new HotelRoom(roomNumber, roomType, isAvailable, pricePerNight, bedCount));
    ServicesManager.Writer.Log("Номер додано.");
}

void DisplayRooms()
{
    if (rooms.Count == 0)
    {
        ServicesManager.Writer.Log("\nНемає номерів для відображення.");
        return;
    }

    foreach (var room in rooms)
    {
        ServicesManager.Writer.Log("");
        room.DisplayInfo();
    }
}

void SearchRoom()
{
    ServicesManager.Writer.Log("\nВиберіть критерії пошуку:");
    ServicesManager.Writer.Log("1. Тип кімнати");
    ServicesManager.Writer.Log("2. Ціна за ніч");
    string choice = ServicesManager.Reader.Read();

    if (choice == "1")
    {
        ServicesManager.Writer.Log("Введіть тип кімнати (Standard (0), Superior (1), Deluxe (2), Suite (3)): ");
        if (!Enum.TryParse<RoomType>(ServicesManager.Reader.Read(), out RoomType roomType) || !Enum.IsDefined(typeof(RoomType), roomType))
        {
            ServicesManager.Writer.Log("Невірний тип кімнати.");
            return;
        }

        var foundRooms = rooms.FindAll(r => r.RoomType == roomType).ToList();
        if (foundRooms.Count > 0)
        {
            foreach (var room in foundRooms)
            {
                room.DisplayInfo();
            }
        }
        else
        {
            ServicesManager.Writer.Log("Номери не знайдено.");
        }
    }
    else if (choice == "2")
    {
        ServicesManager.Writer.Log("Введіть максимальну ціну за ніч: ");
        if (!double.TryParse(ServicesManager.Reader.Read(), out double price) || price < 0)
        {
            ServicesManager.Writer.Log("Невірна ціна.");
            return;
        }

        var foundRooms = rooms.FindAll(r => r.PricePerNight <= price).ToList();
        if (foundRooms.Count > 0)
        {
            foreach (var room in foundRooms)
            {
                room.DisplayInfo();
            }
        }
        else
        {
            ServicesManager.Writer.Log("Номери не знайдено.");
        }
    }
    else
    {
        ServicesManager.Writer.Log("Невірний вибір.");
    }
}

void RemoveRoom()
{
    ServicesManager.Writer.Log("\nВиберіть варіант видалення:");
    ServicesManager.Writer.Log("1. Видалити за номером кімнати");
    ServicesManager.Writer.Log("2. Видалити за ціною");
    string choice = ServicesManager.Reader.Read();

    if (choice == "1")
    {
        ServicesManager.Writer.Log("\nВведіть номер кімнати для видалення: ");
        if (!int.TryParse(ServicesManager.Reader.Read(), out int roomNumber))
        {
            ServicesManager.Writer.Log("Невірний номер кімнати.");
            return;
        }

        var room = rooms.Find(r => r.RoomNumber == roomNumber);

        if (room != null)
        {
            rooms.Remove(room);
            ServicesManager.Writer.Log("Номер видалено.");
        }
        else
        {
            ServicesManager.Writer.Log("Номер не знайдено.");
        }
    }
    else if (choice == "2")
    {
        ServicesManager.Writer.Log("Введіть максимальну ціну для видалення: ");
        if (!double.TryParse(ServicesManager.Reader.Read(), out double price) || price < 0)
        {
            ServicesManager.Writer.Log("Невірна ціна.");
            return;
        }

        var roomsToRemove = rooms.FindAll(r => r.PricePerNight <= price).ToList();

        if (roomsToRemove.Count > 0)
        {
            foreach (var room in roomsToRemove)
            {
                rooms.Remove(room);
                ServicesManager.Writer.Log($"Номер {room.RoomNumber} за ціною {room.PricePerNight} грн. видалено.");
            }
        }
        else
        {
            ServicesManager.Writer.Log("Номери за такою ціною не знайдено.");
        }
    }
    else
    {
        ServicesManager.Writer.Log("Невірний вибір.");
    }
}

void DemonstrateBehavior()
{
    if (rooms.Count == 0)
    {
        ServicesManager.Writer.Log("Немає номерів для демонстрації.");
        return;
    }

    bool running = true;
    while (running)
    {
        ServicesManager.Writer.Log("\nМеню демонстрації поведінки:");
        ServicesManager.Writer.Log("1. Змінити ціну");
        ServicesManager.Writer.Log("2. Змінити кількість ліжок");
        ServicesManager.Writer.Log("3. Змінити доступність");
        ServicesManager.Writer.Log("4. Показати інформацію про номери");
        ServicesManager.Writer.Log("0. Вийти до головного меню");
        ServicesManager.Writer.Log("Оберіть пункт меню: ");

        string choice = ServicesManager.Reader.Read();

        switch (choice)
        {
            case "1":
                UpdateRoomPrice();
                break;
            case "2":
                UpdateRoomBedCount();
                break;
            case "3":
                ChangeRoomAvailability();
                break;
            case "4":
                DisplayRooms();
                break;
            case "0":
                running = false;
                break;
            default:
                ServicesManager.Writer.Log("Невірний вибір, спробуйте ще раз.");
                break;
        }
    }
}

void UpdateRoomPrice()
{
    ServicesManager.Writer.Log("Введіть номер кімнати для зміни ціни: ");
    if (!int.TryParse(ServicesManager.Reader.Read(), out int roomNumber))
    {
        ServicesManager.Writer.Log("Невірний номер кімнати.");
        return;
    }

    var room = rooms.Find(r => r.RoomNumber == roomNumber);

    if (room != null)
    {
        while (true)
        {
            ServicesManager.Writer.Log("Введіть нову ціну за ніч: ");
            if (double.TryParse(ServicesManager.Reader.Read(), out double newPrice))
            {
                ServicesManager.Writer.Log("Виберіть варіант оновлення ціни:");
                ServicesManager.Writer.Log("1. Оновити ціну без валюти");
                ServicesManager.Writer.Log("2. Оновити ціну з валютою");
                string choice = ServicesManager.Reader.Read();

                if (choice == "1")
                {
                    try
                    {
                        room.UpdatePrice(newPrice);
                    }
                    catch (Exception e)
                    {

                        ServicesManager.Writer.Log("Неправильно! " + e.Message);
                    }
                }
                else if (choice == "2")
                {
                    try
                    {
                        ServicesManager.Writer.Log("Введіть валюту: ");
                        string currency = ServicesManager.Reader.Read();
                        room.UpdatePrice(newPrice, currency);
                    }
                    catch (Exception e)
                    {
                        ServicesManager.Writer.Log("Неправильно! " + e.Message);
                    }
                }
                else
                {
                    ServicesManager.Writer.Log("Невірний вибір.");
                    continue;
                }
                break;
            }
            else
            {
                ServicesManager.Writer.Log("Невірна ціна.");

            }
        }
    }

    else
    {
        ServicesManager.Writer.Log("Номер не знайдено.");
    }
}

void UpdateRoomBedCount()
{
    ServicesManager.Writer.Log("Введіть номер кімнати для зміни кількості ліжок: ");
    if (!int.TryParse(ServicesManager.Reader.Read(), out int roomNumber))
    {
        ServicesManager.Writer.Log("Невірний номер кімнати.");
        return;
    }

    var room = rooms.Find(r => r.RoomNumber == roomNumber);

    if (room != null)
    {
        ServicesManager.Writer.Log("Введіть нову кількість ліжок: ");
        if (int.TryParse(ServicesManager.Reader.Read(), out int newBedCount))
        {
            room.UpdateBedCount(newBedCount);
        }
        else
        {
            ServicesManager.Writer.Log("Невірна кількість ліжок.");
        }
    }
    else
    {
        ServicesManager.Writer.Log("Номер не знайдено.");
    }
}

void ChangeRoomAvailability()
{
    ServicesManager.Writer.Log("Введіть номер кімнати для зміни доступності: ");
    if (!int.TryParse(ServicesManager.Reader.Read(), out int roomNumber))
    {
        ServicesManager.Writer.Log("Невірний номер кімнати.");
        return;
    }

    var room = rooms.Find(r => r.RoomNumber == roomNumber);

    if (room != null)
    {
        room.ChangeAvailability();
    }
    else
    {
        ServicesManager.Writer.Log("Номер не знайдено.");
    }
}

void ShowAveragePricePerNight()
{
    double averagePrice = HotelRoom.CalculateAveragePricePerNight(rooms);
    ServicesManager.Writer.Log($"Середня ціна за ніч для всіх номерів: {averagePrice} грн.");
}