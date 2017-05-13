using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Rdf_Converter
{
    public class RdfConverter
    {
        public Dictionary<string, string> RdfReader(string filePath)
        {
            XDocument doc = XDocument.Load(filePath);

            XNamespace pgterms = "http://www.gutenberg.org/2009/pgterms/";
            XNamespace dcterms = "http://purl.org/dc/terms/";
            XNamespace rdf = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";
            var values = (from level in doc.Descendants(pgterms + "ebook")
                          select new
                          {
                              Title = (string)level.Element(dcterms + "title"),
                              Author = (string)level.Elements(dcterms + "creator")
                                                       .Elements(pgterms + "agent")
                                                       .Elements(pgterms + "name")
                                                       .FirstOrDefault(),
                              Language = (string)level.Elements(dcterms + "language")
                                                       .Elements(rdf + "Description")
                                                       .Elements(rdf + "value")
                                                       .FirstOrDefault(),
                              Type = (string)level.Elements(dcterms + "type")
                                                       .Elements(rdf + "Description")
                                                       .Elements(rdf + "value")
                                                       .FirstOrDefault()
                          }).ToList();
            if (values[0].Type != "Text" || values[0].Language != "en")
            {
                return null;
            }
            else
            {
                // get folder name
                string lastFolderName = Path.GetFileName(Path.GetDirectoryName(filePath));
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("ID", lastFolderName);
                if (values[0].Title == null)
                {
                    data.Add("Title", "Unknown Title");
                }
                else
                {
                    data.Add("Title", values[0].Title);
                }
                if (values[0].Author == null)
                {
                    data.Add("Author", "Unknown Author");
                }
                else
                {
                    data.Add("Author", values[0].Author);
                }
                data.Add("Language", values[0].Language);

                return data;
            }
        }

        public void WriteToCSVFile(string csvPath, string[] data)
        {
            //add data to csv file
            StringBuilder csvContent = new StringBuilder();
            csvContent.AppendLine(String.Join(",", data));
            File.AppendAllText(csvPath, csvContent.ToString());
            csvContent.Clear();
        }

        //public string GetAuthor()
        //{
        //    XDocument doc = XDocument.Load(filePath);

        //    XNamespace pgterms = "http://www.gutenberg.org/2009/pgterms/";
        //    XNamespace dcterms = "http://purl.org/dc/terms/";
        //    XNamespace rdf = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";
        //    var values = (from level in doc.Descendants(pgterms + "ebook")
        //                  select new
        //                  {
        //                      Title = (string)level.Element(dcterms + "title"),
        //                      Author = (string)level.Elements(dcterms + "creator")
        //                                               .Elements(pgterms + "agent")
        //                                               .Elements(pgterms + "name")
        //                                               .FirstOrDefault(),
        //                      Language = (string)level.Elements(dcterms + "language")
        //                                               .Elements(rdf + "Description")
        //                                               .Elements(rdf + "value")
        //                                               .FirstOrDefault(),
        //                      Type = (string)level.Elements(dcterms + "type")
        //                                               .Elements(rdf + "Description")
        //                                               .Elements(rdf + "value")
        //                                               .FirstOrDefault()
        //                  }).ToList();
        //    if (values[0].Type != "Text" || values[0].Language != "en")
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        if (values[0].Author == null)
        //        {
        //            data.Add("Author", "Unknown Author");
        //        }
        //        else
        //        {
        //            data.Add("Author", values[0].Author);
        //        }
        //    }
        //}
    }
}