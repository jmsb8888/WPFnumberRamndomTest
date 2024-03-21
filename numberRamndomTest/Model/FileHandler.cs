using Microsoft.VisualBasic.FileIO;
using NotVisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NotVisualBasic.FileIO.CsvTextFieldParser;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ModelRandomTest
{
    internal class FileHandler
    {
        string filePath;
        List<double> data = new List<double>();

        public FileHandler(string filePath)
        {
            this.filePath = filePath;
        }

        public List<double> ReadCsvFile()
        {
            System.IO.StreamReader file = new StreamReader(this.filePath);
            string delimiter = "\n";
            string line;
            while((line = file.ReadLine()) != null) 
            {
                string[] fila = line.Split(delimiter);
                foreach(string s in fila)
                {
                    double aux = double.Parse(s);
                    data.Add(aux);
                }
                 
            }
            return data;
        }
    }
}
