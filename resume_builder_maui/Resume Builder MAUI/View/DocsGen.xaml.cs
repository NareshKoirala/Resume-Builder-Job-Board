using Resume_Builder_MAUI.Model;
using Resume_Builder_MAUI;
using System.Collections.ObjectModel;
using Resume_Builder_MAUI.Services;
using System.Threading.Tasks;

namespace Resume_Builder_MAUI.View;

public partial class DocsGen : ContentPage
{
    public string SelectedJobBoard { get; set; } = "";
    public ObservableCollection<GenJobsModel> GenJobs { get; set; }
    public List<string> JobBoard { get; set; } = new List<string>
    {
        "LinkedIn",
        "Indeed"
    };
    public DocsGen()
    {
        InitializeComponent();

        GenJobs = new ObservableCollection<GenJobsModel>(GlobalVarables.genJobs ?? new List<GenJobsModel>());

        BindingContext = this;

#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
        AddURL?.Clicked += AddURL_Clicked;
#pragma warning restore CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
    }

#pragma warning disable VSTHRD100 // Avoid async void methods
    public async void AddURL_Clicked(object sender, EventArgs e)
#pragma warning restore VSTHRD100 // Avoid async void methods
    {
        if (sender is Button button)
        {
            if (string.IsNullOrWhiteSpace(SelectedJobBoard))
            {
                await DisplayAlert("Error", "Please select a job board.", "OK");
                return;
            }

            string applyURL = UrlExternalLink.Text;

            if (string.IsNullOrWhiteSpace(applyURL))
            {
                await DisplayAlert("Error", "Please enter a valid URL.", "OK");
                return;
            }

            try
            {
                if (SelectedJobBoard == "LinkedIn")
                {
                    string jobDesc = await GlobalVarables.LinkedInJobDescriptionAsync(applyURL);
                    if (string.IsNullOrWhiteSpace(jobDesc))
                    {
                        await DisplayAlert("Error", "No job description found in the URL.", "OK");
                        return;
                    }

                    await OpenAIService.JobDetailsAsync(jobDesc, applyURL);


                    UrlExternalLink.Text = "";
                    JobDescription.Text = "";
                    JobName.Text = "";
                    CompanyName.Text = "";

                }
                else
                {
                    string jobName = JobName.Text;
                    string companyName = CompanyName.Text;
                    string jobDesc = JobDescription.Text;

                    if (string.IsNullOrWhiteSpace(jobName) ||
                        string.IsNullOrWhiteSpace(jobDesc) ||
                        string.IsNullOrWhiteSpace(companyName))
                    {
                        await DisplayAlert("Error", "Fill all the fields.", "OK");
                        return;
                    }

                    var jobs = new JobsModel()
                    {
                        jobApplyLink = applyURL,
                        jobDescription = jobDesc,
                        companyName = companyName
                    };

                    var genJob = new GenJobsModel()
                    {
                        JobName = jobName,
                        Jobs = jobs
                    };

                    UrlExternalLink.Text = "";
                    JobDescription.Text = "";
                    JobName.Text = "";
                    CompanyName.Text = "";

                    await OpenAIService.PercentageAsync(genJob);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An unexpected error occurred: {ex.Message}", "OK");
            }
        }
    }

    public void JobBoardChanged(object sender, EventArgs e)
    {
        if (sender is Picker picker && picker.SelectedItem is string selectedJobBoard)
        {
            SelectedJobBoard = selectedJobBoard;
            if (selectedJobBoard == "LinkedIn")
            {
                UrlExternalLink.IsVisible = true;
                JobDescription.IsVisible = false;
                JobName.IsVisible = false;
                CompanyName.IsVisible = false;
            }
            else
            {
                UrlExternalLink.IsVisible = true;
                JobDescription.IsVisible = true;
                JobName.IsVisible = true;
                CompanyName.IsVisible = true;
            }
        }
    }

#pragma warning disable VSTHRD100 // Avoid async void methods
    private async void OnDownloadResume(object sender, EventArgs e)
#pragma warning restore VSTHRD100 // Avoid async void methods
    {
        if (sender is Button button && button.BindingContext is GenJobsModel job)
        {
            if (job?.Resume?.TitleKeyword == string.Empty)
            {
                await DisplayAlert("Wait", "Generating Resume and Cover Letter", "Ok");
                await OpenAIService.BuildResumePromptAsync(job);
            }
            else
            {
                bool answer = await DisplayAlert("Resume and Cover Letter", "Resume and Cover Letter already generated. \r\nWould you want to Download?", "Yes", "No");

                if (!answer)
                {
                    return;
                }
                // If the resume is already generated, we can directly create the documents
                await DocCreationServcie.CreateResumeAsync(job);
                DocCreationServcie.CreateCoverLetter(job);
            }
        }
    }

    public void RefreshData()
    {
        // This method can be used to refresh the data if needed
        GenJobs.Clear();
        foreach (var job in GlobalVarables.genJobs ?? new List<GenJobsModel>())
        {
            GenJobs.Add(job);
        }
    }

#pragma warning disable VSTHRD100 // Avoid async void methods
    private async void ApplyToLinkClicked(object sender, EventArgs e)
#pragma warning restore VSTHRD100 // Avoid async void methods
    {
        if (sender is Button button && button.BindingContext is GenJobsModel job)
        {
            if (!string.IsNullOrWhiteSpace(job.ApplyLink))
            {
                await Launcher.TryOpenAsync(new Uri(job.ApplyLink));
            }
            else
            {
                await DisplayAlert("Error", "No apply link available for this job.", "OK");
            }
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Refresh the data when the page appears
        RefreshData();
    }
}