using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableauDataExtractor.Domain
{
    class ColumnMap
    {
        public string Key { get; set; }
        public string Value { get; set; }

        private string _table;
        public string Table
        {
            get { return _table; }
            set { _table = value.Replace("[", "").Replace("]", ""); }
        }

    }
}
