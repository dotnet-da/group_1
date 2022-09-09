using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using StreamKing.Data.Accounts;
using StreamKing.Data.Media;

namespace StreamKing.MainApplication.Views
{
    /// <summary>
    /// Interaktionslogik für AdminView.xaml
    /// Administration Page
    /// - Anzeige aller Accounts in Tabelle 
    ///    - Wenn Account ausgewählt wird, in separater Tabelle alle dazugehörigen Logs laden
    ///    - Wenn Account ausgewählt wird, in separater Tabelle alle dazugehörigen Watchlists laden 
    ///    - Möglichkeit neuen Account in die Datenbank zu schreiben
    ///    - Möglichkeit Account Daten zu bearbeiten
    ///    - Möglichkeit Account zu löschen
    /// </summary>
    public partial class AdminView : UserControl
    {
        public AdminView()
        {
            InitializeComponent();
        }

        public Account? ChosenAccount { get; set; }
        public Watchlist? watchlist { get; set; }


        //Logs und Watchlist als PopUp/MessageBox möglich? Oder soll eine weitere ViewPage ("AdminDetailedAccountView.xaml" erstellt werden?)
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (ChosenAccount != null)
            {
                
                if (App._mainWindow is not null)
                {
                    App._mainWindow.SetUpdateAccountView(ChosenAccount);
                }
                
                
            }
            else 
            {
                MessageBox.Show("AllAccountGrid_SelectionChanged: ChosenUser= is NULL");
            }
        }

        private void CloseButton_Clicked(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Window.GetWindow(this)).SetLandingPageView();
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            //Weiterleitung zu einer (leicht veränderten) RegisterPage ohne Captcha
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

            if (ChosenAccount == null)
            {
                DeleteButton.IsEnabled = false;
            }
            else
                checkID.Text = ChosenAccount.FirstName;
                App.DeleteSelectedUser(ChosenAccount);
            
        }

        private void AllAccountsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            DataGridRow? row;
            try
            {
                row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(dataGrid.SelectedIndex);

            }
            catch (Exception)
            {
                ChosenAccount = null;
                DeleteButton.IsEnabled = false;
                EditButton.IsEnabled = false;
                return;
            }
           
            if (row == null)
            {
                ChosenAccount = null;
                MessageBox.Show("AllAccountGrid_SelectionChanged: ChosenUser= is NULL");
                return;
            }
                
            DataGridCell RowColumn = dataGrid.Columns[0].GetCellContent(row).Parent as DataGridCell;
            string CellValue = ((TextBlock)RowColumn.Content).Text;

            ChosenAccount = AllAccountsGrid.SelectedValue as Account;

            if (ChosenAccount!= null) 
            {
                Console.WriteLine(ChosenAccount.Id);
                checkID.Text = ChosenAccount.Id.ToString();
                App.GetUserWatchlist(ChosenAccount);
                watchlist = App._userWatchlist;

                DeleteButton.IsEnabled = true;
                EditButton.IsEnabled = true;
            }

        }
    }
}
