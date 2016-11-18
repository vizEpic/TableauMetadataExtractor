using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TableauDataExtractor.Domain;

namespace TableauDataExtractor.Blob
{
    class Datasource
    {
        public string RepositoryUrl { get; set; }
        public string Name { get; set; }
        public string Caption { get; set; }
        public XElement XML { get; set; }
        public List<Relation> Relations { get; set; }
    }
}
