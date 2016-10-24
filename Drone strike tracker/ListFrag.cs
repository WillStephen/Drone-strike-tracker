using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Drone_strike_tracker.Models;

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

            Refresher.Refresh += async delegate
            {
                await RefreshList(Refresher);
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
                    var list = await client.GetStrikeListAsync(ApiClient.RequestType.Min);
                    startCount = StrikeList.Count;
                    addedElements = CompareLists.Compare(StrikeList, list.Strike);
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
            //Card.InflateMenu(Resource.Menu.maintoolbar);
            itemView.Click += (sender, e) => listener(AdapterPosition);
        }
    }

    public class StrikeListAdapter : RecyclerView.Adapter
    {
        public event EventHandler<int> ItemClick;
        public List<Strike> StrikeList;
        public int ExpandedPosition = -1;

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
            vh.Region.Text = "";
            vh.Country.Text = currentStrike.Country;
            vh.Date.Text = $"{currentStrike.Date:dd MMMM yyyy}";
            vh.Deaths.Text = $"{currentStrike.Deaths} deaths";
            vh.Narrative.Text = currentStrike.Narrative;
            vh.ExpandedArea.Visibility = position == ExpandedPosition ? ViewStates.Visible : ViewStates.Gone;
        }

        public override int ItemCount => StrikeList.Count;

        private void OnClick(int position)
        {
            if (position == ExpandedPosition)
            {
                ExpandedPosition = -1;
                NotifyItemChanged(position);
                return;
            }

            var lastPosition = ExpandedPosition;
            ExpandedPosition = position;

            NotifyItemChanged(lastPosition);
            NotifyItemChanged(ExpandedPosition);
        }
    }
}