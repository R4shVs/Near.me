using NearMe.Models;
using NearMe.ViewModels;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NearMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlacesOnMapPage : ContentPage
    {
        public PlacesOnMapPage(List<Place> places, Place focusPlace)
        {
            ViewModel = new PlacesOnMapViewModel(places, focusPlace);

            InitializeComponent();
            
            ViewModel.LoadDataCommand.Execute(map);
        }

        public PlacesOnMapViewModel ViewModel
        {
            get { return BindingContext as PlacesOnMapViewModel; }
            set { BindingContext = value; }
        }
    }
}