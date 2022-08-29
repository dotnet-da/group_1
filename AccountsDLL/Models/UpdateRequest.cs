using AccountsDLL.Entities;

namespace AccountsDLL.Models
{
    public class UpdateRequest
    {
        public Guid? Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public AccountType? Type { get; set; }
        public AccountStatus? Status { get; set; }
        public int? FailedLogins { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? Birthday { get; set; }
        public DateTime? Created { get; set; }
    }
}
