using System;
using System.IO;
using System.Text;

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
                Console.WriteLine("Введіть коректне число.");
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
                Console.WriteLine("Введіть коректне ціле число.");
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
            Console.WriteLine("Виберіть дію:");
            Console.WriteLine("1 - Вибрати метод інтегрування");
            Console.WriteLine("0 - Вийти з програми");
            int menuChoice = ValidateIntegerInput("Введіть номер дії:");

            if (menuChoice == 0)
            {
                break;
            }

            while (true)
            {
                Console.WriteLine("Виберіть метод інтегрування:");
                Console.WriteLine("1 - Ліві прямокутники");
                Console.WriteLine("2 - Праві прямокутники");
                Console.WriteLine("3 - Прямокутники посередині");
                Console.WriteLine("4 - Трапеції");
                Console.WriteLine("5 - Метод Сімпсона");
                Console.WriteLine("0 - Повернутися в головне меню");
                int methodChoice = ValidateIntegerInput("Введіть номер методу:");

                if (methodChoice == 0)
                {
                    break;
                }

                Console.WriteLine("Введіть спосіб введення даних:");
                Console.WriteLine("1 - Вручну");
                Console.WriteLine("2 - З файлу");
                Console.WriteLine("0 - Повернутися в головне меню");
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
                    a = ValidateInput("Введіть початкову межу інтегрування a:");
                    b = ValidateInput("Введіть кінцеву межу інтегрування b:");
                }
                else
                {
                    Console.WriteLine("Введіть шлях до файлу:");
                    string filePath = Console.ReadLine();
                    string[] lines = File.ReadAllLines(filePath);
                    a = double.Parse(lines[0]);
                    b = double.Parse(lines[1]);
                }

                outputLines[lineIndex++] = "Метод\t\t\tАпроксимація\t\tПохибка";
                outputLines[lineIndex++] = "--------------------------------------------------";

                switch (methodChoice)
                {
                    case 1:
                        do
                        {
                            var watch = System.Diagnostics.Stopwatch.StartNew();
                            result = LeftRectangleMethod(a, b, n);
                            watch.Stop();
                            Console.WriteLine($"Ліві прямокутники (n = {n}): {result}, Час виконання: {watch.ElapsedMilliseconds} мс");
                            outputLines[lineIndex++] = $"Ліві прямокутники\t\t{result}\t\t{Math.Abs(result)}\t\t{watch.ElapsedMilliseconds} мс";
                            n *= 2;
                        } while (Math.Abs(result) > E);
                        break;
                    case 2:
                        do
                        {
                            var watch = System.Diagnostics.Stopwatch.StartNew();
                            result = RightRectangleMethod(a, b, n);
                            watch.Stop();
                            Console.WriteLine($"Праві прямокутники (n = {n}): {result}, Час виконання: {watch.ElapsedMilliseconds} мс");
                            outputLines[lineIndex++] = $"Праві прямокутники\t\t{result}\t\t{Math.Abs(result)}\t\t{watch.ElapsedMilliseconds} мс";
                            n *= 2;
                        } while (Math.Abs(result) > E);
                        break;
                    case 3:
                        do
                        {
                            var watch = System.Diagnostics.Stopwatch.StartNew();
                            result = MidpointRectangleMethod(a, b, n);
                            watch.Stop();
                            Console.WriteLine($"Прямокутники посередині (n = {n}): {result}, Час виконання: {watch.ElapsedMilliseconds} мс");
                            outputLines[lineIndex++] = $"Прямокутники посередині\t{result}\t\t{Math.Abs(result)}\t\t{watch.ElapsedMilliseconds} мс";
                            n *= 2;
                        } while (Math.Abs(result) > E);
                        break;
                    case 4:
                        do
                        {
                            var watch = System.Diagnostics.Stopwatch.StartNew();
                            result = TrapezoidalMethod(a, b, n);
                            watch.Stop();
                            Console.WriteLine($"Трапеції (n = {n}): {result}, Час виконання: {watch.ElapsedMilliseconds} мс");
                            outputLines[lineIndex++] = $"Трапеції\t\t{result}\t\t{Math.Abs(result)}\t\t{watch.ElapsedMilliseconds} мс";
                            n *= 2;
                        } while (Math.Abs(result) > E);
                        break;
                    case 5:
                        do
                        {
                            var watch = System.Diagnostics.Stopwatch.StartNew();
                            result = SimpsonMethod(a, b, n);
                            watch.Stop();
                            Console.WriteLine($"Метод Сімпсона (n = {n}): {result}, Час виконання: {watch.ElapsedMilliseconds} мс");
                            outputLines[lineIndex++] = $"Метод Сімпсона\t\t{result}\t\t{Math.Abs(result)}\t\t{watch.ElapsedMilliseconds} мс";
                            n *= 2;
                        } while (Math.Abs(result) > E);
                        break;
                    default:
                        Console.WriteLine("Невірний вибір методу.");
                        break;
                }

                outputLines[lineIndex++] = $"Обраний метод: {methodChoice}";
                outputLines[lineIndex++] = $"Тип вводу: {(inputChoice == 1 ? "Вручну" : "З файлу")}";
                outputLines[lineIndex++] = $"Границі: [{a};{b}]";

                double analyticalResult = AnalyticalSolution(a, b);
                Console.WriteLine($"Аналітичне значення: {analyticalResult}");
                outputLines[lineIndex++] = $"Аналітичне значення\t{analyticalResult}\t\t0";

                Array.Resize(ref outputLines, lineIndex);

                SaveResults(fileName, outputLines);
                Console.WriteLine($"Результати збережено в файлі {fileName}");
            }
        }
    }
}