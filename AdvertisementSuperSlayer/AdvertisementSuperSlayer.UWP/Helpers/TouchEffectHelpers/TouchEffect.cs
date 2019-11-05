using System;
using System.Linq;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ResolutionGroupName("XamarinDocs")]
[assembly: ExportEffect(typeof(AdvertisementSuperSlayer.UWP.Helpers.TouchEffectHelpers.TouchEffect), "TouchEffect")]

namespace AdvertisementSuperSlayer.UWP.Helpers.TouchEffectHelpers
{
    public class TouchEffect : PlatformEffect
    {
        FrameworkElement frameworkElement;
        AdvertisementSuperSlayer.Helpers.TouchEffectHelpers.TouchEffect effect;
        Action<Element, AdvertisementSuperSlayer.Helpers.TouchEffectHelpers.TouchActionEventArgs> onTouchAction;

        protected override void OnAttached()
        {
            // Get the Windows FrameworkElement corresponding to the Element that the effect is attached to
            frameworkElement = Control == null ? Container : Control;

            // Get access to the TouchEffect class in the .NET Standard library
            effect = (AdvertisementSuperSlayer.Helpers.TouchEffectHelpers.TouchEffect)Element.Effects.
                        FirstOrDefault(e => e is AdvertisementSuperSlayer.Helpers.TouchEffectHelpers.TouchEffect);

            if (effect != null && frameworkElement != null)
            {
                // Save the method to call on touch events
                onTouchAction = effect.OnTouchAction;

                // Set event handlers on FrameworkElement
                frameworkElement.PointerEntered += OnPointerEntered;
                frameworkElement.PointerPressed += OnPointerPressed;
                frameworkElement.PointerMoved += OnPointerMoved;
                frameworkElement.PointerReleased += OnPointerReleased;
                frameworkElement.PointerExited += OnPointerExited;
                frameworkElement.PointerCanceled += OnPointerCancelled;
            }
        }

        protected override void OnDetached()
        {
            if (onTouchAction != null)
            {
                // Release event handlers on FrameworkElement
                frameworkElement.PointerEntered -= OnPointerEntered;
                frameworkElement.PointerPressed -= OnPointerPressed;
                frameworkElement.PointerMoved -= OnPointerMoved;
                frameworkElement.PointerReleased -= OnPointerReleased;
                frameworkElement.PointerExited -= OnPointerEntered;
                frameworkElement.PointerCanceled -= OnPointerCancelled;
            }
        }

        void OnPointerEntered(object sender, PointerRoutedEventArgs args)
        {
            CommonHandler(sender, AdvertisementSuperSlayer.Helpers.TouchEffectHelpers.TouchActionType.Entered, args);
        }

        void OnPointerPressed(object sender, PointerRoutedEventArgs args)
        {
            CommonHandler(sender, AdvertisementSuperSlayer.Helpers.TouchEffectHelpers.TouchActionType.Pressed, args);

            // Check setting of Capture property
            if (effect.Capture)
            {
                (sender as FrameworkElement).CapturePointer(args.Pointer);
            }
        }

        void OnPointerMoved(object sender, PointerRoutedEventArgs args)
        {
            CommonHandler(sender, AdvertisementSuperSlayer.Helpers.TouchEffectHelpers.TouchActionType.Moved, args);
        }

        void OnPointerReleased(object sender, PointerRoutedEventArgs args)
        {
            CommonHandler(sender, AdvertisementSuperSlayer.Helpers.TouchEffectHelpers.TouchActionType.Released, args);
        }

        void OnPointerExited(object sender, PointerRoutedEventArgs args)
        {
            CommonHandler(sender, AdvertisementSuperSlayer.Helpers.TouchEffectHelpers.TouchActionType.Exited, args);
        }

        void OnPointerCancelled(object sender, PointerRoutedEventArgs args)
        {
            CommonHandler(sender, AdvertisementSuperSlayer.Helpers.TouchEffectHelpers.TouchActionType.Cancelled, args);
        }

        void CommonHandler(object sender, AdvertisementSuperSlayer.Helpers.TouchEffectHelpers.TouchActionType touchActionType, PointerRoutedEventArgs args)
        {
            PointerPoint pointerPoint = args.GetCurrentPoint(sender as UIElement);
            Windows.Foundation.Point windowsPoint = pointerPoint.Position;

            onTouchAction(Element, new AdvertisementSuperSlayer.Helpers.TouchEffectHelpers.TouchActionEventArgs(args.Pointer.PointerId,
                                                            touchActionType,
                                                            new Point(windowsPoint.X, windowsPoint.Y),
                                                            args.Pointer.IsInContact));
        }
    }
}
