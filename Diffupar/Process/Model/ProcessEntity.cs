using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Process.Model
{
    internal class ProcessEntity
    {
        public int QueryId { get; set; }

        public string ProviderName { get; set; }

        public string TableDestinoName { get; set; }

        public string TableOrigenName { get; set; }

        public string Where { get; set; }

        public string queryExceute { get; set; }

        public string DbSourceConnection { get; set; }

        public string DbDestinoConnection { get; set; }

        public IList<SqlBulkCopyColumnMapping> ColumnMapping { get; set; }

        public DataTable TdataImport { get; set; }
    }

}
