using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableauDataExtractor.Blob;

namespace TableauDataExtractor.Domain
{
    class DatasourceDependancy
    {
        public string DatasourceName { get; set; }
        public string ColumnName { get; set; }

        public override bool Equals(object obj)
        {
            DatasourceDependancy d = obj as DatasourceDependancy;
            return d != null && d.DatasourceName == this.DatasourceName && d.ColumnName == this.ColumnName;
        }

        public override int GetHashCode()
        {
            return this.DatasourceName.GetHashCode() ^ this.ColumnName.GetHashCode();
        }

    }
}
