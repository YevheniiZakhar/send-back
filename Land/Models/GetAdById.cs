namespace Land.Models
{
    public class AdById : Ad
    {
        public AdById(Ad ad)
        {
            Id = ad.Id;
            Name = ad.Name;
            File1 = ad.File1;
            File2 = ad.File2;
            File3 = ad.File3;
            File4 = ad.File4;
            File5 = ad.File5;
            File6 = ad.File6;
            File7= ad.File7;
            File8 = ad.File8;
            Description = ad.Description;
            Price = ad.Price;
            CategoryId = ad.CategoryId;
            LocalityId = ad.LocalityId;
            Phone = ad.Phone; 
            UserName = ad.UserName;
            CreatedDate = ad.CreatedDate;
        }

        public string Locality { get; set; }
    }
}
