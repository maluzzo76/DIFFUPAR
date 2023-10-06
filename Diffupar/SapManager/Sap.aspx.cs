using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;

namespace SapManager
{
    public partial class Sap : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            try
            {
                lberror.Text = string.Empty;
                string _sapConn = ConfigurationManager.ConnectionStrings["SAP"].ConnectionString;
                string _sapSch = ConfigurationManager.AppSettings["SapSchemma"].ToString();

                string _query = string.Format("select * from {0}.{1};", _sapSch, txtTableName.Text);
                //string _mq = "select SCHEMA_NAME, TABLE_NAME, * from SYS.M_TABLES where SCHEMA_NAME = 'DIFFUPARSA' and TABLE_NAME = '@RBI_MARCA';";
                string _mq = txtTableName.Text;


                DataSet _ds = ADO.SAPHana.getByQuery(_mq, _sapConn);

                    grdData.DataSource = _ds.Tables[0];
                    grdData.DataBind();

            }
            catch (Exception ex) {  lberror.Text = ex.Message; }
        }
    }
}