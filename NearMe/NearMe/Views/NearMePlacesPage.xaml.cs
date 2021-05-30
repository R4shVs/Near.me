using NearMe.Models;
using NearMe.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NearMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NearMePlacesPage : ContentPage
    {
        public NearMePlacesPage(StartPoint st, PlaceType pt)
        {
            ViewModel = new NearMePlacesViewModel(this, st, pt);
            InitializeComponent();
            
            Title = pt.Title;

            ViewModel.LoadDataCommand.Execute(null);
        }

        public NearMePlacesViewModel ViewModel
        {
            get { return BindingContext as NearMePlacesViewModel; }
            set { BindingContext = value; }
        }

        /*
         * Metodo invocato quando viene premuto il bottone
         * per tornare alla pagina precedente
         */
        protected override bool OnBackButtonPressed()
        {
            ViewModel.OnBackButtonPressedCommand.Execute(null);

            return base.OnBackButtonPressed();
        }

        private void PlaceTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            Place place = e.ItemData as Place;
            
            ViewModel.NavToNextPageCommand.Execute(place);
        }
    }
}