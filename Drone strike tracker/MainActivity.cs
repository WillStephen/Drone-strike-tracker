using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Android.App;
using Android.Widget;
using Android.OS;

namespace Drone_strike_tracker
{
    [Activity(Label = "Drone_strike_tracker", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/Theme.Custom")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            var tlbMain = FindViewById<Toolbar>(Resource.Id.toolbar1);
            tlbMain.InflateMenu(Resource.Menu.maintoolbar);
            tlbMain.MenuItemClick += async delegate
            {
                await Task.Run(async () =>
                {
                    var client = new ApiClient();
                    var data = await client.GetStrikeListAsync();
                }
                );
            };
        }
    }
}