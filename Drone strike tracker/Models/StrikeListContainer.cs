using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Drone_strike_tracker.Models
{
    public class StrikeListContainer
    {
        public string Status { get; set; }

        public List<Strike> Strike { get; set; }
    }
}