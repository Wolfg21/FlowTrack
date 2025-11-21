using System;

namespace FlowTrack.Domain.Shared;

public sealed class DateRange
{
    private DateRange(DateTime start, DateTime end)
    {
        Start = start;
        End = end;
    }

    public DateTime Start { get; }

    public DateTime End { get; }

    public bool Contains(DateTime value) => value >= Start && value <= End;

    public static DateRange Create(DateTime start, DateTime end)
    {
        if (end < start)
        {
            throw new ArgumentException("End date can not be earlier than start date.");
        }

        return new DateRange(start, end);
    }
}
