using System;
using System.Threading.Tasks;
using AdvertisementSuperSlayer.Droid.Helpers;
using AdvertisementSuperSlayer.Helpers;
using Android.Gms.Ads;
using Xamarin.Forms;

[assembly: Dependency(typeof(AdmobInterstitial))]
namespace AdvertisementSuperSlayer.Droid.Helpers
{
    class AdmobInterstitial : AdmobHelper
    {
        [Obsolete]
        public Task Display(string adId)
        {
            var displayTask = new TaskCompletionSource<bool>();
            InterstitialAd AdInterstitial = new InterstitialAd(Forms.Context)
            {
                AdUnitId = adId
            };
            {
                var adInterstitialListener = new AdInterstitialListener(AdInterstitial)
                {
                    AdClosed = () =>
                    {
                        if (displayTask != null)
                        {
                            displayTask.TrySetResult(AdInterstitial.IsLoaded);
                            displayTask = null;
                        }
                    },
                    AdFailed = () =>
                    {
                        if (displayTask != null)
                        {
                            displayTask.TrySetResult(AdInterstitial.IsLoaded);
                            displayTask = null;
                        }
                    }
                };

                AdRequest.Builder requestBuilder = new AdRequest.Builder();
                AdInterstitial.AdListener = adInterstitialListener;
                AdInterstitial.LoadAd(requestBuilder.Build());
            }

            return Task.WhenAll(displayTask.Task);
        }
    }
}