﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Psi;
using Microsoft.Psi.DeviceManagement;
using OpenSense.PipelineBuilder.Configurations;

namespace OpenSense.PipelineBuilder.Controls.Configuration {
    public partial class AzureKinectSensorControl : UserControl {

        private AzureKinectSensorConfiguration Config => DataContext as AzureKinectSensorConfiguration;

        public AzureKinectSensorControl() {
            InitializeComponent();
        }

        private void ComboBoxDevice_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e) {
            if (Config != null) {
                var index = Microsoft.Psi.AzureKinect.AzureKinectSensor.AllDevices.ToList().FindIndex(d => d.DeviceId == Config.Raw.DeviceIndex);
                ComboBoxDevice.SelectedIndex = index >= 0 ? index : 0;
            }
        }

        private void ComboBoxDevice_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (Config != null && ComboBoxDevice.SelectedItem is CameraDeviceInfo dev) {
                Config.Raw.DeviceIndex = dev.DeviceId;
            }
        }

        private void ComboBoxDefaultDeliveryPolicy_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e) {
            var index = ComboBoxDefaultDeliveryPolicy.Items.Cast<ComboBoxItem>().ToList().FindIndex(i => i.Tag as DeliveryPolicy == Config?.DefaultDeliveryPolicy);
            ComboBoxDefaultDeliveryPolicy.SelectedIndex = index >= 0 ? index : 0;
        }

        private void ComboBoxDefaultDeliveryPolicy_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (Config != null) {
                Config.DefaultDeliveryPolicy = ((DeliveryPolicy)((ComboBoxItem)ComboBoxDefaultDeliveryPolicy.SelectedItem).Tag);
            }
        }

        private void ComboBoxBodyTrackerDeliveryPolicy_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e) {
            var index = ComboBoxBodyTrackerDeliveryPolicy.Items.Cast<ComboBoxItem>().ToList().FindIndex(i => i.Tag as DeliveryPolicy == Config?.BodyTrackerDeliveryPolicy);
            ComboBoxBodyTrackerDeliveryPolicy.SelectedIndex = index >= 0 ? index : 0;
        }

        private void ComboBoxBodyTrackerDeliveryPolicy_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (Config != null) {
                Config.BodyTrackerDeliveryPolicy = ((DeliveryPolicy)((ComboBoxItem)ComboBoxBodyTrackerDeliveryPolicy.SelectedItem).Tag);
            }
        }

        private void CheckBoxBodyTrackerConfiguration_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e) {
            if (Config != null) {
                if (Config.Raw.BodyTrackerConfiguration is null) {
                    CheckBoxBodyTrackerConfiguration.IsChecked = false;
                    ContentControlBodyTrackerConfiguration.Children.Clear();
                } else {
                    CheckBoxBodyTrackerConfiguration.IsChecked = true;
                    var control = new AzureKinectBodyTrackerControl() { DataContext = new AzureKinectBodyTrackerConfiguration() { Raw = Config.Raw.BodyTrackerConfiguration } };
                    ContentControlBodyTrackerConfiguration.Children.Add(control);
                }
            }
        }

        private void CheckBoxBodyTrackerConfiguration_Checked(object sender, RoutedEventArgs e) {
            if (Config != null) {
                if (Config.Raw.BodyTrackerConfiguration != null) {
                    return;//DataContextChanged may cause this method be invoked
                }
                Config.Raw.BodyTrackerConfiguration = new Microsoft.Psi.AzureKinect.AzureKinectBodyTrackerConfiguration();
                var control = new AzureKinectBodyTrackerControl() { DataContext = new AzureKinectBodyTrackerConfiguration() { Raw = Config.Raw.BodyTrackerConfiguration } };
                ContentControlBodyTrackerConfiguration.Children.Add(control);
            }
        }

        private void CheckBoxBodyTrackerConfiguration_Unchecked(object sender, RoutedEventArgs e) {
            if (Config != null) {
                Config.Raw.BodyTrackerConfiguration = null;
                ContentControlBodyTrackerConfiguration.Children.Clear();
            }
        }
    }
}