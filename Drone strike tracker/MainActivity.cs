using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Support.V4.App;
using Android.Support.V7.Widget;
using Android.Support.V7.App;
using Android.Support.V7.AppCompat;
using Android.Views;
using Android.Widget;
using Drone_strike_tracker.Models;
using ViewPager = Android.Support.V4.View.ViewPager;
using Fragment = Android.Support.V4.App.Fragment;
using FragmentManager = Android.Support.V4.App.FragmentManager;
using Toolbar = Android.Support.V7.Widget.Toolbar;

//using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace Drone_strike_tracker
{
    [Activity(Label = "Drone_strike_tracker", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/Theme.Custom")]
    public class MainActivity : AppCompatActivity
    {
        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            var tlbMain = FindViewById<Toolbar>(Resource.Id.toolbar1);
            tlbMain.InflateMenu(Resource.Menu.maintoolbar);

            SetSupportActionBar(tlbMain);
            SupportActionBar.SetDisplayShowTitleEnabled(true);
            SupportActionBar.SetTitle(Resource.String.ApplicationName);

            tlbMain.InflateMenu(Resource.Menu.maintoolbar);
            var fragments = new Fragment[]
            {
                new ListFragment(),
                new MapFragment()
            };

            var titles = Android.Runtime.CharSequence.ArrayFromStringArray(new[]
                {
                    "Strikes",
                    "Map",
                });

            var viewPager = FindViewById<ViewPager>(Resource.Id.viewpager);
            viewPager.Adapter = new TabsFragmentPagerAdapter(SupportFragmentManager, fragments, titles);

            var tabLayout = FindViewById<Android.Support.Design.Widget.TabLayout>(Resource.Id.tabs);
            tabLayout.SetTabTextColors(Resource.Color.normalTab, Resource.Color.selectedTab);
            tabLayout.SetupWithViewPager(viewPager);
            tabLayout.SetSelectedTabIndicatorColor(Resource.Color.accent);

            tlbMain.InflateMenu(Resource.Menu.maintoolbar);
        }

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.maintoolbar, menu);
            return base.OnPrepareOptionsMenu(menu);
        }
    }

    public class TabsFragmentPagerAdapter : FragmentPagerAdapter
    {
        private readonly Fragment[] fragments;

        private readonly Java.Lang.ICharSequence[] titles;

        public TabsFragmentPagerAdapter(FragmentManager fm, Fragment[] fragments, Java.Lang.ICharSequence[] titles) : base(fm)
        {
            this.fragments = fragments;
            this.titles = titles;
        }
        public override int Count
        {
            get
            {
                return fragments.Length;
            }
        }

        public override Fragment GetItem(int position)
        {
            return fragments[position];
        }

        public override Java.Lang.ICharSequence GetPageTitleFormatted(int position)
        {
            return titles[position];
        }
    }
}