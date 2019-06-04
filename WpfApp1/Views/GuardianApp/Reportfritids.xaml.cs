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
using System.Windows.Shapes;
using WpfApp1.Models;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Reportfritids.xaml
    /// </summary>
    public partial class Reportfritids : Window
    {
        List<Child> children = new List<Child>();
        List<Attendance> attendances = new List<Attendance>();
        List<Date> dates = new List<Date>();
        List<Weeks> weeks = new List<Weeks>();
        List<Attendancecategory> attendancecategories = new List<Attendancecategory>();
        List<Meal> meals = new List<Meal>(); 

        public Reportfritids()
        {
            InitializeComponent();

            DataBinding();
        }

        public void DataBinding()
        {
            //Hämta barn
            children = DbOperations.GetChildrenOfGuardian();

            comboBoxChildren.ItemsSource = children;
            comboBoxChildren.DisplayMemberPath = "Fullinformation";

            comboBoxChildren.SelectedIndex = 0;

            comboBoxChildMeals.ItemsSource = children;
            comboBoxChildMeals.DisplayMemberPath = "Fullinformation";
            comboBoxChildMeals.SelectedIndex = 0;

            //Hämta barn se
            children = DbOperations.GetChildrenOfGuardian();

            comboBoxChildren2.ItemsSource = children;
            comboBoxChildren2.DisplayMemberPath = "Fullinformation";

            comboBoxChildren2.SelectedIndex = 0;

            //Hämta veckor
            weeks = DbOperations.GetWeek();

            comboBoxWeek.ItemsSource = weeks;
            comboBoxWeek.DisplayMemberPath = "InformationWeek";

            //Hämta dagar
            dates = DbOperations.GetDays();

            comboBoxDay.ItemsSource = dates;
            comboBoxDay.DisplayMemberPath = "InformationDay";

            //Hämta Morgon/Kväll
            attendancecategories = DbOperations.GetFritidsMorningEvening();

            comboBoxType.ItemsSource = attendancecategories;
            comboBoxType.DisplayMemberPath = "Fullinformation";
        }

        private void ComboBoxChildren_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxChildren.SelectedItem != null)
            {
                Activechild.Setactivechild((Child)comboBoxChildren.SelectedItem);
            }
        }

        private void ComboBoxChildren2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxChildren2.SelectedItem != null)
            {
                Activechild.Setactivechild((Child)comboBoxChildren2.SelectedItem);
                attendances = DbOperations.Getfritidsguardian();
                
                ListView.ItemsSource = attendances;
                
                ListView.Items.Refresh();
            }
        }

        private void ComboBoxDay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxDay.SelectedItem != null)
            {
                ActiveDate.Setactivatedate((Date)comboBoxDay.SelectedItem);
            }
        }

        private void ComboBoxWeek_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void BtnReportAbscence_Click(object sender, RoutedEventArgs e)
        {
            int i = comboBoxType.SelectedIndex;
            string comment = txtbxComment.Text;

            if (i == 0)
            {
                int attendanceid = 7;
                attendances = DbOperations.GuardianReportFritidsBreakfast(comment, attendanceid);

                attendanceid = 3;
                attendances = DbOperations.GuardianReportFritids(comment, attendanceid);
            }
            else if (i == 1)
            {
                int attendanceid = 7;
                attendances = DbOperations.GuardianReportFritidsBreakfast(comment, attendanceid);
            }
            else if (i == 2)
            {
                int attendanceid = 3;
                attendances = DbOperations.GuardianReportFritids(comment, attendanceid);
            }

            UpdatedMessage();
        }

        public async void UpdatedMessage()
        {
            lblUpdated.Visibility = Visibility.Visible;
            await Task.Delay(3500);
            lblUpdated.Visibility = Visibility.Hidden;
        }

        private void ComboBoxType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            int i = comboBoxType.SelectedIndex;

            if (i == 0)
            {
                chxbxBreakfast.IsEnabled = true;
            }
            else if (i == 1)
            {
                chxbxBreakfast.IsEnabled = true;
            }
            else if (i == 2)
            {
                chxbxBreakfast.IsEnabled = false;
            }
        }

        private void Seereports_Loaded_1(object sender, RoutedEventArgs e)
        {
            

            attendances = DbOperations.Getfritidsguardian();

            ListView.ItemsSource = attendances;
        }

        private void Seereportedmeals_Loaded(object sender, RoutedEventArgs e)
        {
            meals = DbOperations.GetMeals();
            ListViewMeals.ItemsSource = meals;
            
        }

        private void ComboBoxChildMeals_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Activechild.Setactivechild((Child)comboBoxChildMeals.SelectedItem);

            meals = DbOperations.GetMeals();
            ListViewMeals.ItemsSource = meals;
            ListViewMeals.Items.Refresh();
        }
    }
}
