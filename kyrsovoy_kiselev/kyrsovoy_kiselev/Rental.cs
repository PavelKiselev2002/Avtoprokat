using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace kyrsovoy_kiselev
{
    public partial class Rental : Form
    {
        public Rental()
        {
            InitializeComponent();
        }
        string ConnectionString = "Data Source=STUDENT-402-006;Initial Catalog=Avtoprokat;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        String Dogovor =
           "Select Dogovor_ID AS [№ Договора], Klient.FIO AS [Клиент],Avto.Avto_ID AS [№ Авто], Avto.Marka AS[Марка Авто],Avto.Model AS [Модель авто],Avto.Gos_number AS[Гос.номер]," +
            "Data_arendy AS[Дата аренды], Data_plan AS[Возврат плановый],Data_fakt AS[Возврат фактический], Suma_oplaty AS[Сумма оплаты],Zalog_vzyt AS[Принят залог], Zalog_vozvrat AS[Возвращен залог],Avto.Probeg_fakt AS Пробег " +
            "From Dogovor,Klient,Avto Where Dogovor.Klient_ID=Klient.Klient_ID AND Dogovor.Avto_ID=Avto.Avto_ID"; 
        String Client = "Select Klient_ID AS [№], FIO AS ФИО, Pasport AS Паспорт, Prava AS [Водительское удостоверение], Age AS Возраст, Experience AS [Стаж вождения], Telephone AS Телефон from Klient";
        String Class = "Select Class_ID AS [№],Class_name AS Наиминование, Zalog AS Залог FROM Class";
        String Avto = "Select Avto_ID  AS [№],Marka AS Марка,Model AS Модель,Color AS Цвет,Gos_number AS [Гос.номер],Date_release AS [Год выпуска],DTP AS ДТП,Class_name AS Класс,Class.Zalog AS Залог," +
            "Places AS[Кол - во мест], Doors AS[Кол - во дверей],Conditioner AS Кондиционер,Gearbox AS[Коробка передач], Price_new AS[Цена нового],Probeg_norm AS[Пробег норма]," +
            "Probeg_fakt AS[Пробег фактический],Price_iznos AS[Стоимость износа], Price_sytki AS[Цена за сутки],Status AS Статус FROM Avto,Class Where Avto.Class_ID=Class.Class_ID And Status='Свободен'";

        ClassAdo classAdo = new ClassAdo();
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConnectionString;
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "AddKlient";

            command.Parameters.Add("@FIO", SqlDbType.NVarChar, 70);
            command.Parameters.Add("@Pasport", SqlDbType.NVarChar, 120);
            command.Parameters.Add("@Prava", SqlDbType.NVarChar,50 );
            command.Parameters.Add("@Age", SqlDbType.Int);
            command.Parameters.Add("@Experience", SqlDbType.Int);
            command.Parameters.Add("@Telephone", SqlDbType.Int);

            command.Parameters["@FIO"].Value = txtFIO.Text;
            command.Parameters["@Pasport"].Value = txtPasport.Text;
            command.Parameters["@Prava"].Value = txtPrava.Text;
            command.Parameters["@Age"].Value = nAge.Value;
            command.Parameters["@Experience"].Value = nExperience.Value;
            command.Parameters["@Telephone"].Value = txtPhone.Text;

            command.ExecuteNonQuery();
            MessageBox.Show($"Новый клиент добавлен в базу данных! ");
            classAdo.DataGridBind(Client, dgvClient);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Rental_Load(object sender, EventArgs e)
        {
            classAdo.DataGridBind(Class, dgvClass);
            classAdo.DataGridBind(Client, dgvClient);
            classAdo.DataGridBind(Avto, dgvAvto);
            classAdo.DataGridBind(Dogovor, dgvDogovor);
           // Avto += " And Status='Свободен'";
            classAdo.DataGridBind(Avto, dgvAddDogovorAvto);
            classAdo.DataGridBind(Client, dgvAddDogovorClient);

            classAdo.ComboBoxBind(Class, cbClass_ID, "Наиминование", "№");
            classAdo.ComboBoxBind(Class, cbSearchClass, "Наиминование", "№");
            //classAdo.ComboBoxBind(Avto, cbSearchConditioner, "Кондиционер", "№");
            //classAdo.ComboBoxBind(Avto, cbSearchGB, "Коробка передач", "№");
            classAdo.ComboBoxBind(Avto, cbSearchColor, "Цвет", "№");
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void txtAge_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConnectionString;
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "AddDogovor";

            int nRow = dgvAddDogovorClient.CurrentRow.Index;
            int nRowA = dgvAddDogovorAvto.CurrentRow.Index;

            command.Parameters.Add("@Klient_ID", SqlDbType.Int);
            command.Parameters.Add("@Avto_ID", SqlDbType.Int);
            command.Parameters.Add("@Data_arendy", SqlDbType.Date);
            command.Parameters.Add("@Data_plan", SqlDbType.Date);
            command.Parameters.Add("@Zalog_vzyt", SqlDbType.Decimal);

            command.Parameters["@Klient_ID"].Value = int.Parse(dgvAddDogovorClient[0, nRow].Value.ToString());
            command.Parameters["@Avto_ID"].Value = int.Parse(dgvAddDogovorAvto[0, nRowA].Value.ToString());
            command.Parameters["@Data_arendy"].Value = dDogovotrDataArenda.Value;
            command.Parameters["@Data_plan"].Value = dData_plan.Value;
            command.Parameters["@Zalog_vzyt"].Value = nZ.Value;

            command.ExecuteNonQuery();
            MessageBox.Show($"Добавлен новый договор.");
            
            SqlConnection connection1 = new SqlConnection();
            connection1.ConnectionString = ConnectionString;
            connection1.Open();

            SqlCommand command1 = connection1.CreateCommand();
            command1.CommandType = CommandType.StoredProcedure;
            command1.CommandText = "UpdStatus";

            command1.Parameters.Add("@Avto_ID", SqlDbType.Int);
            command1.Parameters.Add("@Status", SqlDbType.NChar, 25);

            command1.Parameters["@Avto_ID"].Value = int.Parse(dgvAddDogovorAvto[0, nRowA].Value.ToString());
            command1.Parameters["@Status"].Value = "Арендован";

            command1.ExecuteNonQuery();
            classAdo.DataGridBind(Avto, dgvAddDogovorAvto);
            classAdo.DataGridBind(Dogovor, dgvDogovor);

        }

        private void label34_Click(object sender, EventArgs e)
        {

        }

        private void txtClass_ID_TextChanged(object sender, EventArgs e)
        {

        }

        private void nPrice_new_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnAddClass_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConnectionString;
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "AddClass";

            command.Parameters.Add("@Class_name", SqlDbType.NVarChar, 25);
            command.Parameters.Add("@Zalog", SqlDbType.Decimal);

            command.Parameters["@Class_name"].Value = txtClass.Text;
            command.Parameters["@Zalog"].Value = nClassZalog.Text;

            command.ExecuteNonQuery();
            MessageBox.Show($"Добавлен новый класс.");
            classAdo.DataGridBind(Class, dgvClass);
        }

        private void btnDelClass_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConnectionString;
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "ClassDelete";

            int nRow = dgvClass.CurrentRow.Index;
            command.Parameters.Add("@Class_ID", SqlDbType.Int);

            command.Parameters["@Class_ID"].Value = int.Parse(dgvClass[0, nRow].Value.ToString());

            command.ExecuteNonQuery();
            MessageBox.Show($"Класс удален.");
            classAdo.DataGridBind(Class, dgvClass);
        }

        private void btnUpClass_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConnectionString;
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "UpdClass";

            int nRow = dgvClass.CurrentRow.Index;
            command.Parameters.Add("@Class_ID", SqlDbType.Int);
            command.Parameters.Add("@Class_name", SqlDbType.NChar, 25);

            command.Parameters["@Class_ID"].Value = int.Parse(dgvClass[0, nRow].Value.ToString());
            command.Parameters["@Class_name"].Value = txtClass.Text;

            command.ExecuteNonQuery();
            MessageBox.Show($"Данные о классе изменены.");
            classAdo.DataGridBind(Class, dgvClass);
        }

        private void btnAddAvto_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConnectionString;
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "AddAvto";

            command.Parameters.Add("@Marka", SqlDbType.NVarChar, 20);
            command.Parameters.Add("@Model", SqlDbType.NVarChar, 20);
            command.Parameters.Add("@Color", SqlDbType.NChar, 20);
            command.Parameters.Add("@Gos_number", SqlDbType.NVarChar, 20);
            command.Parameters.Add("@Date_release", SqlDbType.Date);
            command.Parameters.Add("@DTP", SqlDbType.NVarChar,150);
            command.Parameters.Add("@Class_ID", SqlDbType.Int);
            command.Parameters.Add("@Places", SqlDbType.Int);
            command.Parameters.Add("@Doors", SqlDbType.Int);
            command.Parameters.Add("@Conditioner", SqlDbType.NVarChar, 10);
            command.Parameters.Add("@Gearbox", SqlDbType.NVarChar, 20);
            command.Parameters.Add("@Price_new", SqlDbType.Decimal);
            command.Parameters.Add("@Probeg_norm", SqlDbType.Int);
            command.Parameters.Add("@Probeg_fakt", SqlDbType.Int);
            command.Parameters.Add("@Price_iznos", SqlDbType.Decimal);
            command.Parameters.Add("@Price_sytki", SqlDbType.Decimal);
            command.Parameters.Add("@Status", SqlDbType.NVarChar, 20);

            command.Parameters["@Marka"].Value = txtMarka.Text;
            command.Parameters["@Model"].Value = txtModel.Text;
            command.Parameters["@Color"].Value = txtColor.Text;
            command.Parameters["@Gos_number"].Value = txtGos_number.Text;
            command.Parameters["@Date_release"].Value = dtDate_release.Value;
            command.Parameters["@DTP"].Value = txtDTP.Text;
            command.Parameters["@Class_ID"].Value = cbClass_ID.SelectedValue;
            command.Parameters["@Places"].Value = nPlaces.Text;
            command.Parameters["@Doors"].Value = nDoors.Text;
            command.Parameters["@Conditioner"].Value = cbConditioner.Text;
            command.Parameters["@Gearbox"].Value = cbGearbox.Text;
            command.Parameters["@Price_new"].Value = nPrice_new.Text;
            command.Parameters["@Probeg_norm"].Value = nProbeg_norm.Text;
            command.Parameters["@Probeg_fakt"].Value = nProbeg_fakt.Text;
            command.Parameters["@Price_iznos"].Value = 0;
            command.Parameters["@Price_sytki"].Value = nPrice_sytki.Text;
            command.Parameters["@Status"].Value = "Свободен";

            command.ExecuteNonQuery();
            MessageBox.Show($"Добавлен новый автомобиль.");
            classAdo.DataGridBind(Avto, dgvAvto);
        }

        private void cbClass_ID_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnDelAvto_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConnectionString;
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "AvtoDelete";

            int nRow = dgvAvto.CurrentRow.Index;
            command.Parameters.Add("@Avto_ID", SqlDbType.Int);

            command.Parameters["@Avto_ID"].Value = int.Parse(dgvAvto[0, nRow].Value.ToString());

            command.ExecuteNonQuery();
            MessageBox.Show($"Автомобиль удален.");
            classAdo.DataGridBind(Avto, dgvAvto);
        }

        private void btnUpAvto_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConnectionString;
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "UpdAvto";

            int nRow = dgvAvto.CurrentRow.Index;

            command.Parameters.Add("@Avto_ID", SqlDbType.Int);
            command.Parameters.Add("@Marka", SqlDbType.NVarChar, 20);
            command.Parameters.Add("@Model", SqlDbType.NVarChar, 20);
            command.Parameters.Add("@Color", SqlDbType.NChar, 20);
            command.Parameters.Add("@Gos_number", SqlDbType.NVarChar, 20);
            command.Parameters.Add("@Date_release", SqlDbType.Date);
            command.Parameters.Add("@DTP", SqlDbType.NVarChar, 150);
            command.Parameters.Add("@Class_ID", SqlDbType.Int);
            command.Parameters.Add("@Places", SqlDbType.Int);
            command.Parameters.Add("@Doors", SqlDbType.Int);
            command.Parameters.Add("@Conditioner", SqlDbType.NVarChar, 10);
            command.Parameters.Add("@Gearbox", SqlDbType.NVarChar, 20);
            command.Parameters.Add("@Price_new", SqlDbType.Decimal);
            command.Parameters.Add("@Probeg_norm", SqlDbType.Int);
            command.Parameters.Add("@Probeg_fakt", SqlDbType.Int);
            command.Parameters.Add("@Price_iznos", SqlDbType.Decimal);
            command.Parameters.Add("@Price_sytki", SqlDbType.Decimal);

            command.Parameters["@Avto_ID"].Value = int.Parse(dgvAvto[0, nRow].Value.ToString());
            command.Parameters["@Marka"].Value = txtMarka.Text;
            command.Parameters["@Model"].Value = txtModel.Text;
            command.Parameters["@Color"].Value = txtColor.Text;
            command.Parameters["@Gos_number"].Value = txtGos_number.Text;
            command.Parameters["@Date_release"].Value = dtDate_release.Value;
            command.Parameters["@DTP"].Value = txtDTP.Text;
            command.Parameters["@Class_ID"].Value = cbClass_ID.SelectedValue;
            command.Parameters["@Places"].Value = nPlaces.Text;
            command.Parameters["@Doors"].Value = nDoors.Text;
            command.Parameters["@Conditioner"].Value = cbConditioner.Text;
            command.Parameters["@Gearbox"].Value = cbGearbox.Text;
            command.Parameters["@Price_new"].Value = nPrice_new.Text;
            command.Parameters["@Probeg_norm"].Value = nProbeg_norm.Text;
            command.Parameters["@Probeg_fakt"].Value = nProbeg_fakt.Text;
            command.Parameters["@Price_iznos"].Value = 0;
                command.Parameters["@Price_sytki"].Value = nPrice_sytki.Text;

            command.ExecuteNonQuery();
            MessageBox.Show($"Данные о автомобиле изменены.");
            classAdo.DataGridBind(Avto, dgvAvto);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConnectionString;
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "UpdClient";

            int nRow = dgvClient.CurrentRow.Index;

            command.Parameters.Add("@Klient_ID", SqlDbType.Int);
            command.Parameters.Add("@FIO", SqlDbType.NVarChar, 70);
            command.Parameters.Add("@Pasport", SqlDbType.NVarChar, 120);
            command.Parameters.Add("@Prava", SqlDbType.NVarChar, 50);
            command.Parameters.Add("@Age", SqlDbType.Int);
            command.Parameters.Add("@Experience", SqlDbType.Int);
            command.Parameters.Add("@Telephone", SqlDbType.Int);

            command.Parameters["@Klient_ID"].Value = int.Parse(dgvClient[0, nRow].Value.ToString());
            command.Parameters["@FIO"].Value = txtFIO.Text;
            command.Parameters["@Pasport"].Value = txtPasport.Text;
            command.Parameters["@Prava"].Value = txtPrava.Text;
            command.Parameters["@Age"].Value = nAge.Value;
            command.Parameters["@Experience"].Value = nExperience.Value;
            command.Parameters["@Telephone"].Value = txtPhone.Text;

            command.ExecuteNonQuery();
            MessageBox.Show($"Данные о клиенте изменены.");
            classAdo.DataGridBind(Client, dgvClient);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string sql = "";
            if (txtSearch.Text != "")
            {
                sql = Client + " WHERE FIO Like " + "N'" + txtSearch.Text + "%" + "'";
            }
            else
            {
                sql = Client;
            }
            classAdo.DataGridBind(sql, dgvClient);
        }

        private void txtSearchNumber_TextChanged(object sender, EventArgs e)
        {
        //    string sql = "";
        //    if (txtSearchNumber.Text != "")
        //    {
        //        sql = Avto + " AND Gos_number Like " + "N'" + txtSearchNumber.Text + "%" + "'";
        //    }
        //    else
        //    {
        //        sql = Avto;
        //    }
        //    classAdo.DataGridBind(sql, dgvDogovor);
        }

        private void txtSearchDogovorClient_TextChanged(object sender, EventArgs e)
        {
            //string sql = "";
            //if (txtSearchDogovorClient.Text != "")
            //{
            //    sql = Dogovor + " And FIO Like " + "N'" + txtSearchDogovorClient.Text + "%" + "'";
            //}
            //else
            //{
            //    sql = Dogovor;
            //}
            //classAdo.DataGridBind(sql, dgvDogovor);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string sql = Dogovor;

            if (checkData.Checked)
            {
                sql += " AND Dogovor.Data_arendy=" +"'" + dDateArenda.Value.ToShortDateString() + "'";
            }
            if (checkNumber.Checked)
            {
                sql += " AND Gos_number Like " + "N'" + txtSearchNumber.Text + "%" + "'";
            }
            if (checkClient.Checked)
            {
                sql += " And FIO Like " + "N'" + txtSearchDogovorClient.Text + "%" + "'";
            }
            //MessageBox.Show(sql);
            classAdo.DataGridBind(sql, dgvDogovor);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConnectionString;
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "AddVozvrat";

            int nRow = dgvDogovor.CurrentRow.Index;
            int nRowA = dgvDogovor.CurrentRow.Index;

            command.Parameters.Add("@Dogovor_ID", SqlDbType.Int);
            command.Parameters.Add("@Data_fakt", SqlDbType.Date);
            command.Parameters.Add("@Suma_oplaty", SqlDbType.Decimal);
            command.Parameters.Add("@Zalog_vozvrat", SqlDbType.Decimal);

            command.Parameters["@Dogovor_ID"].Value = int.Parse(dgvDogovor[0, nRow].Value.ToString());
            command.Parameters["@Data_fakt"].Value = dData_fakt.Value;
            command.Parameters["@Suma_oplaty"].Value = nSuma_oplaty.Value;
            command.Parameters["@Zalog_vozvrat"].Value = nZalog_vozvrat.Value;                               

            command.ExecuteNonQuery();
            MessageBox.Show($"Добавленны данные о возврате.");


            SqlConnection connection1 = new SqlConnection();
            connection1.ConnectionString = ConnectionString;
            connection1.Open();

            SqlCommand command1 = connection1.CreateCommand();
            command1.CommandType = CommandType.StoredProcedure;
            command1.CommandText = "UpdStatusVozvrat";

            command1.Parameters.Add("@Avto_ID", SqlDbType.Int);
            command1.Parameters.Add("@Status", SqlDbType.NChar, 25);

            command1.Parameters["@Avto_ID"].Value = int.Parse(dgvDogovor[2, nRowA].Value.ToString());
            command1.Parameters["@Status"].Value = "Свободен";
            command1.ExecuteNonQuery();

            SqlConnection connection2 = new SqlConnection();
            connection2.ConnectionString = ConnectionString;
            connection2.Open();

            SqlCommand command2 = connection2.CreateCommand();
            command2.CommandType = CommandType.StoredProcedure;
            command2.CommandText = "UpdProbeg";

            command2.Parameters.Add("@Avto_ID", SqlDbType.Int);
            command2.Parameters.Add("@Probeg_fakt", SqlDbType.Int);

            command2.Parameters["@Avto_ID"].Value = int.Parse(dgvDogovor[2, nRowA].Value.ToString());
            command2.Parameters["@Probeg_fakt"].Value = nNewProbeg.Value;

            command2.ExecuteNonQuery();
            classAdo.DataGridBind(Dogovor, dgvDogovor);
            classAdo.DataGridBind(Avto,dgvAddDogovorAvto);
            classAdo.DataGridBind(Avto, dgvAvto);
        }

        private void nZalog_vozvrat_ValueChanged(object sender, EventArgs e)
        {
          
        }

        private void nZalogVzyt_ValueChanged(object sender, EventArgs e)
        {
            
        }

        private void txtZalogVzyt_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void dData_plan_ValueChanged(object sender, EventArgs e)
        {
            //Залог
            int Zalog = dgvAddDogovorAvto.CurrentRow.Index;
            decimal ZalogNorm = Convert.ToDecimal(dgvAddDogovorAvto[8, Zalog].Value.ToString());
            nZ.Value = ZalogNorm;

            //Сумма оплаты
            int day = (dData_plan.Value - dDogovotrDataArenda.Value).Days;
            int avto = dgvAddDogovorAvto.CurrentRow.Index;
            decimal price = Convert.ToDecimal(dgvAddDogovorAvto[17, avto].Value.ToString());
            decimal StoimArendy = day * price;
            nSumaAdd.Value = StoimArendy;


        }

        private void dData_fakt_ValueChanged(object sender, EventArgs e)
        {
            //Сумма
            int day = (dData_fakt.Value - dDogovotrDataArenda.Value).Days;
            int avto = dgvAddDogovorAvto.CurrentRow.Index;
            decimal price = Convert.ToDecimal(dgvAddDogovorAvto[17, avto].Value.ToString());
            decimal StoimArendy = day * price;
            nSuma_oplaty.Value = StoimArendy;

            //Залог
            int Zalog = dgvDogovor.CurrentRow.Index;
            decimal ZalogNorm = Convert.ToDecimal(dgvDogovor[10, Zalog].Value.ToString());
            nZalog_vozvrat.Value = ZalogNorm;
            //Пробег
            //int IdAvto = Convert.ToInt32(dgvDogovor[2, Zalog].Value.ToString());
            //string prob = "select Probeg_fakt from Avto where Avto.Avto_ID=" + IdAvto;
            //// int probeg = dgvDogovor.CurrentRow.Index;
            //// decimal probegSt = Convert.ToDecimal(dgvDogovor[10, probeg].Value.ToString());
            //nNewProbeg.Value = Convert.ToDecimal(prob.);//probegSt;
            int Rowprobeg = dgvDogovor.CurrentRow.Index;
            decimal newpobeg = Convert.ToDecimal(dgvDogovor[12, Rowprobeg].Value.ToString());
            nNewProbeg.Value = newpobeg;

        }

        private void nZ_ValueChanged(object sender, EventArgs e)
        {
            int RowZalog = dgvAddDogovorAvto.CurrentRow.Index;
            decimal ZalogNorm = Convert.ToDecimal(dgvAddDogovorAvto[8, RowZalog].Value.ToString());
            nZ.Value = ZalogNorm;
        }

        private void dDogovotrDataArenda_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dgvClient_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void gbAuto_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            string sql = "";
            if (txtSearchClientAddDogovor.Text != "")
            {
                sql = Client + " Where FIO Like " + "N'" + txtSearchClientAddDogovor.Text + "%" + "'";
            }
            else
            {
                sql = Client;
            }
            classAdo.DataGridBind(sql, dgvAddDogovorClient);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string sql = Avto;

            if (checkClass.Checked)
            {
                sql += " AND Class.Class_name=" + "'" + cbSearchClass.Text + "'";
            }
            if (checkMarka.Checked)
            {
                sql += " AND Avto.Marka= " + "'" + txtSearchMarka.Text  + "'";
            }
            if (checkPrice.Checked)
            {
                sql += " And  Avto.Price_sytki <= " + "'" + nSearchPrice.Value + "'";
            }

            if (checkConditioner.Checked)
            {
                sql += " AND Avto.Conditioner=" + "'" + cbSearchConditioner.Text + "'";
            }
            if (checkGB.Checked)
            {
                sql += " AND Avto.Gearbox= " + "'" + cbSearchGB.Text + "'";
            }
            if (checkColor.Checked)
            {
                sql += " And  Avto.Color = " + "'" + cbSearchColor.Text + "'";
            }
            //MessageBox.Show(sql);
            classAdo.DataGridBind(sql, dgvAddDogovorAvto);
        }

        private void cbGearbox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
