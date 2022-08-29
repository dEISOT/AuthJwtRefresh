namespace AuthJwtRefresh.Models
{
    public class RefreshToken
    {
        public  Guid Id { get; set; }
        public string Token { get; set; }
        public DateTime ExpireAt { get; set; }
        public Guid AccountId { get; set; }
        public virtual Account Account { get; set; }
    }
}
