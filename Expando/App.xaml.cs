using System;
using System.Windows;
using System.Windows.Controls;
using Expando.Core;

namespace Expando
{
    /// <summary>
    /// Representing the main Expando application class.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Creates a new instance of the <see cref="App"/> class.
        /// </summary>
        public App()
        {
            return;
        }

        /// <summary>
        /// Attempts to load user profiles data and shows an error box if it's unsuccessful.
        /// </summary>
        public static void ReloadUserData()
        {
            // loads the users data
            if (UserProfile.LoadUsersData() != 0)
            {
                _ = MessageBox.Show(Messages.CantLoadUserData, "Load Profiles", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Expando.MainWindow mw = new Expando.MainWindow();
            MainWindow = mw;
            MainWindow.Show();

            // loads users data on startup
            ReloadUserData();
        }

        /// <summary>
        /// Creates a new <paramref name="transaction"/> item with defined behavior for <paramref name="transaction"/> modification (<paramref name="onModify"/>) and removal (<paramref name="onRemove"/>).
        /// </summary>
        /// <param name="transaction">Target transaction.</param>
        /// <param name="onModify">Action on modification.</param>
        /// <param name="onRemove">Action on removal.</param>
        /// <returns></returns>
        public static ListBoxItem CreateTransactionItem(Transaction transaction, Action onModify, Action onRemove)
        {
            // context menu items for the item
            // modify transaction item
            MenuItem miModify = new MenuItem
            {
                Header = "Modify",
                InputGestureText = "F2"
            };

            miModify.Click += (s, e) =>
            {
                onModify();
            };

            // remove transaction item
            MenuItem miRemove = new MenuItem
            {
                Header = "Delete",
                InputGestureText = "Del"
            };

            miRemove.Click += (s, e) =>
            {
                onRemove();
            };

            // listbox item itself
            ListBoxItem lbi = new ListBoxItem
            {
                Tag = transaction.Id,
                ContextMenu = new ContextMenu
                {
                    Items =
                    {
                        miModify,
                        miRemove,
                    }
                }
            };

            // container border
            Border bd = new Border();

            // value info (label)
            Label lbValue = new Label
            {
                // set content as the transaction value with the currency format
                Content = transaction.Value.ToString("C"),
                FontSize = 16
            };

            // description info (label)
            Label lbDescription = new Label
            {
                // get the transaction description
                Content = (string.IsNullOrEmpty(transaction.Description.Trim()) == false ? transaction.Description : Messages.NoDescription),
                FontSize = 12
            };

            // data panel
            StackPanel sp = new StackPanel
            {
                Children =
            {
                lbValue,
                lbDescription,
            }
            };

            // set data panel as the border content
            bd.Child = sp;

            // set the listbox item content
            lbi.Content = bd;
            return lbi;
        }
    }

}
