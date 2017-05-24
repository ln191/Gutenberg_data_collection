using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rdf_Converter
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            RdfConverter rdf = new RdfConverter();
            string[] filePaths = Directory.GetFiles(@"test\", "*.rdf",
                                         SearchOption.AllDirectories);
            List<Dictionary<string, string>> Books = new List<Dictionary<string, string>>();
            List<Dictionary<string, string>> relations = new List<Dictionary<string, string>>();
            Dictionary<string, string> bookData = new Dictionary<string, string>();
            //Dictionary<string, string> bookAuthorRelations = new Dictionary<string, string>();
            List<string> authors = new List<string>();
            int i = 0;

            Console.WriteLine("input csv books file path:");
            string csvBooksPath = Console.ReadLine();
            Console.WriteLine("input csv authors file path:");
            string csvAuthorPath = Console.ReadLine();
            Console.WriteLine("input csv book author relation file path:");
            string csvBookAuthorRelationPath = Console.ReadLine();

            StringBuilder csvBookBuilder = new StringBuilder();
            csvBookBuilder.AppendLine("id,title,language");
            File.AppendAllText(csvBooksPath, csvBookBuilder.ToString());
            csvBookBuilder.Clear();

            StringBuilder csvBookAuthorRelationBuilder = new StringBuilder();
            csvBookAuthorRelationBuilder.AppendLine("book_id,author");
            File.AppendAllText(csvBookAuthorRelationPath, csvBookAuthorRelationBuilder.ToString());
            csvBookAuthorRelationBuilder.Clear();

            StringBuilder csvAuthorBuilder = new StringBuilder();
            csvAuthorBuilder.AppendLine("author");
            File.AppendAllText(csvAuthorPath, csvAuthorBuilder.ToString());
            csvAuthorBuilder.Clear();

            foreach (string path in filePaths)
            {
                bookData = rdf.RdfReader(path);
                if (bookData != null)
                {
                    //remove newline from title string
                    bookData["Title"] = bookData["Title"].Replace("\r\n", "  "); //windows
                    bookData["Title"] = bookData["Title"].Replace("\n", "  "); //linux
                    bookData["Title"] = bookData["Title"].Replace("\r", "  "); //mac
                    bookData["Title"] = bookData["Title"].Replace("\"", "  ");
                    bookData["Title"] = bookData["Title"].Replace("\'", "  ");
                    bookData["Author"] = bookData["Author"].Replace("\"", "  ");
                    bookData["Author"] = "\"" + bookData["Author"] + "\"";
                    //if title has comma in the string, the string will be surrounded by double quotes
                    if (bookData["Title"].Contains(","))
                    {
                        bookData["Title"] = "\"" + bookData["Title"].Replace("\r\n", "  ") + "\"";
                    }

                    if (!authors.Contains(bookData["Author"]))
                    {
                        authors.Add(bookData["Author"]);
                    }

                    //add Book id and corresponding author to a relation dictionary
                    Dictionary<string, string> bookAuthorRelations = new Dictionary<string, string>();
                    bookAuthorRelations.Add("BookID", bookData["ID"]);
                    bookAuthorRelations.Add("Author", bookData["Author"]);
                    relations.Add(bookAuthorRelations);

                    //remove author from book data since we do not want it in our books csv
                    bookData.Remove("Author");
                    //add it to a list of books
                    Books.Add(bookData);
                }

                i++;
                if (i == 5000)
                {
                    //add books to books csv
                    foreach (var book in Books)
                    {
                        string[] rowData = book.Values.ToArray();
                        csvBookBuilder.AppendLine(String.Join(",", rowData));
                    }
                    File.AppendAllText(csvBooksPath, csvBookBuilder.ToString());
                    csvBookBuilder.Clear();
                    Books.Clear();

                    //add book author relations to csv file
                    foreach (var relation in relations)
                    {
                        string[] relationData = relation.Values.ToArray();
                        csvBookAuthorRelationBuilder.AppendLine(String.Join(",", relationData));
                    }
                    File.AppendAllText(csvBookAuthorRelationPath, csvBookAuthorRelationBuilder.ToString());
                    csvBookAuthorRelationBuilder.Clear();
                    relations.Clear();

                    i = 0;
                    Console.WriteLine("added 5000 to csv");
                }
                //Console.WriteLine(path);
            }
            //add books to books csv
            foreach (var author in authors)
            {
                csvAuthorBuilder.AppendLine(author);
            }
            File.AppendAllText(csvAuthorPath, csvAuthorBuilder.ToString());
            csvAuthorBuilder.Clear();
            Console.WriteLine("csv file has been created, is Done!!!");

            //add book author relations to csv file
            foreach (var relation in relations)
            {
                string[] relationData = relation.Values.ToArray();
                csvBookAuthorRelationBuilder.AppendLine(String.Join(",", relationData));
            }
            File.AppendAllText(csvBookAuthorRelationPath, csvBookAuthorRelationBuilder.ToString());
            csvBookAuthorRelationBuilder.Clear();
            relations.Clear();

            //add books to books csv
            foreach (var book in Books)
            {
                string[] rowData = book.Values.ToArray();
                csvBookBuilder.AppendLine(String.Join(",", rowData));
            }
            File.AppendAllText(csvBooksPath, csvBookBuilder.ToString());
            csvBookBuilder.Clear();
            Books.Clear();

            Console.Read();
        }
    }
}