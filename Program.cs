using System;
using System.IO;
using System.Text;

public class TextViewer
{
    // Метод для зміни кольору тексту у консолі
    public static void ChangeColor(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ResetColor();
    }
}

class Program
{
    // Функція для обчислення значення підінтегральної функції
    static double Function(double x)
    {
        return Math.Cos(0.07 * 7 + 0.5 * x) / (0.4 + Math.Sqrt(Math.Pow(x, 2) + 7));
    }

    // Метод лівих прямокутників
    static double LeftRectangleMethod(double a, double b, int n)
    {
        double h = (b - a) / n;
        double sum = 0;
        for (int i = 0; i < n; i++)
        {
            sum += Function(a + i * h);
        }
        return h * sum;
    }

    // Метод правих прямокутників
    static double RightRectangleMethod(double a, double b, int n)
    {
        double h = (b - a) / n;
        double sum = 0;
        for (int i = 1; i <= n; i++)
        {
            sum += Function(a + i * h);
        }
        return h * sum;
    }

    // Метод прямокутників посередині
    static double MidpointRectangleMethod(double a, double b, int n)
    {
        double h = (b - a) / n;
        double sum = 0;
        for (int i = 0; i < n; i++)
        {
            sum += Function(a + (i + 0.5) * h);
        }
        return h * sum;
    }

    // Метод трапецій
    static double TrapezoidalMethod(double a, double b, int n)
    {
        double h = (b - a) / n;
        double sum = (Function(a) + Function(b)) / 2;
        for (int i = 1; i < n; i++)
        {
            sum += Function(a + i * h);
        }
        return h * sum;
    }

    // Метод Сімпсона
    static double SimpsonMethod(double a, double b, int n)
    {
        if (n % 2 != 0)
        {
            throw new ArgumentException("Кількість інтервалів повинна бути парна.");
        }

        double h = (b - a) / n;
        double sum = Function(a) + Function(b);
        for (int i = 1; i < n; i++)
        {
            double x = a + i * h;
            sum += 2 * (i % 2 == 0 ? 2 : 1) * Function(x);
        }
        return (h / 3) * sum;
    }

    // Збереження результатів в файл
    static void SaveResults(string fileName, string[] results)
    {
        File.WriteAllLines(fileName, results);
    }

    // Обчислення аналітичного значення методом Ньютона-Лейбница
    static double AnalyticalSolution(double a, double b)
    {
        // Первинна функція
        double F(double x)
        {
            return Math.Log(0.4 + Math.Sqrt(Math.Pow(x, 2) + 7)) / 0.5;
        }

        return F(b) - F(a);
    }

    // Валідація вводу чисел
    static double ValidateInput(string prompt)
    {
        double result;
        while (true)
        {
            Console.WriteLine(prompt);
            if (double.TryParse(Console.ReadLine(), out result))
            {
                return result;
            }
            else
            {
                TextViewer.ChangeColor("ПОМИЛКА: Некоректне введення. Введіть коректне число.", ConsoleColor.Red);
            }
        }
    }

    // Валідація вводу цілих чисел
    static int ValidateIntegerInput(string prompt)
    {
        int result;
        while (true)
        {
            Console.WriteLine(prompt);
            if (int.TryParse(Console.ReadLine(), out result))
            {
                return result;
            }
            else
            {
                TextViewer.ChangeColor("ПОМИЛКА: Некоректне введення. Введіть коректне ціле число.", ConsoleColor.Red);
            }
        }
    }

    static void Main()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.OutputEncoding = Encoding.Unicode;

        string fileName = $"Results_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt";

