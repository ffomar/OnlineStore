namespace OnlineStoreApp.Models;

public class ErrorLog
{
    public int Id { get; set; }

    public string Signature { get; set; } = string.Empty;

    public string ExceptionType { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public string Path { get; set; } = string.Empty;

    public int HitCount { get; set; }

    public DateTime FirstSeenUtc { get; set; }

    public DateTime LastSeenUtc { get; set; }
}
