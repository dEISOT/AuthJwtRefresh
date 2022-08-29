namespace AuthJwtRefresh.Models
{
    public class Account
    {
        public Account()
        {
            RefreshTokens = new List<RefreshToken>();
        }
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public virtual List<RefreshToken> RefreshTokens { get; set; }
        public string Role { get; set; }
    }
}
