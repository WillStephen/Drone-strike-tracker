using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;

namespace Drone_strike_tracker
{
    [Activity(Label = "SettingsActivity", Theme = "@style/Theme.Custom")]
    public class SettingsActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Settings);
            var toolbar = FindViewById<Toolbar>(Resource.Id.topToolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "Settings";
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);

            FragmentManager.BeginTransaction().Replace(Android.Resource.Id.Content, new SettingsFragment()).Commit();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                OnBackPressed();
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}