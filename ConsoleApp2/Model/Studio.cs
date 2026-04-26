using System;

namespace Homework2;

public class Studio
{
    public int Id { get; set; }
    public string Name { get; set; }

    public Studio(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public Studio() : this(0, "") { }

    public override string ToString() => $"[{Id}] Студия: {Name}";
}