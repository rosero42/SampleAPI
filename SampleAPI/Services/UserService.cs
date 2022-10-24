using SampleAPI.Model;

namespace SampleAPI.Services
{
    public class UserService
    {
        public static int nextId = 3;
        static List<UserItem> Users { get; }
        static UserService()
        {
            Users = new List<UserItem>
            {
                /*new UserItem {Id=1,UserName="rosero"},
                new UserItem {Id=2, UserName="tomnaps"}*/
            };
        }
        public static List<UserItem> GetAll() => Users;
        public static UserItem? Get(int id) => Users.FirstOrDefault(i => i.Id == id);
        public static UserItem? Get(string username) => Users.FirstOrDefault(i => i.UserName == username);
        public static void Add(UserItem user)
        {
            user.Id = nextId++;
            Users.Add(user);

        }
        public static void Add(string username)
        {
             Users.Add(new UserItem {Id=nextId++,UserName=username});
        }

        public static void Update(UserItem user)
        {
            var index = Users.FindIndex(i => i.Id == user.Id);
            if (index == -1)
                return;
            Users[index] = user;
        }

        public static void Delete(string name)
        {
            var user = Users.FirstOrDefault(i => i.UserName == name);
            if (user is null)
                return;
            Users.Remove(user);
        }
    }
}
