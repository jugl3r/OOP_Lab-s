using System;

namespace Lab4
{
    // Константа, определяемая вне класса (по требованию методички)
    static class Constants
    {
        public const int DEFAULT_MAX_SIZE = 100;
    }

    // Абстрактный тип данных: Множество символов (АТД)
    class CharSet
    {
        // 1. Указатель на начало массива (в C# ссылка на массив)
        private char[] elements;
        // 2. Максимальный размер массива
        private int maxSize;
        // 3. Текущий размер массива
        private int currentSize;

        // Конструктор по умолчанию 
        public CharSet(int size = Constants.DEFAULT_MAX_SIZE)
        {
            maxSize = size;
            currentSize = 0;
            elements = new char[maxSize];
        }

        // Конструктор копирования (заменяет перегрузку оператора присваивания, недоступную в C#)
        public CharSet(CharSet other)
        {
            this.maxSize = other.maxSize;
            this.currentSize = other.currentSize;
            this.elements = new char[this.maxSize];
            for (int i = 0; i < this.currentSize; i++)
            {
                this.elements[i] = other.elements[i];
            }
        }

        // Деструктор
        ~CharSet()
        {
            // В C# сборщик мусора сам очищает память массива, но для выполнения требований лабы
            // мы добавили финализатор.
            elements = null;
        }

        // Вспомогательный метод: проверка наличия элемента в множестве
        private bool Contains(char c)
        {
            for (int i = 0; i < currentSize; i++)
            {
                if (elements[i] == c) return true;
            }
            return false;
        }

        // Вспомогательный метод: добавление элемента (без проверки на дубликаты для внутреннего использования)
        private void AddInternal(char c)
        {
            if (currentSize < maxSize)
            {
                if (!Contains(c))
                {
                    elements[currentSize] = c;
                    currentSize++;
                }
            }
            else
            {
                Console.WriteLine("Ошибка: Множество переполнено!");
            }
        }

        // Метод ввода
        public void Input()
        {
            Console.WriteLine("Введите символы множества подряд (например, abcde).");
            string input = Console.ReadLine();
            if (input != null)
            {
                foreach (char c in input)
                {
                    AddInternal(c);
                }
            }
        }

        // Метод вывода
        public void Print()
        {
            Console.Write("{ ");
            for (int i = 0; i < currentSize; i++)
            {
                Console.Write("'" + elements[i] + "'");
                if (i < currentSize - 1) Console.Write(", ");
            }
            Console.WriteLine(" }");
        }

        // Перегрузка операции: char + set
        public static CharSet operator +(char c, CharSet set)
        {
            // Создаем новый объект с помощью конструктора копирования
            CharSet result = new CharSet(set);
            result.AddInternal(c);
            return result;
        }

        // Перегрузка операции: set + set (Объединение множеств)
        public static CharSet operator +(CharSet s1, CharSet s2)
        {
            CharSet result = new CharSet(s1.maxSize + s2.maxSize);
            // Копируем первый
            for (int i = 0; i < s1.currentSize; i++)
            {
                result.AddInternal(s1.elements[i]);
            }
            // Копируем второй (дубликаты не добавятся благодаря Contains в AddInternal)
            for (int i = 0; i < s2.currentSize; i++)
            {
                result.AddInternal(s2.elements[i]);
            }
            return result;
        }

        // Перегрузка операции: проверка на равенство (set == set)
        public static bool operator ==(CharSet s1, CharSet s2)
        {
            // Проверка на null (если объекты ссылочного типа сравниваются с null)
            if (ReferenceEquals(s1, null) && ReferenceEquals(s2, null)) return true;
            if (ReferenceEquals(s1, null) || ReferenceEquals(s2, null)) return false;

            if (s1.currentSize != s2.currentSize) return false;

            // Множества равны, если каждый элемент одного есть в другом
            for (int i = 0; i < s1.currentSize; i++)
            {
                if (!s2.Contains(s1.elements[i])) return false;
            }
            return true;
        }

        // Перегрузка операции: проверка на неравенство (обязательно в C# при перегрузке ==)
        public static bool operator !=(CharSet s1, CharSet s2)
        {
            return !(s1 == s2);
        }

        // Переопределение Equals и GetHashCode (рекомендуется в C# при перегрузке ==)
        public override bool Equals(object obj)
        {
            CharSet other = obj as CharSet;
            if (other == null) return false;
            return this == other;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            for (int i = 0; i < currentSize; i++)
            {
                hash = hash * 23 + elements[i].GetHashCode();
            }
            return hash;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Лабораторная работа №4. Вариант 1 (АТД: Множество Char) ---\n");

            CharSet set1 = new CharSet();
            CharSet set2 = new CharSet();

            while (true)
            {
                Console.WriteLine("\n=== МЕНЮ ===");
                Console.WriteLine("1. Ввести Множество 1 (set1)");
                Console.WriteLine("2. Ввести Множество 2 (set2)");
                Console.WriteLine("3. Вывести множества");
                Console.WriteLine("4. Операция: char + set1");
                Console.WriteLine("5. Операция: set1 + set2 (объединение)");
                Console.WriteLine("6. Проверка равенства: set1 == set2");
                Console.WriteLine("7. Проверка конструктора копирования (имитация присваивания)");
                Console.WriteLine("0. Выход");
                Console.Write("Ваш выбор: ");

                string choice = Console.ReadLine();
                if (choice == "0") break;

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Ввод Множества 1:");
                        set1.Input();
                        break;
                    case "2":
                        Console.WriteLine("Ввод Множества 2:");
                        set2.Input();
                        break;
                    case "3":
                        Console.Write("Множество 1: ");
                        set1.Print();
                        Console.Write("Множество 2: ");
                        set2.Print();
                        break;
                    case "4":
                        Console.Write("Введите один символ для добавления в set1: ");
                        string s = Console.ReadLine();
                        if (!string.IsNullOrEmpty(s))
                        {
                            char c = s[0];
                            set1 = c+ set1;
                            Console.Write("Результат (c + set1): ");
                            set1.Print();
                        }
                        break;
                    case "5":
                        CharSet union = set1 + set2;
                        Console.Write("Результат объединения (set1 + set2): ");
                        union.Print();
                        break;
                    case "6":
                        bool areEqual = (set1 == set2);
                        if (areEqual)
                            Console.WriteLine("Результат: Множества РАВНЫ (set1 == set2).");
                        else
                            Console.WriteLine("Результат: Множества НЕ РАВНЫ (set1 != set2).");
                        break;
                    case "7":
                        Console.WriteLine("Создаем копию Множества 1...");
                        CharSet copy = new CharSet(set1);
                        Console.Write("Копия: ");
                        copy.Print();
                        Console.WriteLine("Добавляем символ 'Z' в копию (не должно повлиять на оригинал)...");
                        copy = 'Z' + copy;
                        Console.Write("Оригинал: ");
                        set1.Print();
                        Console.Write("Измененная копия: ");
                        copy.Print();
                        break;
                    default:
                        Console.WriteLine("Неверный выбор.");
                        break;
                }
            }
        }
    }
}