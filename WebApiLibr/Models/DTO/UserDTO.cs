using WebApiLibr.Models.EntitiesModel;

namespace WebApiLibr.Models.DTO
{
    public class UserDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public Roles Role{ get; set; }

    }
}
