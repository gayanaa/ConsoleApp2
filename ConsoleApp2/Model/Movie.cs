using System;

namespace Homework2;

/// <summary> Класс, представляющий сущность "Фильм". </summary>
public class Movie
{
    private int _budgetMln;

    /// <summary> Уникальный идентификатор фильма. </summary>
    public int Id { get; set; }

    /// <summary> ID связанной студии. </summary>
    public int StudioId { get; set; }

    /// <summary> Название фильма. </summary>
    public string Title { get; set; }

    /// <summary> Название студии. </summary>
    public string StudioName { get; set; }

    /// <summary> Бюджет фильма с валидацией. </summary>
    public int BudgetMln
    {
        get => _budgetMln;
        set
        {
            if (value < 0) throw new ArgumentException("Бюджет не может быть отрицательным!");
            _budgetMln = value;
        }
    }

    /// <summary> Полный конструктор. </summary>
    public Movie(int id, int studioId, string title, int budgetMln, string studioName)
    {
        Id = id;
        StudioId = studioId;
        Title = title;
        BudgetMln = budgetMln;
        StudioName = studioName; // Исправлена ошибка CS0103
    }

    /// <summary> Конструктор по умолчанию через цепочку this. </summary>
    public Movie() : this(0, 0, "Неизвестно", 0, "Нет данных") { }

    /// <summary> Переопределение ToString. </summary>
    public override string ToString() =>
        $"ID: {Id,-3} | {Title,-25} | Studio: {StudioName,-15} | Бюджет: {BudgetMln} млн$";
}