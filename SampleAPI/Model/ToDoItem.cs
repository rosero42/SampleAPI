namespace SampleAPI.Model
{
    public enum ToDoType
    {
        Housework,
        Schoolwork,
        Career,
        Shopping,
        Misc
    }

    public enum Priority
    {
        Low,
        Medium,
        High,
    }
    public class ToDoItem
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string type { get; set; }
        public bool isComplete { get; set; }
        public string? Created { get; set; }
        public Priority Priority { get; set; }
    }


}
