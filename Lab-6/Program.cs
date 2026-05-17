using System;

namespace lab_6
{
    // Пользовательское исключение
    public class UseException : Exception
    {
        public UseException() : base("Произошла пользовательская ошибка.") { }

        public UseException(string message) : base(message) { }
    }

    // Класс Треугольник
    class Triangle
    {
        public double A { get; }
        public double B { get; }
        public double C { get; }

        // Конструктор класса
        public Triangle(double a, double b, double c)
        {
            // Проверка на положительные стороны при создании объекта
            if (a <= 0 || b <= 0 || c <= 0)
            {
                throw new UseException($"Стороны треугольника должны быть строго положительными. Передано: a={a}, b={b}, c={c}");
            }

            // Проверка неравенства треугольника при создании объекта
            if (a + b <= c || a + c <= b || b + c <= a)
            {
                throw new UseException($"Треугольник с такими сторонами не существует. Передано: a={a}, b={b}, c={c}");
            }

            A = a;
            B = b;
            C = c;
        }

        // Метод для вычисления площади
        public double CalculateArea()
        {
            double p = (A + B + C) / 2.0;
            return Math.Sqrt(p * (p - A) * (p - B) * (p - C));
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.Write("Введите длину стороны a: ");
                double userA = ParseInput(Console.ReadLine());

                Console.Write("Введите длину стороны b: ");
                double userB = ParseInput(Console.ReadLine());

                Console.Write("Введите длину стороны c: ");
                double userC = ParseInput(Console.ReadLine());

                // Создание пользовательского объекта
                Triangle userTriangle = new Triangle(userA, userB, userC);
                Console.WriteLine($"\nПлощадь треугольника S = {userTriangle.CalculateArea():F4}");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"\n[FormatException] Ошибка формата (Вводите только ЦЕЛЫЕ ЧИСЛА).\n{ex.Message}");
            }
            catch (OverflowException ex)
            {
                Console.WriteLine($"\n[OverflowException] Ошибка переполнения: Введенное число слишком большое или маленькое.\n{ex.Message}");
            }
            catch (UseException ex)
            {
                Console.WriteLine($"\n[UseException] Логическая ошибка: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[Exception] Непредвиденная ошибка: {ex.Message}");
            }
        }

        // Вспомогательный метод для более строгого парсинга (чтобы продемонстрировать OverflowException)
        static double ParseInput(string input)
        {
            decimal value = decimal.Parse(input);
            return (double)value;
        }
    }
}
