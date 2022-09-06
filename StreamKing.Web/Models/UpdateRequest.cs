using StreamKing.Data.Accounts;

namespace StreamKing.Web.Models
{
    public class UpdateRequest
    {
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Region { get; set; }
        public DateTime? Birthday { get; set; }

        public Guid? Id { get; set; }
        public string? Username { get; set; }
        public AccountType? Type { get; set; }
        public AccountStatus? Status { get; set; }
        public int? FailedLogins { get; set; }
        public DateTime? Created { get; set; }
    }
}
