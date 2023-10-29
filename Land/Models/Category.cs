namespace Land.Models
{
    [Table("category", Schema = "land")]
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
