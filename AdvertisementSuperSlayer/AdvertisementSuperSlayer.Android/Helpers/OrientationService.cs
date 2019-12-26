using System;
using AdvertisementSuperSlayer.Droid.Helpers;
using AdvertisementSuperSlayer.Helpers;
using Android.App;
using Android.Content.PM;
using Xamarin.Forms;

[assembly: Dependency(typeof(OrientationService))]
namespace AdvertisementSuperSlayer.Droid.Helpers
{
    class OrientationService : IOrientationService
    {
        [Obsolete]
        public void Landscape()
        {
            ((Activity)Forms.Context).RequestedOrientation = ScreenOrientation.Landscape;
        }

        [Obsolete]
        public void Portrait()
        {
            ((Activity)Forms.Context).RequestedOrientation = ScreenOrientation.Portrait;
        }
    }
}