using Resume_Builder_MAUI.Model;
using Resume_Builder_MAUI.ViewModel;

namespace Resume_Builder_MAUI.View;

public partial class UserSetting : ContentPage
{
    readonly UserModel userModel;
    private UserViewModel ViewModel { get; set; } = new UserViewModel();
    public UserSetting()
	{
		InitializeComponent();

        userModel = GlobalVarables.userInfo.User;

        BindingContext = userModel;


        // Optionally set up section contexts for entry forms
        EducationSection.BindingContext = ViewModel.educationEntry;
        WorkSection.BindingContext = ViewModel.workEntry;
        CertificateSection.BindingContext = ViewModel.certificateEntry;
        SkillSection.BindingContext = ViewModel.skillEntry;
        ProjectSection.BindingContext = ViewModel.projectEntry;
    }

#pragma warning disable VSTHRD100 // Avoid async void methods
    private async void OnDeleteDbClicked(object sender, EventArgs e)
#pragma warning restore VSTHRD100 // Avoid async void methods
    {
        bool confirm = await DisplayAlert("Confirm", "Delete all user data? This cannot be undone.", "Yes", "No");
        if (confirm)
        {
            try
            {
                await GlobalVarables.service.EmptyAllTablesAsync();
                await DisplayAlert("Deleted", "User database deleted.", "OK");
                // Optionally, clear userInfo
                GlobalVarables.userInfo = new UserViewModel();
                // Navigate back to the main page or reset the app state
                await Shell.Current.GoToAsync("//MainPage");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to delete database: {ex.Message}", "OK");
            }
        }
    }


#pragma warning disable VSTHRD100 // Avoid async void methods
    private async void OnAddEducationClicked(object sender, EventArgs e)
#pragma warning restore VSTHRD100 // Avoid async void methods
    {
        if (string.IsNullOrWhiteSpace(ViewModel.educationEntry.InstitutionName) ||
            string.IsNullOrWhiteSpace(ViewModel.educationEntry.Location) ||
            string.IsNullOrWhiteSpace(ViewModel.educationEntry.Date))
        {
            await DisplayAlert("Error", "Please fill in all fields for education entry.", "OK");
            return;
        }

        AddEducationBTN.Text = "Add Additional Education";

        await GlobalVarables.service.SaveEducationAsync(ViewModel.educationEntry);

        // Validate the education entry before adding
        ViewModel.Education.Add(new EducationEntry
        {
            InstitutionName = ViewModel.educationEntry.InstitutionName,
            Location = ViewModel.educationEntry.Location,
            Date = ViewModel.educationEntry.Date,
            Details = ViewModel.educationEntry.Details
        });
        ViewModel.educationEntry = new EducationEntry();
        EducationSection.BindingContext = ViewModel.educationEntry;
    }

#pragma warning disable VSTHRD100 // Avoid async void methods
    private async void OnAddWorkClicked(object sender, EventArgs e)
#pragma warning restore VSTHRD100 // Avoid async void methods
    {
        if (string.IsNullOrWhiteSpace(ViewModel.workEntry.CompanyName) ||
            string.IsNullOrWhiteSpace(ViewModel.workEntry.Location) ||
            string.IsNullOrWhiteSpace(ViewModel.workEntry.Date))
        {
            await DisplayAlert("Error", "Please fill in all fields for work entry.", "OK");
            return;
        }

        AddWorkBTN.Text = "Add Additional Work/Project Experience";

        await GlobalVarables.service.SaveWorkAsync(ViewModel.workEntry);

        ViewModel.WorkExperience.Add(new WorkEntry
        {
            CompanyName = ViewModel.workEntry.CompanyName,
            Location = ViewModel.workEntry.Location,
            Date = ViewModel.workEntry.Date,
            Details = ViewModel.workEntry.Details
        });
        ViewModel.workEntry = new WorkEntry();
        WorkSection.BindingContext = ViewModel.workEntry;
    }

#pragma warning disable VSTHRD100 // Avoid async void methods
    private async void OnAddCertificateClicked(object sender, EventArgs e)
#pragma warning restore VSTHRD100 // Avoid async void methods
    {
        if (string.IsNullOrWhiteSpace(ViewModel.certificateEntry.CertificateName) ||
            string.IsNullOrWhiteSpace(ViewModel.certificateEntry.Details))
        {
            await DisplayAlert("Error", "Please fill in all fields for certificate entry.", "OK");
            return;
        }

        AddCertBTN.Text = "Add Additional Work/Project Experience";

        await GlobalVarables.service.SaveCertificationAsync(ViewModel.certificateEntry);

        ViewModel.Certificates.Add(new CertificateEntry
        {
            CertificateName = ViewModel.certificateEntry.CertificateName,
            Details = ViewModel.certificateEntry.Details
        });
        ViewModel.certificateEntry = new CertificateEntry();
        CertificateSection.BindingContext = ViewModel.certificateEntry;
    }

#pragma warning disable VSTHRD100 // Avoid async void methods
    private async void OnAddSkillClicked(object sender, EventArgs e)
#pragma warning restore VSTHRD100 // Avoid async void methods
    {
        if (string.IsNullOrWhiteSpace(ViewModel.skillEntry.SkillName))
        {
            await DisplayAlert("Error", "Please enter a skill name.", "OK");
            return;
        }

        await GlobalVarables.service.SaveSkillAsync(ViewModel.skillEntry);

        ViewModel.Skills.Add(new SkillEntry
        {
            SkillName = ViewModel.skillEntry.SkillName
        });
        ViewModel.skillEntry = new SkillEntry();
        SkillSection.BindingContext = ViewModel.skillEntry;
    }

#pragma warning disable VSTHRD100 // Avoid async void methods
    private async void OnAddProjectClicked(object sender, EventArgs e)
#pragma warning restore VSTHRD100 // Avoid async void methods
    {
        if (string.IsNullOrWhiteSpace(ViewModel.projectEntry.ProjectName))
        {
            await DisplayAlert("Error", "Please enter a project name.", "OK");
            return;
        }

        await GlobalVarables.service.SaveProjectAsync(ViewModel.projectEntry);

        ViewModel.Projects.Add(new ProjectEntry
        {
            ProjectName = ViewModel.projectEntry.ProjectName,
            Description = ViewModel.projectEntry.Description
        });
        ViewModel.projectEntry = new ProjectEntry();
        ProjectSection.BindingContext = ViewModel.projectEntry;
    }
    private void ToggleEducationButton_Clicked(object sender, EventArgs e)
    {
        ToggleEducationButton.Source = EducationDropdown.IsVisible ? "arrow_downward.png" : "arrow_upward.png";
        EducationDropdown.IsVisible = !EducationDropdown.IsVisible;
    }

    private void ToggleWorkButton_Clicked(object sender, EventArgs e)
    {
        ToggleWorkButton.Source = WorkDropdown.IsVisible ? "arrow_downward.png" : "arrow_upward.png";
        WorkDropdown.IsVisible = !WorkDropdown.IsVisible;
    }

    private void ToggleCertificateButton_Clicked(object sender, EventArgs e)
    {
        ToggleCertificateButton.Source = CertificateDropdown.IsVisible ? "arrow_downward.png" : "arrow_upward.png";
        CertificateDropdown.IsVisible = !CertificateDropdown.IsVisible;
    }

    private void ToggleSkillButton_Clicked(object sender, EventArgs e)
    {
        ToggleSkillButton.Source = SkillDropdown.IsVisible ? "arrow_downward.png" : "arrow_upward.png";
        SkillDropdown.IsVisible = !SkillDropdown.IsVisible;
    }

    private void ToggleProjectButton_Clicked(object sender, EventArgs e)
    {
        ToggleProjectButton.Source = ProjectDropdown.IsVisible ? "arrow_downward.png" : "arrow_upward.png";
        ProjectDropdown.IsVisible = !ProjectDropdown.IsVisible;
    }

}