using SampleAPI.Services;

namespace SampleAPI.Model
{
    public class UserItem
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public ToDoService ToDo { get; set; }

        public UserItem()
        {
            ToDo = new ToDoService();
        }
    }
}
