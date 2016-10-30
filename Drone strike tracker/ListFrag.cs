using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Drone_strike_tracker.Models;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace Drone_strike_tracker
{
    public class ListFrag : Android.Support.V4.App.Fragment
    {
        private RecyclerView MrecyclerView { get; set; }
        private RecyclerView.LayoutManager MlayoutManager { get; set; }
        private StrikeListAdapter Madapter { get; set; }
        public List<Strike> StrikeList { get; set; }
        private SwipeRefreshLayout Refresher { get; set; }
        public event EventHandler<int> Refreshed;

        public bool Loaded;

        public override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
        }

        public override async void OnStart()
        {
            OnResume();
            if (Loaded != true)
            {
                var progress = ProgressDialog.Show(Activity, "", "Checking for new strikes...", true);
                await RefreshList(Refresher);
                progress.Hide();
                Snackbar.Make(MrecyclerView, $"{Madapter.ItemCount} strikes to date", Snackbar.LengthLong).SetAction("OK", (v) => { }).Show();

                Loaded = true;
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle bundle)
        {
            var rootView = inflater.Inflate(Resource.Layout.strikeList, container, false);

            StrikeList = new List<Strike>();

            MrecyclerView = rootView.FindViewById<RecyclerView>(Resource.Id.recyclerView);

            MlayoutManager = new LinearLayoutManager(Activity);

            MrecyclerView.SetLayoutManager(MlayoutManager);

            Madapter = new StrikeListAdapter(StrikeList);

            MrecyclerView.SetAdapter(Madapter);

            Refresher = rootView.FindViewById<SwipeRefreshLayout>(Resource.Id.refresher);
            Refresher.NestedScrollingEnabled = true;

            Madapter.MenuItemClick += (sender, i) =>
            {
                var strike = Madapter.StrikeList[i.Item2];

                switch (i.Item1.ItemId)
                {
                    case Resource.Id.viewTweet:
                    {
                        var uri = Android.Net.Uri.Parse($"{Settings.TwitterUrlStart}{strike.Tweet_Id}");
                        var intent = new Intent(Intent.ActionView, uri);
                        StartActivity(intent);
                        break;
                    }
                    case Resource.Id.searchInBrowser:
                    {
                        var searchTerm = $"{strike.Country} drone strike {strike.Date:dd MMMM yyyy}";
                        var uri = Android.Net.Uri.Parse(string.Format(Settings.GoogleSearchUrlTemplate, searchTerm));
                        var intent = new Intent(Intent.ActionView, uri);
                        StartActivity(intent);
                        break;
                    }
                    default:
                        return;
                }
            };

            Refresher.Refresh += async delegate
            {
                var startCount = Madapter.ItemCount;
                await RefreshList(Refresher);
                var newStrikes = Madapter.ItemCount - startCount;
                if (newStrikes > 0)
                {
                    Snackbar.Make(MrecyclerView, $"{newStrikes} new strikes", Snackbar.LengthLong).SetAction("OK", (v) => { }).Show();
                }
                else
                {
                    Snackbar.Make(MrecyclerView, "No new strikes", Snackbar.LengthLong).SetAction("OK", (v) => { }).Show();
                }
            };

            return rootView;
        }

        public async Task RefreshList(SwipeRefreshLayout sender)
        {
            var addedElements = 0;
            var startCount = 0;
            try
            {
                await Task.Run(async () =>
                {
                    var client = new ApiClient();
                    var list = await client.GetStrikeListAsync();
                    startCount = StrikeList.Count;
                    addedElements = CompareLists.Compare(Madapter.StrikeList, list.Strike);
                });
            }
            catch (Exception)
            {
                return;
            }

            sender.Refreshing = false;
            if (addedElements > 0)
            {
                Madapter.NotifyItemRangeInserted(0, addedElements);
            }
            MlayoutManager.ScrollToPosition(0);
            Refreshed.Invoke(Refreshed, 0);
        }
    }

    public class StrikeViewHolder : RecyclerView.ViewHolder
    {
        public ImageView Flag { get; set; }
        public TextView Region { get; }
        public TextView Country { get; }
        public TextView Date { get; }
        public TextView Deaths { get; }
        public TextView Narrative { get; }
        public LinearLayout ExpandedArea { get; }
        public CardView Card { get; }
        public Toolbar CardToolbar { get; }

        public StrikeViewHolder(View itemView, Action<int> listener)
            : base(itemView)
        {
            Flag = itemView.FindViewById<ImageView>(Resource.Id.flag);
            Region = itemView.FindViewById<TextView>(Resource.Id.region);
            Country = itemView.FindViewById<TextView>(Resource.Id.country);
            Date = itemView.FindViewById<TextView>(Resource.Id.txtDate);
            Deaths = itemView.FindViewById<TextView>(Resource.Id.txtDeaths);
            Narrative = itemView.FindViewById<TextView>(Resource.Id.txtNarrative);
            ExpandedArea = itemView.FindViewById<LinearLayout>(Resource.Id.expandedArea);
            Card = itemView.FindViewById<CardView>(Resource.Id.card);
            CardToolbar = itemView.FindViewById<Toolbar>(Resource.Id.cardToolbar);
            itemView.Click += (sender, e) => listener(AdapterPosition);
        }
    }

    public class StrikeListAdapter : RecyclerView.Adapter
    {
        public event EventHandler<int> ItemClick;
        public event EventHandler<Tuple<IMenuItem, int>> MenuItemClick;
        public List<Strike> StrikeList;
        //public int ExpandedPosition = -1;

        public StrikeListAdapter(List<Strike> strikeList)
        {
            StrikeList = strikeList;
        }

        public override RecyclerView.ViewHolder
            OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From(parent.Context).
            Inflate(Resource.Layout.StrikeCardView, parent, false);

            var vh = new StrikeViewHolder(itemView, OnClick);
            return vh;
        }

        public override void
            OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var vh = holder as StrikeViewHolder;
            var currentStrike = StrikeList[position];
            vh.Flag.SetImageResource(CountryConverter.Convert(currentStrike.Country));
            vh.Region.Text = currentStrike.Location + ", ";
            vh.Country.Text = currentStrike.Country;
            vh.Date.Text = $"{currentStrike.Date:dd MMMM yyyy}";
            vh.Deaths.Text = $"{currentStrike.Deaths} deaths";
            vh.Narrative.Text = currentStrike.Narrative;
            vh.CardToolbar.InflateMenu(Resource.Menu.cardtoolbar);
            vh.CardToolbar.MenuItemClick += (sender, args) =>
            {
                MenuItemClick(this, new Tuple<IMenuItem, int>(args.Item, position));
            };
            vh.ExpandedArea.Visibility = ViewStates.Gone;
            //vh.ExpandedArea.Visibility = position == ExpandedPosition ? ViewStates.Visible : ViewStates.Gone;
        }

        public override int ItemCount => StrikeList.Count;

        private void OnClick(int position)
        {
            //if (position == ExpandedPosition)
            //{
            //    ExpandedPosition = -1;
            //    NotifyItemChanged(position);
            //    return;
            //}

            //var lastPosition = ExpandedPosition;
            //ExpandedPosition = position;

            //NotifyItemChanged(lastPosition);
            //NotifyItemChanged(ExpandedPosition);
        }
    }
}