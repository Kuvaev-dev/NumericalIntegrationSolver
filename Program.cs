using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace IntegralCalculator
{
    public class TextViewer
    {
        // Метод для зміни кольору тексту у консолі
        public static void ChangeColor(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Green;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.OutputEncoding = Encoding.Unicode;

            while (true) // Головний цикл програми
            {
                // Виведення меню для вибору методу обчислення інтеграла
                Console.WriteLine("Виберіть метод обчислення інтеграла:");
                TextViewer.ChangeColor("\n\t1. Ліві прямокутники", ConsoleColor.Cyan);
                TextViewer.ChangeColor("\t2. Праві прямокутники", ConsoleColor.Cyan);
                TextViewer.ChangeColor("\t3. Середні прямокутники", ConsoleColor.Cyan);
                TextViewer.ChangeColor("\t4. Трапеції", ConsoleColor.Cyan);
                TextViewer.ChangeColor("\t5. Сімпсон", ConsoleColor.Cyan);
                TextViewer.ChangeColor("\t6. Вихід\n", ConsoleColor.Red);
                Console.Write("Ваш вибір (1-6): ");

                int method;
                // Валідація введеного значення методу
                while (!int.TryParse(Console.ReadLine(), out method) || method < 1 || method > 6)
                {
                    TextViewer.ChangeColor("\nПОМИЛКА: Введіть коректне число від 1 до 6.", ConsoleColor.Red);
                }

                // Виведення меню для вибору методу введення даних
                Console.WriteLine("\nВиберіть метод введення даних:");
                TextViewer.ChangeColor("\n\t1. Вручну", ConsoleColor.Cyan);
                TextViewer.ChangeColor("\t2. З файлу\n", ConsoleColor.Cyan);
                Console.Write("Ваш вибір (1-2): ");

                int inputMethod;
                // Валідація введеного значення методу введення даних
                while (!int.TryParse(Console.ReadLine(), out inputMethod) || inputMethod < 1 || inputMethod > 2)
                {
                    TextViewer.ChangeColor("\nПОМИЛКА: Введіть коректне число від 1 до 2.", ConsoleColor.Red);
                }

                double a, b, n;
                if (inputMethod == 1) // Вручне введення даних
                {
                    // Введення початку інтервалу
                    Console.WriteLine("\nВведіть початок інтервалу (a):");
                    while (!double.TryParse(Console.ReadLine(), out a))
                    {
                        TextViewer.ChangeColor("\nПОМИЛКА: Введіть коректне число.", ConsoleColor.Red);
                    }

                    // Введення кінця інтервалу
                    Console.WriteLine("Введіть кінець інтервалу (b):");
                    while (!double.TryParse(Console.ReadLine(), out b) || b <= a)
                    {
                        TextViewer.ChangeColor("\nПОМИЛКА: Введіть коректне число, більше за a.", ConsoleColor.Cyan);
                    }

                    // Введення кількості розділень
                    Console.WriteLine("Введіть кількість розділень (n):");
                    while (!double.TryParse(Console.ReadLine(), out n) || n <= 0)
                    {
                        TextViewer.ChangeColor("\nПОМИЛКА: Введіть коректне число, більше за 0.", ConsoleColor.Red);
                    }
                }
                else // Введення даних з файлу
                {
                    // Введення шляху до файлу
                    Console.WriteLine("Введіть шлях до файлу:");
                    string filePath = Console.ReadLine();
                    while (!File.Exists(filePath))
                    {
                        TextViewer.ChangeColor("\nПОМИЛКА: Файл не існує. Введіть коректний шлях до файлу:", ConsoleColor.Red);
                        filePath = Console.ReadLine();
                    }

                    var lines = File.ReadAllLines(filePath);
                    a = double.Parse(lines[0]);
                    b = double.Parse(lines[1]);
                    n = double.Parse(lines[2]);

                    Console.WriteLine($"a = {a}, b = {b}, n = {n}");
                }

                // Ініціалізація та запуск таймера
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                double result = 0;

                TextViewer.ChangeColor("\nРозв'язання\n", ConsoleColor.Cyan);
                // Вибір методу обчислення
                switch (method)
                {
                    case 1:
                        Console.WriteLine();
                        result = LeftRectangles(a, b, n);
                        break;
                    case 2:
                        Console.WriteLine();
                        result = RightRectangles(a, b, n);
                        break;
                    case 3:
                        Console.WriteLine();
                        result = MidpointRectangles(a, b, n);
                        break;
                    case 4:
                        Console.WriteLine();
                        result = Trapezoid(a, b, n);
                        break;
                    case 5:
                        Console.WriteLine();
                        result = Simpson(a, b, n);
                        break;
                    case 6:
                        return;
                }

                // Зупинка таймера
                stopwatch.Stop();

                // Виведення результату та часу обчислення
                TextViewer.ChangeColor($"\nРезультат: {result}", ConsoleColor.Yellow);
                TextViewer.ChangeColor($"\nЧас обчислення: {stopwatch.ElapsedMilliseconds} мс", ConsoleColor.Cyan);

                // Збереження результатів у файл
                SaveToFile(a, b, n, result, stopwatch.ElapsedMilliseconds, method);
                Console.ReadKey();
                Console.Clear();
            }
        }

        // Функція для обчислення значення функції
        static double Function(double x, double N)
        {
            return Math.Cos(0.07 * N + 0.5 * x) / (0.4 + Math.Sqrt(Math.Pow(x, 2) + N));
        }

        // Методи для обчислення інтеграла з різними методами

        // Метод лівих прямокутників
        static double LeftRectangles(double a, double b, double n)
        {
            double h = (b - a) / n;
            double sum = 0;

            for (double i = a; i < b; i += h)
            {
                double f = Function(i, n);
                sum += f;
                Console.WriteLine($"f({i}) = {f} * {h}");
            }

            double error = Math.Abs((b - a) * h * Function(a, n));
            TextViewer.ChangeColor($"\nПогрішність: {error}", ConsoleColor.DarkMagenta);

            return sum * h;
        }

        // Метод правих прямокутників
        static double RightRectangles(double a, double b, double n)
        {
            double h = (b - a) / n;
            double sum = 0;

            for (double i = a + h; i <= b; i += h)
            {
                double f = Function(i, n);
                sum += f;
                Console.WriteLine($"f({i}) = {f} * {h}");
            }

            double error = Math.Abs((b - a) * h * Function(b, n));
            TextViewer.ChangeColor($"\nПогрішність: {error}", ConsoleColor.DarkMagenta);

            return sum * h;
        }

        // Метод середніх прямокутників
        static double MidpointRectangles(double a, double b, double n)
        {
            double h = (b - a) / n;
            double sum = 0;

            for (double i = a + h / 2; i < b; i += h)
            {
                double f = Function(i, n);
                sum += f;
                Console.WriteLine($"f({i}) = {f} * {h}");
            }

            double error = Math.Abs((b - a) * h * Function((a + b) / 2, n));
            TextViewer.ChangeColor($"\nПогрішність: {error}", ConsoleColor.DarkMagenta);

            return sum * h;
        }

        // Метод трапецій
        static double Trapezoid(double a, double b, double n)
        {
            double h = (b - a) / n;
            double sum = 0;

            for (int i = 1; i < n; i++)
            {
                double f = Function(a + i * h, n);
                sum += f;
                Console.WriteLine($"f({a + i * h}) = {f} * {h}");
            }

            double error = Math.Abs((b - a) / (2 * n) * (Function(a, n) + 2 * sum + Function(b, n)));
            TextViewer.ChangeColor($"\nПогрішність: {error}", ConsoleColor.DarkMagenta);

            return h * (0.5 * (Function(a, n) + Function(b, n)) + sum);
        }

        // Метод Сімпсона
        static double Simpson(double a, double b, double n)
        {
            if (n % 2 != 0)
            {
                throw new ArgumentException("Кількість розділень повинна бути парним числом.");
            }

            double h = (b - a) / n;
            double sum1 = 0;
            double sum2 = 0;

            for (int i = 1; i < n; i += 2)
            {
                double f = Function(a + i * h, n);
                sum1 += f;
                Console.WriteLine($"f({a + i * h}) = {f} * {h}");
            }

            for (int i = 2; i < n - 1; i += 2)
            {
                double f = Function(a + i * h, n);
                sum2 += f;
                Console.WriteLine($"f({a + i * h}) = {f} * {h}");
            }

            double error = Math.Abs((b - a) / (3 * n) * (Function(a, n) + 4 * sum1 + 2 * sum2 + Function(b, n)));
            TextViewer.ChangeColor($"\nПогрішність: {error}", ConsoleColor.DarkMagenta);

            return h / 3 * (Function(a, n) + 4 * sum1 + 2 * sum2 + Function(b, n));
        }

        // Метод для збереження результатів у файл
        static void SaveToFile(double a, double b, double n, double result, long elapsedTime, int method)
        {
            string fileName = $"Result_{DateTime.Now:yyyyMMddHHmmss}.txt";

            using (StreamWriter writer = new StreamWriter(fileName, true))
            {
                writer.WriteLine($"Метод: {method}");
                writer.WriteLine($"a = {a}, b = {b}, n = {n}");
                writer.WriteLine($"Результат: {result}");
                writer.WriteLine($"Час обчислення: {elapsedTime} мс");
                writer.WriteLine(new string('-', 50));
            }

            Console.WriteLine($"\nРезультати збережено у файл {fileName}\n");
        }
    }
}