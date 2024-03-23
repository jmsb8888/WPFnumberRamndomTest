using ModelRandomTest;
using numberRamndomTest.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace numberRamndomTest.Controller
{
    internal class ControllerTest
    {
        readonly MeanTest meanTest;
        readonly VarianceTest varianceTest;
        readonly ChiSquaredTest chiSquaredTest;
        readonly KSTest ksTest;
        readonly PokerTest pokerTest;
        string filePath;
        List<double> Data;
        Dictionary<string, double> results;
        ObservableCollection<FormatTableChiSquare> formatTableChiSquare;
        ObservableCollection<FormatTableKS> FormatTableKs;
        public ControllerTest(string filePath, double EstimationError,int IntervalQuantity)
        {
            this.filePath = filePath;
            this.Data = CreateCsvFile();
            this.meanTest = new MeanTest(Data, EstimationError);
            this.varianceTest = new VarianceTest(Data, EstimationError);
            this.chiSquaredTest = new ChiSquaredTest(Data, EstimationError, IntervalQuantity);
            this.ksTest = new KSTest(Data, EstimationError, IntervalQuantity);
            this.pokerTest = new PokerTest(Data, EstimationError);
            
        }
        public Dictionary<string, double> GetResults()
        {
            return this.results;
        }

        public ObservableCollection<FormatTableChiSquare> GetTableChiSquares()
        {
            return this.formatTableChiSquare;
        }
        public ObservableCollection<FormatTableKS> GetTableKS()
        {
            return this.FormatTableKs;
        }
        private List<double> CreateCsvFile()
        {
            try
            {
                FileHandler dataFile = new FileHandler(filePath);
                List<double> data = dataFile.ReadCsvFile();
                return data;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en la creación del archivo");
                return null;
            }
        }

        public Boolean CreateMeanTest()
        {
            try
            {
                Boolean result = meanTest.TakeTest();
                results = meanTest.GetResults();
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en la ejecución de la Prueba de Medias");
                return false;
            }
        }
        public Boolean CreateVarianceTest()
        {
            try
            {
                Boolean result = varianceTest.testVar();
                results = varianceTest.GetResults();
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en la ejecución de la Prueba de Varianza");
                return false;
            }
        }

        public Boolean CreateCHiSquareTest()
        {
            try
            {
                Boolean result = chiSquaredTest.testChiSquarer();
                results = chiSquaredTest.GetResults();
                formatTableChiSquare = chiSquaredTest.GetTableChiSquares();
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en la ejecución de la Prueba de CHI Cuadrado");
                return false;
            }
        }
        public Boolean CreateKSTest()
        {
            try
            {
                Boolean result = ksTest.testKS();
                results = ksTest.GetResults();
                FormatTableKs = ksTest.GetTableKS();
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en la ejecución de la Prueba de KS");
                return false;
            }
        }
        public Boolean CreatePokerTest()
        {
            try
            {
                Boolean result = pokerTest.TestPoker();
                results = meanTest.GetResults();
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en la ejecución de la Prueba de Poker");
                return false;
            }
        }
    }
}
