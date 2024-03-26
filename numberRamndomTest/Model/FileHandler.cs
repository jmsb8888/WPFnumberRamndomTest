using Microsoft.VisualBasic.FileIO;
using NotVisualBasic.FileIO;
using numberRamndomTest.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            List<double> data = new List<double>();
            using (TextFieldParser parser = new TextFieldParser(this.filePath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(";");

                while (!parser.EndOfData)
                {
                    string[] fila = parser.ReadFields();
                    foreach (string s in fila)
                    {
                        double aux = double.Parse(s, CultureInfo.InvariantCulture);
                        data.Add(aux);
                    }
                }
            }

            return data;
        }

        public List<ModelKS> ReadKsTableCsv()
        {
            List<ModelKS> data = new List<ModelKS>();
            using (TextFieldParser parser = new TextFieldParser(this.filePath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(";");
                while (!parser.EndOfData)
                {
                    string[] row = parser.ReadFields();
                    ModelKS aux = new ModelKS
                    {
                        Quantity = double.Parse(row[0]),
                        Error020 = double.Parse(row[1]),
                        Error010 = double.Parse(row[2]),
                        Error005 = double.Parse(row[3]),
                        Error002 = double.Parse(row[4]),
                        Error001 = double.Parse(row[5]),
                        Error0005 = double.Parse(row[6]),
                        Error0002 = double.Parse(row[7]),
                        Error0001 = double.Parse(row[8])
                    };
                    data.Add(aux);     
                }
            }

            return data;
        }
    }
}
