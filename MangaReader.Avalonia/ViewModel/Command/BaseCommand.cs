﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Threading;

namespace MangaReader.Avalonia.ViewModel.Command
{
  public abstract class BaseCommand : ICommand, INotifyPropertyChanged
  {
    private string name;
    private string icon;
    private bool isVisible = true;

    public string Name
    {
      get { return name; }
      set
      {
        name = value;
        OnPropertyChanged();
      }
    }

    public string Icon
    {
      get { return icon; }
      set
      {
        icon = value;
        OnPropertyChanged();
      }
    }

    public bool IsVisible
    {
      get { return isVisible; }
      set
      {
        isVisible = value;
        OnPropertyChanged();
      }
    }

    bool ICommand.CanExecute(object parameter)
    {
      var canExecute = this.CanExecute(parameter);
      IsVisible = canExecute;
      return canExecute;
    }

    async void ICommand.Execute(object parameter)
    {
      await Execute(parameter).ConfigureAwait(true);
    }

    public virtual bool CanExecute(object parameter)
    {
      return true;
    }

    public abstract Task Execute(object parameter);

    public event EventHandler CanExecuteChanged;

    public virtual void OnCanExecuteChanged()
    {
      if (Dispatcher.UIThread.CheckAccess())
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
      else
        Dispatcher.UIThread.InvokeAsync(() => CanExecuteChanged?.Invoke(this, EventArgs.Empty));
    }
    
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}