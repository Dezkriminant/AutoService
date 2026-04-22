using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using AutoService1.DB;
using AutoService1.Views;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace AutoService1.ViewModels;

public partial class WorkWindowViewModel : ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;

    WorkRepository _workRepository;
    [ObservableProperty] private string _username;
    [ObservableProperty] List<WorkSelected> _works;
    [ObservableProperty] Service _selectedService;
    [ObservableProperty] string _car;
    
    


    public WorkWindowViewModel(IServiceProvider provider, string username, Service selectedService, string car, WorkRepository repository )
    {
        _serviceProvider = provider;
        _username = username;
        _selectedService = selectedService;
        _car = car;
       _workRepository = repository;
       Works = repository.GetWorksByServices(selectedService).Select(work => new WorkSelected(work)).ToList();
       
    }
    
    
    [RelayCommand]
    public void RecieptResult()
    {
        List<Work> works = new List<Work>();

        foreach (WorkSelected s in Works)
        {
            if (s.IsSelected == true)
            {
                works.Add(s.Work);
            }
        }

        var vm = ActivatorUtilities.CreateInstance<ReceiptWindowViewModel>(
            _serviceProvider,
            SelectedService,
            Username,
            Car,
            works);
        var win = _serviceProvider.GetRequiredService<ReceiptWindow>();
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
}
