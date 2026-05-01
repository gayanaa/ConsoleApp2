using System;

namespace Homework2;

public class Movie
{
    public int Id { get; set; }
    public int StudioId { get; set; }
    public string Title { get; set; }
    public int BudgetMln { get; set; }
    public string StudioName { get; set; }

    public Movie(int id, int studioId, string title, int budgetMln, string studioName)
    {
        Id = id;
        StudioId = studioId;
        Title = title;
        BudgetMln = budgetMln;
        StudioName = studioName;
    }

    public Movie() : this(0, 0, "Неизвестно", 0, "Нет данных") { }

    public override string ToString() =>
        $"│ {Id,-3} │ {Title,-20} │ {StudioName,-18} │ {BudgetMln + " млн$",-10} │";
}

