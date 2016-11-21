using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TableauDataExtractor.Domain;
using TableauDataExtractor.TableauMetadata;

namespace TableauDataExtractor.Blob
{
    class TableauXml
    {
        public int RepositoryId { get; set; }
        public XDocument Xml { get; set; }
        public int Id { get; set; }
        public int SiteId { get; set; }
        public string SourceType { get; set; }

        protected ICollection<Column> Columns(Datasource datasource)
        {

            ICollection<Column> columns = new List<Column>();

            var columnsXml = datasource.XML.Elements("column");

            foreach (var col in columnsXml)
            {

                columns.Add(new Column
                {
                    Name = col.Attribute("name") == null ? "" : col.Attribute("name").Value,
                    Caption = col.Attribute("caption") == null ? col.Attribute("name") == null ? "" : col.Attribute("name").Value : col.Attribute("caption").Value,
                    Formula = col.Element("calculation") == null ? "" : col.Element("calculation").Attribute("formula") == null ? "" : col.Element("calculation").Attribute("formula").Value

                });
            }

            foreach (var c in columns.Where(i => i.Name.StartsWith("[Calculation_")))
            {
                foreach (var f in columns.Where(x => x.Formula != null))
                {
                    f.Formula = f.Formula.Replace(c.Name, c.Caption);
                }
            }
         

            var hiddenColumnsXml = datasource.XML.Elements("column").Where(e => e.Attribute("hidden") != null && e.Attribute("hidden").Value == "true");

            var hiddenColumns = new HashSet<String>();

            foreach (var col in hiddenColumnsXml)
            {
                hiddenColumns.Add(col.Attribute("name") == null ? "" : col.Attribute("name").Value);
            }

            for (int i = 0; i < columns.Count(); i++)
            {
                var column = columns.ElementAt(i);
                if (hiddenColumns.Contains(column.Name))
                {
                    column.Hidden = true;
                }
            }

            return columns;
        }

        protected ICollection<ColumnMap> ColumnMaps(Datasource datasource)
        {

            ICollection<ColumnMap> columnMaps = new List<ColumnMap>();

            if (datasource.XML.Element("connection").Element("cols") == null)
                return columnMaps;

            var maps = datasource.XML.Element("connection").Element("cols").Elements("map");

            var relations = datasource.XML.Element("connection").Descendants("relation").Where(x => x.Attribute("type").Value == "table");
            if (relations != null) { 
                foreach (var col in maps)
                {
                    var key = col.Attribute("key") == null ? "" : col.Attribute("key").Value;
                    var value = col.Attribute("value") == null ? "" : col.Attribute("value").Value;
                    var searchTable = value.Split('.')[0].Replace("[", "").Replace("]", "");
                    var table= relations.Where(x => x.Attribute("name").Value == searchTable).FirstOrDefault();

                    var columnMap = new ColumnMap
                    {
                        Key = key,
                        Value = value
                    };

                    if (table != null)
                        columnMap.Table = table.Attribute("table").Value;

                    columnMaps.Add(columnMap);
                
                }
            }

            return columnMaps;
        }

        protected ICollection<DatasourceFilter> DatasourceFilters(Datasource datasource)
        {

            ICollection<DatasourceFilter> filters = new List<DatasourceFilter>();


            var filtersXml = datasource.XML.Elements("filter");


            foreach (var col in filtersXml)
            {

                filters.Add(new DatasourceFilter
                {
                    Column = col.Attribute("column") == null ? "" : col.Attribute("column").Value

                });
            }

            return filters;
        }

