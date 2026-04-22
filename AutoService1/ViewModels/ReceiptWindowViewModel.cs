using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AutoService1.DB;
using AutoService1.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace AutoService1.ViewModels;

public partial class ReceiptWindowViewModel : ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IServiceProvider _provider;
    [ObservableProperty] private string _username;
    [ObservableProperty] List<Work> _works;
    [ObservableProperty] Service _selectedService;
    [ObservableProperty] string _car;
    [ObservableProperty] private decimal price;
    [ObservableProperty] private decimal totalPrice;
    [ObservableProperty] private int discount;
    private OrderRepository _repository;
    private Action _cloceAction;




    public ReceiptWindowViewModel(IServiceProvider provider, string username, Service selectedService, string car, List<Work>  works, OrderRepository repository)
    {
        _serviceProvider = provider;
        Username = username;
        SelectedService = selectedService;
        Car = car; 
        Works = works;
        Price = GetPrice();
        Discount = PriceDiscount();
        TotalPrice = TotalDiscountPrice();
        _repository =  repository;
        
    }

    
    public decimal GetPrice()
    {
        decimal count = 0;
             foreach(var work in _works)
        {
            count += work.Price;
        }
        return count;
   }


    public int PriceDiscount()
    {
        int count = 0;
        
        if (Price >= 10000)
        {
            count = 10;
        }
        else if(Price >= 5000)

        {
            count = 5;
        }
        return count;
    }

    public decimal TotalDiscountPrice()
    {
        if (Discount != 0)
            return Price * (1 - Discount / 100m);
        return Price;
    }


    [RelayCommand]
    public void SaveDB()
    {
        Order order = new Order
        {
            ClientName = Username,
            CarModel = Car,
            ServiceId = SelectedService.Id,
            DiscountPercent = Discount,
            OrderTime = DateTime.Now,
            TotalAmount = TotalPrice
        };
        _repository.InsertOrder(order, Works);
        if (SelectedService == null)
            return;
        var vm = _serviceProvider.GetRequiredService<MainWindowViewModel>();
        var win = _serviceProvider.GetRequiredService<MainWindow>();
        
        vm.SetClose(win.Close);
        win.DataContext = vm;
        win.Show();
        close();
    }
    
    [RelayCommand]
    public void Start()
    {
       if (SelectedService == null)
           return;
       var vm = _serviceProvider.GetRequiredService<MainWindowViewModel>();
        var win = _serviceProvider.GetRequiredService<MainWindow>();
        
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
