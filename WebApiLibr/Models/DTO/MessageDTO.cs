namespace WebApiLibr.Models.DTO
{
    public class MessageDTO
    {
        public string SenderEmail { get; set; }
        public string RecipientEmail { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Text { get; set; }
    }
}
