using BakuBus_WPF.ViewModels;
using BakuBus_WPF.Views;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BakuBus_WPF;

public partial class App : Application
{
    private void Application_Startup(object sender, StartupEventArgs e)
    {
        var key = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build()["mapKey"];

        var homeViewModel = new HomeViewModel(key);
        var homeView = new HomeView();

        homeView.DataContext = homeViewModel;

        homeView.ShowDialog();
    }
}
