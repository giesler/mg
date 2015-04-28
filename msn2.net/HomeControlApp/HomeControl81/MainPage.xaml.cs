﻿using Microsoft.Phone.Controls;
using Microsoft.Phone.Net.NetworkInformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;
using Windows.Networking;
using Windows.Networking.Connectivity;


namespace HomeControl81
{
    public partial class MainPage : PhoneApplicationPage
    {

        const string UpstairHallwayAddress = "24060";
        const string MediaRoomSideLightsAddress = "58666";
        const string GarageSwitchAddress = "28 CA 15 2";
        const string GarageSensorAddress = "28 CA 15 1";
        const string CoopDoorAddress = "32 49 A5 1";

        DispatcherTimer updateTimer;
        DateTime lastToggle;
        DateTime lastChange;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            this.updateTimer = new DispatcherTimer();
            this.updateTimer.Tick += updateTimer_Tick;
            this.updateTimer.Interval = TimeSpan.FromSeconds(3);
            this.updateTimer.Start();

            updateTimer_Tick(null, null);

            NetworkInterface net = NetworkInterface.GetInternetInterface();
            Console.Write(net.ToString());
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (App.Duration.TotalSeconds > 0)
            {
                gardenDripOn.IsEnabled = false;
                gardenDripOff.IsEnabled = false;

                DeviceControl.DeviceControlClient dev = new DeviceControl.DeviceControlClient();
                dev.TurnOnCompleted += dev_TurnOnCompleted;
                dev.TurnOnAsync("Garden drip", App.Duration);
                this.lastChange = DateTime.Now;
                gardenDripStatus.Text = "sending";

                App.Duration = TimeSpan.Zero;
            }
        }

