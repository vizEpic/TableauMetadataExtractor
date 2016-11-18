using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableauDataExtractor.DAL;
using TableauDataExtractor.Blob;

namespace TableauDataExtractor.TableauMetadata
{
    class Repository : IDisposable
    {
        private string STG_SCHEMA = ConfigurationManager.AppSettings["stg_schema"];
        private string MAIN_SCHEMA = ConfigurationManager.AppSettings["main_schema"];
        private  string CONNECTION = ConfigurationManager.ConnectionStrings["TableauContext"].ConnectionString;

        NpgsqlConnection dbConnection;

        public Repository()
        {
            dbConnection = new NpgsqlConnection(CONNECTION);
            dbConnection.Open();
        }

        public void LoadXrefWorkbookDatasource(ICollection<TableauXml> Xmls)
        {
            NpgsqlCommand command = new NpgsqlCommand("TRUNCATE TABLE " + STG_SCHEMA + ".xref_workbook_datasource", dbConnection);
            command.ExecuteNonQuery();

            foreach (var w in Xmls.Where(x=>x.SourceType=="workbook"))
            {
                foreach (var d in w.Datasources)
                {
                    string SQL = String.Format(@"INSERT INTO " + STG_SCHEMA + ".xref_workbook_datasource" + @"
                    (workbook_id,site_id,datasource_repository_url)
                    VALUES ({0},{1},'{2}')", w.Id, w.SiteId, d.RepositoryUrl);

                    command = new NpgsqlCommand(SQL, dbConnection);
                    command.ExecuteNonQuery();
                }

            }
        }

        public void LoadFields(ICollection<TableauXml> Xmls)
        {

            NpgsqlCommand command = new NpgsqlCommand("TRUNCATE TABLE " + STG_SCHEMA + ".fields", dbConnection);
            command.ExecuteNonQuery();
            StringBuilder sb = new StringBuilder();
            sb.Append(@"INSERT INTO " + STG_SCHEMA + ".fields" + @"
                    (source_id, site_id, datasource_repository_url, datasource_caption, datasource_name, field_caption, field_name, remote_column, parent_name, family_name, formula, approximate_count, min_value, max_value, source_type,field_not_used,hidden,datasource_filter) VALUES ");


            foreach (var w in Xmls)
            {
                foreach (var d in w.Fields)
                {
                    sb.Append(String.Format(@"({0},{1},'{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}',{15},{16},{17}),", w.Id, w.SiteId, d.RepositoryUrl, d.DatasourceCaption, d.DatasourceName, d.Caption, d.Name, d.RemoteColumn, d.ParentName, d.Family, d.Formula, d.ApproxCount, d.MinValue, d.MaxValue, w.SourceType,d.FieldNotUsed,d.Hidden,d.DatasourceFilter));
                }

            }
            string SQL = sb.ToString().TrimEnd(',');
            command = new NpgsqlCommand(SQL, dbConnection);
            command.ExecuteNonQuery();
        }

        public GenericRepository<TableauBlob> Blobs()
        {
            return new GenericRepository<TableauBlob>();
        }

        public void LoadStgTable()
        {
            NpgsqlCommand command = new NpgsqlCommand("SELECT "+ MAIN_SCHEMA+".load_stg_tables()", dbConnection);
            command.ExecuteNonQuery();
        }

        public void SwapStgTable()
        {
            NpgsqlCommand command = new NpgsqlCommand("SELECT "+ MAIN_SCHEMA+".swap_stg_tables()", dbConnection);
            command.ExecuteNonQuery();
        }

        public void Dispose()
        {
            dbConnection.Dispose();
        }
    }
}
