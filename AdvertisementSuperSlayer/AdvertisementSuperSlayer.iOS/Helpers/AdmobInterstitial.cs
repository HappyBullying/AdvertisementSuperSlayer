using Google.MobileAds;
using System;
using AdvertisementSuperSlayer.Helpers;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Forms;
using AdvertisementSuperSlayer.iOS.Helpers;

[assembly: Dependency(typeof(AdmobInterstitial))]
namespace AdvertisementSuperSlayer.iOS.Helpers
{
    class AdmobInterstitial : AdmobHelper
    {
        public Task Display(string adId)
        {
            TaskCompletionSource<bool> displayAdTask = new TaskCompletionSource<bool>();
            Interstitial interstitial = new Interstitial(adId);

            interstitial.AdReceived += (sender, args) =>
            {
                if (interstitial.IsReady)
                {
                    UIWindow keyWindow = UIApplication.SharedApplication.KeyWindow;
                    UIViewController rootViewController = keyWindow.RootViewController;

                    while (rootViewController.PresentedViewController != null)
                    {
                        rootViewController = rootViewController.PresentedViewController;
                    }
                }
            };


            interstitial.ScreenDismissed += (sender, args) =>
            {
                if (displayAdTask != null)
                {
                    displayAdTask.TrySetResult(interstitial.IsReady);
                    displayAdTask = null;
                }
            };


            interstitial.ReceiveAdFailed += (sender, e) =>
            {
                displayAdTask.TrySetResult(false);
                displayAdTask.TrySetCanceled();
                displayAdTask = null;
            };

            Request request = Request.GetDefaultRequest();
            interstitial.LoadRequest(request);
            return Task.WhenAll(displayAdTask.Task);
        }
    }
}