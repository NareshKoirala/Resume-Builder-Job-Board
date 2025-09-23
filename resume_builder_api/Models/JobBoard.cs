namespace resume_builder_api.Models
{
    public class JobBoard
    {
        public string country { get; set; }
        public string page { get; set; }
        public List<string> what { get; set; }
        public int max_days_old { get; set; }
        public List<string> what_exclude { get; set; }
        public List<string> what_or { get; set; }
    }
}
