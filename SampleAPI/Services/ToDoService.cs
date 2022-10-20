using SampleAPI.Model;

namespace SampleAPI.Services
{
    public class ToDoService
    {
        static List<ToDoItem> ToDoItems { get; }
        static int nextId = 2;
        static ToDoService()
        {
            ToDoItems = new List<ToDoItem>
            {
                new ToDoItem { Id = 1, Name = "Make a To Do List", isComplete = false, 
                    Created = DateTime.Now.ToString("MM/dd/yyyy"), type="misc", Priority=Priority.High }
            };
        }

        public static List<ToDoItem> GetAll() => ToDoItems;
        public static HashSet<string> ToDoCategories;
        public static ToDoItem? Get(int id) => ToDoItems.FirstOrDefault(i => i.Id == id);   
        public static ToDoItem? Get(string name)=> ToDoItems.FirstOrDefault(i => i.Name == name);
        public static void Add(ToDoItem toDoItem)
        {
            toDoItem.Id = nextId++;
            toDoItem.Created = DateTime.Now.ToString("MM/dd/yyyy");
            toDoItem.type = toDoItem.type.ToLower();
            ToDoItems.Add(toDoItem);
        }

        public static void Add(string Name)
        {
            ToDoItem item = new ToDoItem { Id=nextId++, Name=Name, isComplete=false, 
                Created=DateTime.Now.ToString("MM/dd/yyyy"), type= "misc", Priority=Priority.Medium};
            ToDoItems.Add(item);
        }

        public static void Add(string Name, string category)
        {
            ToDoItem item = new ToDoItem { Id = nextId++, Name = Name, isComplete = false, 
                Created = DateTime.Now.ToString("MM/dd/yyyy"), type =category.ToLower(), Priority = Priority.Medium };
            ToDoItems.Add(item);
        }

        public static void Delete(int id)
        {
            var item = Get(id);
            if (item is null)
                return;
            ToDoItems.Remove(item);
        }
        
        public static void Delete(string name)
        {
            var item = ToDoItems.FirstOrDefault(i => i.Name == name);
            if(item is null)
                return ;
            ToDoItems.Remove(item);
        }

        public static void Update(ToDoItem item)
        {
            var index = ToDoItems.FindIndex(i => i.Id == item.Id);
            if(index == -1)
                return;
            ToDoItems[index] = item;
        }
    }
}
