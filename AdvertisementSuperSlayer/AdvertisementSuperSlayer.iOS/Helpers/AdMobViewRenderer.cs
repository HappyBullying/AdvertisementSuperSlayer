using AdSupport;
using AdvertisementSuperSlayer;
using AdvertisementSuperSlayer.Browser;
using AdvertisementSuperSlayer.iOS.Helpers;
using CoreGraphics;
using Google.MobileAds;
using System;
using System.ComponentModel;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(AAdMobView), typeof(AdMobViewRenderer))]
namespace AdvertisementSuperSlayer.iOS.Helpers
{
    class AdMobViewRenderer : ViewRenderer<AAdMobView, BannerView>
    {
        BannerView adView;
        bool viewOnScreen;

        protected override void OnElementChanged(ElementChangedEventArgs<AAdMobView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement == null)
                return;

            if (e.OldElement == null)
            {
                //if your app privacy →Advertising →Limit Ad Tracking is ensure that is disable.
                if (ASIdentifierManager.SharedManager.IsAdvertisingTrackingEnabled)
                {

                }
                //here add adUnitID
                adView = new BannerView(AdSizeCons.Banner, new CGPoint(0, 0))
                {
                    AdUnitID = Element.AdUnitId,
                    RootViewController = GetRootViewController()
                };
                //if change you adsize like mobile orientation landscape/portrait
                adView.WillChangeAdSizeTo += (sender, args) =>
                {

                };
                //when application will be leave
                adView.WillLeaveApplication += (sender, args) =>
                {

                };
                //when screen dismissed
                adView.ScreenDismissed += (sender, args) =>
                {

                };
                //when screen will apear
                adView.WillPresentScreen += (sender, args) =>
                {

                };

                //call when ads received from google ads
                adView.AdReceived += (sender, args) =>
                {
                    viewOnScreen = true;
                    if (!viewOnScreen) this.AddSubview(adView);
                };

                var request = Request.GetDefaultRequest();

                //e.NewElement.HeightRequest = GetSmartBannerDpHeight();

                adView.LoadRequest(request);

                adView.ReceiveAdFailed += (object sender, BannerViewErrorEventArgs ea) => {
                    viewOnScreen = false;
                    adView.LoadRequest(request);
                };

                base.SetNativeControl(adView);
            }
        }

        private UIViewController GetRootViewController()
        {
            foreach (UIWindow window in UIApplication.SharedApplication.Windows)
            {
                if (window.RootViewController != null)
                {
                    return window.RootViewController;
                }
            }

            return null;
        }

        private int GetSmartBannerDpHeight()
        {
            var dpHeight = (double)UIScreen.MainScreen.Bounds.Height;

            if (dpHeight <= 400) return 32;
            if (dpHeight > 400 && dpHeight <= 720) return 50;
            return 90;
        }
    }
}