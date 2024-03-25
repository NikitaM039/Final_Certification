using WebApiLibr.Models.DTO;

namespace ApiUsers.Abstraction
{
    public interface IUserRepository
    {
        public UserDTO AddUser(LoginModel user);
        public IEnumerable<UserDTO> GetUsers();
        public Guid DeleteUser(string user);
    }
}
