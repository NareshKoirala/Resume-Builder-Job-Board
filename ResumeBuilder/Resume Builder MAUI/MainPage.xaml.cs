using Resume_Builder_MAUI.Services;
using Resume_Builder_MAUI.ViewModel;

namespace Resume_Builder_MAUI
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            NavToUserInfoPage.Clicked += NavToUserInfoPage_Clicked;
        }

#pragma warning disable VSTHRD100 // Avoid async void methods
        private async void NavToUserInfoPage_Clicked(object? sender, EventArgs e)
#pragma warning restore VSTHRD100 // Avoid async void methods
        {
            await GlobalVarables.service.InitializeTablesAsync();

            var user = await GlobalVarables.service.GetUserAsync();
            if (user == null)
            {
                await Shell.Current.GoToAsync("//UserInput");
            }
            else
            {
                GlobalVarables.userInfo.User = user;
                GlobalVarables.jobsInfo = await JobsViewModel.FetchJobsAsync();
                await Shell.Current.GoToAsync("//MainDashboard");
            }

        }
    }
}
