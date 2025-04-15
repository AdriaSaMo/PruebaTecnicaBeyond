namespace PruebaTecnicaBeyond.WebAPI.DTOs
{
    public class TodoItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public bool IsCompleted { get; set; }
        public decimal TotalPercentage { get; set; }
        public List<ProgressionDto> Progressions { get; set; }
    }
}
