using SampleAPI.Model;

namespace SampleAPI.Services
{
    public class ToDoService
    {
        public List<ToDoItem> ToDoItems { get; }
        int nextId = 1;
        public ToDoService()
        {
            ToDoItems = new List<ToDoItem>
            {
                /*new ToDoItem { Id = 1, Name = "Make a To Do List", isComplete = false, 
                    Created = DateTime.Now.ToString("MM/dd/yyyy"), type="misc" }*/
            };
        }

        public  List<ToDoItem> GetAll() => ToDoItems;
        public  ToDoItem? Get(int id) => ToDoItems.FirstOrDefault(i => i.Id == id);   
        public  ToDoItem? Get(string name)=> ToDoItems.FirstOrDefault(i => i.Name == name);
        public void Add(ToDoItem toDoItem)
        {
            toDoItem.type = toDoItem.type.ToLower();
            ToDoItems.Add(toDoItem);
        }

        public void Add(string Name)
        {
            ToDoItem item = new ToDoItem { Id=nextId++, Name=Name, isComplete=false, 
                Created=DateTime.Now.ToString("MM/dd/yyyy"), type= "misc"};
            ToDoItems.Add(item);
        }

        public void Add(string Name, string category)
        {
            ToDoItem item = new ToDoItem { Id = nextId++, Name = Name, isComplete = false, 
                Created = DateTime.Now.ToString("MM/dd/yyyy"), type =category.ToLower()};
            ToDoItems.Add(item);
        }

        public void Delete(int id)
        {
            var item = Get(id);
            if (item is null)
                return;
            ToDoItems.Remove(item);
        }
        
        public void Delete(string name)
        {
            var item = ToDoItems.FirstOrDefault(i => i.Name == name);
            if(item is null)
                return ;
            ToDoItems.Remove(item);
        }

        public void Update(ToDoItem item)
        {
            var index = ToDoItems.FindIndex(i => i.Id == item.Id);
            if(index == -1)
                return;
            string created = ToDoItems[index].Created;
            item.Created = created;
            ToDoItems[index] = item;
        }
    }
}
