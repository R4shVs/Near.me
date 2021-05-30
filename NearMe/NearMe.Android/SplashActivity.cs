using Android.App;
using Android.Content.PM;
using Android.OS;

namespace NearMe.Droid
{
    /*
     * Schermata iniziale
     */
    [Activity(Label = "Near.Me", Icon = "@mipmap/icon", Theme = "@style/Theme.Splash",
        MainLauncher = true, NoHistory = true, ScreenOrientation = ScreenOrientation.Portrait)]
    class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            StartActivity(typeof(MainActivity));
            Finish();
        }
    }
}