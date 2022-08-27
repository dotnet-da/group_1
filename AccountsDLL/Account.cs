using System.Text.Json.Serialization;

namespace AccountsDLL
{
    public enum AccountType
    {
        User,
        Admin
    }

    public enum AccountStatus
    {
        Active,
        ValidationRequired,
        ResetPassword,
        Locked,
    }

    public class Account
    {
        // Important members and credentials
        public Guid Id { get; set; } = new Guid();
        public string UserName { get; set; } = "";
        public string Email { get; set; } = "";
        public AccountType Type { get; set; } = AccountType.User;
        
        // Account management fields
        public AccountStatus status { get; set; } = AccountStatus.ValidationRequired;
        public int failedLogins { get; set; } = 0;

        [JsonIgnore]
        public string Password { get; set; } = "";

        // User data
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public DateTime Birthday { get; set; } = DateTime.Now;
        public DateTime Created { get; set; } = DateTime.Now;

        // Logs
        public List<AccountLog> Logs { get; set; } = new List<AccountLog>();

    }
}