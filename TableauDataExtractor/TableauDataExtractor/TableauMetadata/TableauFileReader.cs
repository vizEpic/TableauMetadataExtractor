using System.IO.Compression;
using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using TableauDataExtractor.Blob;

namespace TableauDataExtractor.TableauMetadata
{
    class TableauFileReader
    {
        private static string[] EXT = { ".twb", ".tds" };
      
        public Stream GetStream(TableauBlob blob)
        {
            using (Stream stream = new MemoryStream(blob.Data))
            {
                ZipArchive zip = new ZipArchive(stream);
                ZipArchiveEntry entry = zip.Entries.Where(i => Path.GetDirectoryName(i.FullName) == "" && EXT.Contains(Path.GetExtension(i.Name))).SingleOrDefault();
                zip=null;
                return entry.Open();
            };
        }
        public static XDocument GetXML(TableauBlob blob)
        {
            Stream stream = new MemoryStream(blob.Data);
            XDocument xml = new XDocument();
            Stream st;
            try
            {
                ZipArchive zip = new ZipArchive(stream);
                ZipArchiveEntry entry = zip.Entries.Where(i => Path.GetDirectoryName(i.FullName) == "" && EXT.Contains(Path.GetExtension(i.Name))).SingleOrDefault();
                st = entry.Open();
                XmlTextReader reader = new XmlTextReader(st);
                xml = XDocument.Load(reader);
              
            }
            catch (InvalidDataException e)
            {
                if (e.Message == "Number of entries expected in End Of Central Directory does not correspond to number of entries in Central Directory."
                    || e.Message=="End of Central Directory record could not be found.")
                {
                    XmlTextReader reader = new XmlTextReader(stream);
                    try
                    {
                        xml =  XDocument.Load(reader);
                        stream.Dispose();
                    }
                    catch ( Exception ){

                        stream.Dispose();

                    }
                }
                }
              return xml;
            }
        
        }
}
    
