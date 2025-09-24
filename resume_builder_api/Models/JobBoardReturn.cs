namespace resume_builder_api.Models
{
    public class JobBoardReturn
    {
        public string id { get; set; } = string.Empty;
        public string title { get; set; } = string.Empty;
        public string company { get; set; } = string.Empty;
        public string location { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string contract_time { get; set; } = string.Empty;
        public string redirect_url { get; set; } = string.Empty;
    }

}
