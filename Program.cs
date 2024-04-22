using System;
using System.Globalization;
using System.IO;

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

    static void Main()
    {
        // Встановлення культури для коректного форматування даних
        CultureInfo.CurrentCulture = new CultureInfo("uk-UA");

        // Формування імені файлу на основі поточної дати та часу
        string fileName = $"Results_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt";

        // Вибір методу інтегрування
        Console.WriteLine("Виберіть метод інтегрування:");
        Console.WriteLine("1 - Ліві прямокутники");
        Console.WriteLine("2 - Праві прямокутники");
        Console.WriteLine("3 - Прямокутники посередині");
        Console.WriteLine("4 - Трапеції");
        Console.WriteLine("5 - Метод Сімпсона");
        int methodChoice = int.Parse(Console.ReadLine());

        // Вибір способу введення даних
        Console.WriteLine("Введіть спосіб введення даних:");
        Console.WriteLine("1 - Вручну");
        Console.WriteLine("2 - З файлу");
        int inputChoice = int.Parse(Console.ReadLine());

        double a, b;
        int n = 1;
        double E = 1e-3;
        double result;
        string[] outputLines = new string[1000]; // Масив для збереження результатів

        int lineIndex = 0;

        // Введення даних вручну
        if (inputChoice == 1)
        {
            Console.WriteLine("Введіть початкову межу інтегрування a:");
            a = double.Parse(Console.ReadLine());

            Console.WriteLine("Введіть кінцеву межу інтегрування b:");
            b = double.Parse(Console.ReadLine());
        }
        // Зчитування даних з файлу
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

        // Вибір методу та обчислення результатів
        switch (methodChoice)
        {
            case 1:
                do
                {
                    result = LeftRectangleMethod(a, b, n);
                    outputLines[lineIndex++] = $"Ліві прямокутники\t\t{result}\t\t{Math.Abs(result)}";
                    n *= 2;
                } while (Math.Abs(result) > E);
                break;
            case 2:
                do
                {
                    result = RightRectangleMethod(a, b, n);
                    outputLines[lineIndex++] = $"Праві прямокутники\t\t{result}\t\t{Math.Abs(result)}";
                    n *= 2;
                } while (Math.Abs(result) > E);
                break;
            case 3:
                do
                {
                    result = MidpointRectangleMethod(a, b, n);
                    outputLines[lineIndex++] = $"Прямокутники посередині\t{result}\t\t{Math.Abs(result)}";
                    n *= 2;
                } while (Math.Abs(result) > E);
                break;
            case 4:
                do
                {
                    result = TrapezoidalMethod(a, b, n);
                    outputLines[lineIndex++] = $"Трапеції\t\t{result}\t\t{Math.Abs(result)}";
                    n *= 2;
                } while (Math.Abs(result) > E);
                break;
            case 5:
                do
                {
                    result = SimpsonMethod(a, b, n);
                    outputLines[lineIndex++] = $"Метод Сімпсона\t\t{result}\t\t{Math.Abs(result)}";
                    n *= 2;
                } while (Math.Abs(result) > E);
                break;
            default:
                Console.WriteLine("Невірний вибір методу.");
                break;
        }

        // Зменшення розміру масиву до кількості заповнених елементів
        Array.Resize(ref outputLines, lineIndex);

        // Збереження результатів в файл
        SaveResults(fileName, outputLines);
        Console.WriteLine($"Результати збережено в файлі {fileName}");
    }
}
