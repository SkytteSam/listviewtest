using System;
using Arendi.SmartRemoteLibrary.Product.Values;

namespace SmartRemote.Views.Controls.Popup.NeighbourGroupPopup
{
  public class GroupListViewItem: ObservableObject
  {
    public event EventHandler NeighbourSelecetedEvent = delegate{};

    public string Name 
    {
      get{ return Localization.Current.GetLocalizedEnumValue(enumGroup); }
    }

    public bool IsSelectedGroup
    {
      get{ return isSelectedGroup; }
      set{ Set(ref isSelectedGroup, value );}
    }

    public bool IsSelecedNeighbour
    {
      get{return isSelectedNeighbour;}
      set
      { 
        Set(ref isSelectedNeighbour, value); 
        NeighbourSelecetedEvent(this, new EventArgs());
      }
    }

    public bool IsEnabled
    {
      get 
      { 
        if(IsSelectedGroup)
        {
          return false;
        }
        if(IsSelecedNeighbour)
        {
          return true;
        }
        return isEnabled;
      }
      set
      {
        
        Set(ref isEnabled, value);
      }
    }

    public Group ThisGroup
    {
      get{return enumGroup;}
    }

    private string name;
    private bool isSelectedGroup;
    private bool isSelectedNeighbour;
    private Group enumGroup; 
    private bool isEnabled;

    public GroupListViewItem(Group enumGroup)
    {
      this.enumGroup = enumGroup; 
      this.isSelectedGroup = false;
      this.isSelectedNeighbour = false;
      this.isEnabled = true;
    }
  }
}
