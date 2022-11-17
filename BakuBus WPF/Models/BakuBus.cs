using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;

namespace BakuBus_WPF.Models;

#nullable disable

public class Attributes : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged(string prorertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prorertyName));


    public string BUS_ID { get; set; }
    public string PLATE { get; set; }
    public string DRIVER_NAME { get; set; }
    public string PREV_STOP { get; set; }
    public string CURRENT_STOP { get; set; }
    public string SPEED { get; set; }
    public string BUS_MODEL { get; set; }
    public string LATITUDE { get; set; }
    public string LONGITUDE { get; set; }
    public string ROUTE_NAME { get; set; }
    public int LAST_UPDATE_TIME { get; set; }
    public string DISPLAY_ROUTE_CODE { get; set; }
    public Location LOCATION { get; set; }

    private Visibility visibility;

    public Visibility VISIBILITY
    {
        get { return visibility; }
        set
        {
            visibility = value;
            OnPropertyChanged(nameof(VISIBILITY));

        }
    }

}

public class Bus
{
    [JsonPropertyName("@attributes")]
    public Attributes Attributes { get; set; }
}

public class BakuBus
{
    [JsonPropertyName("BUS")]
    public List<Bus> Buses { get; set; }
}
