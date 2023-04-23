namespace Land.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Ad
    {   
        public int Id { get; set; }

        [Required(ErrorMessage = "Name field is required")]
        [StringLength(maximumLength: 100, MinimumLength = 1)]
        public string Name { get; set; }

        public byte[]? File1 { get; set; }
        public byte[]? File2 { get; set; }
        public byte[]? File3 { get; set; }
        public byte[]? File4 { get; set; }
        public byte[]? File5 { get; set; }
        public byte[]? File6 { get; set; }
        public byte[]? File7 { get; set; }
        public byte[]? File8 { get; set; }

        public string? Description { get; set; }

        public int? Price { get; set; }

        public int? CategoryId { get; set; }

        public int? LocalityId { get; set; }

        //[NotMapped]
        //public IFormFile[] FrontFiles { get; set; }
        //[NotMapped]
        //public string Str { get; set; }

        //[NotMapped]
        //public IFormFile FrontFile1 { get; set; }
        //[NotMapped]
        //public IFormFile FrontFile2 { get; set; }
        //[NotMapped]
        //public IFormFile FrontFile3 { get; set; }
        //[NotMapped]
        //public IFormFile FrontFile4 { get; set; }
        //[NotMapped]
        //public IFormFile FrontFile5 { get; set; }
        //[NotMapped]
        //public IFormFile FrontFile6 { get; set; }
        [NotMapped]
        public string? Locality { get; set; }

        public string? Phone { get; set; }

        public string? UserName { get; set; }

        public DateTime? CreatedDate {  get; set; }
    }
}
