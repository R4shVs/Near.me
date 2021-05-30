using Xamarin.Forms;
using NearMe.Views;

namespace NearMe
{
    public partial class App : Application
    {
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider
                   .RegisterLicense(Data.Secrets.SyncfusionLicense);

            InitializeComponent();

            MainPage = new NavigationPage(new HomePage())
            {
                BarBackgroundColor = (Color)Resources["colorPrimary"],
                BarTextColor = Color.White,
            };
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
