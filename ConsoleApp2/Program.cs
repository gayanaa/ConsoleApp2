using System;
using System.Collections.Generic;
using System.IO;

namespace Homework2;

class Program
{
    static void Main(string[] args)
    {
        DatabaseManager db = new DatabaseManager();
        db.InitializeDatabase();

        // Создаем тестовые файлы CSV для первой проверки
        File.WriteAllLines("studios.csv", new[] { "1;Warner Bros.", "2;Universal" });
        File.WriteAllLines("movies.csv", new[] { "1;1;Inception;160", "2;2;Jurassic Park;63" });

        while (true)
        {
            Console.WriteLine("\n--- МЕНЮ (Вариант 2, Группа 3) ---");
            Console.WriteLine("1. Импорт из CSV");
            Console.WriteLine("2. Показать фильмы");
            Console.WriteLine("3. Отчет со статистикой (Группа 3)");
            Console.WriteLine("0. Выход");
            Console.Write("Выбор: ");

            string choice = Console.ReadLine();
            if (choice == "0") break;

            switch (choice)
            {
                case "1":
                    db.ImportFromCsv("studios.csv", "movies.csv");
                    Console.WriteLine("Данные загружены!");
                    break;
                case "2":
                    var movies = db.GetAllMovies();
                    foreach (var m in movies) Console.WriteLine(m);
                    break;
                case "3":
                    var data = db.GetAllMovies();
                    string report = new ReportBuilder(data)
                        .AddHeader("Киностудии и фильмы")
                        .AddContent()
                        .AddStatistics()
                        .Build();
                    Console.WriteLine(report);
                    break;
            }
        }
    }
}