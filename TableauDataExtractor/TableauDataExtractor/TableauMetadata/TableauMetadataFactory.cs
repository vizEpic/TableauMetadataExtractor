using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TableauDataExtractor.Blob;
using TableauDataExtractor.DAL;

namespace TableauDataExtractor.TableauMetadata
{
    class TableauMetadataFactory
    {
        private IEnumerable<TableauBlob> _blobs;
        private ICollection<TableauXml> _xmls;
        public ICollection<TableauXml> Xmls
        {
            get
            {
                return this._xmls;
            }
        }

        public TableauMetadataFactory(GenericRepository<TableauBlob> blobsRepo)
        {
            _blobs =blobsRepo.All();
            _xmls = new List<TableauXml>();

            double count = blobsRepo.Count();
            double i = 1;
            foreach (var b in _blobs)
            {
                _xmls.Add(new TableauXml() { Id = b.Id, RepositoryId = b.RepositoryId, SiteId = b.SiteId, Xml = TableauFileReader.GetXML(b),SourceType=b.SourceType });
                var per_complete =Math.Round((i / count) * 100,0);
                Console.Write("\r{0}% Parsing is Completed", per_complete);
                i++;
            }

            Console.WriteLine("");

        }

    }
}
