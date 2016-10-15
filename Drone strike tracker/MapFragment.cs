
using Android.App;
using Android.OS;
using Android.Views;

namespace Drone_strike_tracker
{
    public class MapFragment : Android.Support.V4.App.Fragment
    {
        public MapFragment()
        {
        }

        public override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle bundle)
        {
            return inflater.Inflate(Resource.Layout.mapfrag, container, false);
        }
    }
}