        void QueueUpdate()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.Update), null);
        }

        void Update(object sender)
        {
            this.updateTimer_Tick(null, null);
        }

        void updateTimer_Tick(object sender, EventArgs e)
        {
            bool isLocal = false;
            foreach (HostName name in NetworkInformation.GetHostNames())
            {
                if (name.RawName != null && name.RawName.ToString().StartsWith("192.168.1."))
                {
                    isLocal = true;
                }
            }

            IsyData.ISYClient isy = new IsyData.ISYClient();
            if (isLocal)
            {
                isy = new IsyData.ISYClient("BasicHttpBinding_IISY", "http://192.168.1.25:8808/isy.svc");
            }
            isy.GetGroupsCompleted += isy_GetNodeCompleted;
            isy.GetGroupsAsync(GarageSensorAddress);

            DeviceControl.DeviceControlClient dev = new DeviceControl.DeviceControlClient();
            if (isLocal)
            {
                dev = new DeviceControl.DeviceControlClient("BasicHttpBinding_IDeviceControl", "http://192.168.1.25:8808/DeviceControl.svc");
            }
            dev.GetDeviceStatusCompleted += dev_GetDeviceStatusCompleted;
            dev.GetDeviceStatusAsync("Garden Drip");

            Dispatcher.BeginInvoke(() =>
            {
                if (DateTime.Now.AddSeconds(33) > lastToggle)
                {
                    updateTimer.Interval = TimeSpan.FromSeconds(1);
                }
                else
                {
                    updateTimer.Interval = TimeSpan.FromSeconds(5);
                }
            });
        }

        void dev_GetDeviceStatusCompleted(object sender, DeviceControl.GetDeviceStatusCompletedEventArgs e)
        {
            if (this.lastChange.AddSeconds(5) < DateTime.Now)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    if (e.Error == null)
                    {
                        this.gardenDripStatus.Text = e.Result.StatusText;
                        this.gardenDripOff.IsEnabled = e.Result.IsOn;
                        this.gardenDripOn.IsEnabled = !e.Result.IsOn;
                    }
                    else
                    {
                        this.gardenDripStatus.Text = "error";
                        this.gardenDripOff.IsEnabled = false;
                        this.gardenDripOn.IsEnabled = false;
                    }
                });
            }
        }

        void isy_GetNodeCompleted(object sender, IsyData.GetGroupsCompletedEventArgs e)
        {
            if (this.lastChange.AddSeconds(5) < DateTime.Now)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    if (e.Error == null)
                    {
                        List<IsyData.GroupData> groups = e.Result.ToList();

                        coopStatus.Text = GetStatus(groups, CoopDoorAddress);
                        garageStatus.Text = GetStatus(groups, GarageSensorAddress);
                        mediaRoomStatus.Text = GetStatus(groups, MediaRoomSideLightsAddress);
                        upstairsHallStatus.Text = GetStatus(groups, UpstairHallwayAddress);
                    }
                    else
                    {
                        coopStatus.Text = "error";
                        garageStatus.Text = "error";
                        mediaRoomStatus.Text = "error";
                        upstairsHallStatus.Text = "error";
                    }
                });
            }
        }

        string GetStatus(List<IsyData.GroupData> groups, string address)
        {
            foreach (IsyData.GroupData group in groups)
            {
                if (group.Address == address)
                {
                    return group.Status;
                }

                IsyData.NodeData node = group.Nodes.FirstOrDefault(n => n.Address == address);
                if (node != null)
                {
                    return node.Status;
                }
            }

            return "unknown";
        }
        
        private void upstairHallOn_Click(object sender, RoutedEventArgs e)
        {
            IsyData.ISYClient isy = new IsyData.ISYClient();
            upstairHallOn.IsEnabled = false;
            upstairHallOff.IsEnabled = false;
            isy.TurnOnCompleted += upsairsHallOn_TurnOnCompleted;
            isy.TurnOnAsync(UpstairHallwayAddress);
            this.lastChange = DateTime.Now;
            upstairsHallStatus.Text = "sending";
        }

        void upsairsHallOn_TurnOnCompleted(object sender, IsyData.TurnOnCompletedEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                upstairHallOn.IsEnabled = true;
                upstairHallOff.IsEnabled = true;

                if (e.Error != null)
                {
                    MessageBox.Show(e.Error.Message);
                    upstairsHallStatus.Text = "error";
                }
                else
                {
                    upstairsHallStatus.Text = e.Result.Status;
                }
            });
        }
        
        private void upstairHallOff_Click(object sender, RoutedEventArgs e)
        {
            IsyData.ISYClient isy = new IsyData.ISYClient();
            upstairHallOn.IsEnabled = false;
            upstairHallOff.IsEnabled = false;
            isy.TurnOffCompleted += upstairsHall_TurnOffCompleted;
            isy.TurnOffAsync(UpstairHallwayAddress);
        }

        void upstairsHall_TurnOffCompleted(object sender, IsyData.TurnOffCompletedEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                upstairHallOn.IsEnabled = true;

                if (e.Error != null)
                {
                    MessageBox.Show(e.Error.Message);
                    upstairsHallStatus.Text = "error";
                }
                else
                {
                    upstairsHallStatus.Text = e.Result.Status;
                }
            });
        }

        private void toggleGarage_Click(object sender, RoutedEventArgs e)
        {
            IsyData.ISYClient isy = new IsyData.ISYClient();
            toggleGarage.IsEnabled = false;
            isy.TurnOnCompleted += toggleGarage_TurnOnCompleted;
            isy.TurnOnAsync(GarageSwitchAddress);
            this.lastChange = DateTime.Now;
            garageStatus.Text = "sending";
        }

        void toggleGarage_TurnOnCompleted(object sender, IsyData.TurnOnCompletedEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                toggleGarage.IsEnabled = true;
                if (e.Error != null)
                {
                    MessageBox.Show(e.Error.Message);
                    garageStatus.Text = "error";
                }
                else
                {
                    garageStatus.Text = e.Result.Status;
                }

                this.lastToggle = DateTime.Now;                
            });
        }

        private void mediaRoomOn_Click(object sender, RoutedEventArgs e)
        {
            IsyData.ISYClient isy = new IsyData.ISYClient();
            mediaRoomOn.IsEnabled = false;
            mediaRoomOff.IsEnabled = false;
            isy.TurnOnCompleted += mediaRoom_TurnOnCompleted;
            isy.TurnOnAsync(MediaRoomSideLightsAddress);
            this.lastChange = DateTime.Now;
            mediaRoomStatus.Text = "sending";
        }

        void mediaRoom_TurnOnCompleted(object sender, IsyData.TurnOnCompletedEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                mediaRoomOn.IsEnabled = true;
                mediaRoomOff.IsEnabled = true;

                if (e.Error != null)
                {
                    MessageBox.Show(e.Error.Message);
                    mediaRoomStatus.Text = "error";
                }
                else
                {
                    mediaRoomStatus.Text = e.Result.Status;
                }
            });
        }

        private void mediaRoomOff_Click(object sender, RoutedEventArgs e)
        {
            IsyData.ISYClient isy = new IsyData.ISYClient();
            mediaRoomOff.IsEnabled = false;
            mediaRoomOn.IsEnabled = false;
            isy.TurnOffCompleted += mediaRoomOff_TurnOffCompleted;
            isy.TurnOffAsync(MediaRoomSideLightsAddress);
            this.lastChange = DateTime.Now;
        }

        void mediaRoomOff_TurnOffCompleted(object sender, IsyData.TurnOffCompletedEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                mediaRoomOn.IsEnabled = true;
                mediaRoomOff.IsEnabled = true;

                if (e.Error != null)
                {
                    MessageBox.Show(e.Error.Message);
                    mediaRoomStatus.Text = "error";
                }
                else
                {
                    mediaRoomStatus.Text = e.Result.Status;
                }
            });
        }

        private void gardenDripOff_Click(object sender, RoutedEventArgs e)
        {
            gardenDripOn.IsEnabled = false;
            gardenDripOff.IsEnabled = false;

            DeviceControl.DeviceControlClient dev = new DeviceControl.DeviceControlClient();
            dev.TurnOffCompleted += dev_TurnOffCompleted;
            dev.TurnOffAsync("Garden drip");
            gardenDripStatus.Text = "sending";
            this.lastChange = DateTime.Now;
        }

        void dev_TurnOffCompleted(object sender, DeviceControl.TurnOffCompletedEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                if (e.Error != null)
                {
                    MessageBox.Show(e.Error.Message);
                    gardenDripOff.IsEnabled = true;
                    gardenDripStatus.Text = "error";
                }
                else
                {
                    gardenDripOn.IsEnabled = true;
                    gardenDripStatus.Text = e.Result.StatusText;
                }
            });
        }

        private void gardenDripOn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SelectTime.xaml", UriKind.Relative));
        }

        void dev_TurnOnCompleted(object sender, DeviceControl.TurnOnCompletedEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                DeviceControl.DeviceControlClient dev = (DeviceControl.DeviceControlClient)sender;
                
                gardenDripOff.IsEnabled = true;
                gardenDripOn.IsEnabled = true;

                if (e.Error != null)
                {
                    MessageBox.Show(e.Error.Message);
                    gardenDripStatus.Text = "error";
                }
                else
                {
                    gardenDripOn.IsEnabled = false;
                    gardenDripStatus.Text = e.Result.StatusText;
                }
            });
        }
    }
}