        protected ICollection<MetadataRecord> MetadataRecords(Datasource datasource)
        {
            ICollection<MetadataRecord> metadataRecords = new List<MetadataRecord>();

            if (datasource.XML.Element("connection").Element("metadata-records") != null)
            {
                IEnumerable<XElement> metadataXMls;

                if (datasource.XML.Elements("extract").Where(e=>e.Attribute("enabled").Value=="true").SingleOrDefault()!=null)
                {
                    metadataXMls = datasource.XML.Element("extract").Element("connection").Element("metadata-records").Elements().Where(e => e.Attribute("class").Value != "capability");
                }
                else
                {
                    metadataXMls = datasource.XML.Element("connection").Element("metadata-records").Elements().Where(e => e.Attribute("class").Value != "capability");
                }


                foreach (var record in metadataXMls)
                {
                    MetadataRecord mr = new MetadataRecord();

                    mr.LocalName = record.Element("local-name") == null ? "" : record.Element("local-name").Value;
                    mr.RemoteName = record.Element("remote-name") == null ? "" : record.Element("remote-name").Value;
                    mr.ParentName = record.Element("parent-name") == null ? "" : record.Element("parent-name").Value;
                    mr.Family = record.Element("family") == null ? "" : record.Element("family").Value;
                    if (datasource.Relations != null)
                    {
                        var nameWithSchema = datasource.Relations.Where(x => x.Name == mr.Family).SingleOrDefault();
                        if (nameWithSchema != null)
                        {
                            mr.Family = nameWithSchema.Table;
                        }
                    }
                    

                    mr.ApproxCount = record.Element("approx-count") == null ? "" : record.Element("approx-count").Value;

                    if (record.Element("statistics") != null)
                    {
                        var min = record.Element("statistics").Elements().SingleOrDefault(e => e.Attribute("aggregation").Value == "Min");
                        var max = record.Element("statistics").Elements().SingleOrDefault(e => e.Attribute("aggregation").Value == "Max") ;
                        mr.StatisticMin = min == null ? "" : min.Value;
                        mr.StatisticMax = max == null ? "" : max.Value;
                    }

                    metadataRecords.Add(mr);
                }
            }
            return metadataRecords;
        }

        protected IEnumerable<XElement> DatasourcesXML()
        {
            if (this.SourceType == "workbook")
            {
                var datasources = Xml.Root.Elements().Where(el => el.Name == "datasources").Elements();
                var excludeDatasource = datasources.Where(d => d.Attribute("hasconnection") != null && d.Attribute("hasconnection").Value == "false");
                return datasources.Except(excludeDatasource);
            } else {

                return Xml.Elements("datasource");
            }
            
        }

        public virtual ICollection<Datasource> Datasources
        {
            get
            {
                ICollection<Datasource> ds = new List<Datasource>();

                var datasources = DatasourcesXML();

                foreach (var datasource in datasources)
                {

                    Datasource dt = new Datasource();
                    dt.RepositoryUrl = datasource.Element("repository-location") == null ? "" : datasource.Element("repository-location").Attribute("id").Value;

                    if (datasource.Attribute("formatted-name") == null)
                    {
                        dt.Name = datasource.Attribute("name").Value;
                        dt.Caption = datasource.Attribute("caption") == null ? datasource.Attribute("name").Value : datasource.Attribute("caption").Value;
                        dt.Relations = Relations(datasource);
                    }
                    else
                    {
                        dt.Name = datasource.Attribute("formatted-name").Value;
                        dt.Caption = datasource.Attribute("formatted-name").Value;
                        dt.Relations = Relations(datasource);
                    }
                    
                    dt.XML = datasource;
                    ds.Add(dt);
                }

                return ds;
            }

        }

        protected HashSet<DatasourceDependancy> DatasourceDependancies()
        {

             var datasourceDependanciesXml = Xml.Descendants("datasource-dependencies");

             HashSet<DatasourceDependancy> datasourceDependancies = new HashSet<DatasourceDependancy>();

             foreach( var datasourceDependancyXml in datasourceDependanciesXml) {

                var datasourceName = datasourceDependancyXml.Attribute("datasource").Value;

                var columnsXml = datasourceDependancyXml.Elements("column");

                foreach( var columnXml in columnsXml)
                {
                    var columnName = columnXml.Attribute("name").Value;
                  
                    datasourceDependancies.Add(new DatasourceDependancy() { DatasourceName = datasourceName, ColumnName = columnName });

                }



            }

            return datasourceDependancies;


        }

        protected List<Relation> Relations(XElement datasource)
        {
            var list = new HashSet<Relation>();
            
            var relations = datasource.Element("connection").Elements("relation");
            list = RecursiveRelations(relations, list);
            return list.ToList();

        }

