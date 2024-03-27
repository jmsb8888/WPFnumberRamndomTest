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
using System.Runtime.ConstrainedExecution;
namespace numberRamndomTest
{
    public partial class MainWindow : Window
    {
        // Ruta del archivo seleccionado
        string filePath = string.Empty;
        // Variables para indicar qué pruebas realizar
        Boolean DoMeansTest = false;
        Boolean DoVarianceTest = false;
        Boolean DoChiSquareTest = false;
        Boolean DoKSTest = false;
        Boolean DoPokerTest = false;
        Boolean DoAllTest = false;
        Boolean isAddFile = false;
        // Error estimado y cantidad de intervalos
        double ErrorEstimated = 0;
        int IntervalsQuantity = 0;
        // Instancia de ViewModelVisibility para el manejo de la visibilidad de la interfaz
        ViewModelVisibility viewModelVisibility = new ViewModelVisibility();
        public MainWindow()
        {
            InitializeComponent();
            // Establecer el contexto de datos para la ventana principal
            this.DataContext = viewModelVisibility;
            // Deshabilitar el cuadro de texto de intervalos hasta que se agregue un archivo
            IntervalTextBox.IsEnabled = isAddFile;
            // Inicializar la visibilidad de los controles según las pruebas seleccionadas
            viewModelVisibility.IsMeanVisible = ControlVisibility(DoMeansTest);
            viewModelVisibility.IsVarianceVisible = ControlVisibility(DoVarianceTest);
            viewModelVisibility.IsChiSquareVisible = ControlVisibility(DoChiSquareTest);
            viewModelVisibility.IsKsTestVisible = ControlVisibility(DoKSTest);
            viewModelVisibility.IsPokerVisible = ControlVisibility(DoPokerTest);
            // Deshabilitar el botón de inicio hasta que se agregue un archivo
            btnStart.IsEnabled = false;
            CbxErrors.IsEnabled = false;
            IntervalTextBox.IsEnabled = false;
            ActicateAndDeactivateAll();
        }
        //activar o desactivar todos los botones relacionados con las pruebas
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
        //activar el botón de inicio cuando al menos una prueba está seleccionada
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
        // asignar el error seleccionado desde el ComboBox
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
        // Restringir la entrada de texto en el cuadro de texto de intervalos a números enteros
        private void NumberTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        // Actualizar la cantidad de intervalos según el texto ingresado
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
        // Controlar la visibilidad de un control
        private Visibility ControlVisibility(bool value)
        {
            return value ? Visibility.Visible : Visibility.Collapsed;
        }
        // Establecer el estado (habilitado/deshabilitado) de un botón y cambiar su color de fondo
        private void SetButtonState(Button button, bool isEnabled)
        {
            Color DeactiveColor = (Color)ColorConverter.ConvertFromString("#DDDDDD");
            Color activeColor = (Color)ColorConverter.ConvertFromString("#4a9d9c");
            button.Background = isEnabled ? new SolidColorBrush(activeColor): new SolidColorBrush(DeactiveColor);
        }
        // Establecer el estado (habilitado/deshabilitado) de un botón
        private void SetButtonEnabled(Button button, bool isEnabled)
        {
            button.IsEnabled = isEnabled;
        }
        // Cargar un archivo CSV
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
        // Activar/desactivar la prueba de medias
        private void Test_Means(object sender, RoutedEventArgs e)
        {
           
            DoMeansTest = !DoMeansTest;
            SetButtonState(btnMeans, DoMeansTest);
            ActivateStart();

        }
        //Activar/desactivar la prueba de varianzas
        private void Test_Variance(object sender, RoutedEventArgs e)
        {
            DoVarianceTest = !DoVarianceTest;
            SetButtonState(btnVar, DoVarianceTest);
            ActivateStart();
        }
        //Activar/desactivar la prueba de Chi cuadrado
        private void Test_CHI_Square(object sender, RoutedEventArgs e)
        {
            DoChiSquareTest = !DoChiSquareTest;
            SetButtonState(btnCHI, DoChiSquareTest);
            ActivateStart();
        }
        //Activar/desactivar la prueba KS
        private void Test_KS(object sender, RoutedEventArgs e)
        {
            DoKSTest= !DoKSTest;
            SetButtonState(btnKS, DoKSTest);
            ActivateStart();
        }
        //Activar/desactivar la prueba de Poker
        private void Test_Poker(object sender, RoutedEventArgs e)
        {
            DoPokerTest= !DoPokerTest;
            SetButtonState(btnPoker, DoPokerTest);
            ActivateStart();
        }
        //Activar/desactivar la todas las pruebas
        private void All_Test(object sender, RoutedEventArgs e)
        {
            bool newValue = !DoAllTest;

            DoAllTest = newValue;

            DoMeansTest = newValue;
            DoVarianceTest = newValue;
            DoChiSquareTest = newValue;
            DoKSTest = newValue;
            DoPokerTest = newValue;
            // Establecer el estado de los botones individuales y el botón de prueba global
            SetButtonState(btnMeans, DoMeansTest);
            SetButtonState(btnMeans, DoMeansTest);
            SetButtonState(btnVar, DoVarianceTest);
            SetButtonState(btnCHI, DoChiSquareTest);
            SetButtonState(btnKS, DoKSTest);
            SetButtonState(btnPoker, DoPokerTest);
            SetButtonState(btnAll, DoAllTest);
            // Activar el botón de inicio si se selecciona alguna prueba
            ActivateStart();
        }
        // Iniciar las pruebas
        private void Start_Test(object sender, RoutedEventArgs e)
        {
            // Crear una instancia del controlador de prueba con la ruta del archivo, error estimado y cantidad de intervalos
            ControllerTest controller = new ControllerTest(filePath, ErrorEstimated, IntervalsQuantity);

            //                  MessageBox.Show("intervalos" + IntervalsQuantity + " error " + ErrorEstimated);
            // Diccionario que contiene las pruebas y las funciones asociadas a ellas
            var tests = new Dictionary<string, Func<bool>>
                {
                    { "DoMeansTest", () => controller.CreateMeanTest() },
                    { "DoVarianceTest", () => controller.CreateVarianceTest() },
                    { "DoChiSquareTest", () => controller.CreateCHiSquareTest() },
                    { "DoKSTest", () => controller.CreateKSTest() },
                    { "DoPokerTest", () => controller.CreatePokerTest() }
                };
            // Diccionario que contiene el estado de las pruebas
            var conditions = new Dictionary<string, bool>
                {
                    { "DoMeansTest", DoMeansTest },
                    { "DoVarianceTest", DoVarianceTest },
                    { "DoChiSquareTest", DoChiSquareTest },
                    { "DoKSTest", DoKSTest },
                    { "DoPokerTest", DoPokerTest }
                };
            // Establecer la visibilidad  de los elementos de la interfaz según las pruebas seleccionadas
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
            // Ejecutar las pruebas
            ExecuteTest(conditions, tests, controller);
        }
        // Ejecutar las pruebas
        private void ExecuteTest(Dictionary<string, bool> conditions, Dictionary<string, Func<bool>> tests, ControllerTest controller)
        {
            // Documento de flujo para mostrar los resultados
            FlowDocument document = new FlowDocument();
            int count = 0;
            // Iterar a través de las condiciones de las pruebas
            foreach (var condition in conditions)
            {
                // Verificar si la prueba está seleccionada y si existe una función asociada
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
        // Imprimir los resultados de las pruebas en un RichTextBox
        private void PrintResult(string nameTest, ControllerTest controller, bool resultTest, RichTextBox rich)
        {
            // Obtener los resultados de la prueba desde el controlador
            Dictionary<string, double> result = controller.GetResults();
            // Crear un párrafo para el título de la prueba
            Paragraph titleParagraph = new Paragraph(new Run(nameTest));
            titleParagraph.FontWeight = FontWeights.Bold;
            titleParagraph.FontSize = 25;
            rich.Document.Blocks.Add(titleParagraph);
            // Iterar a través de los resultados y agregarlos al RichTextBox
            foreach (var pair in result)
            {
                Paragraph paragraph = new Paragraph(new Run($"{pair.Key} {pair.Value.ToString("N5", CultureInfo.InvariantCulture)}"));
                paragraph.FontSize = 22;
                rich.Document.Blocks.Add(paragraph);
            }
            // Agregar el resultado de la prueba al RichTextBox
            Paragraph resultParagraph = new Paragraph(new Run("Resultado de la prueba: " + resultTest));
            resultParagraph.FontSize = 22; 
            rich.Document.Blocks.Add(resultParagraph);
        }


        //Crear la tabla de resultados chi cuadrado
        private void CreateTableCHiSquare(ObservableCollection<FormatTableChiSquare> data)
        {
            Chi2Table.ItemsSource = data;
            CreateGraph(data, "Chi2");    
        }
        //Crear la tabla de resultados KS
        private void CreateTableKs(ObservableCollection<FormatTableKS> data)
        {
           KSTestTable.ItemsSource = data;
            CreateGraph(data, "KS");  
        }
        //Crear la tabla de resultados Poker
        private void CreateTablePoker(ObservableCollection<FormatTablePoker> data)
        {
            PokerTestTable.ItemsSource = data;
        }
        //Crear las graficas de resultados
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
        //Actualizar el grafico
        private void UpdateGraph(string propertyOne, string propertyTwo)
        {
            viewModelVisibility.OnPropertyChanged(propertyOne);
            viewModelVisibility.OnPropertyChanged(propertyTwo);
        }
        //Reinciar la aplicacion para leeer un nuevo csv
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