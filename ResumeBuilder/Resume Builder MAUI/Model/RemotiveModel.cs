using Newtonsoft.Json;

namespace Resume_Builder_MAUI.Model;

// var myDeserializedClass = JsonConvert.DeserializeObject<RemotiveJobsResponse>(myJsonResponse);

public class RemotiveJobsResponse
{
    [JsonProperty("00-warning")]
    public string? _00warning { get; set; }

    [JsonProperty("0-legal-notice")]
    public string? _0legalnotice { get; set; }

    [JsonProperty("job-count")]
    public int jobcount { get; set; }

    [JsonProperty("total-job-count")]
    public int totaljobcount { get; set; }
    public List<Job>? jobs { get; set; }
}


public class Job
{
    public int? id { get; set; }
    public string? url { get; set; }
    public string? title { get; set; }
    public string? company_name { get; set; }
    public string? company_logo { get; set; }
    public string? category { get; set; }
    public List<string>? tags { get; set; }
    public string? job_type { get; set; }
    public DateTime? publication_date { get; set; }
    public string? candidate_required_location { get; set; }
    public string? salary { get; set; }
    public string? description { get; set; }
    public string? company_logo_url { get; set; }
}

