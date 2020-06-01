using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreGraphics;
using Foundation;
using Google.MobileAds;
using GuessNumber.Controls;
using GuessNumber.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(AdControlView),typeof(AdControlViewRenderer))]
namespace GuessNumber.iOS.Renderers 
{
    public class AdControlViewRenderer : ViewRenderer<AdControlView, BannerView>
    {
        private BannerView _bannerView;

        protected override void OnElementChanged(ElementChangedEventArgs<AdControlView> e)
        {
            base.OnElementChanged(e);

            if (Control == null && e.NewElement != null)
                SetNativeControl(CreateBannerView());
        }

        private BannerView CreateBannerView()
        {
            if (_bannerView != null)
                return _bannerView;

            var origin = new CGPoint(0, UIScreen.MainScreen.Bounds.Size.Height - AdSizeCons.Banner.Size.Height);

            _bannerView = new BannerView(AdSizeCons.SmartBannerPortrait, origin)
            {
                AdUnitID = Element.AdUnitId,
                RootViewController = GetVisibleViewController()
            };

            _bannerView.LoadRequest(GetRequest());

            return _bannerView;
        }

        private Request GetRequest()
        {
            var request = Request.GetDefaultRequest();
#if DEBUG
            request.TestDevices = new[] { Request.SimulatorId.ToString() };
#endif

            return request;

        }

        private UIViewController GetVisibleViewController()
        {
            foreach (UIWindow window in UIApplication.SharedApplication.Windows)
            {
                if (window.RootViewController != null)
                    return window.RootViewController;
            }

            return null;
        }
    }
}