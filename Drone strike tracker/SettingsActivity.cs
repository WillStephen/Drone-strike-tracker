using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.Widget;

namespace Drone_strike_tracker
{
    [Activity(Label = "SettingsActivity", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class SettingsActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Settings);
            var toolbar = FindViewById<Toolbar>(Resource.Id.topToolbar);
            toolbar.Title = "Settings";
            FragmentManager.BeginTransaction().Replace(Android.Resource.Id.Content, new SettingsFragment()).Commit();
        }
    }
}