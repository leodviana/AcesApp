using Android.App;
using Android.Content;
using Android.Util;
using AndroidX.AppCompat.App;
using System.Threading.Tasks;

namespace AcesApp.Droid
{
    [Activity(Theme = "@style/MainTheme.Splash",
              MainLauncher = false,
              NoHistory = true)]
    public class SplashActivity2 : AppCompatActivity
    {
        static readonly string TAG = "X:" + typeof(SplashActivity).Name;
        // Launches the startup task
        protected override void OnResume()
        {
            base.OnResume();
            Task startupWork = new Task(() => { SimulateStartup(); });
            startupWork.Start();
        }
        public override void OnBackPressed() { }

        // Simulates background work that happens behind the splash screen
        async void SimulateStartup()
        {
            Log.Debug(TAG, "Performing some startup work that takes a bit of time.");

            //await Task.Delay(12000);

            Log.Debug(TAG, "Startup work is finished - starting MainActivity.");
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}
