using ModelRandomTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace numberRamndomTest.Controller
{
    internal class Controller
    {
        readonly MeanTest meanTest;
        readonly VarianceTest varianceTest;
        readonly ChiSquaredTest chiSquaredTest;
        readonly KSTest ksTest;
        readonly PokerTest pokerTest;
        string filePath;
        List<double> Data;
        public Controller(string filePath, double EstimationError,int IntervalQuantity)
        {
            this.filePath = filePath;
            this.Data = CreateCsvFile();
            this.meanTest = new MeanTest(Data, EstimationError);
            this.varianceTest = new VarianceTest(Data, EstimationError);
            this.chiSquaredTest = new ChiSquaredTest(Data, EstimationError, IntervalQuantity);
            this.ksTest = new KSTest(Data, EstimationError, IntervalQuantity);
            this.pokerTest = new PokerTest(Data, EstimationError);
            
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

        private Boolean CreateMeanTest()
        {
            try
            {
                Boolean result = meanTest.TakeTest();
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en la ejecución de la Prueba de Medias");
                return false;
            }
        }
        private Boolean CreateVarianceTest()
        {
            try
            {
                Boolean result = varianceTest.testVar();
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en la ejecución de la Prueba de Varianza");
                return false;
            }
        }

        private Boolean CreateCHiSquareTest()
        {
            try
            {
                Boolean result = chiSquaredTest.testChiSquarer();
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en la ejecución de la Prueba de CHI Cuadrado");
                return false;
            }
        }
        private Boolean CreateKSTest()
        {
            try
            {
                Boolean result = ksTest.testKS();
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en la ejecución de la Prueba de KS");
                return false;
            }
        }
        private Boolean CreatePokerTest()
        {
            try
            {
                Boolean result = pokerTest.testPoker();
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
