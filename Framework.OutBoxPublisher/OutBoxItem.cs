namespace Framework.OutBoxPublisher;

public class OutBoxItem
{
    public long Id { get; set; }
    public Guid EventId { get; set; }
    public string EventBody { get; set; } = string.Empty;
    public string EventType { get; set; } = string.Empty;
}