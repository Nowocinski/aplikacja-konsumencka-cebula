namespace WebApplication.Infrastructure.DTO
{
    public class AccountDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool Blocked { get; set; }
    }
}
