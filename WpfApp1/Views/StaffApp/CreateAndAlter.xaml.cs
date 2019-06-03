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
    /// Interaction logic for CreateAndAlter.xaml
    /// </summary>
    public partial class CreateAndAlter : Window
    {
        public CreateAndAlter()
        {
            InitializeComponent();
        }

        List<Child> children = new List<Child>();
        List<Guardian> guardian = new List<Guardian>();
        List<Connections> connections = new List<Connections>();

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            DbOperations.DeleteChild();
            ListViewChildren.SelectedItem = null;
            ClearTextbox();
        }



        private void BtnAddNew_Click(object sender, RoutedEventArgs e)
        {

            if (txtboxFirstName != null && txtboxLastName != null)
            {
                try
                {
                    DbOperations.AddNewChild(txtboxFirstName.Text, txtboxLastName.Text);
                }
                catch (Exception)
                {

                    MessageBox.Show("Fyll i fält",
                                    "Felmeddelande");
                }
            }
            ClearTextbox();
            ListViewGuardians.ItemsSource = DbOperations.GetAllGuardians();

        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            ListViewChildren.SelectedItem = null;
            ClearTextbox();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Activated(object sender, EventArgs e)
        {
            lblStaffFirstname.Content = $"Inloggad som {Activestaff.Firstname} {Activestaff.Lastname}";

            children = DbOperations.GetAllChildren();
            ListViewChildren.ItemsSource = children;
            guardian = DbOperations.GetAllGuardians();
            ListViewGuardians.ItemsSource = guardian;
            connections = DbOperations.GetChildGuardian();
            ListViewConnections.ItemsSource = connections;
            ListViewConnections.DisplayMemberPath = "Fullinformation";

        }

        public void UpdateChild()
        {
            txtboxFirstName.Text = Activechild.Firstname;
            txtboxLastName.Text = Activechild.Lastname;
            txtboxClass.Text = Activechild.Class;
        }

        public void UpdateGuardian()
        {
            txtboxFirstNameGuardian.Text = Activeguardian.Firstname;
            txtboxLastNameGuardian.Text = Activeguardian.Lastname;
            txtboxPhoneGuardian.Text = Activeguardian.Phone.ToString();
            txtboxEmailGuardian.Text = Activeguardian.Email;
        }
        public void ClearTextbox()
        {
            txtboxFirstNameGuardian.Clear();
            txtboxLastNameGuardian.Clear();
            txtboxPhoneGuardian.Clear();
            txtboxEmailGuardian.Clear();

            txtboxFirstName.Clear();
            txtboxLastName.Clear();
            txtboxClass.Clear();
        }
        private void ListViewGuardians_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Activeguardian.Setactiveguardian((Guardian)ListViewGuardians.SelectedItem);

            }
            catch (Exception)
            {

                return;
            }
            UpdateGuardian();

        }

        private void ListViewChildren_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Activechild.Setactivechild((Child)ListViewChildren.SelectedItem);
            }
            catch (Exception)
            {
                return;
            }
            UpdateChild();
        }

        private void BtnChange_Click(object sender, RoutedEventArgs e)
        {

            Activechild.Setactivechild((Child)ListViewChildren.SelectedItem);
            DbOperations.UpdateChildProperties(txtboxFirstName.Text, txtboxLastName.Text);
            ListViewChildren.ItemsSource = children;
            ClearTextbox();
        }
        private void SaveGuardian_Click(object sender, RoutedEventArgs e)
        {
            Activeguardian.Setactiveguardian((Guardian)ListViewGuardians.SelectedItem);
            DbOperations.UpdateGuardianProperties(Convert.ToInt32(txtboxPhoneGuardian.Text), txtboxEmailGuardian.Text, txtboxFirstNameGuardian.Text, txtboxLastNameGuardian.Text);
            ListViewGuardians.ItemsSource = DbOperations.GetAllGuardians();
            ClearTextbox();

        }

        private void BtnCancelGuardian_Click(object sender, RoutedEventArgs e)
        {
            ListViewGuardians.SelectedItem = null;
            ClearTextbox();
        }

        private void BtnDeleteGuardian_Click(object sender, RoutedEventArgs e)
        {
            DbOperations.DeleteGuardian();
            ListViewGuardians.SelectedItem = null;
            ClearTextbox();
        }

        private void AddNewGuardian_Click(object sender, RoutedEventArgs e)
        {

            if (txtboxFirstNameGuardian != null && txtboxLastNameGuardian != null)
            {
                try
                {
                    DbOperations.AddNewGuardian(Convert.ToInt32(txtboxPhoneGuardian.Text), txtboxFirstNameGuardian.Text, txtboxLastNameGuardian.Text, txtboxEmailGuardian.Text);
                }
                catch (Exception)
                {

                    MessageBox.Show("Fyll i fält",
                                    "Felmeddelande");
                }
            }
            ClearTextbox();
            ListViewGuardians.ItemsSource = DbOperations.GetAllGuardians();
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            DbOperations.ConnectChildAndGuardian();
            ListViewGuardians.SelectedItem = null;
            ListViewChildren.SelectedItem = null;
            ListViewConnections.SelectedItem = null;
            connections = DbOperations.GetChildGuardian();
            ListViewConnections.ItemsSource = connections;
            ListViewConnections.DisplayMemberPath = "Fullinformation";
        }

        private void ListViewConnections_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
 
        }

        private void RemoveConnection_Click(object sender, RoutedEventArgs e)
        {
            DbOperations.DeleteConnection();
            ListViewConnections.SelectedItem = null;
            connections = DbOperations.GetChildGuardian();
            ListViewConnections.ItemsSource = connections;
            ListViewConnections.DisplayMemberPath = "Fullinformation";
        }
    }
}