        while (true)
        {
            Console.WriteLine("\nВиберіть дію:");
            TextViewer.ChangeColor("\t1 - Вибрати метод інтегрування", ConsoleColor.Cyan);
            TextViewer.ChangeColor("\t0 - Вийти з програми", ConsoleColor.Red);
            int menuChoice = ValidateIntegerInput("Введіть номер дії:");

            if (menuChoice == 0)
            {
                break;
            }

            while (true)
            {
                Console.WriteLine("\nВиберіть метод інтегрування:");
                TextViewer.ChangeColor("\t1 - Ліві прямокутники", ConsoleColor.Cyan);
                TextViewer.ChangeColor("\t2 - Праві прямокутники", ConsoleColor.Cyan);
                TextViewer.ChangeColor("\t3 - Прямокутники посередині", ConsoleColor.Cyan);
                TextViewer.ChangeColor("\t4 - Трапеції", ConsoleColor.Cyan);
                TextViewer.ChangeColor("\t5 - Метод Сімпсона", ConsoleColor.Cyan);
                TextViewer.ChangeColor("\t0 - Повернутися в головне меню", ConsoleColor.Red);
                int methodChoice = ValidateIntegerInput("Введіть номер методу:");

                if (methodChoice == 0)
                {
                    break;
                }

                Console.WriteLine("\nВведіть спосіб введення даних:");
                TextViewer.ChangeColor("\t1 - Вручну", ConsoleColor.Cyan);
                TextViewer.ChangeColor("\t2 - З файлу", ConsoleColor.Cyan);
                TextViewer.ChangeColor("\t0 - Повернутися в головне меню", ConsoleColor.Red);
                int inputChoice = ValidateIntegerInput("Введіть номер способу:");

                if (inputChoice == 0)
                {
                    break;
                }

                double a, b;
                int n = 1;
                double E = 1e-3;
                double result;
                string[] outputLines = new string[1000];
                int lineIndex = 0;

                if (inputChoice == 1)
                {
                    a = ValidateInput("\nВведіть початкову межу інтегрування a:");
                    b = ValidateInput("\nВведіть кінцеву межу інтегрування b:");
                }
                else
                {
                    Console.WriteLine("\nВведіть шлях до файлу:");
                    string filePath = Console.ReadLine();
                    string[] lines = File.ReadAllLines(filePath);
                    a = double.Parse(lines[0]);
                    b = double.Parse(lines[1]);
                }

                outputLines[lineIndex++] = "Метод\t\t\tАпроксимація\t\tПохибка";
                outputLines[lineIndex++] = "--------------------------------------------------";

                int counter = 1;
                switch (methodChoice)
                {
                    case 1:
                        do
                        {
                            var watch = System.Diagnostics.Stopwatch.StartNew();
                            result = LeftRectangleMethod(a, b, n);
                            watch.Stop();
                            TextViewer.ChangeColor($"{counter} - Ліві прямокутники (n = {n}): {result}, Час виконання: {watch.ElapsedMilliseconds} мс", ConsoleColor.Magenta);
                            outputLines[lineIndex++] = $"{counter} - Ліві прямокутники\t\t{result}\t\t{Math.Abs(result)}\t\t{watch.ElapsedMilliseconds} мс";
                            n *= 2;
                            counter++;
                        } while (Math.Abs(result) > E);
                        break;
                    case 2:
                        do
                        {
                            var watch = System.Diagnostics.Stopwatch.StartNew();
                            result = RightRectangleMethod(a, b, n);
                            watch.Stop();
                            TextViewer.ChangeColor($"{counter} - Праві прямокутники (n = {n}): {result}, Час виконання: {watch.ElapsedMilliseconds} мс", ConsoleColor.Magenta);
                            outputLines[lineIndex++] = $"{counter} - Праві прямокутники\t\t{result}\t\t{Math.Abs(result)}\t\t{watch.ElapsedMilliseconds} мс";
                            n *= 2;
                        } while (Math.Abs(result) > E);
                        break;
                    case 3:
                        do
                        {
                            var watch = System.Diagnostics.Stopwatch.StartNew();
                            result = MidpointRectangleMethod(a, b, n);
                            watch.Stop();
                            TextViewer.ChangeColor($"{counter} - Прямокутники посередині (n = {n}): {result}, Час виконання: {watch.ElapsedMilliseconds} мс", ConsoleColor.Magenta);
                            outputLines[lineIndex++] = $"{counter} - Прямокутники посередині\t{result}\t\t{Math.Abs(result)}\t\t{watch.ElapsedMilliseconds} мс";
                            n *= 2;
                        } while (Math.Abs(result) > E);
                        break;
                    case 4:
                        do
                        {
                            var watch = System.Diagnostics.Stopwatch.StartNew();
                            result = TrapezoidalMethod(a, b, n);
                            watch.Stop();
                            TextViewer.ChangeColor($"{counter} - Трапеції (n = {n}): {result}, Час виконання: {watch.ElapsedMilliseconds} мс", ConsoleColor.Magenta);
                            outputLines[lineIndex++] = $"{counter} - Трапеції\t\t{result}\t\t{Math.Abs(result)}\t\t{watch.ElapsedMilliseconds} мс";
                            n *= 2;
                        } while (Math.Abs(result) > E);
                        break;
                    case 5:
                        do
                        {
                            var watch = System.Diagnostics.Stopwatch.StartNew();
                            result = SimpsonMethod(a, b, n);
                            watch.Stop();
                            TextViewer.ChangeColor($"{counter} - Метод Сімпсона (n = {n}): {result}, Час виконання: {watch.ElapsedMilliseconds} мс", ConsoleColor.Magenta);
                            outputLines[lineIndex++] = $"{counter} - Метод Сімпсона\t\t{result}\t\t{Math.Abs(result)}\t\t{watch.ElapsedMilliseconds} мс";
                            n *= 2;
                        } while (Math.Abs(result) > E);
                        break;
                    default:
                        TextViewer.ChangeColor("ПОМИЛКА: Невірний вибір методу. Повторіть спробу.", ConsoleColor.Red);
                        break;
                }

                outputLines[lineIndex++] = $"Обраний метод: {methodChoice}";
                outputLines[lineIndex++] = $"Тип вводу: {(inputChoice == 1 ? "Вручну" : "З файлу")}";
                outputLines[lineIndex++] = $"Границі: [{a};{b}]";

                double analyticalResult = AnalyticalSolution(a, b);
                TextViewer.ChangeColor($"Аналітичне значення: {analyticalResult}", ConsoleColor.Yellow);
                outputLines[lineIndex++] = $"Аналітичне значення\t{analyticalResult}\t\t0";

                Array.Resize(ref outputLines, lineIndex);

                SaveResults(fileName, outputLines);
                TextViewer.ChangeColor($"Результати збережено в файлі {fileName}", ConsoleColor.Yellow);
            }
        }
    }
}