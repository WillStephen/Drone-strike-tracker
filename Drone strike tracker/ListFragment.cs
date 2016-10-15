using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Drone_strike_tracker.Models;

namespace Drone_strike_tracker
{
    public class ListFragment : Android.Support.V4.App.Fragment
    {
        private RecyclerView _mRecyclerView;
        private RecyclerView.LayoutManager _mLayoutManager;
        private StrikeListAdapter _mAdapter;
        private List<Strike> _strikeList;

        public ListFragment()
        {
        }

        public override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle bundle)
        {
            var rootView = inflater.Inflate(Resource.Layout.strikeList, container, false);

            _strikeList = new List<Strike>();

            _mRecyclerView = rootView.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            _mRecyclerView.SetBackgroundColor(new Color(238, 238, 238));

            _mLayoutManager = new LinearLayoutManager(Activity);

            _mRecyclerView.SetLayoutManager(_mLayoutManager);

            _mAdapter = new StrikeListAdapter(_strikeList);

            _mRecyclerView.SetAdapter(_mAdapter);

            var refresher = rootView.FindViewById<SwipeRefreshLayout>(Resource.Id.refresher);
            refresher.NestedScrollingEnabled = true;

            var progress = ProgressDialog.Show(this.Activity, "", "Checking for new strikes...", true);

            Task.Run(async () =>
            {
                await RefreshList(refresher);
            }).Wait();
            _mAdapter.StrikeList = _strikeList;
            _mAdapter.NotifyDataSetChanged();
            progress.Hide();

            refresher.Refresh += async delegate
            {
                await RefreshList(refresher);
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
                    startCount = _strikeList.Count;
                    addedElements = CompareLists.Compare(_strikeList, list.Strike);
                });
            }
            catch (Exception)
            {
                return;
            }
            //Activity.RunOnUiThread(() =>
            //{
            //    sender.Refreshing = false;
            //    if (addedElements > 0)
            //    {
            //        _mAdapter.NotifyItemRangeInserted(0, addedElements);
            //    }
            //    _mLayoutManager.ScrollToPosition(0);
            //});
        }
    }

    public class StrikeViewHolder : RecyclerView.ViewHolder
    {
        public TextView Title { get; }
        public TextView Subtitle { get; }
        public TextView Text { get; }
        public LinearLayout Container { get; }
        public LinearLayout ExpandedArea { get; }
        public ImageView ImageView { get; set; }

        public StrikeViewHolder(View itemView, Action<int> listener)
            : base(itemView)
        {
            Title = itemView.FindViewById<TextView>(Resource.Id.textView);
            Subtitle = itemView.FindViewById<TextView>(Resource.Id.textView1);
            Text = itemView.FindViewById<TextView>(Resource.Id.textView2);
            Container = itemView.FindViewById<LinearLayout>(Resource.Id.container);
            ExpandedArea = itemView.FindViewById<LinearLayout>(Resource.Id.expandedArea);
            ImageView = itemView.FindViewById<ImageView>(Resource.Id.mapView);
            ImageView.SetScaleType(ImageView.ScaleType.FitXy);

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
            vh.Title.Text = $"{currentStrike.Country}, {currentStrike.Date:dd MMMM}";
            vh.Subtitle.Text = $"{currentStrike.Deaths} deaths";
            vh.Text.Text = currentStrike.Narrative;
            vh.ImageView.SetImageResource(Resource.Drawable.Capture);
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