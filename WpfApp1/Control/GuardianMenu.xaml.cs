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
using WpfApp1.Models;
using WpfApp1;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for GuardianMenu.xaml
    /// </summary>
    public partial class GuardianMenu : UserControl
    {
        public GuardianMenu()
        {
            InitializeComponent();

            
        }

        //https://stackoverflow.com/questions/39886903/how-do-i-access-buttons-inside-a-usercontrol-from-xaml

        private void Button_Click_LogOut(object sender, RoutedEventArgs e)
        {
            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);

            MainWindow mainWindow = new MainWindow();

            mainWindow.Show();

            window.Close();

        }

        private void Schedule_Click(object sender, RoutedEventArgs e)
        {
            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);

            Reportfritids reportfritids = new Reportfritids();

            reportfritids.Show();

            window.Close();
        }

        private void Meals_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);

            Profile profile = new Profile();

            profile.Show();

            window.Close();
        }

        private void Attendance_Click(object sender, RoutedEventArgs e)
        {
            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);

            Reportabscence reportabscence = new Reportabscence();

            reportabscence.Show();

            window.Close();
        }

        private void Contact_Click(object sender, RoutedEventArgs e)
        {
            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);

            Contact contact = new Contact();

            contact.Show();

            window.Close();
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);

            window.WindowState = WindowState.Minimized;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);

            window.Close();
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {

            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);

            ScheduleView scheduleView = new ScheduleView();

            scheduleView.Show();

            window.Close();
        }
    }
}
