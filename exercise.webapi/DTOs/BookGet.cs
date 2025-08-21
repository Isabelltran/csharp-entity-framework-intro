namespace exercise.webapi.DTOs
{
    public class BookGet
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int AuthorId { get; set; }
        public AuthorGet Author { get; set; }
    }
}
