using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using AutoService1.DB;
using AutoService1.ViewModels;
using AutoService1.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace AutoService1.Views;

public partial class PasswordWindowViewModel : ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IServiceProvider _provider;
    [ObservableProperty] string username;
    [ObservableProperty] List<Service> _serviceList;
    [ObservableProperty] Service selectedService;
    [ObservableProperty] string car;
    [ObservableProperty] private string _password = " ";
    [ObservableProperty] private string _error = " ";
    
    public PasswordWindowViewModel(IServiceProvider provider, ServiceRepository repository)
    {
        _provider = provider;
        _serviceList = repository.GetServicesByTest(); 
        //    _password = password;
        //  _error = error;
    }

    
    [RelayCommand]
    public void Start()
    {
          if (SelectedService == null)
            return;
          var vm = ActivatorUtilities.CreateInstance<AdminWindowViewModel>(
            _provider,
            SelectedService,
            Username,
            Car);

        if (_password != "123")
        {
            Error = "Пароль неверный";
        }
        else
        {
            var win = _provider.GetRequiredService<AdminWindow>();
            win.DataContext = vm;
            win.Show();
        }
    }

    private Action close;
    
    public void SetClose(Action close)
    {
        this.close = close;
    }
    
    [RelayCommand]
    public void Close()
    {
        var vm = _provider.GetRequiredService<MainWindowViewModel>();
        var win = _provider.GetRequiredService<MainWindow>();
        win.DataContext = vm;
        vm.SetClose(win.Close);
        win.DataContext = vm;
        win.Show();
        close();
    }
}
