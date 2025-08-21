namespace Resume_Builder_MAUI.Model;
// var myDeserializedClass = JsonConvert.DeserializeObject<JSearchJobsResponse>(myJsonResponse);

public class JSearchJobsResponse
{
    public string? status { get; set; }
    public string? request_id { get; set; }
    public Parameters? parameters { get; set; }
    public List<Datum>? data { get; set; }
}

public class ApplyOption
{
    public string? publisher { get; set; }
    public string? apply_link { get; set; }
    public bool? is_direct { get; set; }
}

public class Datum
{
    public string? job_id { get; set; }
    public string? job_title { get; set; }
    public string? employer_name { get; set; }
    public string? employer_logo { get; set; }
    public string? employer_website { get; set; }
    public string? job_publisher { get; set; }
    public string? job_employment_type { get; set; }
    public List<string>? job_employment_types { get; set; }
    public string? job_apply_link { get; set; }
    public bool? job_apply_is_direct { get; set; }
    public List<ApplyOption>? apply_options { get; set; }
    public string? job_description { get; set; }
    public bool? job_is_remote { get; set; }
    public string? job_posted_at { get; set; }
    public int? job_posted_at_timestamp { get; set; }
    public DateTime? job_posted_at_datetime_utc { get; set; }
    public string? job_location { get; set; }
    public string? job_city { get; set; }
    public string? job_state { get; set; }
    public string? job_country { get; set; }
    public double? job_latitude { get; set; }
    public double? job_longitude { get; set; }
    public List<string>? job_benefits { get; set; }
    public string? job_google_link { get; set; }
    public int? job_min_salary { get; set; }
    public int? job_max_salary { get; set; }
    public string? job_salary_period { get; set; }
    public JobHighlights? job_highlights { get; set; }
    public string? job_onet_soc { get; set; }
    public string? job_onet_job_zone { get; set; }
    public object? job_salary { get; set; }
}

public class JobHighlights
{
    public List<string>? Qualifications { get; set; }
    public List<string>? Benefits { get; set; }
    public List<string>? Responsibilities { get; set; }
}

public class Parameters
{
    public string? job_id { get; set; }
    public string? query { get; set; }
    public int? page { get; set; }
    public int? num_pages { get; set; }
    public string? date_posted { get; set; }
    public string? country { get; set; }
    public string? language { get; set; }
}



