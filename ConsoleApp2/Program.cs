using System;
using System.Collections.Generic;

namespace Homework2;

class Program
{
    static void Main()
    {
        var db = new DatabaseManager("cinema.db");

        while (true)
        {
            Console.WriteLine("\n=================================================");
            Console.WriteLine(" [1] Список | [2] Добавить | [3] Изменить ");
            Console.WriteLine(" [4] Удалить | [5] Отчёты   | [0] Выход");
            Console.WriteLine("=================================================");
            Console.Write(" Выберите действие: ");
            var choice = Console.ReadLine();
            if (choice == "0") break;

            try
            {
                switch (choice)
                {
                    case "1":
                        new ReportBuilder(db).SetTitle("СПИСОК ФИЛЬМОВ")
                            .SetHeaders("ID", "Название", "Студия", "Бюджет")
                            .SetQuery("SELECT m.movie_id, m.title, s.name, m.budget_mln FROM movie m JOIN studio s ON m.studio_id = s.studio_id")
                            .Footer("Всего записей")
                            .Print();
                        break;
                    case "2":
                        // 1. Сначала показываем справочник студий (требование задания)
                        Console.WriteLine("\nСПРАВОЧНИК СТУДИЙ:");
                        db.GetTable("SELECT * FROM studio").ForEach(s => Console.WriteLine($"[{s[0]}] {s[1]}"));

                        // 2. Запрашиваем название
                        Console.Write("Название фильма: ");
                        string t = Console.ReadLine();

                        // 3. Запрашиваем ID студии с проверкой (TryParse)
                        Console.Write("ID студии: ");
                        if (!int.TryParse(Console.ReadLine(), out int sid))
                        {
                            Console.WriteLine("Ошибка: ID студии должен быть числом!");
                            break;
                        }

                        // 4. ДОБАВЛЕНО: Запрос бюджета с проверкой (TryParse)
                        Console.Write("Бюджет (млн$): ");
                        if (!int.TryParse(Console.ReadLine(), out int b))
                        {
                            Console.WriteLine("Ошибка: Бюджет должен быть целым числом!");
                            break;
                        }

                        // 5. Сохранение в базу
                        db.AddMovie(t, sid, b);
                        Console.WriteLine(">> Фильм успешно добавлен.");
                        break;
                    case "3":
                        // 1. Показываем текущий список, чтобы пользователь видел ID
                        new ReportBuilder(db).SetTitle("РЕДАКТИРОВАНИЕ (ВЫБЕРИТЕ ID)")
                            .SetQuery("SELECT m.movie_id, m.title, s.name, m.budget_mln FROM movie m JOIN studio s ON m.studio_id = s.studio_id")
                            .Print();

                        Console.Write("\nВведите ID фильма для изменения: ");
                        if (!int.TryParse(Console.ReadLine(), out int eid)) break;

                        // 2. Извлекаем текущие данные из БД
                        var curr = db.GetTable($"SELECT title, studio_id, budget_mln FROM movie WHERE movie_id={eid}");
                        if (curr.Count == 0) { Console.WriteLine("Фильм не найден."); break; }

                        string oldTitle = curr[0][0];
                        string oldStudioId = curr[0][1];
                        string oldBudget = curr[0][2];

                        // 3. Редактируем Название
                        Console.Write($"Новое название [{oldTitle}]: ");
                        string nt = Console.ReadLine();
                        if (string.IsNullOrEmpty(nt)) nt = oldTitle;

                        // 4. Редактируем ID студии
                        Console.WriteLine("Справочник студий:");
                        db.GetTable("SELECT * FROM studio").ForEach(s => Console.WriteLine($"[{s[0]}] {s[1]}"));
                        Console.Write($"Новый ID студии [{oldStudioId}]: ");
                        string nsInput = Console.ReadLine();
                        int ns = string.IsNullOrEmpty(nsInput) ? int.Parse(oldStudioId) : (int.TryParse(nsInput, out int resS) ? resS : int.Parse(oldStudioId));

                        // 5. Редактируем Бюджет
                        Console.Write($"Новый бюджет [{oldBudget}]: ");
                        string nbInput = Console.ReadLine();
                        int nb = string.IsNullOrEmpty(nbInput) ? int.Parse(oldBudget) : (int.TryParse(nbInput, out int resB) ? resB : int.Parse(oldBudget));

                        // 6. Сохраняем изменения в БД
                        db.UpdateMovie(eid, nt, ns, nb);
                        Console.WriteLine(">> Изменения сохранены.");
                        break;
                    case "4":
                        Console.Write("ID для удаления: ");
                        if (int.TryParse(Console.ReadLine(), out int did)) db.DeleteMovie(did);
                        break;
                    case "5": // ТРИ ОТЧЕТА
                        Console.WriteLine("\n1. По алфавиту | 2. Кол-во по студиям | 3. Средний бюджет");
                        var rC = Console.ReadLine();
                        var rb = new ReportBuilder(db);
                        if (rC == "1") rb.SetTitle("ОТЧЕТ 1").SetHeaders("Фильм", "Студия").SetQuery("SELECT m.title, s.name FROM movie m JOIN studio s ON m.studio_id = s.studio_id ORDER BY m.title").Footer("Итого").Print();
                        if (rC == "2") rb.SetTitle("ОТЧЕТ 2").SetHeaders("Студия", "Кол-во").SetQuery("SELECT s.name, COUNT(*) FROM movie m JOIN studio s ON m.studio_id = s.studio_id GROUP BY s.name").Print();
                        if (rC == "3") rb.SetTitle("ОТЧЕТ 3").SetHeaders("Студия", "Средний $").SetQuery("SELECT s.name, AVG(m.budget_mln) FROM movie m JOIN studio s ON m.studio_id = s.studio_id GROUP BY s.name").Print();
                        break;
                }
            }
            catch (Exception ex) { Console.WriteLine($"Ошибка: {ex.Message}"); }
        }
    }
}