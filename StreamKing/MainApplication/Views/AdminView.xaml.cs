using System;
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
        //kann ChosenAccount über einen Klick- oder DoppelKlick im DataGrid mit der UserListe eingestellt werden? 


        //Logs und Watchlist als PopUp/MessageBox möglich? Oder soll eine weitere ViewPage ("AdminDetailedAccountView.xaml" erstellt werden?)

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            //Weiterleitung zu der SettingsPage des ausgewählten Accounts

        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            //Weiterleitung zu einer (leicht veränderten) RegisterPage ohne Captcha
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            //siehe App.deleteCurrentUser 
            //oder Weiterleitung zur SettingPage und löschen von da?
            
        }

        private void DataGrid_Unselected(object sender, RoutedEventArgs e)
        {
            ChosenAccount = null;
        }

        private void DataGrid_Selected(object sender, RoutedEventArgs e)
        {
            // unsicher, ob das so stimmt
            ChosenAccount = AllAccountsGrid.SelectedValue as Account;

        }

        private void AllUsers_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void AllUsers_Unselected(object sender, RoutedEventArgs e)
        {

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
                return;
            }
           
            if (row == null)
            {
                ChosenAccount = null;
                return;
            }
                

            DataGridCell RowColumn = dataGrid.Columns[0].GetCellContent(row).Parent as DataGridCell;
            string CellValue = ((TextBlock)RowColumn.Content).Text;

            ChosenAccount = AllAccountsGrid.SelectedValue as Account;

            if (ChosenAccount!= null)
                Console.WriteLine(ChosenAccount.Id);


        }
    }
}
