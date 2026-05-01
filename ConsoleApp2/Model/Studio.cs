using System;

namespace Homework2;

/// <summary>
/// Представляет информацию о киностудии.
/// </summary>
public class Studio
{
    private int _id;

    /// <summary> Уникальный номер студии. </summary>
    public int Id
    {
        get => _id;
        set
        {
            if (value < 0) throw new ArgumentException("ID не может быть отрицательным!");
            _id = value;
        }
    }

    /// <summary> Название студии. </summary>
    public string Name { get; set; }

    /// <summary>
    /// Полный конструктор со всеми параметрами.
    /// </summary>
    public Studio(int id, string name)
    {
        Id = id;
        Name = name;
    }

    /// <summary>
    /// Конструктор по умолчанию, вызывающий полный через цепочку this(...).
    /// </summary>
    public Studio() : this(0, "Новая студия")
    {
    }

    /// <summary> Переопределение для удобного вывода. </summary>
    public override string ToString() => $"[{Id}] Студия: {Name}";
}


