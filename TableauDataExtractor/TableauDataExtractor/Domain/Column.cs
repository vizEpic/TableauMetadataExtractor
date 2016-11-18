using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableauDataExtractor.Blob
{
    class Column
    {
        private string _caption;
        public string Caption
        {
            get { return _caption; }
            set { _caption = value.Replace("[", "").Replace("]",""); }
        }
        public string Name { get; set; }
        public string Formula { get; set; }
        public bool Hidden { get; set; }
    }
}
