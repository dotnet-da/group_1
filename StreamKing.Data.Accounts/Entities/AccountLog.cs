using System.ComponentModel.DataAnnotations;

namespace StreamKing.Data.Accounts
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
        [Key]
        public int Id { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public LogLevel Level { get; set; } = LogLevel.Debug;
        public string Source { get; set; } = "";
        public string Message { get; set; } = "";

    }
}
