namespace WebApiLibr.Models.EntitiesModel
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public byte[] Password { get; set; }
        public byte[] Salt { get; set; } 
        public Roles RoleId { get; set; }
        public Role Role { get; set; }

        public virtual List<Message> SendMessages { get; set; }
        public virtual List<Message> ReceiveMessages { get; set; }
    }
}
