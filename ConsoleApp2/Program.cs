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
                    case "3": // РЕДАКТИРОВАНИЕ
                        Console.Write("ID для правки: ");
                        if (!int.TryParse(Console.ReadLine(), out int eid)) break;

                        var curr = db.GetTable($"SELECT title, studio_id, budget_mln FROM movie WHERE movie_id={eid}");
                        if (curr.Count == 0) break;

                        // Показываем текущее значение, Enter — оставляем как было
                        Console.Write($"Новое название [{curr[0][0]}]: ");
                        string nt = Console.ReadLine();
                        nt = string.IsNullOrEmpty(nt) ? curr[0][0] : nt;

                        db.UpdateMovie(eid, nt, int.Parse(curr[0][1]), int.Parse(curr[0][2]));
                        Console.WriteLine("Данные обновлены.");
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