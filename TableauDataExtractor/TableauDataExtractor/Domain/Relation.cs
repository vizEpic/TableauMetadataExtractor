using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableauDataExtractor.Domain
{
    public class Relation
    {
        public string Name { get; set; }
        private string _table;
        public string Table
        {
            get { return _table; }
            set { _table = value.Replace("[", "").Replace("]", ""); }
        }
    }
}
