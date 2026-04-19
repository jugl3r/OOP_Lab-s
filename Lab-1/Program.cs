using System;

namespace Variant1
{
    //  первый класс PostalAddress
    class PostalAddress
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Building { get; set; }
        public string ZipCode { get; set; }

        // Конструктор по умолчанию
        public PostalAddress()
        {
            Country = "Неизвестно";
            City = "Неизвестно";
            Street = "Неизвестно";
            Building = "Неизвестно";
            ZipCode = "000000";
            Console.WriteLine("Создан объект PostalAddress (по умолчанию).");
        }

        // Конструктор с параметрами
        public PostalAddress(string country, string city, string street, string building, string zipCode)
        {
            Country = country;
            City = city;
            Street = street;
            Building = building;
            ZipCode = zipCode;
            Console.WriteLine(string.Format("Создан объект PostalAddress ({0}, {1}).", City, Street));
        }

        // Деструктор (Финализатор) - для демонстрации уничтожения объекта
        ~PostalAddress()
        {
            // c си шарп деструкторы вызываются сборщиком мусора, 
            // поэтому вывод может не появиться сразу в консоли
            Console.WriteLine(string.Format("Уничтожен объект PostalAddress ({0}, {1}).", City, Street));
        }

        public void PrintAddress()
        {
            Console.WriteLine(string.Format("Адрес: {0}, {1}, г. {2}, ул. {3}, д. {4}", ZipCode, Country, City, Street, Building));
        }
    }

    // второй класс Money
    class Money
    {
        private long rubles;
        private byte kopecks;

        public long Rubles
        {
            get { return rubles; }
            set { rubles = value; }
        }

        public byte Kopecks
        {
            get { return kopecks; }
            set
            {
                if (value >= 100)
                {
                    rubles += value / 100;
                    kopecks = (byte)(value % 100);
                }
                else
                {
                    kopecks = value;
                }
            }
        }

        public Money(long rubles, byte kopecks)
        {
            this.rubles = rubles;
            this.kopecks = 0;
            this.Kopecks = kopecks; // Используем свойство для корректной обработки копеек >= 100
        }

        private double ToDouble()
        {
            return rubles + kopecks / 100.0;
        }

        private static Money FromDouble(double value)
        {
            if (value < 0) throw new InvalidOperationException("Отрицательная сумма недопустима.");
            long r = (long)value;
            byte k = (byte)Math.Round((value - r) * 100);
            if (k >= 100)
            {
                r += k / 100;
                k = (byte)(k % 100);
            }
            return new Money(r, k);
        }

        // Вывод с разделением дробной и целой части запятой
        public override string ToString()
        {
            return string.Format("{0},{1:D2}", rubles, kopecks);
        }

        // Сложение сумм
        public static Money operator +(Money a, Money b)
        {
            long totalKopecks = a.kopecks + b.kopecks;
            long totalRubles = a.rubles + b.rubles + totalKopecks / 100;
            return new Money(totalRubles, (byte)(totalKopecks % 100));
        }

        // Вычитание сумм
        public static Money operator -(Money a, Money b)
        {
            long totalKopecks1 = a.rubles * 100 + a.kopecks;
            long totalKopecks2 = b.rubles * 100 + b.kopecks;
            if (totalKopecks1 < totalKopecks2)
                throw new InvalidOperationException("Отрицательная сумма недопустима (вычитаемое больше уменьшаемого).");
            
            long result = totalKopecks1 - totalKopecks2;
            return new Money(result / 100, (byte)(result % 100));
        }

        // Деление сумм (сумма на сумму = число)
        public static double operator /(Money a, Money b)
        {
            double val1 = a.ToDouble();
            double val2 = b.ToDouble();
            if (val2 == 0) throw new DivideByZeroException("Деление на ноль.");
            return val1 / val2;
        }

        // Деление суммы на дробное число
        public static Money operator /(Money a, double b)
        {
            if (b <= 0) throw new DivideByZeroException("Деление на ноль или отрицательное число недопустимо.");
            return FromDouble(a.ToDouble() / b);
        }

        // Умножение суммы на дробное число
        public static Money operator *(Money a, double b)
        {
            if (b < 0) throw new InvalidOperationException("Умножение на отрицательное число недопустимо.");
            return FromDouble(a.ToDouble() * b);
        }

        public static Money operator *(double a, Money b)
        {
            return b * a;
        }

        // Операции сравнения
        public static bool operator ==(Money a, Money b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null)) return true;
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;
            return a.rubles == b.rubles && a.kopecks == b.kopecks;
        }

        public static bool operator !=(Money a, Money b)
        {
            return !(a == b);
        }

        public static bool operator <(Money a, Money b)
        {
            if (a.rubles < b.rubles) return true;
            if (a.rubles == b.rubles && a.kopecks < b.kopecks) return true;
            return false;
        }

        public static bool operator >(Money a, Money b)
        {
            return b < a;
        }

        public static bool operator <=(Money a, Money b)
        {
            return a < b || a == b;
        }

        public static bool operator >=(Money a, Money b)
        {
            return a > b || a == b;
        }

        public override bool Equals(object obj)
        {
            if (obj is Money)
                return this == (Money)obj;
            return false;
        }

        public override int GetHashCode()
        {
            return rubles.GetHashCode() ^ kopecks.GetHashCode();
        }
    }

    class Program
    {
        static PostalAddress address;
        static Money m1;
        static Money m2;

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\n=== ГЛАВНОЕ МЕНЮ ===");
                Console.WriteLine("1. Работа с почтовым адресом (PostalAddress)");
                Console.WriteLine("2. Работа с деньгами (Money)");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите действие: ");
                string choice = Console.ReadLine();

                if (choice == "1") AddressMenu();
                else if (choice == "2") MoneyMenu();
                else if (choice == "0") break;
                else Console.WriteLine("Неверный ввод.");
            }
        }

        static void AddressMenu()
        {
            while (true)
            {
                Console.WriteLine("\n--- Меню PostalAddress ---");
                Console.WriteLine("1. Создать адрес");
                Console.WriteLine("2. Изменить страну");
                Console.WriteLine("3. Изменить город");
                Console.WriteLine("4. Изменить улицу");
                Console.WriteLine("5. Изменить дом");
                Console.WriteLine("6. Изменить индекс");
                Console.WriteLine("7. Вывести адрес");
                Console.WriteLine("8. Уничтожить адрес");
                Console.WriteLine("0. Назад");
                Console.Write("Выбор: ");
                string choice = Console.ReadLine();

                if (choice == "0") break;

                switch (choice)
                {
                    case "1":
                        Console.Write("Страна: "); string country = Console.ReadLine();
                        Console.Write("Город: "); string city = Console.ReadLine();
                        Console.Write("Улица: "); string street = Console.ReadLine();
                        Console.Write("Дом: "); string building = Console.ReadLine();
                        Console.Write("Индекс: "); string zip = Console.ReadLine();
                        address = new PostalAddress(country, city, street, building, zip);
                        break;
                    case "2":
                        if (address == null) { Console.WriteLine("Адрес не создан."); break; }
                        Console.Write("Новая страна: "); address.Country = Console.ReadLine();
                        break;
                    case "3":
                        if (address == null) { Console.WriteLine("Адрес не создан."); break; }
                        Console.Write("Новый город: "); address.City = Console.ReadLine();
                        break;
                    case "4":
                        if (address == null) { Console.WriteLine("Адрес не создан."); break; }
                        Console.Write("Новая улица: "); address.Street = Console.ReadLine();
                        break;
                    case "5":
                        if (address == null) { Console.WriteLine("Адрес не создан."); break; }
                        Console.Write("Новый дом: "); address.Building = Console.ReadLine();
                        break;
                    case "6":
                        if (address == null) { Console.WriteLine("Адрес не создан."); break; }
                        Console.Write("Новый индекс: "); address.ZipCode = Console.ReadLine();
                        break;
                    case "7":
                        if (address == null) { Console.WriteLine("Адрес не создан."); break; }
                        address.PrintAddress();
                        break;
                    case "8":
                        address = null;
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        Console.WriteLine("Ссылка на адрес удалена. Сборщик мусора вызван.");
                        break;
                    default:
                        Console.WriteLine("Неверный ввод.");
                        break;
                }
            }
        }

        static Money ReadMoney(string name)
        {
            Console.WriteLine(string.Format("Ввод суммы {0}:", name));
            Console.Write("Рубли (целое): ");
            long r = long.Parse(Console.ReadLine());
            Console.Write("Копейки (целое): ");
            byte k = byte.Parse(Console.ReadLine());
            return new Money(r, k);
        }

        static void MoneyMenu()
        {
            if (m1 == null) m1 = new Money(0, 0);
            if (m2 == null) m2 = new Money(0, 0);

            while (true)
            {
                Console.WriteLine("\n--- Меню Money ---");
                Console.WriteLine(string.Format("Сумма 1: {0}", m1));
                Console.WriteLine(string.Format("Сумма 2: {0}", m2));
                Console.WriteLine("1. Ввести Сумму 1");
                Console.WriteLine("2. Ввести Сумму 2");
                Console.WriteLine("3. Сложение (Сумма 1 + Сумма 2)");
                Console.WriteLine("4. Вычитание (Сумма 1 - Сумма 2)");
                Console.WriteLine("5. Деление сумм (Сумма 1 / Сумма 2)");
                Console.WriteLine("6. Умножение Суммы 1 на дробное число");
                Console.WriteLine("7. Деление Суммы 1 на дробное число");
                Console.WriteLine("8. Сравнить суммы");
                Console.WriteLine("0. Назад");
                Console.Write("Выбор: ");
                string choice = Console.ReadLine();

                if (choice == "0") break;

                try
                {
                    switch (choice)
                    {
                        case "1": m1 = ReadMoney("1"); break;
                        case "2": m2 = ReadMoney("2"); break;
                        case "3": Console.WriteLine(string.Format("Результат: {0}", m1 + m2)); break;
                        case "4": Console.WriteLine(string.Format("Результат: {0}", m1 - m2)); break;
                        case "5": Console.WriteLine(string.Format("Результат: {0:F4}", m1 / m2)); break;
                        case "6":
                            Console.Write("Введите число: ");
                            double mul = double.Parse(Console.ReadLine());
                            Console.WriteLine(string.Format("Результат: {0}", m1 * mul));
                            break;
                        case "7":
                            Console.Write("Введите число: ");
                            double div = double.Parse(Console.ReadLine());
                            Console.WriteLine(string.Format("Результат: {0}", m1 / div));
                            break;
                        case "8":
                            Console.WriteLine(string.Format("Сумма 1 > Сумма 2: {0}", m1 > m2));
                            Console.WriteLine(string.Format("Сумма 1 < Сумма 2: {0}", m1 < m2));
                            Console.WriteLine(string.Format("Сумма 1 == Сумма 2: {0}", m1 == m2));
                            break;
                        default: Console.WriteLine("Неверный ввод."); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("Ошибка: {0}", ex.Message));
                }
            }
        }
    }
}
