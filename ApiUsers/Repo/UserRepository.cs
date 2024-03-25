using ApiUsers.Abstraction;
using AutoMapper;
using System.Security.Cryptography;
using System.Text;
using WebApiLibr;
using WebApiLibr.Models.DTO;
using WebApiLibr.Models.EntitiesModel;

namespace ApiUsers.Repo
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public UserRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
           _mapper = mapper;
        }


        public UserDTO AddUser(LoginModel user)
        {
            using (_context)
            {
                var entityUser = _context.Users.FirstOrDefault(x => x.Email.Equals(user.Email));
                if (entityUser == null)
                {
                    byte[] salt = new byte[16];
                    new Random().NextBytes(salt);

                    var data = Encoding.ASCII.GetBytes(user.Password);
                    //var data = Encoding.ASCII.GetBytes(user.Password).Concat(salt).ToArray();

                    SHA512 shaM = new SHA512Managed();

                    if (_context.Users.Count(x => x.RoleId == Roles.Admin) == 0)
                    {
                        entityUser = new User
                        {
                            Email = user.Email,
                            Password = shaM.ComputeHash(data),
                            Salt = salt,
                            RoleId = Roles.Admin,
                            Role = new Role() { RoleId = Roles.Admin, RoleName = Roles.Admin.ToString()}
                            
                        };
                    }
                    else
                    {
                        entityUser = new User
                        {
                            Email = user.Email,
                            Password = shaM.ComputeHash(data),
                            Salt = salt,
                            RoleId = Roles.User,
                            Role = new Role() { RoleId = Roles.User, RoleName = Roles.User.ToString() }
                        };
                    }
                    _context.Users.Add(entityUser);
                    _context.SaveChanges();
                }
                else
                {
                    var data = Encoding.ASCII.GetBytes(user.Password);
                    //var data = Encoding.ASCII.GetBytes(user.Password).Concat(entityUser.Salt).ToArray();

                    SHA512 shaM = new SHA512Managed();
                    var pas = shaM.ComputeHash(data);
                    if (!entityUser.Password.SequenceEqual(pas))
                        return null;
                }
                var dto = _mapper.Map<UserDTO>(entityUser);
                return dto;

            }
            
        }

        public Guid DeleteUser(string userEmail)
        {
            using (_context)
            {
                var entityProduct = _context.Users.FirstOrDefault(x => x.Email.Equals(userEmail));
                if (entityProduct != null)
                {
                    _context.Users.Remove(entityProduct);
                    _context.SaveChanges();
                }
                return entityProduct.Id;
            }
        }
        public IEnumerable<UserDTO> GetUsers()
        {
            using (_context)
            {
                var listUsers = _context.Users.Select(x => _mapper.Map<UserDTO>(x)).ToList();
                return listUsers;
            }
        }
    }
}
