using BakuBus_WPF.Commands;
using BakuBus_WPF.Models;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace BakuBus_WPF.ViewModels;

internal class HomeViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    private void NotifyPropertyChanged(string prorertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prorertyName));

    public ObservableCollection<string> ComboBoxBuses { get; set; }
    
    private string _mySelectedItem;
    public string MySelectedItem
    {
        get { return _mySelectedItem; }
        set
        {
            MessageBox.Show(value);

            _mySelectedItem = value;
        }
    }
    //private List<string> comboBoxBuses;

    //public List<string> ComboBoxBuses
    //{
    //    get { return comboBoxBuses; }
    //    set
    //    {
    //        comboBoxBuses = value;
    //        NotifyPropertyChanged(nameof(ComboBoxBuses));
    //    }
    //}

    public ICommand BusSelectCommand { get; set; }
    public int CurrentIndex { get; set; }
    private BakuBus? bakuBus;

    public BakuBus? BakuBus
    {
        get { return bakuBus; }
        set
        {
            bakuBus = value;
            NotifyPropertyChanged(nameof(BakuBus));
        }
    }
    private ApplicationIdCredentialsProvider? mapKey;


    public ApplicationIdCredentialsProvider? MapKey
    {
        get { return mapKey; }
        set
        {
            mapKey = value;
            NotifyPropertyChanged(nameof(MapKey));
        }
    }


    public HomeViewModel(string key)
    {
        MapKey = new ApplicationIdCredentialsProvider(key);

        ComboBoxBuses = new();
        ComboBoxBuses.Add("View All");

        UpdateBakuBusStatus();


        DispatcherTimer timer = new();
        timer.Interval = new TimeSpan(10000);
        timer.Tick += Timer_Tick;
        timer.Start();

        BusSelectCommand = new RelayCommand(ExecuteBusSelectCommand);
    }


    private void ExecuteBusSelectCommand(object? parametr)
    {
        if (parametr is MapItemsControl map)
        {
            var busName = ComboBoxBuses[CurrentIndex];

            foreach (var bus in map.Items.OfType<Bus>())
            {

                if (bus.Attributes.DISPLAY_ROUTE_CODE != busName)
                    bus.Attributes.VISIBILITY = Visibility.Collapsed;
                else
                    bus.Attributes.VISIBILITY = Visibility.Visible;
            }

        }
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        UpdateBakuBusStatus();
    }

    private async void UpdateBakuBusStatus()
    {
        try
        {
            var client = new HttpClient();
            var jsonStr = await client.GetStringAsync("https://www.bakubus.az/az/ajax/apiNew1");
            BakuBus = JsonSerializer.Deserialize<BakuBus>(jsonStr);
        }
        catch (Exception)
        {
            //BakuBus = JsonSerializer.Deserialize<BakuBus>(File.ReadAllText("../../../bakubusApi.json"));
        }

        if (BakuBus != null)
        {
            double latitude, longitude;

            foreach (var bus in BakuBus.Buses)
            {
                latitude = double.Parse(bus.Attributes.LATITUDE);
                longitude = double.Parse(bus.Attributes.LONGITUDE);

                bus.Attributes.LOCATION = new Location(latitude, longitude);
            }

            foreach (var bus in BakuBus.Buses)
            {
                if (!ComboBoxBuses.Contains(bus.Attributes.DISPLAY_ROUTE_CODE))
                    ComboBoxBuses.Add(bus.Attributes.DISPLAY_ROUTE_CODE);
            }
        }

    }
}
