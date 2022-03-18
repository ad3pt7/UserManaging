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
using UserManaging.Model;

namespace UserManaging.View
{
    public partial class UserManagingWindow : Window
    {
        public UserManagingWindow()
        {
            InitializeComponent();
            UsersDAL dAL = new UsersDAL();
            UsersListView.ItemsSource = dAL.GetUsers();
        }

        /// <summary>
        /// Открывает окно с новым пользователем
        /// </summary>
        private void AddUser(object sender, RoutedEventArgs e)
        {
            new AddUpdateUserWindow(null,this).Show();
        }
        /// <summary>
        /// Открывает окно с выбранным пользователем
        /// </summary>
        private void UserListViewItemClick(object sender, MouseButtonEventArgs e)
        {
            var user = UsersListView.SelectedItem as User;
            if (user != null)
            {
                new AddUpdateUserWindow(user,this).Show();
            }
        }
    }
}
