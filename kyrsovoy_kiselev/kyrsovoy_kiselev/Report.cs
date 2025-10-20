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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using Excel = Microsoft.Office.Interop.Excel;

namespace kyrsovoy_kiselev
{
    public partial class Report : Form
    {
        public Report()
        {
            InitializeComponent();
        }
        string ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=avtoservis;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False;Command Timeout=30";
        String Avto = 
           "Select Avto_ID  AS [№],Marka AS Марка,Model AS Модель,Color AS Цвет,Gos_number AS [Гос.номер],Date_release AS [Год выпуска],DTP AS ДТП,Class_name AS Класс," +
           "Places AS[Кол - во мест], Doors AS[Кол - во дверей],Conditioner AS Кондиционер,Gearbox AS[Коробка передач], Price_new AS[Цена нового],Probeg_norm AS[Пробег норма]," +
           "Probeg_fakt AS[Пробег фактический],Price_iznos AS[Стоимость износа], Price_sytki AS[Цена за сутки],Status AS Статус FROM Avto,Class Where Avto.Class_ID=Class.Class_ID ";
        String Dogovor =
           "Select Dogovor_ID AS [№ Договора], Klient.FIO AS [Клиент],Avto.Avto_ID AS [№ Авто], Avto.Marka AS[Марка Авто],Avto.Model AS [Модель авто],Avto.Gos_number AS[Гос.номер]," +
           "Data_arendy AS[Дата аренды],Data_plan AS[Возврат плановый],Data_fakt AS[Возврат фактический], Suma_oplaty AS[Сумма оплаты],Zalog_vzyt AS[Принят залог], Zalog_vozvrat AS" +
           "[Возвращен залог] From Dogovor,Klient,Avto Where Dogovor.Klient_ID=Klient.Klient_ID AND Dogovor.Avto_ID=Avto.Avto_ID";
        String Class = "Select Class_ID AS [№],Class_name AS Наиминование, Zalog AS Залог FROM Class";


        string ReportOrendy = "";

        ClassAdo classAdo = new ClassAdo();
        private void button4_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConnectionString;
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "Report";

            command.Parameters.Add("@OtezdDatePlan", SqlDbType.Date);
            command.Parameters.Add("@OtezdDateFact", SqlDbType.Date);

            command.Parameters["@OtezdDatePlan"].Value = DateReportStart.Value;
            command.Parameters["@OtezdDateFact"].Value = DateReportEnd.Value;

            SqlDataAdapter reportAdapter = new SqlDataAdapter();
            reportAdapter.SelectCommand = command;
            DataSet dsReport = new DataSet();
            reportAdapter.Fill(dsReport, "Report");
            dgvReport.DataSource = dsReport.Tables[0].DefaultView;

            command.ExecuteNonQuery();
            MessageBox.Show($"Отчет оформлен.");
            classAdo.DataGridBind(ReportOrendy, dgvReport);
        }