        protected HashSet<Relation> RecursiveRelations(IEnumerable<XElement> relations, HashSet<Relation> list)
        {

            if (relations.Count()==0)
                return list;

            foreach (var relation in relations)
            {

                if (relation.Attribute("table") != null)
                {
                    var name = relation.Attribute("name").Value;
                    var table = relation.Attribute("table").Value;
                    list.Add(new Relation() { Name = name, Table = table });
                 
                }

                

            }
            return RecursiveRelations(relations.Elements("relation"), list);
        }

        public IEnumerable<Field> Fields
        {
            get
            {
                List<Field> fields = new List<Field>();


                var datasources = this.Datasources;

                foreach (var datasource in datasources)
                {
                    ICollection<Column> columns = Columns(datasource);
                    ICollection<MetadataRecord> metadataRecords = MetadataRecords(datasource);
                    ICollection<DatasourceFilter> filters = DatasourceFilters(datasource);
                    ICollection<ColumnMap> columnMaps = ColumnMaps(datasource);

      
                    var matchColumn = from cl in columns
                             join mr in metadataRecords on cl.Name equals mr.LocalName into temp
                             from tp in temp.DefaultIfEmpty()
                             join fl in filters on cl.Name equals fl.Column into temp2
                             from tp2 in temp2.DefaultIfEmpty()
                             join cm in columnMaps on cl.Name equals cm.Key into temp3
                             from tp3 in temp3.DefaultIfEmpty()
                                      select new Field
                             {
                                 DatasourceCaption = datasource.Caption,
                                 DatasourceName = datasource.Name,
                                 RepositoryUrl = datasource.RepositoryUrl,
                                 Caption = cl.Caption,
                                 Name = cl.Name,
                                 RemoteColumn = (tp == null ? String.Empty : tp.RemoteName),
                                 ParentName = (tp == null ? String.Empty : tp.ParentName),
                                 Formula = cl.Formula,
                                 ApproxCount = (tp == null ? String.Empty : tp.ApproxCount),
                                 MinValue=(tp == null ? String.Empty : tp.StatisticMin),
                                 MaxValue=(tp == null ? String.Empty : tp.StatisticMax),
                                 Family=(tp == null ? (tp3!=null) ? tp3.Table: String.Empty : tp.Family),
                                 Hidden=cl.Hidden,
                                 DatasourceFilter= (tp2 == null ? false : true)
                             };

                    var matchMetadata = from mr in metadataRecords
                                join cl in columns on mr.LocalName equals cl.Name into temp
                                from tp in temp.DefaultIfEmpty()
                                join fl in filters on mr.LocalName equals fl.Column into temp2
                                from tp2 in temp2.DefaultIfEmpty()
                                select new Field
                                      {
                                          DatasourceCaption = datasource.Caption,
                                          DatasourceName = datasource.Name,
                                          RepositoryUrl = datasource.RepositoryUrl,
                                          Caption = mr.LocalName,
                                          Name = mr.LocalName,
                                          RemoteColumn = mr.RemoteName,
                                          ParentName = mr.ParentName,
                                          Formula = (tp == null ? String.Empty : tp.Formula),
                                          ApproxCount = mr.ApproxCount,
                                          MinValue = mr.StatisticMin,
                                          MaxValue = mr.StatisticMax,
                                          Family = mr.Family,
                                          Hidden= (tp == null ? false : tp.Hidden),
                                          DatasourceFilter = (tp2 == null ? false : true)

                                };

                    fields = fields.Union(matchColumn).ToList();
                    fields = fields.Union(matchMetadata, new FieldComparer()).ToList();
                }

                var datasourceDependancies = DatasourceDependancies();

                for (int i = 0; i < fields.Count(); i++)
                {
                    var field = fields.ElementAt(i);
                    if (!datasourceDependancies.Contains((new DatasourceDependancy() { DatasourceName = field.DatasourceName, ColumnName = field.Name })))
                    {

                        field.FieldNotUsed = true;
                    }
                }


                return fields.AsEnumerable();

            }
        }
    }
}
