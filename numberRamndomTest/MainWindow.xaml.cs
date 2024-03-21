using Microsoft.Win32;
using ModelRandomTest;
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
        Boolean DoMeansTest = false;
        Boolean DoVarianceTest = false;
        Boolean DoChiSquareTest = false;
        Boolean DoKSTest = false;
        Boolean DoPokerTest = false;
        Boolean DoAllTest = false;
        Boolean isAddFile = false;

        public MainWindow()
        {
            InitializeComponent();
            ActicateAndDeactivate();
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
                isAddFile = true;
                ActicateAndDeactivate() ;
            }
        }
       private void ActicateAndDeactivate()
        {
            btnMeans.IsEnabled = isAddFile;
            btnVar.IsEnabled = isAddFile;
            btnCHI.IsEnabled = isAddFile;
            btnKS.IsEnabled = isAddFile;
            btnPoker.IsEnabled = isAddFile;
            btnStart.IsEnabled = isAddFile;
            btnAll.IsEnabled = isAddFile;

        }
        private void Test_Means(object sender, RoutedEventArgs e)
        {
           
            DoMeansTest = !DoMeansTest;
            MessageBox.Show($"Se seleccionó el archivo: {DoMeansTest}");

        }

        private void Test_Variance(object sender, RoutedEventArgs e)
        {
            DoVarianceTest = !DoVarianceTest;
        }

        private void Test_CHI_Square(object sender, RoutedEventArgs e)
        {
            DoChiSquareTest = !DoChiSquareTest;
        }

        private void Test_KS(object sender, RoutedEventArgs e)
        {
            DoKSTest= !DoKSTest;
        }
        private void Test_Poker(object sender, RoutedEventArgs e)
        {
            DoPokerTest= !DoPokerTest;
        }
        private void All_Test(object sender, RoutedEventArgs e)
        {
            bool newValue = !DoAllTest;
            DoMeansTest = newValue;
            DoVarianceTest = newValue;
            DoChiSquareTest = newValue;
            DoKSTest = newValue;
            DoPokerTest = newValue;
        }
        private void Start_Test(object sender, RoutedEventArgs e)
        {

        }
    }
}