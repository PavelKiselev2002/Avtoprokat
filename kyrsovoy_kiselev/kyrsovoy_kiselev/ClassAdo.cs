using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;

namespace kyrsovoy_kiselev
{
    class ClassAdo
    {
        private string connectionString;
        public SqlCommand cmd;
        public ClassAdo()
        {
            connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=avtoservis;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False;Command Timeout=30";
        }
        public DataSet GetDataSet(string sqlQuery)
        {
            SqlDataAdapter adapter = new SqlDataAdapter(sqlQuery, connectionString);
            DataSet ds = new DataSet();
            adapter.Fill(ds, "Table");
            return ds;
        }
        public void DataGridBind(string sqlQuery, DataGridView dataGridView)
        {
            DataSet ds = GetDataSet(sqlQuery);
            dataGridView.DataSource = ds.Tables[0].DefaultView;
        }
        public void ComboBoxBind(string sqlQuery, ComboBox comboBox, string displayMember, string valueMember)
        {
            DataSet ds = GetDataSet(sqlQuery);
            comboBox.DataSource = ds.Tables[0];
            comboBox.DisplayMember = displayMember;
            comboBox.ValueMember = valueMember;
            comboBox.SelectedValue = ds.Tables[0].Rows[0][valueMember].ToString();
        }
        public SqlCommand StProcExec(string commandText)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            cmd = connection.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = commandText;
            return cmd;
        }
        public void DataGridBind(string CS, string sql, DataGridView dgv)
        {
            SqlDataAdapter adapter = new SqlDataAdapter(sql, CS);

            DataSet ds = new DataSet();
            adapter.Fill(ds, "Table");
            dgv.DataSource = ds.Tables[0].DefaultView;
        }
    }
}
