namespace CodePulse.API.Models.Domain
{
    public class BlogPost
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string ShortDiscription { get; set; }
        public string Containt { get; set; }
        public string FeatureImageUrl { get; set; }
        public string UrlHandle { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Auther { get; set; }
        public bool IsVisible { get; set; }
        public ICollection<Category> Categories { get; set; }

    }
}
