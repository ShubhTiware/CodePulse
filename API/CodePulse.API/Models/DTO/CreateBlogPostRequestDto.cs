namespace CodePulse.API.Models.DTO
{
    public class CreateBlogPostRequestDto
    {
        public string Title { get; set; }
        public string ShortDiscription { get; set; }
        public string Containt { get; set; }
        public string FeatureImageUrl { get; set; }
        public string UrlHandle { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Auther { get; set; }
        public bool IsVisible { get; set; }
        public Guid[] Categories { get; set; }
    }
}
