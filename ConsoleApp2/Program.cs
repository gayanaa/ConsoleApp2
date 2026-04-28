using System;
using System.IO;

namespace Homework2;

class Program
{
    static void Main()
    {
        var db = new DatabaseManager("cinema.db");

        // Подготовка файлов
        File.WriteAllLines("studios.csv", new[] { "id;name", "1;Warner Bros.", "2;Universal Pictures", "3;Paramount Pictures", "4;Walt Disney" });
        File.WriteAllLines("movies.csv", new[] {
            "id;sid;title;budget",
            "1;1;Начало;160", "2;1;Дюна;165", "3;1;Довод;205",
            "4;2;Парк Юрского периода;63", "5;2;Челюсти;9", "6;2;Инопланетянин;10",
            "7;3;Титаник;200", "8;3;Гладиатор;103",
            "9;4;Король Лев;45", "10;4;Холодное сердце;150"
        });

        while (true)
        {
            Console.WriteLine("\n--- УПРАВЛЕНИЕ КИНОТЕАТРОМ ---");
            Console.WriteLine("1. Импорт из CSV (ОЧИЩАЕТ БАЗУ!)");
            Console.WriteLine("2. Список всех фильмов");
            Console.WriteLine("3. Добавить новый фильм");
            Console.WriteLine("4. Удалить фильм по ID");
            Console.WriteLine("0. Выход");
            Console.Write("Выбор: ");

            var choice = Console.ReadLine(); // Устранено предупреждение CS8600
            if (string.IsNullOrEmpty(choice) || choice == "0") break;

            try
            {
                switch (choice)
                {
                    case "1":
                        db.ImportFromCsv("studios.csv", "movies.csv");
                        Console.WriteLine("База обновлена данными из CSV.");
                        break;
                    case "2":
                        db.GetAllMovies().ForEach(Console.WriteLine);
                        break;
                    case "3":
                        Console.Write("Название: "); string t = Console.ReadLine() ?? "";
                        Console.Write("ID студии (1-4): "); int sid = int.Parse(Console.ReadLine() ?? "1");
                        Console.Write("Бюджет (млн$): "); int b = int.Parse(Console.ReadLine() ?? "0");
                        db.AddMovie(new Movie(0, sid, t, b, ""));
                        Console.WriteLine("Фильм добавлен в базу данных!");
                        break;
                    case "4":
                        Console.Write("Введите ID для удаления: ");
                        db.DeleteMovie(int.Parse(Console.ReadLine() ?? "0"));
                        Console.WriteLine("Запись удалена.");
                        break;
                }
            }
            catch (Exception ex) { Console.WriteLine($"Ошибка: {ex.Message}"); }
        }
    }
}