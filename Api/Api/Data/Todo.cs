namespace Api.Data
{
    public class Todo : BaseEntity
    {
        public string Description { get; set; } = string.Empty;
        public bool IsDone { get; set; } = false;
        public int UserId { get; set; }
        public virtual User? User { get; set; }
    }
}