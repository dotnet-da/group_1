namespace AccountsDLL
{
    public enum LogLevel
    {
        Debug,
        Info,
        Warning,
        Error,
    }
    public class AccountLog
    {

        public DateTime Timestamp { get; set; } = DateTime.Now;
        public LogLevel Level { get; set; } = LogLevel.Debug;
        public string Source { get; set; } = "";
        public string Message { get; set; } = "";

    }
}
