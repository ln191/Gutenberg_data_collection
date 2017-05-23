using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gutenberg_data_collection
{
    public class CSVConverter
    {
        /// <summary>
        /// Extracts all the data from CSV file
        /// </summary>
        /// <param name="csvPath">The string path to the CSV file</param>
        /// <returns>A List of each row in CSV file, as a lists of string</returns>
        public List<List<string>> ExtractCSVData(string csvPath)
        {
            using (TextFieldParser parser = new TextFieldParser(csvPath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                List<List<string>> dataLists = new List<List<string>>();
                while (!parser.EndOfData)
                {
                    //Processing row
                    List<string> fields = parser.ReadFields().ToList();
                    dataLists.Add(fields);
                }
                Console.WriteLine("Extraction Done");
                return dataLists;
            }
        }

        /// <summary>
        /// Extracts given column data from the TSV file
        /// </summary>
        /// <param name="csvPath">The string path to the TSV file</param>
        /// <param name="columumDataSelect">What columns to Extract data from</param>
        /// <returns>A List of each row in TSV file, as a lists of string</returns>
        public List<List<string>> ExtractCSVData(string csvPath, List<int> columumDataSelect)
        {
            using (TextFieldParser parser = new TextFieldParser(csvPath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters("\t");
                List<List<string>> dataLists = new List<List<string>>();
                while (!parser.EndOfData)
                {
                    //Processing row
                    List<string> fields = parser.ReadFields().ToList();
                    List<string> selectedData = new List<string>();

                    foreach (int fieldNum in columumDataSelect)
                    {
                        Console.WriteLine(fieldNum + " " + fields[fieldNum]);
                        if (fieldNum == 1 && fields[fieldNum].Contains(","))
                        {
                            string city_name = fields[fieldNum];
                            city_name = "\"" + city_name + "\"";
                            selectedData.Add(city_name);
                        }
                        else
                        {
                            selectedData.Add(fields[fieldNum]);
                        }
                    }
                    dataLists.Add(selectedData);
                }
                Console.WriteLine("Extraction Done");
                return dataLists;
            }
        }

        /// <summary>
        /// Extracts given column data from the TSV file
        /// </summary>
        /// <param name="csvPath">The string path to the TSV file</param>
        /// <param name="columumDataSelect">What columns to Extract data from</param>
        /// <returns>A List of each row in TSV file, as a lists of string</returns>
        public void ExtractCSVData2(string tsvPath, string csvPath, List<int> columumDataSelect)
        {
            using (TextFieldParser parser = new TextFieldParser(tsvPath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters("\t");
                //List<List<string>> dataLists = new List<List<string>>();
                StringBuilder csvContent = new StringBuilder();
                csvContent.AppendLine("id,name,latitude,longitude");
                File.AppendAllText(csvPath, csvContent.ToString());
                csvContent.Clear();
                int i = 0;
                while (!parser.EndOfData)
                {
                    try
                    {
                        //Processing row
                        List<string> fields = parser.ReadFields().ToList();
                        List<string> selectedData = new List<string>();

                        foreach (int fieldNum in columumDataSelect)
                        {
                            if (fieldNum == 1 && fields[fieldNum].Contains(","))
                            {
                                string city_name = fields[fieldNum];
                                city_name = "\"" + city_name + "\"";
                                selectedData.Add(city_name);
                            }
                            else
                            {
                                selectedData.Add(fields[fieldNum]);
                            }
                        }
                        csvContent.AppendLine(String.Join(",", selectedData.ToArray()));
                        if (i == 5000)
                        {
                            File.AppendAllText(csvPath, csvContent.ToString());
                            csvContent.Clear();
                            Console.WriteLine("a 5000 rows added...");
                            i = 0;
                        }
                        i++;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Could Not Read Row, moving on");
                    }

                    //dataLists.Add(selectedData);

                    //InsertValuesInCSV(dataLists, "test.csv");
                }
                File.AppendAllText(csvPath, csvContent.ToString());
                csvContent.Clear();
                Console.WriteLine("Extraction Done");
            }
        }

        //   public void InsertValuesInCSV(List<List<string>> data, string csvPath)
        //    {
        //        Console.WriteLine("Creating new CSV file with the extracted data...");
        //        StringBuilder csvContent = new StringBuilder();
        //    csvContent.AppendLine(headerLine);

        //        foreach (List<string> row in data)
        //        {
        //            csvContent.AppendLine(String.Join(",", row.ToArray()));
        //        }
        //File.AppendAllText(csvPath, csvContent.ToString());

        //        Console.WriteLine("csv has been created");
        //    }
    }
}