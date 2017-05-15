using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;

namespace ExtractCitiesFromBooks
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ReadBook bookReader = new ReadBook();
            string[] filePaths = Directory.GetFiles(@"UnzipBooks\", "*.txt",
                                       SearchOption.AllDirectories);
            List<string> citiesInBook = new List<string>();
            Console.WriteLine("input csv books file path:");
            string csvBooksPath = Console.ReadLine();

            StringBuilder csvBookBuilder = new StringBuilder();
            csvBookBuilder.AppendLine("book_id,city_id");
            File.AppendAllText(csvBooksPath, csvBookBuilder.ToString());
            csvBookBuilder.Clear();

            //loads the cities in to a dictionary
            Dictionary<string, string> cities = new Dictionary<string, string>();
            var strLines = File.ReadLines("cities.csv");
            foreach (var line in strLines)
            {
                try
                {
                    cities.Add(line.Split(',')[0], line.Split(',')[1]);
                }
                catch (Exception)
                {
                    Console.WriteLine("already had that key name");
                }
            }
            //loops through all txt paths
            foreach (string path in filePaths)
            {
                //read every line in the given txt file
                foreach (var line in File.ReadLines(path))
                {
                    if (line != "" && line != string.Empty)
                    {
                        //Finds all word that start with capital letter in the line
                        var strwords = FilterWords(line);
                        //loops through the words
                        foreach (string str in strwords)
                        {
                            //if a match is found and it is not in the list, it will be added
                            if (!citiesInBook.Contains(str) && cities.ContainsValue(str))
                            {
                                string[] city_ids = cities.Where(x => x.Value == str).Select(pair => pair.Key).ToArray();
                                foreach (string city_id in city_ids)
                                {
                                    citiesInBook.Add(city_id);
                                }
                            }
                        }
                    }
                }
                //adds the city ids from book to string builder
                foreach (string city in citiesInBook)
                {
                    csvBookBuilder.AppendLine(Path.GetFileNameWithoutExtension(path) + "," + city);
                }
                citiesInBook.Clear();
                //write to the csv file
                File.AppendAllText(csvBooksPath, csvBookBuilder.ToString());
                csvBookBuilder.Clear();
                Console.WriteLine("Book: " + Path.GetFileNameWithoutExtension(path) + " Done, moving on to next book ");
            }
            Console.WriteLine("All Done!!!!");

            Console.Read();
        }

        private static IEnumerable<string> FilterWords(string str)
        {
            var upper = str.Split(' ').Where(s =>
            {
                if (s != string.Empty)
                {
                    return Char.IsUpper(s, 0);
                }

                return false;
            });

            return upper;
        }
    }
}