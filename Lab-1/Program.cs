using System;
using System.Collections.Generic;

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
        static List<PostalAddress> addresses = new List<PostalAddress>();

        static List<Money> monies = new List<Money>();
        static int activeMoney1Index = -1;
        static int activeMoney2Index = -1;

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

        static int SelectAddressIndex()
        {
            if (addresses.Count == 0)
            {
                Console.WriteLine("Список адресов пуст.");
                return -1;
            }
            Console.WriteLine("Список адресов:");
            for (int i = 0; i < addresses.Count; i++)
            {
                Console.Write(string.Format("{0}. ", i + 1));
                addresses[i].PrintAddress();
            }
            Console.Write(string.Format("Выберите номер адреса (1 - {0}): ", addresses.Count));
            int index;
            if (int.TryParse(Console.ReadLine(), out index) && index >= 1 && index <= addresses.Count)
            {
                return index - 1;
            }
            Console.WriteLine("Неверный номер.");
            return -1;
        }

        static void AddressMenu()
        {
            while (true)
            {
                Console.WriteLine("\n--- Меню PostalAddress ---");
                Console.WriteLine(string.Format("Всего адресов: {0}", addresses.Count));
                
                Console.WriteLine("1. Создать новый адрес");
                Console.WriteLine("2. Вывести все адреса");
                Console.WriteLine("3. Изменить страну адреса");
                Console.WriteLine("4. Изменить город адреса");
                Console.WriteLine("5. Изменить улицу адреса");
                Console.WriteLine("6. Изменить дом адреса");
                Console.WriteLine("7. Изменить индекс адреса");
                Console.WriteLine("8. Вывести конкретный адрес");
                Console.WriteLine("9. Удалить адрес");
                Console.WriteLine("0. Назад");
                Console.Write("Выбор: ");
                string choice = Console.ReadLine();

                if (choice == "0") break;

                int idx;
                switch (choice)
                {
                    case "1":
                        Console.Write("Страна: "); string country = Console.ReadLine();
                        Console.Write("Город: "); string city = Console.ReadLine();
                        Console.Write("Улица: "); string street = Console.ReadLine();
                        Console.Write("Дом: "); string building = Console.ReadLine();
                        Console.Write("Индекс: "); string zip = Console.ReadLine();
                        addresses.Add(new PostalAddress(country, city, street, building, zip));
                        Console.WriteLine("Новый адрес создан.");
                        break;
                    case "2":
                        if (addresses.Count == 0) { Console.WriteLine("Список адресов пуст."); break; }
                        for (int i = 0; i < addresses.Count; i++)
                        {
                            Console.Write(string.Format("{0}. ", i + 1));
                            addresses[i].PrintAddress();
                        }
                        break;
                    case "3":
                        idx = SelectAddressIndex();
                        if (idx != -1) { Console.Write("Новая страна: "); addresses[idx].Country = Console.ReadLine(); }
                        break;
                    case "4":
                        idx = SelectAddressIndex();
                        if (idx != -1) { Console.Write("Новый город: "); addresses[idx].City = Console.ReadLine(); }
                        break;
                    case "5":
                        idx = SelectAddressIndex();
                        if (idx != -1) { Console.Write("Новая улица: "); addresses[idx].Street = Console.ReadLine(); }
                        break;
                    case "6":
                        idx = SelectAddressIndex();
                        if (idx != -1) { Console.Write("Новый дом: "); addresses[idx].Building = Console.ReadLine(); }
                        break;
                    case "7":
                        idx = SelectAddressIndex();
                        if (idx != -1) { Console.Write("Новый индекс: "); addresses[idx].ZipCode = Console.ReadLine(); }
                        break;
                    case "8":
                        idx = SelectAddressIndex();
                        if (idx != -1) { addresses[idx].PrintAddress(); }
                        break;
                    case "9":
                        idx = SelectAddressIndex();
                        if (idx != -1) 
                        { 
                            addresses.RemoveAt(idx);
                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                            Console.WriteLine("Адрес удален из списка.");
                        }
                        break;
                    default:
                        Console.WriteLine("Неверный ввод.");
                        break;
                }
            }
        }

        static Money ReadMoney()
        {
            Console.WriteLine("Ввод новой суммы:");
            Console.Write("Рубли (целое): ");
            long r = long.Parse(Console.ReadLine());
            Console.Write("Копейки (целое): ");
            byte k = byte.Parse(Console.ReadLine());
            return new Money(r, k);
        }

        static void MoneyMenu()
        {
            while (true)
            {
                Console.WriteLine("\n--- Меню Money ---");
                Console.WriteLine(string.Format("Всего сумм: {0}", monies.Count));
                
                string m1Str = (activeMoney1Index >= 0 && activeMoney1Index < monies.Count) ? string.Format("№{0} ({1})", activeMoney1Index + 1, monies[activeMoney1Index]) : "не выбрана";
                string m2Str = (activeMoney2Index >= 0 && activeMoney2Index < monies.Count) ? string.Format("№{0} ({1})", activeMoney2Index + 1, monies[activeMoney2Index]) : "не выбрана";
                
                Console.WriteLine(string.Format("Выбранная Сумма 1: {0}", m1Str));
                Console.WriteLine(string.Format("Выбранная Сумма 2: {0}", m2Str));
                
                Console.WriteLine("1. Создать новую сумму");
                Console.WriteLine("2. Вывести все суммы");
                Console.WriteLine("3. Выбрать Сумму 1");
                Console.WriteLine("4. Выбрать Сумму 2");
                Console.WriteLine("5. Сложение (Сумма 1 + Сумма 2)");
                Console.WriteLine("6. Вычитание (Сумма 1 - Сумма 2)");
                Console.WriteLine("7. Деление сумм (Сумма 1 / Сумма 2)");
                Console.WriteLine("8. Умножение Суммы 1 на дробное число");
                Console.WriteLine("9. Деление Суммы 1 на дробное число");
                Console.WriteLine("10. Сравнить суммы 1 и 2");
                Console.WriteLine("0. Назад");
                Console.Write("Выбор: ");
                string choice = Console.ReadLine();

                if (choice == "0") break;

                try
                {
                    switch (choice)
                    {
                        case "1": 
                            monies.Add(ReadMoney()); 
                            if (activeMoney1Index == -1) activeMoney1Index = monies.Count - 1;
                            else if (activeMoney2Index == -1) activeMoney2Index = monies.Count - 1;
                            Console.WriteLine("Новая сумма добавлена в список.");
                            break;
                        case "2":
                            if (monies.Count == 0) { Console.WriteLine("Список сумм пуст."); break; }
                            for (int i = 0; i < monies.Count; i++)
                            {
                                Console.WriteLine(string.Format("{0}. {1}", i + 1, monies[i]));
                            }
                            break;
                        case "3":
                            if (monies.Count == 0) { Console.WriteLine("Список пуст."); break; }
                            Console.Write(string.Format("Введите номер для Суммы 1 (1 - {0}): ", monies.Count));
                            int idx1;
                            if (int.TryParse(Console.ReadLine(), out idx1) && idx1 >= 1 && idx1 <= monies.Count)
                                activeMoney1Index = idx1 - 1;
                            else Console.WriteLine("Неверный номер.");
                            break;
                        case "4":
                            if (monies.Count == 0) { Console.WriteLine("Список пуст."); break; }
                            Console.Write(string.Format("Введите номер для Суммы 2 (1 - {0}): ", monies.Count));
                            int idx2;
                            if (int.TryParse(Console.ReadLine(), out idx2) && idx2 >= 1 && idx2 <= monies.Count)
                                activeMoney2Index = idx2 - 1;
                            else Console.WriteLine("Неверный номер.");
                            break;
                        case "5": 
                            if (activeMoney1Index < 0 || activeMoney2Index < 0) { Console.WriteLine("Выберите обе суммы."); break; }
                            Console.WriteLine(string.Format("Результат: {0}", monies[activeMoney1Index] + monies[activeMoney2Index])); 
                            break;
                        case "6": 
                            if (activeMoney1Index < 0 || activeMoney2Index < 0) { Console.WriteLine("Выберите обе суммы."); break; }
                            Console.WriteLine(string.Format("Результат: {0}", monies[activeMoney1Index] - monies[activeMoney2Index])); 
                            break;
                        case "7": 
                            if (activeMoney1Index < 0 || activeMoney2Index < 0) { Console.WriteLine("Выберите обе суммы."); break; }
                            Console.WriteLine(string.Format("Результат: {0:F4}", monies[activeMoney1Index] / monies[activeMoney2Index])); 
                            break;
                        case "8":
                            if (activeMoney1Index < 0) { Console.WriteLine("Сумма 1 не выбрана."); break; }
                            Console.Write("Введите число: ");
                            double mul = double.Parse(Console.ReadLine());
                            Console.WriteLine(string.Format("Результат: {0}", monies[activeMoney1Index] * mul));
                            break;
                        case "9":
                            if (activeMoney1Index < 0) { Console.WriteLine("Сумма 1 не выбрана."); break; }
                            Console.Write("Введите число: ");
                            double div = double.Parse(Console.ReadLine());
                            Console.WriteLine(string.Format("Результат: {0}", monies[activeMoney1Index] / div));
                            break;
                        case "10":
                            if (activeMoney1Index < 0 || activeMoney2Index < 0) { Console.WriteLine("Выберите обе суммы."); break; }
                            Money a = monies[activeMoney1Index];
                            Money b = monies[activeMoney2Index];
                            Console.WriteLine(string.Format("Сумма 1 > Сумма 2: {0}", a > b));
                            Console.WriteLine(string.Format("Сумма 1 < Сумма 2: {0}", a < b));
                            Console.WriteLine(string.Format("Сумма 1 == Сумма 2: {0}", a == b));
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
