using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableauDataExtractor.Blob
{
    class TableauBlob
    {
        public string SourceType { get; set; }
        public int Id { get; set; }
        public int SiteId { get; set; }
        public int RepositoryId { get; set; }
        public Byte[] Data { get; set; }
    }
}
