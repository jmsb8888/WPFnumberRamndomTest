using Microsoft.Win32;
using ModelRandomTest;
using numberRamndomTest.Controller;
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
            ActicateAndDeactivateAll();
        }
       /* private void SetTestState(bool means, bool variance, bool chiSquare, bool ks, bool poker)
        {
            DoMeansTest = means;
            DoVarianceTest = variance;
            DoChiSquareTest = chiSquare;
            DoKSTest = ks;
            DoPokerTest = poker;
        }
        private void SelectAndDeselect()
        {
            SetButtonState(btnMeans, DoMeansTest);
            SetButtonState(btnVar, DoVarianceTest);
            SetButtonState(btnCHI, DoChiSquareTest);
            SetButtonState(btnKS, DoKSTest);
            SetButtonState(btnPoker, DoPokerTest);
            SetButtonState(btnStart, isAddFile);
            SetButtonState(btnAll, DoAllTest);
        }*/
         private void ActicateAndDeactivateAll()
        {
            btnMeans.IsEnabled = isAddFile;
            btnVar.IsEnabled = isAddFile;
            btnCHI.IsEnabled = isAddFile;
            btnKS.IsEnabled = isAddFile;
            btnPoker.IsEnabled = isAddFile;
            btnStart.IsEnabled = isAddFile;
            btnAll.IsEnabled = isAddFile;

        }
        private void SetButtonState(Button button, bool isEnabled)
        {
            button.Background = isEnabled ? Brushes.Green: null;
        }
        private void Load_File(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos CSV (*.csv)|*.csv|Todos los archivos (*.*)|*.*";
            bool? result = openFileDialog.ShowDialog();
          
            if (result == true)
            {
                filePath = openFileDialog.FileName;
                MessageBox.Show($"Se seleccionó el archivo: {filePath}");
                isAddFile = true;
                ActicateAndDeactivateAll() ;
            }
        }
      
        private void Test_Means(object sender, RoutedEventArgs e)
        {
           
            DoMeansTest = !DoMeansTest;
            SetButtonState(btnMeans, DoMeansTest);
            /*SetTestState(DoMeansTest, false, false, false, false);
            SelectAndDeselect();*/

        }

        private void Test_Variance(object sender, RoutedEventArgs e)
        {
            DoVarianceTest = !DoVarianceTest;
            /*SetTestState(false, DoVarianceTest, false, false, false);
            SelectAndDeselect();*/
            SetButtonState(btnVar, DoVarianceTest);

        }

        private void Test_CHI_Square(object sender, RoutedEventArgs e)
        {
            DoChiSquareTest = !DoChiSquareTest;
            /*SetTestState(false, false, DoChiSquareTest, false, false);
            SelectAndDeselect();*/
            SetButtonState(btnCHI, DoChiSquareTest);
        }

        private void Test_KS(object sender, RoutedEventArgs e)
        {
            DoKSTest= !DoKSTest;
            /*SetTestState(false, false, false, DoKSTest, false);
            SelectAndDeselect();*/
            SetButtonState(btnKS, DoKSTest);
        }
        private void Test_Poker(object sender, RoutedEventArgs e)
        {
            DoPokerTest= !DoPokerTest;
            /*SetTestState(false, false, false, false, DoPokerTest);
            SelectAndDeselect();*/
            SetButtonState(btnPoker, DoPokerTest);
        }
        private void All_Test(object sender, RoutedEventArgs e)
        {
            bool newValue = !DoAllTest;
            DoMeansTest = newValue;
            DoVarianceTest = newValue;
            DoChiSquareTest = newValue;
            DoKSTest = newValue;
            DoPokerTest = newValue;

            SetButtonState(btnMeans, DoMeansTest);
            SetButtonState(btnVar, DoVarianceTest);
            SetButtonState(btnCHI, DoChiSquareTest);
            SetButtonState(btnKS, DoKSTest);
            SetButtonState(btnPoker, DoPokerTest);
            SetButtonState(btnStart, isAddFile);

        }
        private void Start_Test(object sender, RoutedEventArgs e)
        {
            ControllerTest controller = new ControllerTest(filePath, 0.05, 100);
            var tests = new Dictionary<string, Func<bool>>
                {
                    { "DoMeansTest", () => controller.CreateMeanTest() },
                    { "DoVarianceTest", () => controller.CreateVarianceTest() },
                    { "DoChiSquareTest", () => controller.CreateCHiSquareTest() },
                    { "DoKSTest", () => controller.CreateKSTest() },
                    { "DoPokerTest", () => controller.CreatePokerTest() }
                };

            var conditions = new Dictionary<string, bool>
                {
                    { "DoMeansTest", DoMeansTest },
                    { "DoVarianceTest", DoVarianceTest },
                    { "DoChiSquareTest", DoChiSquareTest },
                    { "DoKSTest", DoKSTest },
                    { "DoPokerTest", DoPokerTest }
                };

            ExecuteTest(conditions, tests, controller);
        }

        private void ExecuteTest(Dictionary<string, bool> conditions, Dictionary<string, Func<bool>> tests, ControllerTest controller)
        {
            foreach (var condition in conditions)
            {
                if (condition.Value && tests.TryGetValue(condition.Key, out var testAction))
                {
                    bool result = testAction();
                    PrintResult(condition.Key, controller, result);
                }
            }
        }
        private void PrintResult(string nameTest, ControllerTest controller, bool resultTest)
        {
            Dictionary<string, double> result = controller.GetResults();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(nameTest);
            foreach (var pair in result)
            {
                sb.AppendLine($"Clave: {pair.Key}, Valor: {pair.Value}");
            }
            sb.AppendLine("resultado de la prueba: " + resultTest);
            sb.AppendLine("");
            sb.AppendLine("");
            sb.AppendLine("");
            TxtBlockResult.Text = sb.ToString();
        }
    }
}