using SampleAPI.Services;

namespace SampleAPI.Model
{
    public class UserItem
    {
        public int Id;
        public string? UserName;
        public ToDoService ToDo;

        public UserItem()
        {
            ToDo = new ToDoService();
        }
    }
}
