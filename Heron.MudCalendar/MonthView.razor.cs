using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Extensions;
using MudBlazor.Utilities;

namespace Heron.MudCalendar;

public partial class MonthView<T> where T : CalendarItem
{
    [Parameter] public Color Color { get; set; } = Color.Primary;

    [Parameter] public DateTime CurrentDay { get; set; }

    [Parameter] public bool HighlightToday { get; set; } = true;

    [Parameter] public RenderFragment<T>? CellTemplate { get; set; }

    [Parameter] public IEnumerable<T> Items { get; set; } = new List<T>();

    private List<CalendarCell<T>> _cells = new();

    protected override void OnParametersSet()
    {
        BuildCells();
    }

    private string DayClassname(CalendarCell<T> calendarCell)
    {
        return new CssBuilder()
            .AddClass("mud-cal-month-cell-day")
            .AddClass("mud-cal-month-outside", calendarCell.Outside)
            .Build();
    }

    private string DayStyle(CalendarCell<T> calendarCell)
    {
        return new StyleBuilder()
            .AddStyle("border", $"1px solid var(--mud-palette-{Color.ToDescriptionString()})",
                calendarCell.Today && HighlightToday)
            .Build();
    }

    private void BuildCells()
    {
        var cells = new List<CalendarCell<T>>();
        var monthStart = new DateTime(CurrentDay.Year, CurrentDay.Month, 1);
        var monthEnd = new DateTime(CurrentDay.AddMonths(1).Year, CurrentDay.AddMonths(1).Month, 1).AddDays(-1);

        var range = new CalendarDateRange(CurrentDay, CalendarView.Month);
        if (range.Start != null && range.End != null)
        {
            var date = range.Start.Value;
            var lastDate = range.End.Value;
            while (date <= lastDate)
            {
                var cell = new CalendarCell<T> { Date = date };
                if (date.Date == DateTime.Today) cell.Today = true;
                if (date < monthStart || date > monthEnd)
                {
                    cell.Outside = true;
                }

                cell.Items = Items.Where(i => i.Start >= date && i.Start < date.AddDays(1)).ToList();

                cells.Add(cell);

                // Next day
                date = date.AddDays(1);
            }
        }

        _cells = cells;
    }
}