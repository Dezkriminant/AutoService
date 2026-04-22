﻿using Avalonia;
using System;
using AutoService1.DB;
using AutoService1.ViewModels;
using AutoService1.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AutoService1;


sealed class Program
{

    [STAThread]
    public static void Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder().
            ConfigureAppConfiguration((context, config) =>
            {
                config.SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .AddEnvironmentVariables();
            }).
            ConfigureServices((c,s) =>
            {
                s.Configure<DatabaseConnection>(c.Configuration.
                    GetSection("DatabaseConnection"));
                s.AddTransient<MainWindow>();
                s.AddTransient<MainWindowViewModel>();
                s.AddTransient<OrdersItemsRepository>();
                s.AddTransient<OrderRepository>();
                s.AddTransient<ServiceRepository>();
                s.AddTransient<WorkRepository>();
                s.AddTransient<WorkWindowViewModel>();
                s.AddTransient< WorkWindow>();
                s.AddTransient<ReceiptWindow>();
                s.AddTransient<ReceiptWindowViewModel>();
                
               
            }).
            Build();
        BuildAvaloniaApp(host.Services)
            .StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp(IServiceProvider serviceProvider)
        => AppBuilder.Configure(()=> new App(serviceProvider))
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}




