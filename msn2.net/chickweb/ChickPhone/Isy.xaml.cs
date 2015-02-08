﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Threading;

namespace ChickPhone
{
    public partial class Isy : PhoneApplicationPage
    {
        const string UpstairHallwayAddress = "24060";
        const string MediaRoomSideLightsAddress = "58666";
        const string GarageSwitchAddress = "28 CA 15 2";  
        const string GarageSensorAddress = "28 CA 15 1";
        const string CoopDoorAddress = "32 49 A5 1";

        DispatcherTimer updateTimer;
        DateTime lastToggle;

        public Isy()
        {
            InitializeComponent();

            this.updateTimer = new DispatcherTimer();
            this.updateTimer.Tick += updateTimer_Tick;
            this.updateTimer.Interval = TimeSpan.FromSeconds(3);
            this.updateTimer.Start();

            updateTimer_Tick(null, null);
        }

        void updateTimer_Tick(object sender, EventArgs e)
        {
            IsyData.ISYClient isy = new IsyData.ISYClient();
            isy.GetGroupsCompleted += isy_GetNodeCompleted;
            isy.GetGroupsAsync(GarageSensorAddress);

            if (DateTime.Now.AddSeconds(33) > lastToggle)
            {
                updateTimer.Interval = TimeSpan.FromSeconds(1);
            }
            else
            {
                updateTimer.Interval = TimeSpan.FromSeconds(5);
            }
        }

        void isy_GetNodeCompleted(object sender, IsyData.GetGroupsCompletedEventArgs e)
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        private void upstairHallOn_Click(object sender, RoutedEventArgs e)
        {
            IsyData.ISYClient isy = new IsyData.ISYClient();
            upstairHallOn.IsEnabled = false;
            upstairHallOff.IsEnabled = false;
            isy.TurnOnCompleted += upstairsHallTurnOnCompleted;
            isy.TurnOnAsync(UpstairHallwayAddress);
        }

        void upstairsHallTurnOnCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            upstairHallOn.IsEnabled = true;
            upstairHallOff.IsEnabled = true;

            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
        }

        private void upstairHallOff_Click(object sender, RoutedEventArgs e)
        {
            IsyData.ISYClient isy = new IsyData.ISYClient();
            upstairHallOn.IsEnabled = false;
            upstairHallOff.IsEnabled = false;
            isy.TurnOffCompleted += upstairsHallTurnOnCompleted;
            isy.TurnOffAsync(UpstairHallwayAddress);
        }
        
        private void toggleGarage_Click(object sender, RoutedEventArgs e)
        {
            IsyData.ISYClient isy = new IsyData.ISYClient();
            toggleGarage.IsEnabled = false;
            isy.TurnOnCompleted += onGarageToggleCompleted;
            isy.TurnOnAsync(GarageSwitchAddress);
        }
        
        void onGarageToggleCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            toggleGarage.IsEnabled = true;
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }

            this.lastToggle = DateTime.Now;
            updateTimer_Tick(null, null);
        }

        private void mediaRoomOn_Click(object sender, RoutedEventArgs e)
        {
            IsyData.ISYClient isy = new IsyData.ISYClient();
            mediaRoomOn.IsEnabled = false;
            mediaRoomOff.IsEnabled = false;
            isy.TurnOnCompleted += mediaRoom_TurnOnCompleted;
            isy.TurnOnAsync(MediaRoomSideLightsAddress);
        }

        void mediaRoom_TurnOnCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            mediaRoomOn.IsEnabled = true;
            mediaRoomOff.IsEnabled = true;

            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
        }

        private void mediaRoomOff_Click(object sender, RoutedEventArgs e)
        {
            IsyData.ISYClient isy = new IsyData.ISYClient();
            mediaRoomOff.IsEnabled = false;
            mediaRoomOn.IsEnabled = false;
            isy.TurnOffCompleted += mediaRoom_TurnOnCompleted;
            isy.TurnOffAsync(MediaRoomSideLightsAddress);
        }
        
    }
}