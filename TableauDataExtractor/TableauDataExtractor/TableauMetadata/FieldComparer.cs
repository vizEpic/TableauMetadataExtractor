using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableauDataExtractor.Blob;


namespace TableauDataExtractor.TableauMetadata
{
    class FieldComparer : IEqualityComparer<Field>
    {
        public bool Equals(Field x, Field y)
        {
            return (x.Name == y.Name && x.DatasourceCaption == y.DatasourceCaption);
        }

        // If Equals() returns true for a pair of objects 
        // then GetHashCode() must return the same value for these objects.

        public int GetHashCode(Field myModel)
        {
            return myModel.Name.GetHashCode() ^ myModel.DatasourceCaption.GetHashCode();
        }
    }
}
