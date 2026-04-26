using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Homework2;

public class ReportBuilder
{
    private StringBuilder _sb = new StringBuilder();
    private List<Movie> _data;

    public ReportBuilder(List<Movie> data) => _data = data;

    public ReportBuilder AddHeader(string title)
    {
        _sb.AppendLine("========================================");
        _sb.AppendLine($"ОТЧЕТ: {title.ToUpper()}");
        _sb.AppendLine("========================================");
        return this;
    }

    public ReportBuilder AddContent()
    {
        foreach (var item in _data)
        {
            _sb.AppendLine(item.ToString());
        }
        return this;
    }

    public ReportBuilder AddStatistics()
    {
        if (_data.Any())
        {
            double avg = _data.Average(m => m.BudgetMln);
            _sb.AppendLine("----------------------------------------");
            _sb.AppendLine($"Всего фильмов в отчете: {_data.Count}");
            _sb.AppendLine($"Средний бюджет: {avg:F2} млн$");
        }
        return this;
    }

    public string Build() => _sb.ToString();
}