namespace TaskTracker.Models.Auth
{
    public class JwtInfo
    {
        public string Role { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Expiration { get; set; }
        public Guid UserId { get; set; }
    }
}
