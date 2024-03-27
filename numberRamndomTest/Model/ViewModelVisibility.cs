using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LiveCharts;
using LiveCharts.Wpf;

namespace numberRamndomTest.Model
{
    //Implementa INotifyPropertyChanged para la actualización de la interfaz de usuario
    public class ViewModelVisibility : INotifyPropertyChanged
    {
        // Propiedad para la visibilidad del test Chi cuadrado
        private Visibility _isCHiSquarerVisible = Visibility.Collapsed;
        public Visibility IsChiSquareVisible
        {
            get { return _isCHiSquarerVisible; }
            set
            {
                if (_isCHiSquarerVisible != value)
                {
                    _isCHiSquarerVisible = value;
                    OnPropertyChanged(nameof(IsChiSquareVisible));
                }
            }
        }
        // Propiedad para la visibilidad del test KS
        private Visibility _isKsTestVisible = Visibility.Collapsed;
        public Visibility IsKsTestVisible
        {
            get { return _isKsTestVisible; }
            set
            {
                if (_isKsTestVisible != value)
                {
                    _isKsTestVisible = value;
                    OnPropertyChanged(nameof(IsKsTestVisible));
                }
            }
        }
        // Propiedad para la visibilidad del test de Poker
        private Visibility _isPokerTestVisible = Visibility.Collapsed;
        public Visibility IsPokerVisible
        {
            get { return _isPokerTestVisible; }
            set
            {
                if (_isPokerTestVisible != value)
                {
                    _isPokerTestVisible = value;
                    OnPropertyChanged(nameof(IsPokerVisible));
                }
            }
        }
        // Propiedad para la visibilidad del test de media
        private Visibility _isMeanTestVisible = Visibility.Collapsed;
        public Visibility IsMeanVisible
        {
            get { return _isMeanTestVisible; }
            set
            {
                if (_isMeanTestVisible != value)
                {
                    _isMeanTestVisible = value;
                    OnPropertyChanged(nameof(IsMeanVisible));
                }
            }
        }
        // Propiedad para la visibilidad del test de varianza
        private Visibility _isVarianceTestVisible = Visibility.Collapsed;
        public Visibility IsVarianceVisible
        {
            get { return _isVarianceTestVisible; }
            set
            {
                if (_isVarianceTestVisible != value)
                {
                    _isVarianceTestVisible = value;
                    OnPropertyChanged(nameof(IsVarianceVisible));
                }
            }
        }
        // Colecciones para almacenar datos para gráficos
        public SeriesCollection ChartSeriesChiSquare { get; set; }
        public List<string> IntervalsChiSquare { get; set; }
        public SeriesCollection ChartSeriesKS { get; set; }
        public List<string> IntervalsKs { get; set; }
        // Constructor de la clase ViewModelVisibility
        public ViewModelVisibility()
        {
            ChartSeriesChiSquare = new SeriesCollection();
            IntervalsChiSquare = new List<string>();
            ChartSeriesKS = new SeriesCollection();
            IntervalsKs = new List<string>();
        }
        /* public void AddNewSeriesToChart(IEnumerable<int> newFrequencies, string seriesTitle)
         {
             var newSeries = new ColumnSeries
             {
                 Title = seriesTitle,
                 Values = new ChartValues<int>(newFrequencies)
             };
             ChartSeriesChiSquare.Add(newSeries);
             OnPropertyChanged(nameof(ChartSeriesChiSquare));
         }*/
        // Evento PropertyChanged para la notificación de cambios en las propiedades
        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        // Propiedades para la altura de las filas en la interfaz de usuario
        private int _rowHeightChiSquarer;
        public int RowHeightChiSquarer
        {
            get { return _rowHeightChiSquarer; }
            set
            {
                _rowHeightChiSquarer = value;
                OnPropertyChanged("RowHeightChiSquarer");
            }
        }
        private int _rowHeighKS;
        public int RowHeightKS
        {
            get { return _rowHeighKS; }
            set
            {
                _rowHeighKS = value;
                OnPropertyChanged("RowHeightKS");
            }
        }


    }
}
