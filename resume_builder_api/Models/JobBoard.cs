namespace resume_builder_api.Models
{
    public class JobBoard
    {
        public string country { get; set; } = "ca";
        public string page { get; set; } = "1";
        public List<string> what { get; set; } = new List<string> { "Software Engineer" };
        public int max_days_old { get; set; } = 30;
        public List<string> what_exclude { get; set; } = new List<string> { "senior" };
        public List<string> what_or { get; set; } = new List<string> { "Toronto", "Edmonton" };
    }
}
