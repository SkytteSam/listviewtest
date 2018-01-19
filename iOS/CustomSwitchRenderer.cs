using System;
using Arendi.SmartRemote.Renderers;
using SmartRemote;
using SmartRemote.Views.Controls.CustomView;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomSwitch), typeof(CustomSwitchRenderer))]
namespace Arendi.SmartRemote.Renderers
{
  public class CustomSwitchRenderer: SwitchRenderer
  {
    private UIColor orangeColor = Styling.Colors.Orange.ToUIColor();
    private UIColor greyColor = Styling.Colors.PopupGreyButton.ToUIColor();
    private UIColor lightGreyColor = Styling.Colors.PopupGreyButton.AddLuminosity(50).ToUIColor();

    protected override void OnElementChanged (ElementChangedEventArgs<Switch> e)
    {
      base.OnElementChanged (e);

      if (Control != null) 
      {
        // do whatever you want to the UISwitch here!
        Control.OnTintColor = orangeColor;

        var o = e.NewElement.IsEnabled;
      }
    }
  }
}
