using System;
using SmartRemote.Views.Controls.CustomView;
using Xamarin.Forms;

namespace SmartRemote.Views.Controls.Popup.NeighbourGroupPopup
{
  public class GroupSwitchCell: ViewCell
  {

    private CustomLabel nameLabel; 
    private Switch selectedSwitch;

    public GroupSwitchCell()
    {
      nameLabel = new CustomLabel
      {
        Text = "Title",
        TextColor = Styling.Colors.PopupMessage,
        FontSize = Styling.FontSizes.Medium,
        HorizontalOptions = LayoutOptions.StartAndExpand,
        VerticalOptions = LayoutOptions.Center,
        Lines = 1,
      };
      nameLabel.SetBinding(CustomLabel.TextProperty, "Name");

      selectedSwitch = new CustomSwitch();
      selectedSwitch.VerticalOptions = LayoutOptions.Center;
      selectedSwitch.SetBinding(Switch.IsToggledProperty, "IsSelecedNeighbour");

      var stacklayout = new StackLayout()
      {
        Orientation = StackOrientation.Horizontal,
        HorizontalOptions = LayoutOptions.FillAndExpand,
        Children = {nameLabel, selectedSwitch}
      };

      View = stacklayout;
      this.SetBinding(ViewCell.IsEnabledProperty, "IsEnabled");
    }

    protected override void OnTapped()
    {
      base.OnTapped();
      selectedSwitch.IsToggled = !selectedSwitch.IsToggled;
    }

    protected override void OnPropertyChanged(string propertyName = null)
    {
      base.OnPropertyChanged(propertyName);

      if(propertyName == IsEnabledProperty.PropertyName)
      {
        selectedSwitch.IsEnabled = IsEnabled;
        nameLabel.Opacity = IsEnabled? 1:0.5;
      }
    }
  }
}
