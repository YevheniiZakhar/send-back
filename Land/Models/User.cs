namespace Land.Models
{
    [Table("user", Schema = "land")]
    public class User
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
    }
}
