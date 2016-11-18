using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableauDataExtractor.Blob
{
    class MetadataRecord
    {
        public string LocalName { get; set; }
        public string RemoteName { get; set; }
        public string ParentName { get; set; }
        public string Family { get; set; }
        public string ApproxCount { get; set; }
        public string StatisticMin { get; set; }
        public string StatisticMax { get; set; }
    }
}
