using AdvertisementSuperSlayer.Helpers;
using AdvertisementSuperSlayer.iOS.Helpers;
using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(OrientationService))]
namespace AdvertisementSuperSlayer.iOS.Helpers
{
    class OrientationService : IOrientationService
    {
        public void Landscape()
        {
            UIDevice.CurrentDevice.SetValueForKey(new NSNumber((int)UIInterfaceOrientation.LandscapeLeft), new NSString("orientation"));
        }

        public void Portrait()
        {
            UIDevice.CurrentDevice.SetValueForKey(new NSNumber((int)UIInterfaceOrientation.Portrait), new NSString("orientation"));
        }
    }
}