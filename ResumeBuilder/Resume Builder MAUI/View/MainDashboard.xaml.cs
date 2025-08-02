using Resume_Builder_MAUI.Model;
using Resume_Builder_MAUI.Services;
using Resume_Builder_MAUI.ViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Resume_Builder_MAUI.View
{
    public partial class MainDashboard : ContentPage, INotifyPropertyChanged
    {
        ObservableCollection<JobsModel> jobs { get; set; } = new ObservableCollection<JobsModel>();

        private int _loadCount = 0;
        private const int _pageSize = 20;

        public new event PropertyChangedEventHandler? PropertyChanged;

        // Update the event handler to accept nullable sender parameter to match EventHandler delegate
        private void OnRemainingItemsThresholdReached(object? sender, EventArgs e)
        {
            LoadMoreJobs();
        }

        private void LoadMoreJobs()
        {
            var newJobs = GlobalVarables.jobsInfo?.Skip(_loadCount).Take(_pageSize).ToList();
            if (newJobs != null && newJobs.Count != 0)
            {
                foreach (var job in newJobs)
                {
                    jobs.Add(job);
                }
                _loadCount += _pageSize;
            }
        }

        public MainDashboard()
        {
            InitializeComponent();

            Shell.SetTabBarIsVisible(this, true);

            BindingContext = this;

            JobsCollectionView.ItemsSource = jobs;

            JobsCollectionView.RemainingItemsThreshold = _pageSize / 2;
            JobsCollectionView.RemainingItemsThresholdReached += OnRemainingItemsThresholdReached;

            LoadMoreJobs();
        }

#pragma warning disable VSTHRD100 // Avoid async void methods
        public async void OnGenerateClicked(object sender, EventArgs e)
#pragma warning restore VSTHRD100 // Avoid async void methods
        {
            if (sender is Button button && button.BindingContext is JobsModel job)
            {
                // Check if the job already exists in the list
                if (!GlobalVarables.genJobs.Any(gj => gj.JobId == job.jobId))
                {
                    // Create a new GenJobsModel instance
                    var genJob = new GenJobsModel
                    {
                        JobId = job.jobId,
                        JobName = job.jobTitle,
                        ApplyLink = job.jobApplyLink,
                        Jobs = job ,
                        Resume = new ResumeModel(), // Initialize with empty strings
                        CoverLetter = new CoverLetterModel(), // Initialize with empty strings
                    };

                    await OpenAIService.PercentageAsync(genJob);

                    // Remove the job from the main jobs list
                    JobsViewModel.RemoveJobFromList(job);
                    jobs.Remove(job);
                }
            }
        }

#pragma warning disable VSTHRD100 // Avoid async void methods
        public async void OnDetailsClicked(object sender, EventArgs e)
#pragma warning restore VSTHRD100 // Avoid async void methods
        {
            if (sender is Button button && button.BindingContext is JobsModel job)
            {
                await DisplayAlert("Job Details",
                    $"Title: {job.jobTitle}\nLocation: {job.jobLocation}\nDescription: {job.jobDescription}",
                    "OK");
            }
        }
    }
}