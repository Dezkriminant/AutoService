using System;
using System.Collections.Generic;
using System.Xml;
using AutoService1.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using AutoService1.DB;


namespace AutoService1.Views;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IServiceProvider _provider;
    [ObservableProperty] string username;
    [ObservableProperty] List<Service> _serviceList;
    [ObservableProperty] Service selectedService;
    [ObservableProperty] string car;

    public MainWindowViewModel(IServiceProvider provider, ServiceRepository repository)
    {
        _provider = provider;
        _serviceList = repository.GetServicesByTest();
    }

    [RelayCommand]
    public void StartTest()
    {
        if (SelectedService == null)
            return;
        var vm = ActivatorUtilities.CreateInstance<WorkWindowViewModel>(
            _provider,
            SelectedService,
            Username,
            Car);
        var win = _provider.GetRequiredService<WorkWindow>();
        vm.SetClose(win.Close);
        win.DataContext = vm;
        win.Show();
        close();
    }

    private Action close;
  
      public void SetClose(Action close)
      {
          this.close = close;
      }
      
      
      [RelayCommand]
      public void OpenPassword()
      {
          var vm = _provider.GetRequiredService<PasswordWindowViewModel>();
          var win = _provider.GetRequiredService<PasswordWindow>();
          vm.SetClose(win.Close);
          win.DataContext = vm;
          win.Show();
          close();
      }
    
      
}