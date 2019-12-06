using System;
using Android.Gms.Ads;

namespace AdvertisementSuperSlayer.Droid.Helpers
{
    class AdInterstitialListener : AdListener
    {
        private readonly InterstitialAd _interstitialAd;

        public AdInterstitialListener(InterstitialAd interstitialAd)
        {
            _interstitialAd = interstitialAd;
        }

        public Action AdLoaded { get; set; }
        public Action AdClosed { get; set; }
        public Action AdFailed { get; set; }

        public override void OnAdLoaded()
        {
            base.OnAdLoaded();

            if (_interstitialAd.IsLoaded)
            {
                _interstitialAd.Show();
            }
            AdLoaded?.Invoke();
        }

        public override void OnAdClosed()
        {
            base.OnAdClosed();
            AdClosed?.Invoke();
        }

        public override void OnAdFailedToLoad(int errorCode)
        {
            base.OnAdFailedToLoad(errorCode);
            AdFailed?.Invoke();
        }
    }
}