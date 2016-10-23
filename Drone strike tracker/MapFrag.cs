using System;
using System.Collections.Generic;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Media;
using Android.OS;
using Android.Views;
using Drone_strike_tracker.Models;

namespace Drone_strike_tracker
{
    public class MapFrag : Android.Support.V4.App.Fragment, IOnMapReadyCallback
    {
        public event Action<GoogleMap> MapReadyAction;
        public GoogleMap Map { get; private set; }
        public SupportMapFragment MapFragment { get; set; }
        public int MapType;

        public void OnMapReady(GoogleMap googleMap)
        {
            Map = googleMap;
            MapReadyAction?.Invoke(Map);
        }

        public override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle bundle)
        {
            var layout = inflater.Inflate(Resource.Layout.mapfrag, container, false);

            var mapFrag = (SupportMapFragment)ChildFragmentManager.FindFragmentById(Resource.Id.map);
            
            mapFrag.GetMapAsync(this);

            switch (Settings.MapType)
            {
                case "Hybrid":
                {
                    MapType = 4;
                    break;
                }
                case "Normal":
                {
                    MapType = 1;
                    break;
                }
                case "Satellite":
                {
                    MapType = 2;
                    break;
                }
                case "Terrain":
                {
                    MapType = 3;
                    break;
                }
                default:
                {
                    MapType = 0;
                    break;
                }
            }

            MapReadyAction += delegate (GoogleMap googleMap)
            {
                Map = googleMap;
                Map.MapType = MapType;
                Map.UiSettings.ZoomControlsEnabled = true;
                Map.UiSettings.CompassEnabled = false;
            };

            return layout;
        }

        public void AddMarkers(List<Strike> strikeList)
        {
            foreach (var strike in strikeList)
            {
                if (strike.Lat != null && strike.Lon != null)
                {
                    var markerOpt1 = new MarkerOptions();
                    markerOpt1.SetPosition(new LatLng(strike.Lat.Value, strike.Lon.Value));
                    markerOpt1.SetTitle($"{strike.Country}, {strike.Date:dd MMMM, yyyy}");
                    Map.AddMarker(markerOpt1);
                }   
            }

            var location = new LatLng(15, 48);
            var builder = CameraPosition.InvokeBuilder();
            builder.Target(location);
            builder.Zoom(2);
            builder.Bearing(0);
            builder.Tilt(0);
            var cameraPosition = builder.Build();
            var cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);

            Map.MoveCamera(cameraUpdate);
        }
    }
}