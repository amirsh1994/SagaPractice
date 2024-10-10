namespace ApiGateWay;

public class RegisterServiceModel
{
    public string Url { get; set; }

    public string Name { get; set; } = string.Empty;

    public bool IsHealthy { get; set; }
}