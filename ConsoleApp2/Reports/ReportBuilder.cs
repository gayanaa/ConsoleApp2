using System;
using System.Collections.Generic;

namespace Homework2;

public class ReportBuilder
{
    private readonly DatabaseManager _db;
    private string _query;
    private string _title;
    private string _footerLabel;
    private string[] _headers;

    public ReportBuilder(DatabaseManager db) => _db = db;

    public ReportBuilder SetQuery(string sql) { _query = sql; return this; }
    public ReportBuilder SetTitle(string title) { _title = title; return this; }
    public ReportBuilder SetHeaders(params string[] headers) { _headers = headers; return this; }

    // Промежуточный метод Footer согласно требованию
    public ReportBuilder Footer(string label) { _footerLabel = label; return this; }

    public void Print()
    {
        Console.WriteLine($"\n>>> {_title} <<<");
        var data = _db.GetTable(_query);

        if (_headers != null)
            Console.WriteLine(string.Join(" | ", _headers));

        Console.WriteLine(new string('-', 60));
        foreach (var row in data)
            Console.WriteLine(string.Join(" | ", row));

        // Вывод итоговой строки, если задан Footer
        if (!string.IsNullOrEmpty(_footerLabel))
            Console.WriteLine($"* {_footerLabel}: {data.Count} *");
    }
}
