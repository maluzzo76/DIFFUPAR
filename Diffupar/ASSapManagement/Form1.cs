using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASSapManagement
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        void SapManager(string dns, string usuario, string pass, string query)
        {
            try
            {                
                //string _query = "select SCHEMA_NAME, TABLE_NAME from SYS.M_TABLES where SCHEMA_NAME ='DIFFUPARSA' and TABLE_NAME='@RBI_MARCA' ;";             
                string _sapC = string.Format("Dsn={0}; uid = {1}; pwd = {2};", dns,usuario,pass);

                DataTable _dt = ADO.SAPHana.getByQuery(query, _sapC).Tables[0];

                dgResult.DataSource = _dt;
                
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
            }
        }

        private void btnEjecutar_Click(object sender, EventArgs e)
        {
            SapManager(txtDns.Text,txtUsuario.Text,txtPassword.Text, txtQuery.Text);
        }
    }
}
