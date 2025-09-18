namespace resume_builder_api.Models
{
    public class JobBoardReturn
    {
        public string id { get; set; } = string.Empty;
        public string title { get; set; } = string.Empty;
        public string company { get; set; } = string.Empty;
        public string location { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string salaryRange { get; set; } = string.Empty;
        public List<Badge> badge { get; set; } = new List<Badge> { new Badge() };
    }

    public class Badge
    {
        public string text { get; set; } = "New";
        public string color { get; set; } = "purple";
    }
}
