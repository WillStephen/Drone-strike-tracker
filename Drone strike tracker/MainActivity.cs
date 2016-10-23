using System;
using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Support.V7.App;
using Android.Views;
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
        private Color Selected { get; set; }
        private Color Deselected { get; set; }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            
            var tlbMain = FindViewById<Toolbar>(Resource.Id.topToolbar);
            tlbMain.InflateMenu(Resource.Menu.maintoolbar);

            SetSupportActionBar(tlbMain);
            SupportActionBar.SetDisplayShowTitleEnabled(true);
            SupportActionBar.SetTitle(Resource.String.ApplicationName);

            var mapFrag = new MapFrag();
            var listFrag = new ListFrag();
            var fragments = new Fragment[]
            {
                listFrag,
                mapFrag
            };

            var titles = Android.Runtime.CharSequence.ArrayFromStringArray(new[]
                {
                    "Strikes",
                    "Map",
                });

            var viewPager = FindViewById<ViewPager>(Resource.Id.viewpager);
            var adapter = new TabsFragmentPagerAdapter(SupportFragmentManager, fragments, titles);
            viewPager.Adapter = adapter;

            var tabLayout = FindViewById<Android.Support.Design.Widget.TabLayout>(Resource.Id.tabs);
            tabLayout.SetSelectedTabIndicatorColor(Resource.Color.selectedTab);

            tabLayout.SetupWithViewPager(viewPager);

            listFrag.Refreshed += delegate
            {
                mapFrag.MapReadyAction += delegate
                {
                    mapFrag.AddMarkers(listFrag.StrikeList);
                };
            };

            mapFrag.MapReadyAction += delegate
            {
                listFrag.Refreshed += delegate
                {
                    mapFrag.AddMarkers(listFrag.StrikeList);
                };
            };

            Selected = new Color(ContextCompat.GetColor(this, Resource.Color.selectedTab));
            Deselected = new Color(ContextCompat.GetColor(this, Resource.Color.normalTab));

            tabLayout.GetTabAt(0).SetIcon(Resource.Drawable.ic_list_white_24dp);
            tabLayout.GetTabAt(1).SetIcon(Resource.Drawable.ic_map_white_24dp);

            UpdateTabs(viewPager.CurrentItem, tabLayout, tlbMain, adapter);

            viewPager.PageSelected += (sender, args) =>
            {
                UpdateTabs(args.Position, tabLayout, tlbMain, adapter);
            };
        }

        public void UpdateTabs(int selectedPosition, TabLayout tabs, Toolbar tlb, TabsFragmentPagerAdapter adapter)
        {
            tabs.GetTabAt(selectedPosition).Icon.SetColorFilter(Selected, PorterDuff.Mode.SrcIn);
            tabs.GetTabAt(1 - selectedPosition).Icon.SetColorFilter(Deselected, PorterDuff.Mode.SrcIn);
            tlb.Title = adapter.GetTitle(selectedPosition);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.maintoolbar, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menu_settings:
                {
                    StartActivity(typeof(SettingsActivity));
                    return true;
                }

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }

    public class TabsFragmentPagerAdapter : FragmentPagerAdapter
    {
        private readonly Fragment[] _fragments;

        private readonly Java.Lang.ICharSequence[] _titles;

        public TabsFragmentPagerAdapter(FragmentManager fm, Fragment[] fragments, Java.Lang.ICharSequence[] titles) : base(fm)
        {
            _fragments = fragments;
            _titles = titles;
        }
        public override int Count => _fragments.Length;

        public override Fragment GetItem(int position)
        {
            return _fragments[position];
        }

        public string GetTitle(int position)
        {
            return _titles[position].ToString();
        }

        public override Java.Lang.ICharSequence GetPageTitleFormatted(int position)
        {
            return null;
        }
    }
}