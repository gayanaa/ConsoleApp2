using System;

namespace Homework2;

public class Movie
{
    public int Id { get; set; }
    public int StudioId { get; set; }
    public string Title { get; set; }
    public int BudgetMln { get; set; }

    public Movie(int id, int studioId, string title, int budgetMln)
    {
        Id = id;
        StudioId = studioId;
        Title = title;
        BudgetMln = budgetMln;
    }

    public Movie() : this(0, 0, "", 0) { }

    public override string ToString() => $"Фильм: {Title} | Бюджет: {BudgetMln} млн$ | Студия ID: {StudioId}";
}