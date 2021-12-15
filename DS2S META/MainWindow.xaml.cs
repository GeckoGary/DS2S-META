﻿using Bluegrams.Application;
using Octokit;
using PropertyHook;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace DS2S_META
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Properties.Settings Settings;
        public MainWindow()
        {
            PortableSettingsProvider.SettingsFileName = "DS2 Meta.config";
            PortableSettingsProvider.ApplyProvider(Properties.Settings.Default);
            Settings = Properties.Settings.Default;
            InitializeComponent();
            if (Settings.ShowWarning)
            {
                var warning = new METAWarning()
                {
                    Title = "Online Warning",
                    Width = 350,
                    Height = 240
                };
                warning.ShowDialog();
            }
                
        }

        DS2SHook Hook => ViewModel.Hook;
        bool FormLoaded
        {
            get => ViewModel.GameLoaded;
            set => ViewModel.GameLoaded = value;
        }
        public bool Reading
        {
            get => ViewModel.Reading;
            set => ViewModel.Reading = value;
        }

        Timer updateTimer = new Timer();

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            var version= fileVersionInfo.ProductVersion;

            Title = "DS2 META " + version;
            EnableTabs(false);
            InitAllTabs();

            try
            {
                GitHubClient gitHubClient = new GitHubClient(new ProductHeaderValue("DS2-META"));
                Release release = await gitHubClient.Repository.Release.GetLatest("Nordgaren", "DS2-META");
                Version gitVersion = Version.Parse(release.TagName.ToLower().Replace("v", ""));
                Version exeVersion = Version.Parse(version);
                if (gitVersion > exeVersion) //Compare latest version to current version
                {
                    labelCheckVersion.Content = "App out of date";
                    link.NavigateUri = new Uri(release.HtmlUrl);
                    llbNewVersion.Visibility = Visibility.Visible;
                }
                else if (gitVersion == exeVersion)
                {
                    labelCheckVersion.Content = "App up to date";
                }
                else
                {
                    labelCheckVersion.Content = "App version unreleased. Be wary of bugs!";
                }
            }
            catch (Exception ex) when (ex is HttpRequestException || ex is ApiException || ex is ArgumentException)
            {
                labelCheckVersion.Content = "Current app version unknown";
            }
            catch (Exception ex)
            {
                labelCheckVersion.Content = "Something is very broke, contact DS2 META repo owner";
                MessageBox.Show(ex.Message);
            }
            updateTimer.Interval = 16;
            updateTimer.Elapsed += UpdateTimer_Elapsed;
            updateTimer.Enabled = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            updateTimer.Stop();
            SaveAllTabs();

            Settings.Save();
            //ResetAllTabs();
        }

        

        private void UpdateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                UpdateMainProperties();
                if (Hook.Hooked)
                {
                    if (Hook.Loaded)
                    {
                        if (!FormLoaded)
                        {
                            FormLoaded = true;
                            Reading = true;
                            ReloadAllTabs();
                            Reading = false;
                            EnableTabs(true);
                        }
                        else
                        {
                            Reading = true;
                            UpdateProperties();
                            UpdateAllTabs();
                            Reading = false;
                        }
                    }
                    else if (FormLoaded)
                    {
                        Reading = true;
                        UpdateProperties();
                        EnableTabs(false);
                        FormLoaded = false;
                        Reading = false;
                    }
                }
            }));
            
        }

        private void UpdateMainProperties()
        {
            Hook.UpdateMainProperties();
            ViewModel.UpdateMainProperties();
        }

        private void InitAllTabs()
        {
            metaItems.InitTab();
            metaPlayer.InitTab();
            InitHotkeys();
        }
        private void UpdateProperties()
        {
            Hook.UpdateStatsProperties();
            Hook.UpdatePlayerProperties();
        }
        private void EnableTabs(bool enable)
        {
            metaPlayer.EnableCtrls(enable);
            metaStats.EnableCtrls(enable);
            metaBonfire.EnableCtrls(enable);
        }
        private void ReloadAllTabs()
        {
            metaPlayer.ReloadCtrl();
            metaStats.ReloadCtrl();
            metaItems.ReloadCtrl();
        }
        private void UpdateAllTabs()
        {
            metaPlayer.UpdateCtrl();
            metaStats.UpdateCtrl();
            metaBonfire.UpdateCtrl();
        }

        private void link_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.ToString());
        }

        private void SaveAllTabs()
        {
            SaveHotkeys();
        }
    }
}
