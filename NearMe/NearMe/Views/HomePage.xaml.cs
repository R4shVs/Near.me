using NearMe.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NearMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            ViewModel = new HomeViewModel(this);

            InitializeComponent();

            ViewModel.LoadDataCommand.Execute(null);
        }

        public HomeViewModel ViewModel
        {
            get { return BindingContext as HomeViewModel; }
            set { BindingContext = value; }
        }
    }
}