namespace ReadTrack.Shared.Models.Analytics;

public class TimeSegment<T>
{
    public DateTime Date { get; set; }
    public T? Value { get; set; }
    public SegmentType Type { get; set; }
}