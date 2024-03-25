namespace WebApiLibr.Models.EntitiesModel
{
    public class Message
    {
        public Guid Id { get; set; }
        public Guid SenderId { get; set; }
        public virtual User Sender { get; set; }
        public Guid RecipientId { get; set; }
        public virtual User Recipient { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Text { get; set; }
    }
}
