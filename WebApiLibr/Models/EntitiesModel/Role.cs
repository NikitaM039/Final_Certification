namespace WebApiLibr.Models.EntitiesModel
{
    public class Role
    {
        public Roles RoleId { get; set; }
        public string RoleName { get; set; }
        public virtual List<User> Users { get; set; }
    }
}
