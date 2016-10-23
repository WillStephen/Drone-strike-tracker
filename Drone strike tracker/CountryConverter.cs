using System.Globalization;
using System.Linq;
using Android;
using Android.Content.Res;

namespace Drone_strike_tracker
{
    public static class CountryConverter
    {
        public static int Convert(string country)
        {
            switch (country)
            {
                case "Yemen":
                    {
                        return Resource.Drawable.ye;
                    }
                case "Pakistan":
                    {
                        return Resource.Drawable.pk;
                    }
                case "Somalia":
                    {
                        return Resource.Drawable.so;
                    }
                case "Syria":
                    {
                        return Resource.Drawable.sy;
                    }
                case "Turkey":
                    {
                        return Resource.Drawable.tr;
                    }
                case "Russia":
                    {
                        return Resource.Drawable.ru;
                    }
                default:
                    {
                        //CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
                        //CultureInfo cInfo = cultures.FirstOrDefault(culture => new RegionInfo(culture.LCID).EnglishName == country);
                        //return (Resources.GetIdentifier(cInfo.TwoLetterISOLanguageName, "drawable", "Drone_strike_tracker.Drone_strike_tracker");
                        return 6;
                    }
            }
        }
    }
}