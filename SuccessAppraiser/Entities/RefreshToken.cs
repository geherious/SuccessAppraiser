namespace SuccessAppraiser.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public Guid Token {  get; set; }
        public DateTime Expires { get; set; }
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
