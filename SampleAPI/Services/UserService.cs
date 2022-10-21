using SampleAPI.Model;

namespace SampleAPI.Services
{
    public class UserService
    {
        public static int nextId = 2;
        public static List<UserItem> Users;
        
        public UserService()
        {
            Users = new List<UserItem>
            {
                new UserItem{Id=1,UserName="rosero"}
            };
        }
        public static List<UserItem> GetAll() => Users;
        public static UserItem? Get(int id) => Users.FirstOrDefault(i => i.Id == id);
        public static void Add(UserItem user)
        {
            user.Id = nextId++;
            Users.Add(user);
        }
    }
}
