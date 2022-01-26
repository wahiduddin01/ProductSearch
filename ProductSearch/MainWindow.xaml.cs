using System;
using System.Collections.Generic;
using System.Data;
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

namespace ProductSearch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string sku;
        private string artist;
        private string title;
        private string productId;
        public MainWindow()
        {
            InitializeComponent();
            FillDataGrid();
        }

        public void FillDataGrid()
        {
            DataTable dtProduct = new DataTable();
            DBModule dBModule = new DBModule();
            dtProduct = dBModule.FillDataTable();

            if (dtProduct != null && dtProduct.Rows.Count > 0)
            {
                dtGrid.ItemsSource = dtProduct.DefaultView;
            }
            else
            {
                dtGrid.ItemsSource = null;
            }
        }

        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {
            string SKU = txtSKU.Text.Trim();    
            string artist = txtArtisit.Text.Trim().ToUpper();
            string title = txtTitlle.Text.Trim().ToUpper();

            DataTable dtProduct = new DataTable();
            DBModule dBModule = new DBModule();
            dtProduct = dBModule.SearchDB(SKU, artist, title);
            dtGrid.ItemsSource = dtProduct.DefaultView;

        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = (DataGridRow)sender;
            DataRowView drv = (DataRowView)row.Item;
            
            productId = drv[0].ToString();
            sku = drv[1].ToString();
            title = drv[2].ToString();
            artist = drv[3].ToString();

            txtSKU.Text = sku;
            txtArtisit.Text = artist;
            txtTitlle.Text = title;

            searchBtn.Visibility = Visibility.Hidden;
            saveBtn.Visibility = Visibility.Visible;
            cancelBtn.Visibility = Visibility.Visible;

        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            if ( (txtSKU.Text.Trim().Equals(String.Empty)) || 
                (txtArtisit.Text.Trim().Equals(String.Empty)) || (txtTitlle.Text.Trim().Equals(String.Empty)))
            {
                MessageBox.Show("There are empty fields", "Information", MessageBoxButton.OK, MessageBoxImage.Hand);
                return;
            }

            if ( (txtSKU.Text.Trim().ToUpper().Equals(sku.ToUpper())) && (txtArtisit.Text.Trim().ToUpper().Equals(artist.ToUpper())) 
                && (txtTitlle.Text.Trim().ToUpper().Equals(title.ToUpper())) )
            {
                MessageBox.Show("No changes were made", "Information", MessageBoxButton.OK, MessageBoxImage.Hand);
                return;
            }

            string skuToSave = txtSKU.Text.Trim();
            string artistToSave = txtArtisit.Text.Trim();
            string titleToSave = txtTitlle.Text.Trim();

            DBModule dBModule = new DBModule();
            dBModule.UpdateDB(productId, skuToSave, artistToSave, titleToSave);
            MessageBox.Show("Table updated", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            
            searchBtn.Visibility = Visibility.Visible;
            saveBtn.Visibility = Visibility.Hidden;
            cancelBtn.Visibility = Visibility.Hidden;

            txtSKU.Clear();
            txtArtisit.Clear();
            txtTitlle.Clear();
            
            FillDataGrid();

        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            txtSKU.Clear();
            txtArtisit.Clear();
            txtTitlle.Clear();

            searchBtn.Visibility = Visibility.Visible;
            saveBtn.Visibility = Visibility.Hidden;
            cancelBtn.Visibility = Visibility.Hidden;
        }
    }
}
