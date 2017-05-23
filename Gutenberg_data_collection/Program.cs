using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gutenberg_data_collection
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            CSVConverter converter = new CSVConverter();
            Console.WriteLine("Hey please type in the path of the Tsv file you want to extract data from: ");
            string tsvPath = Console.ReadLine();
            Console.WriteLine("please type in the path for the output csv file: ");
            string outcsvPath = Console.ReadLine();
            //Console.WriteLine("Now Select what columns your want to extract data from:");
            //Console.WriteLine()
            List<int> choosenColumums = new List<int>(new int[] { 0, 1, 4, 5 });
            Console.WriteLine("Start extracting data...");
            //converter.InsertValuesInCSV(converter.ExtractCSVData(csvPath, choosenColumums), outcsvPath, "id,name,latitude,longitude");
            converter.ExtractCSVData2(tsvPath, outcsvPath, choosenColumums);
            Console.Read();
        }
    }
}