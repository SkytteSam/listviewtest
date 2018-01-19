using System;
namespace ListViewTest.Views
{
  public class ListViewTest
  {
    public bool IsOk { get; set; }
    public List<GroupListViewItem> Values
    {
      get { return values; }
      set { values = value; }
    }

    public List<Group> SelectedNeighbourGroups
    {
      get
      {
        var v = values.Where(i => i.IsSelecedNeighbour == true)
                        .Select(i => i.ThisGroup).ToList();
        return v;
      }
      set { selectedNeighbourGroups = value; }
    }


    private InputType _inputType;
    private CustomButton _okButton;
    private ListView groupsList;
    private readonly Group selectedGroup;

    private List<GroupListViewItem> values;
    private List<Group> selectedNeighbourGroups;

    public ListViewTest(string title,
                                Group selectedGroup,
                                List<Group> neighbourGroups,
                                string okButton,
                                string cancelButton)
    {
      this.selectedGroup = selectedGroup;
      this.selectedNeighbourGroups = neighbourGroups;
      InitPopup(title, okButton, cancelButton);
    }

    private List<Group> ToGroupListFromByteList(List<byte> bytes)
    {
      List<Group> groupList = new List<Group>();

      foreach (var item in bytes)
      {
        groupList.Add((Group)Enum.Parse(typeof(Group), item.ToString()));
      }

      return groupList;
    }

    void InitPopup(string title, string okButton, string cancelButton)
    {
      CreateTitle(title, Styling.Colors.PopupMessage);
      CreateMessage(string.Format(Localization.Current.SelectNeighbourGroupPopupMessage, Localization.Current.GetLocalizedEnumValue(selectedGroup)), Styling.Colors.PopupMessage);
      CreateGroupList();
      CreateGroupListView();
      CreateButton(okButton, cancelButton, Styling.Colors.Button, OnOkClicked, OnCancelClicked);
    }

    private void OnOkClicked()
    {
      RaiseOnResult();
    }

    private void OnCancelClicked()
    {
      RaiseOnResult();
    }

    protected override void CreateButton(string title, Color color, Action action)
    {
      //Not in use
    }

    protected override void CreateMessage(string message, Color color, string errorMessage = "")
    {
      var messageLabel = new CustomLabel
      {
        Text = message,
        FontSize = Styling.FontSizes.PopupText,
        FontName = FontName.DinOfficeLight,
        HorizontalOptions = LayoutOptions.CenterAndExpand,
        HorizontalTextAlignment = TextAlignment.Center,
        TextColor = color,
        LineBreakMode = LineBreakMode.TailTruncation,
        Lines = 0
      };

      Children.Add(messageLabel);
    }

    protected override void CreateTitle(string message, Color color)
    {
      var titleLabel = new CustomLabel
      {
        Text = message,
        FontSize = Styling.FontSizes.PopupTitle,
        FontName = FontName.DinOfficeMedium,
        HorizontalOptions = LayoutOptions.CenterAndExpand,
        TextColor = color,
        LineBreakMode = LineBreakMode.TailTruncation
      };

      Children.Add(titleLabel);
    }

    protected void CreateButton(string okTitle, string cancelTitle, Color color, Action okAction, Action cancelAction)
    {
      var layout = new StackLayout
      {
        Orientation = StackOrientation.Horizontal,
        HorizontalOptions = LayoutOptions.Center,
      };

      _okButton = new CustomButton
      {
        Text = okTitle,
        TextFontName = FontName.DinOfficeMedium,
        BackgroundColor = color,
        HorizontalOptions = LayoutOptions.Center,
        Padding = new Thickness(20, 5, 20, 5),
        Clickable = false
      };

      var cancelButton = new CustomButton
      {
        Text = cancelTitle,
        TextFontName = FontName.DinOfficeMedium,
        BackgroundColor = color,
        HorizontalOptions = LayoutOptions.Center,
        Padding = new Thickness(20, 5, 20, 5)
      };

      _okButton.Clicked += delegate {
        IsOk = true;
        okAction();
      };

      cancelButton.Clicked += delegate {
        IsOk = false;
        cancelAction();
      };

      layout.Children.Add(_okButton);
      layout.Children.Add(cancelButton);

      Children.Add(layout);
    }

    private void CreateGroupList()
    {
      SupportedList<Group> groupValues;
      (App.DeviceManager.Current as BleProductBase).SupportedGroups(out groupValues);
      var groups = groupValues.List.Cast<Group>().ToList();

      //Group 0 is not avelible for selection. 
      groups.Remove(Group.Group0);

      var groupListItems = new List<GroupListViewItem>();

      foreach (Group group in groups)
      {
        var groupListItem = new GroupListViewItem(group);

        groupListItem.IsSelectedGroup = (groupListItem.ThisGroup == selectedGroup) ? true : false;
        groupListItem.IsSelecedNeighbour = selectedNeighbourGroups.Contains(group) ? true : false;

        groupListItem.NeighbourSelecetedEvent += delegate {
          ControlIfMaxAmountIsSelected();
        };

        groupListItems.Add(groupListItem);
      }

      Values = groupListItems;
    }

    protected void CreateGroupListView()
    {
      groupsList = new ListView(ListViewCachingStrategy.RetainElement)
      {
        SeparatorVisibility = SeparatorVisibility.Default,
        SeparatorColor = Styling.Colors.PopupGreyButton,
      };
      groupsList.ItemsSource = Values;
      groupsList.ItemTemplate = new DataTemplate(typeof(GroupSwitchCell));
      groupsList.ScrollTo(values.FirstOrDefault(i => i.IsSelectedGroup), ScrollToPosition.Center, false);
      //Direct reset selected item to null. 
      groupsList.ItemSelected += (sender, e) => { groupsList.SelectedItem = null; };

      Children.Add(groupsList);
    }

    private void ControlIfMaxAmountIsSelected()
    {
      var neighbourList = values.Where(i => i.IsSelecedNeighbour == true).ToList();
      if (neighbourList.Count() >= 10)
      {
        foreach (var item in Values)
        {
          item.IsEnabled = false;
        }
      }
      else
      {
        foreach (var item in Values)
        {
          item.IsEnabled = true;
        }
      }

      _okButton.Clickable = neighbourList.Any();
    }

    public ListViewTest()
    {
    }
  }
}
