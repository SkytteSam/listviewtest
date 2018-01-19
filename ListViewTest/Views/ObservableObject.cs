using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Reflection;
using System.Collections.Generic;

namespace SmartRemote
{
  public abstract class ObservableObject : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void RaisePropertyChanged<T>([CallerMemberName] string propertyName = null, T oldValue = default(T), T newValue = default(T))
    {
      if (string.IsNullOrEmpty(propertyName))
      {
        throw new ArgumentException("This method cannot be called with an empty string", "propertyName");
      }

      RaisePropertyChanged(propertyName);
    }

    protected virtual void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
    {
      var handler = PropertyChanged;

      if (handler != null)
      {
        var propertyName = GetPropertyName(propertyExpression);

        if (handler != null)
        {
          handler(this, new PropertyChangedEventArgs(propertyName));
        }
      }
    }

    protected virtual void RaisePropertyChanged(string propertyName) 
    {
      var handler = PropertyChanged;
      if (handler != null)
      {
        handler(this, new PropertyChangedEventArgs(propertyName));
      }
    }

    protected bool Set<T>(string propertyName, ref T field, T newValue = default(T))
    {
      if (EqualityComparer<T>.Default.Equals(field, newValue))
      {
        return false;
      }

      var oldValue = field;
      field = newValue;
      RaisePropertyChanged(propertyName, oldValue, field);
      return true;
    }

    protected bool Set<T>(ref T field, T newValue = default(T), [CallerMemberName] string propertyName = null)
    {
      if (EqualityComparer<T>.Default.Equals(field, newValue))
      {
        return false;
      }

      var oldValue = field;
      field = newValue;
      RaisePropertyChanged(propertyName, oldValue, field);
      return true;
    }

    public static string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
    {
      if (propertyExpression == null)
      {
        throw new ArgumentNullException("propertyExpression");
      }
      var body = propertyExpression.Body as MemberExpression;
      if (body == null)
      {
        throw new ArgumentException("Invalid argument", "propertyExpression");
      }
      var property = body.Member as PropertyInfo;
      if (property == null)
      {
        throw new ArgumentException("Argument is not a property", "propertyExpression");
      }
      return property.Name;
    }
  }
}