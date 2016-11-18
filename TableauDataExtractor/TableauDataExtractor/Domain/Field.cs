using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableauDataExtractor.Blob
{
    class Field
    {
        private string _datasourcecaption;
        private string _datasourceName;
        private string _repositoryurl;
        private string _caption;
        private string _formula;
        private string _name;
        private string _remotecolumn;
        private string _parentname;
        private string _approxcount;
        private string _minvalue;
        private string _maxvalue;
        private string _family;
        public string DatasourceCaption
        {
            get { return _datasourcecaption; }
            set { _datasourcecaption = value.Replace("'", "\""); }
        }
        public string DatasourceName
        {
            get { return _datasourceName; }
            set { _datasourceName = value.Replace("'", "\""); }
        }
        public string RepositoryUrl
        {
            get { return _repositoryurl; }
            set { _repositoryurl = value.Replace("'", "\""); }
        }
        public string Caption
        {
            get { return _caption; }
            set { _caption = value.Replace("'", "\"").Replace("[", "").Replace("]", ""); }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value.Replace("'", "\""); }
        }
        public string RemoteColumn
        {
            get { return _remotecolumn; }
            set { _remotecolumn = value.Replace("'", "\""); }
        }
        public string ParentName
        {
            get { return _parentname; }
            set { _parentname = value.Replace("'", "\""); }
        }

        public string Formula
        {
            get { return _formula; }
            set { _formula = value.Replace("'",""); }
        }

        public string ApproxCount
        {
            get { return _approxcount; }
            set { _approxcount = value == null ? "" : value.Replace("'", "\""); }
        }
        public string MinValue
        {
            get { return _minvalue; }
            set { _minvalue = value == null ? "" : value.Replace("'", "\"").Replace("\"", "").Replace("#", ""); }
        }
        public string MaxValue
        {
            get { return _maxvalue; }
            set { _maxvalue = value == null ? "" : value.Replace("'", "\"").Replace("\"", "").Replace("#", ""); }
        }

        public string Family
        {
            get { return _family; }
            set { _family = value == null ? "" : value.Replace("'", "\""); }
        }

        public bool FieldNotUsed { get; set; }
        public bool Hidden { get; set; }

        public bool DatasourceFilter { get; set; }
    }
}
