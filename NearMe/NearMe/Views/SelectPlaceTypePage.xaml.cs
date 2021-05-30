using NearMe.Models;
using NearMe.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NearMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectPlaceTypePage : ContentPage
    {
        public SelectPlaceTypePage(StartPoint startPoint)
        {
            ViewModel = new SelectPlaceTypeViewModel(this, startPoint);
            InitializeComponent();
            ViewModel.LoadDataCommand.Execute(null);
        }

        public SelectPlaceTypeViewModel ViewModel
        {
            get { return BindingContext as SelectPlaceTypeViewModel; }
            set { BindingContext = value; }
        }

        // Metodo invocato quando una tipologia viene premuta
        private void PlaceTypeTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            PlaceType placeType = e.ItemData as PlaceType;
            
            ViewModel.NavToNextPageCommand.Execute(placeType);
        }
    }
}