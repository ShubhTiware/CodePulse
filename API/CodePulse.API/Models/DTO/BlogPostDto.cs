namespace CodePulse.API.Models.DTO
{
    public class BlogPostDto
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

        public List<CatagoryDto> Categories { get; set; } = new List<CatagoryDto>();
    }
}
