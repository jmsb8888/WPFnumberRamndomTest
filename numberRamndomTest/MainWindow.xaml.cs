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
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Reflection;
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
        double ErrorEstimated = 0;
        int IntervalsQuantity = 0;
        ViewModelVisibility viewModelVisibility = new ViewModelVisibility();
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = viewModelVisibility;
            IntervalTextBox.IsEnabled = isAddFile;

            viewModelVisibility.IsMeanVisible = ControlVisibility(DoMeansTest);
            viewModelVisibility.IsVarianceVisible = ControlVisibility(DoVarianceTest);
            viewModelVisibility.IsChiSquareVisible = ControlVisibility(DoChiSquareTest);
            viewModelVisibility.IsKsTestVisible = ControlVisibility(DoKSTest);
            viewModelVisibility.IsPokerVisible = ControlVisibility(DoPokerTest);
            btnStart.IsEnabled = false;
            CbxErrors.IsEnabled = false;
            IntervalTextBox.IsEnabled = false;
            ActicateAndDeactivateAll();
        }
         private void ActicateAndDeactivateAll()
        {
            btnMeans.IsEnabled = isAddFile;
            btnVar.IsEnabled = isAddFile;
            btnCHI.IsEnabled = isAddFile;
            btnKS.IsEnabled = isAddFile;
            btnPoker.IsEnabled = isAddFile;
            btnAll.IsEnabled = isAddFile;
            CbxErrors.IsEnabled= isAddFile;
            IntervalTextBox.IsEnabled= isAddFile;




        }
        private void ActivateStart()
        {
            if (DoMeansTest || DoVarianceTest || DoChiSquareTest || DoKSTest || DoPokerTest)
            {
                btnStart.IsEnabled = true;
            }
            else
            {
                btnStart.IsEnabled = false;
            }
        }
        private void AssignError(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            double Error = 0;
            if (comboBox.SelectedItem != null)
            {
                ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
                string selectValue = selectedItem.Content.ToString().Trim();
                if (!double.TryParse(selectValue, out Error))
                {
                    MessageBox.Show("El valor seleccionado no es un número válido.");
                }
            }
            this.ErrorEstimated = Error;
        }
        private void NumberTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void NumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (int.TryParse(textBox.Text, out int value))
            {
                if (value > 280)
                {
                    textBox.Text = "280";
                }
                this.IntervalsQuantity = value;
            }
            else
            {
                textBox.Text = "";
            }
        }

        private Visibility ControlVisibility(bool value)
        {
            return value ? Visibility.Visible : Visibility.Collapsed;
        }
        private void SetButtonState(Button button, bool isEnabled)
        {
            Color DeactiveColor = (Color)ColorConverter.ConvertFromString("#DDDDDD");
            Color activeColor = (Color)ColorConverter.ConvertFromString("#4a9d9c");
            button.Background = isEnabled ? new SolidColorBrush(activeColor): new SolidColorBrush(DeactiveColor);
        }
        private void SetButtonEnabled(Button button, bool isEnabled)
        {
            button.IsEnabled = isEnabled;
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
            ActivateStart();

        }

        private void Test_Variance(object sender, RoutedEventArgs e)
        {
            DoVarianceTest = !DoVarianceTest;
            SetButtonState(btnVar, DoVarianceTest);
            ActivateStart();
        }

        private void Test_CHI_Square(object sender, RoutedEventArgs e)
        {
            DoChiSquareTest = !DoChiSquareTest;
            SetButtonState(btnCHI, DoChiSquareTest);
            ActivateStart();
        }

        private void Test_KS(object sender, RoutedEventArgs e)
        {
            DoKSTest= !DoKSTest;
            SetButtonState(btnKS, DoKSTest);
            ActivateStart();
        }
        private void Test_Poker(object sender, RoutedEventArgs e)
        {
            DoPokerTest= !DoPokerTest;
            SetButtonState(btnPoker, DoPokerTest);
            ActivateStart();
        }
        private void All_Test(object sender, RoutedEventArgs e)
        {
            bool newValue = !DoAllTest;

            DoAllTest = newValue;

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
            SetButtonState(btnAll, DoAllTest);

            ActivateStart();
        }
        private void Start_Test(object sender, RoutedEventArgs e)
        {

            ControllerTest controller = new ControllerTest(filePath, ErrorEstimated, IntervalsQuantity);
            MessageBox.Show("intervalos" + IntervalsQuantity + " error " + ErrorEstimated);
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
            viewModelVisibility.RowHeightChiSquarer = (DoChiSquareTest) ? 500 : 0;
            viewModelVisibility.RowHeightKS = (DoKSTest) ? 500 : 0;
            isAddFile = false;
            btnLoad.IsEnabled = false;
            ActicateAndDeactivateAll();
            btnStart.IsEnabled = false;
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
                        PrintResult("Prueba de Medias", controller, result, MeanTest);
                    }
                    else if (count == 1)
                    {
                        PrintResult("Prueba de Varianzas", controller, result, VarianceTest);
                    }else if(count == 2 && condition.Value)
                    {
                        PrintResult("Prueba de CHI Cuadrado", controller, result, CHiSquareTest);
                        CreateTableCHiSquare(controller.GetTableChiSquares());
                    }else if(count == 3 && condition.Value)
                    {
                        PrintResult("Prueba KS", controller, result, KsTest);
                        CreateTableKs(controller.GetTableKS());
                    }else if (count == 4 && condition.Value)
                    {
                        PrintResult("Prueba de Poker", controller, result, PokerTest);
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
                Paragraph paragraph = new Paragraph(new Run($"{pair.Key} {pair.Value.ToString("N5", CultureInfo.InvariantCulture)}"));
                paragraph.FontSize = 22;
                rich.Document.Blocks.Add(paragraph);
            }
            Paragraph resultParagraph = new Paragraph(new Run("Resultado de la prueba: " + resultTest));
            resultParagraph.FontSize = 22; 
            rich.Document.Blocks.Add(resultParagraph);
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
        private void RestartApplication(object sender, RoutedEventArgs e)
        {

            filePath = string.Empty;
            DoMeansTest = false;
            DoVarianceTest = false;
            DoChiSquareTest = false;
            DoKSTest = false;
            DoPokerTest = false;
            DoAllTest = false;
            isAddFile = false;
            ErrorEstimated = 0;
            double selectedError = 0;
            if (CbxErrors.SelectedItem != null)
            {
                ComboBoxItem selectedItem = (ComboBoxItem)CbxErrors.SelectedItem;
                string selectedValue = selectedItem.Content.ToString().Trim();
                double.TryParse(selectedValue, out selectedError);
            }
            else
            {
                selectedError = 0.05;
            }
            ErrorEstimated = selectedError;
            IntervalsQuantity = 8;
            IntervalTextBox.Text = IntervalsQuantity.ToString();
            btnLoad.IsEnabled = true;
            btnStart.IsEnabled = false;
            btnMeans.IsEnabled = false;
            btnVar.IsEnabled = false;
            btnCHI.IsEnabled = false;
            btnKS.IsEnabled = false;
            btnPoker.IsEnabled = false;
            btnAll.IsEnabled = false;
            CbxErrors.IsEnabled = false;
            IntervalTextBox.IsEnabled = false;
            MeanTest.Document.Blocks.Clear();
            VarianceTest.Document.Blocks.Clear();
            CHiSquareTest.Document.Blocks.Clear();
            KsTest.Document.Blocks.Clear();
            PokerTest.Document.Blocks.Clear();
            Chi2Table.ItemsSource = null;
            KSTestTable.ItemsSource = null;
            PokerTestTable.ItemsSource = null;
            viewModelVisibility.ChartSeriesChiSquare.Clear();
            viewModelVisibility.IntervalsChiSquare.Clear();
            viewModelVisibility.ChartSeriesKS.Clear();
            viewModelVisibility.IntervalsKs.Clear();
            viewModelVisibility.IsMeanVisible = Visibility.Collapsed;
            viewModelVisibility.IsVarianceVisible = Visibility.Collapsed;
            viewModelVisibility.IsChiSquareVisible = Visibility.Collapsed;
            viewModelVisibility.IsKsTestVisible = Visibility.Collapsed;
            viewModelVisibility.IsPokerVisible = Visibility.Collapsed;
            viewModelVisibility.RowHeightChiSquarer = 0;
            viewModelVisibility.RowHeightKS = 0;

            
            ActicateAndDeactivateAll();
            SetButtonState(btnMeans, false);
            SetButtonState(btnVar, false);
            SetButtonState(btnCHI, false);
            SetButtonState(btnKS, false);
            SetButtonState(btnPoker, false);
            SetButtonState(btnAll, false);
        }
    }
}