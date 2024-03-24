﻿using MathNet.Numerics.Distributions;
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
using System.Windows.Automation;
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
        ViewModelVisibility viewModelVisibility = new ViewModelVisibility();
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = viewModelVisibility;
            viewModelVisibility.IsMeanVisible = ControlVisibility(DoMeansTest);
            viewModelVisibility.IsVarianceVisible = ControlVisibility(DoVarianceTest);
            viewModelVisibility.IsChiSquareVisible = ControlVisibility(DoChiSquareTest);
            viewModelVisibility.IsKsTestVisible = ControlVisibility(DoKSTest);
            viewModelVisibility.IsPokerVisible = ControlVisibility(DoPokerTest);
            ActicateAndDeactivateAll();
        }
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
        private Visibility ControlVisibility(bool value)
        {
            return value ? Visibility.Visible : Visibility.Collapsed;
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
            ControllerTest controller = new ControllerTest(filePath, 0.05, 0);
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
            viewModelVisibility.IsMeanVisible = ControlVisibility(DoMeansTest);
            viewModelVisibility.IsVarianceVisible = ControlVisibility(DoVarianceTest);
            viewModelVisibility.IsChiSquareVisible = ControlVisibility(DoChiSquareTest);
            viewModelVisibility.IsKsTestVisible = ControlVisibility(DoKSTest);
            viewModelVisibility.IsPokerVisible = ControlVisibility(DoPokerTest);


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
                    if (count == 0)
                    {
                        PrintResult(condition.Key, controller, result, MeanTest);
                    }
                    else if (count == 1)
                    {
                        PrintResult(condition.Key, controller, result, VarianceTest);
                    }else if(count == 2 && condition.Value)
                    {
                        PrintResult(condition.Key, controller, result, CHiSquareTest);
                        CreateTableCHiSquare(controller.GetTableChiSquares());
                    }else if(count == 3 && condition.Value)
                    {
                        PrintResult(condition.Key, controller, result, KsTest);
                        CreateTableKs(controller.GetTableKS());
                    }else if (count == 4 && condition.Value)
                    {
                        PrintResult(condition.Key, controller, result, PokerTest);
                        CreateTablePoker(controller.GetTablePoker());
                    }
                }
                count++;
            }
            
        }

        private void PrintResult(string nameTest, ControllerTest controller, bool resultTest, RichTextBox rich)
        {
            Dictionary<string, double> result = controller.GetResults();
            Paragraph titleParagraph = new Paragraph(new Run(nameTest));
            titleParagraph.FontWeight = FontWeights.Bold;
            titleParagraph.FontSize = 25;
            rich.Document.Blocks.Add(titleParagraph);

            foreach (var pair in result)
            {
                Paragraph paragraph = new Paragraph(new Run($"{pair.Key}: {pair.Value.ToString("N5", CultureInfo.InvariantCulture)}"));
                paragraph.FontSize = 22;
                rich.Document.Blocks.Add(paragraph);
            }

            // Resultado de la prueba
            Paragraph resultParagraph = new Paragraph(new Run("Resultado de la prueba: " + resultTest));
            resultParagraph.FontSize = 22; 
            rich.Document.Blocks.Add(resultParagraph);

            // Añadir espacios en blanco
            for (int i = 0; i < 3; i++)
            {
                Paragraph blankParagraph = new Paragraph(new Run(""));
                rich.Document.Blocks.Add(blankParagraph);
            }
        }
        
        

        private void CreateTableCHiSquare(ObservableCollection<FormatTableChiSquare> data)
        {
            Chi2Table.ItemsSource = data;
            CreateGraph(data, "Chi2");    
        }
        private void CreateTableKs(ObservableCollection<FormatTableKS> data)
        {
           KSTestTable.ItemsSource = data;
            CreateGraph(data, "KS");  
        }
        private void CreateTablePoker(ObservableCollection<FormatTablePoker> data)
        {
            PokerTestTable.ItemsSource = data;
        }
        private void CreateGraph<T>(ObservableCollection<T> data, string typeGraph) where T : IFormatData
        {
            List<string> intervals = new List<string>();
            List<int> frecuencys = new List<int>();
            foreach (var item in data)
            {
                intervals.Add(item.Index.ToString());
                frecuencys.Add((int)item.ObtainedFrequency);
            }
            var serie = new LiveCharts.SeriesCollection
                {
                    new ColumnSeries
                    {
                        Title = "Frecuencia",
                        Values = new ChartValues<int>(frecuencys)
                    }
                };
            if (typeGraph.Equals("Chi2"))
            {
                viewModelVisibility.ChartSeriesChiSquare = serie;
                viewModelVisibility.IntervalsChiSquare = intervals;
                UpdateGraph(nameof(viewModelVisibility.ChartSeriesChiSquare), nameof(viewModelVisibility.IntervalsChiSquare));
            }
            else if (typeGraph.Equals("KS"))
            {
                viewModelVisibility.ChartSeriesKS = serie;
                viewModelVisibility.IntervalsKs = intervals;
                UpdateGraph(nameof(viewModelVisibility.ChartSeriesKS), nameof(viewModelVisibility.IntervalsKs));
            }
        }
        private void UpdateGraph(string propertyOne, string propertyTwo)
        {
            viewModelVisibility.OnPropertyChanged(propertyOne);
            viewModelVisibility.OnPropertyChanged(propertyTwo);
        }
    }
}