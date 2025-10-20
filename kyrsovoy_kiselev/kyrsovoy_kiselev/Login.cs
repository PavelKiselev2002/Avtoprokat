using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kyrsovoy_kiselev
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
        String str = "";
        private void button1_Click(object sender, EventArgs e)
        {
            ClassAdo classAdo = new ClassAdo();
            String sqlUser = "Select [User].UserID,[User].UserName,[Role].RoleID,[User].UserLogin,[User].UserPassword,[Role].RoleName" +
                " FROM [User],[Role] WHERE [User].RoleID=[Role].RoleID AND UserLogin =" + "'" + txtLogin.Text + "'" + "AND UserPassword=" + "'" + txtPassword.Text + "'";
            //MessageBox.Show(sqlUler);
            DataSet ds = classAdo.GetDataSet(sqlUser);

            if (ds.Tables[0].Rows.Count == 0)
            {
                MessageBox.Show("Пароль не верен! Обратитесь к администратору по  телефону +7-928-222-33-44");             
            }
            else
            {
                string userName = ds.Tables[0].Rows[0][1].ToString() + " " + ds.Tables[0].Rows[0][3].ToString() + " " + ds.Tables[0].Rows[0][4].ToString();
                string role = ds.Tables[0].Rows[0][2].ToString();
                string mes = ds.Tables[0].Rows[0][1].ToString();
                MessageBox.Show("Добро пожаловать в систему " + mes);
           
                if (role == "2")
                {
                    Report report = new Report();
                    report.ShowDialog();
                }
                
                Rental rental = new Rental();
                rental.ShowDialog();
                //rental.lblUser.Text = userName; 
            }
        }

        private void txtLogin_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

