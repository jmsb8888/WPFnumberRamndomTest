using Microsoft.Win32;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace numberRamndomTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string filePath = string.Empty;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Load_File(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos de texto (*.txt)|*.txt|Todos los archivos (*.*)|*.*";
            bool? result = openFileDialog.ShowDialog();
        
            if (result == true)
            {
                filePath = openFileDialog.FileName;
                MessageBox.Show($"Se seleccionó el archivo: {filePath}");
            }
        }

        private void Test_Means(object sender, RoutedEventArgs e)
        {

        }

        private void Test_Variance(object sender, RoutedEventArgs e)
        {

        }

        private void Test_CHI_Square(object sender, RoutedEventArgs e)
        {

        }

        private void Test_KS(object sender, RoutedEventArgs e)
        {

        }
        private void Test_Poker(object sender, RoutedEventArgs e)
        {

        }
    }
}