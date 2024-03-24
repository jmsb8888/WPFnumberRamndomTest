using MathNet.Numerics.Distributions;
using Microsoft.Win32;
using ModelRandomTest;
using numberRamndomTest.Controller;
using numberRamndomTest.Model;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Wpf;
//using System.Windows.Forms.DataVisualization;
namespace numberRamndomTest
{
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

        }

        private void Test_Variance(object sender, RoutedEventArgs e)
        {
            DoVarianceTest = !DoVarianceTest;
            SetButtonState(btnVar, DoVarianceTest);

        }

        private void Test_CHI_Square(object sender, RoutedEventArgs e)
        {
            DoChiSquareTest = !DoChiSquareTest;
            SetButtonState(btnCHI, DoChiSquareTest);
        }

        private void Test_KS(object sender, RoutedEventArgs e)
        {
            DoKSTest= !DoKSTest;
            SetButtonState(btnKS, DoKSTest);
        }
        private void Test_Poker(object sender, RoutedEventArgs e)
        {
            DoPokerTest= !DoPokerTest;
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
            ControllerTest controller = new ControllerTest(filePath, 0.05, 8);
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
            FlowDocument document = new FlowDocument();
            int count = 0;
            foreach (var condition in conditions)
            {
                
                if (condition.Value && tests.TryGetValue(condition.Key, out var testAction))
                {
                    bool result = testAction();
                    document = PrintResult(condition.Key, controller, result, document);
                    if(count == 2 && condition.Value)
                    {
                        document = CreateTableCHiSquare(6, controller.GetTableChiSquares(), document);
                    }else if(count == 3 && condition.Value)
                    {
                        document = CreateTableKs(9, controller.GetTableKS(), document);
                    }else if (count == 4 && condition.Value)
                    {
                        document = CreateTablePoker(5, controller.GetTablePoker(), document);
                    }
                }
                count++;
            }
            FlDoResult.Document = document;
        }

        private FlowDocument PrintResult(string nameTest, ControllerTest controller, bool resultTest, FlowDocument document)
        {
            Dictionary<string, double> result = controller.GetResults();
            Paragraph paragraph = new Paragraph(new Run(nameTest));
            document.Blocks.Add(paragraph);
            foreach (var pair in result)
            {
                paragraph = new Paragraph(new Run($"Clave: {pair.Key}, Valor: {pair.Value.ToString(CultureInfo.InvariantCulture)}"));
                document.Blocks.Add(paragraph);
            }
            paragraph = new Paragraph(new Run("Resultado de la prueba: " + resultTest));
            document.Blocks.Add(paragraph);
            for (int i = 0; i < 3; i++)
            {
                paragraph = new Paragraph(new Run(""));
                document.Blocks.Add(paragraph);
            }
         
            return document;
        }
        private FlowDocument CreateTableCHiSquare(int numColumns, ObservableCollection<FormatTableChiSquare> data, FlowDocument document)
        {
            Table table = new Table();
            for (int i = 0; i < numColumns; i++)
            {
                TableColumn column = new TableColumn();
                table.Columns.Add(column);
            }

            table.RowGroups.Add(new TableRowGroup());

            foreach (var item in data)
            {
                TableRow row = new TableRow();
                table.RowGroups[0].Rows.Add(row);
                row.Cells.Add(new TableCell(new Paragraph(new Run(item.Index.ToString()))));
                row.Cells.Add(new TableCell(new Paragraph(new Run(item.beginning.ToString()))));
                row.Cells.Add(new TableCell(new Paragraph(new Run(item.End.ToString()))));
                row.Cells.Add(new TableCell(new Paragraph(new Run(item.ObtainedFrequency.ToString()))));
                row.Cells.Add(new TableCell(new Paragraph(new Run(item.ExpectedFrequency.ToString()))));
                row.Cells.Add(new TableCell(new Paragraph(new Run(item.CHiSquarer.ToString()))));
            }
            document.Blocks.Add(table);
            document = CreateGraph(data, document);
            return document;
        }
        private FlowDocument CreateTableKs(int numColumns, ObservableCollection<FormatTableKS> data, FlowDocument document)
        {
            Table table = new Table();
            for (int i = 0; i < numColumns; i++)
            {
                TableColumn column = new TableColumn();
                table.Columns.Add(column);
            }

            table.RowGroups.Add(new TableRowGroup());

            foreach (var item in data)
            {
                TableRow row = new TableRow();
                table.RowGroups[0].Rows.Add(row);
                row.Cells.Add(new TableCell(new Paragraph(new Run(item.Index.ToString()))));
                row.Cells.Add(new TableCell(new Paragraph(new Run(item.Beginning.ToString()))));
                row.Cells.Add(new TableCell(new Paragraph(new Run(item.End.ToString()))));
                row.Cells.Add(new TableCell(new Paragraph(new Run(item.ObtainedFrequency.ToString()))));
                row.Cells.Add(new TableCell(new Paragraph(new Run(item.AcomulatedObtainedFrecuency.ToString()))));
                row.Cells.Add(new TableCell(new Paragraph(new Run(item.ObtainedProbability.ToString()))));
                row.Cells.Add(new TableCell(new Paragraph(new Run(item.AcomulatedExpectedFrequency.ToString()))));
                row.Cells.Add(new TableCell(new Paragraph(new Run(item.ExpectedProbability.ToString()))));
                row.Cells.Add(new TableCell(new Paragraph(new Run(item.Difference.ToString()))));
            }
            document.Blocks.Add(table);
            document = CreateGraph(data, document);
            return document;
        }
        private FlowDocument CreateTablePoker(int numColumns, ObservableCollection<FormatTablePoker> data, FlowDocument document)
        {
            Table table = new Table();
            for (int i = 0; i < numColumns; i++)
            {
                TableColumn column = new TableColumn();
                table.Columns.Add(column);
            }

            table.RowGroups.Add(new TableRowGroup());

            foreach (var item in data)
            {
                TableRow row = new TableRow();
                table.RowGroups[0].Rows.Add(row);
                row.Cells.Add(new TableCell(new Paragraph(new Run(item.Hand.ToString()))));
                row.Cells.Add(new TableCell(new Paragraph(new Run(item.ObservedQuantity.ToString()))));
                row.Cells.Add(new TableCell(new Paragraph(new Run(item.Probability.ToString()))));
                row.Cells.Add(new TableCell(new Paragraph(new Run(item.ExpectedProbability.ToString()))));
                row.Cells.Add(new TableCell(new Paragraph(new Run(item.Result.ToString()))));
            }
            document.Blocks.Add(table);
            return document;
        }
        private FlowDocument CreateGraph<T>(ObservableCollection<T> data, FlowDocument document) where T : IFormatData
        {
            List<string> intervals = new List<string>();
            List<int> frecuencys = new List<int>();
            foreach (var item in data)
            {
                intervals.Add(item.Index.ToString());
                frecuencys.Add((int) item.ObtainedFrequency);
            }
            var chart = new CartesianChart
            {
                Series = new LiveCharts.SeriesCollection
                {
                    new ColumnSeries
                    {
                        Title = "Frecuencia",
                        Values = new ChartValues<int>(frecuencys)
                    }
                },
                LegendLocation = LegendLocation.Right,
                AxisX = new AxesCollection
                {
                    new LiveCharts.Wpf.Axis
                    {
                        Title = "Categorías",
                        Labels = intervals
                    }
                },
                AxisY = new AxesCollection
                {
                    new LiveCharts.Wpf.Axis
                    {
                        Title = "Frecuencia"
                    }
                },
                Height = 400
            };
            document.Blocks.Add(new BlockUIContainer(chart));
            return document;
        }
    }
}