        private void cbClass_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void Report_Load(object sender, EventArgs e)
        {
            classAdo.DataGridBind(Avto, dgvAmort);
            classAdo.DataGridBind(Dogovor, dgvReport);
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int P = dgvAmort.CurrentRow.Index; ;
            decimal Price_new = Convert.ToDecimal(dgvAmort[12, P].Value.ToString());
            decimal Probeg_norm = Convert.ToDecimal(dgvAmort[13, P].Value.ToString());
            decimal Probeg_fakt = Convert.ToDecimal(dgvAmort[14, P].Value.ToString());
            decimal AmortNorm = Price_new / Probeg_norm;
            decimal Amort = Probeg_fakt * AmortNorm;
            decimal StoimSiznosom = Price_new - Amort;

            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConnectionString;
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "UpdPrice_iznos";

            int nRow = dgvAmort.CurrentRow.Index;
            command.Parameters.Add("@Avto_ID", SqlDbType.Int);
            command.Parameters.Add("@Price_iznos", SqlDbType.Int);

            command.Parameters["@Avto_ID"].Value = int.Parse(dgvAmort[0, nRow].Value.ToString());
            command.Parameters["@Price_iznos"].Value = StoimSiznosom;

            command.ExecuteNonQuery();
            MessageBox.Show($"Данные о износе обновлены.");
            classAdo.DataGridBind(Avto, dgvAmort);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (rbClass.Checked)
            {
                string ReportClass = "Select Class.Class_name, Count(Dogovor.Dogovor_ID) AS [Кол-во договоров],Sum(Dogovor.Suma_oplaty) AS [Общая сумма аренды] " +
               "FROM Dogovor, Class, Avto, Klient " +
               "Where Dogovor.Avto_ID = Avto.Avto_ID AND Dogovor.Klient_ID = Klient.Klient_ID AND Avto.Class_ID = Class.Class_ID  AND Dogovor.Data_arendy" +
               " Between " + "'" + DateReportStart.Value.ToShortDateString() + "'" + " AND " + "'" + DateReportEnd.Value.ToShortDateString() + "'" + "" +
               " Group By  Class.Class_name";

                classAdo.DataGridBind(ReportClass, dgvReport);

                double sum = 0;
                for (int i = 0; i < dgvReport.RowCount - 1; i++)
                {
                    sum += Convert.ToDouble(dgvReport[2, i].Value);
                }
                int lastRow = dgvReport.RowCount - 1;
                dgvReport[1, lastRow].Value = "Всего";
                dgvReport[2, lastRow].Value = sum.ToString();                    
            }
            if (rbMarka.Checked)
            {
                string ReportMarka = "Select Avto.Marka, Count(Dogovor.Dogovor_ID) AS [Кол-во договоров],Sum(Dogovor.Suma_oplaty) AS [Общая сумма аренды]" +
                    "FROM Dogovor, Class, Avto, Klient " +
                    "Where Dogovor.Avto_ID = Avto.Avto_ID AND Dogovor.Klient_ID = Klient.Klient_ID AND Avto.Class_ID = Class.Class_ID  AND Dogovor.Data_arendy" +
                    " Between " + "'" + DateReportStart.Value.ToShortDateString() + "'" + " AND " + "'" + DateReportEnd.Value.ToShortDateString() + "'" + "" +
                    " Group By  Avto.Marka";
                classAdo.DataGridBind(ReportMarka, dgvReport);

                double sum = 0;
                for (int i = 0; i < dgvReport.RowCount - 1; i++)
                {
                    sum += Convert.ToDouble(dgvReport[2, i].Value);
                }
                int lastRow = dgvReport.RowCount - 1;
                dgvReport[1, lastRow].Value = "Всего";
                dgvReport[2, lastRow].Value = sum.ToString();
            }
            if (rbKlient.Checked)
            {
                string ReportKlient = "Select Klient.FIO, Count(Dogovor.Dogovor_ID) AS [Кол-во договоров],Sum(Dogovor.Suma_oplaty) AS [Общая сумма аренды]" +
                    "FROM Dogovor, Class, Avto, Klient " +
                    "Where Dogovor.Avto_ID = Avto.Avto_ID AND Dogovor.Klient_ID = Klient.Klient_ID AND Avto.Class_ID = Class.Class_ID  AND Dogovor.Data_arendy" +
                    " Between " + "'" + DateReportStart.Value.ToShortDateString() + "'" + " AND " + "'" + DateReportEnd.Value.ToShortDateString() + "'" + "" +
                    " Group By  Klient.FIO";
                classAdo.DataGridBind(ReportKlient, dgvReport);

                double sum = 0;
                for (int i = 0; i < dgvReport.RowCount - 1; i++)
                {
                    sum += Convert.ToDouble(dgvReport[2, i].Value);
                }
                int lastRow = dgvReport.RowCount - 1;
                dgvReport[1, lastRow].Value = "Всего";
                dgvReport[2, lastRow].Value = sum.ToString();
            }              
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string title = "";
            //добавить заголовок в период дат
            title += "Отчёт за период с " + DateReportStart.Value.ToShortDateString() + " по " + DateReportEnd.Value.ToShortDateString();
            Microsoft.Office.Interop.Excel.Application Excelapp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook Excelworkbook;
            Microsoft.Office.Interop.Excel.Worksheet Excelworksheet;
            Microsoft.Office.Interop.Excel.Range ExcelCells;
            Excelworkbook = Excelapp.Workbooks.Add(System.Reflection.Missing.Value);
            Excelworksheet = (Microsoft.Office.Interop.Excel.Worksheet)Excelworkbook.Worksheets.get_Item(1);
            Excelapp.Cells[1, 1] = title;
            for (int i = 0; i < dgvReport.ColumnCount - 1; i++)
            {
                Excelapp.Cells[2, i + 1] = dgvReport.Columns[i].HeaderCell.Value;
            }
            for (int i = 0; i < dgvReport.Rows.Count; i++)
            {
                for (int j = 0; j < dgvReport.ColumnCount; j++)
                {
                    Excelapp.Cells[i + 3, j + 1] = dgvReport[j, i].Value;
                }
            }
            int istr = dgvReport.Rows.Count + 1;
            ExcelCells = Excelapp.Range[Excelworksheet.Columns[2], Excelworksheet.Cells[istr, dgvReport.ColumnCount]];
            ExcelCells.EntireColumn.AutoFit();
            ExcelCells = Excelapp.Range[Excelworksheet.Columns[1], Excelworksheet.Columns[dgvReport.ColumnCount - 1]];
            ExcelCells.HorizontalAlignment = Microsoft.Office.Interop.Excel.Constants.xlLeft;
            ExcelCells = Excelapp.Range[Excelworksheet.Cells[2, 1], Excelworksheet.Cells[istr + 1, dgvReport.ColumnCount]];
            ExcelCells.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            ExcelCells.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;
            Excelapp.Visible = true;
            Excelapp.UserControl = true;
        }
    }
}
