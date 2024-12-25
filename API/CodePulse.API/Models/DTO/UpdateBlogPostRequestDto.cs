namespace CodePulse.API.Models.DTO
{
    public class UpdateBlogPostRequestDto
    {
        public string Title { get; set; }
        public string ShortDiscription { get; set; }
        public string Containt { get; set; }
        public string FeatureImageUrl { get; set; }
        public string UrlHandle { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Auther { get; set; }
        public bool IsVisible { get; set; }
        public List<Guid> Categories { get; set; } = new List<Guid>();
    }
}
