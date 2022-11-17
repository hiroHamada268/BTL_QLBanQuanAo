using Microsoft.SqlServer.Server;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace BTL_QLCuaHangBanQuanAo.Model.Database
{
    internal class DataProvider
    {
        /*
         *  Mieu ta: Đường dẫn kết nối đến cơ sở dữ liệu sqlserver 
         **/
        //private string connectionStr = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" + System.IO.Directory.GetCurrentDirectory().ToString() + "\\DataBase\\Data.mdf;Integrated Security=True";
        //private readonly string connectionStr = @"Data Source=ADMIN\SQLEXPRESS;Initial Catalog=Winform_CuaHangQuanAo;Integrated Security=True";
        private readonly string connectionStr = @"Data Source=ADMIN\SQLEXPRESS;Initial Catalog=Winform_QLCuaHangBanQuanAo;Integrated Security=True";
        private static DataProvider instance;
        public SqlCommand sqlCommand;
        public SqlDataAdapter adapter;
        public DataProvider()
        {
        }

        // Khoi tao mot bien instance thể hiện của dataprovider, việc sử dụng biến static là để khi mà muốn thực hiện 1 câu lệnh sql thì đều cần gọi tới
        // một dataprovider thì dẫn đến nó sẽ tạo nhiều connectionStr kết nối nhiều lần đến db, thì biến static sẽ tạo 1 vùng nhớ độc lập giúp chỉ truy suất lên db 1 lần
        public static DataProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DataProvider();
                };
                return instance;
            }
            private set => instance = value;
        }

        /*
         *  Input: Chuỗi truy vấn thực thi trên sql
         *  Output: Một datatable
         *  Desc: Thực thi câu lệnh truy vấn 
         * **/
        public DataTable ExecuteQuery(string query)
        {
            DataTable dtb = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                connection.Open();

                sqlCommand = new SqlCommand(query, connection);
                adapter = new SqlDataAdapter(sqlCommand);
                adapter.Fill(dtb);

                connection.Close();
            }
            return dtb;
        }

        /*
         *  Input: Chuỗi truy vấn thực thi trên sql
         *  Output: Số lượng truy vấn thành công
         *  Desc: Thực thi câu lệnh truy vấn chỉ đối với "INSERT, UPDATE, DELETE" 
         * **/
        public int ExecuteNonQuery(string query)
        {
            int data = 0;
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                connection.Open();

                sqlCommand = new SqlCommand(query, connection);
                data = sqlCommand.ExecuteNonQuery();

                connection.Close();
            }
            return data;
        }
        public int ExecuteNonQuery(string query, byte[] Img)
        {
            int data = 0;
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                connection.Open();

                sqlCommand = new SqlCommand(query, connection);
                sqlCommand.Parameters.Add("@Img", SqlDbType.Image, Img.Length).Value = Img;
                data = sqlCommand.ExecuteNonQuery();

                connection.Close();
            }
            return data;
        }

        /*
         *  Input: Chuỗi truy vấn thực thi trên sql
         *  Output: Gía trị của câu truy vấn
         *  Desc: Thực thi câu lệnh truy vấn, và trả về 1 giá trị trên hàng 1, cột 1, 
         *        thường sd để lấy giá trị sau khi thực hiện 1 phép tính
         * **/
        public object ExecuteScalar(string query)
        {
            object data = 0;

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                connection.Open();

                sqlCommand = new SqlCommand(query, connection);
                data = sqlCommand.ExecuteScalar();

                connection.Close();

            }
            return data;
        }
        
        public void FillCBO(string sql, ComboBox cbo, string name)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                connection.Open();
                DataTable table = ExecuteQuery(sql);
                cbo.DataSource = table;
                cbo.ValueMember = name;
                cbo.SelectedIndex = -1;
            }
        }

        public void FillTxt(string sql, TextBox Txt,string s)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
               
                connection.Open();
                DataTable table = ExecuteQuery(sql);
                DataRow data = table.Rows[0];
                Txt.Text = data[s].ToString();
            }
        }
        public string SinhMaHDB()
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                connection.Open();
                DataTable dt = ExecuteQuery("SELECT dbo.SinhMaHDB()");
                string str = dt.Rows[0][0].ToString();
                return str;
                connection.Close();
            }
        }
        public string SinhMaHDN()
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                connection.Open();
                DataTable dt = ExecuteQuery("SELECT dbo.SinhMaHDN()");
                string str = dt.Rows[0][0].ToString();
                return str;
                connection.Close();
            }
        }

        /*
         * Mieu ta: Xuat file excel dữ liệu được lấy từ datagridview
         *
         **/
        public void ExportFileExcel(DataGridView dataGridView, string title)
        {
            Microsoft.Office.Interop.Excel.Application exApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook exBook = exApp.Workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
            Microsoft.Office.Interop.Excel.Worksheet exSheet = (Microsoft.Office.Interop.Excel.Worksheet)exBook.Worksheets[1];

            exSheet.get_Range("B2:C3").Font.Bold = true;

            // =============================== Title ===============================
            Microsoft.Office.Interop.Excel.Range head = exSheet.get_Range("A1", "F1");
            head.MergeCells = true;
            head.Value2 = title;
            head.Font.Bold = true;
            head.Font.Name = "Times New Roman";
            head.Font.Size = "20";
            head.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

            // =============================== Column ===============================
            Microsoft.Office.Interop.Excel.Range cl1 = exSheet.get_Range("A3");
            cl1.Value = "Ma Hang Hoa";
            cl1.ColumnWidth = 12;

            exSheet.get_Range("B3").Value = "Ten Hang";
            Microsoft.Office.Interop.Excel.Range cl2 = exSheet.get_Range("B3");
            cl2.Value2 = "Ten Hang";
            cl2.ColumnWidth = 25.0;

            Microsoft.Office.Interop.Excel.Range cl3 = exSheet.get_Range("C3");
            cl3.Value2 = "Chat Lieu";
            cl3.ColumnWidth = 12.0;

            Microsoft.Office.Interop.Excel.Range cl4 = exSheet.get_Range("D3");
            cl4.Value2 = "So Luong";
            cl4.ColumnWidth = 10.5;

            Microsoft.Office.Interop.Excel.Range cl5 = exSheet.get_Range("E3");
            cl5.Value2 = "Don Gia Nhap";
            cl5.ColumnWidth = 20.5;

            Microsoft.Office.Interop.Excel.Range cl6 = exSheet.get_Range("F3");
            cl6.Value2 = "Don Gia Ban";
            cl6.ColumnWidth = 18.5;

            // Thiet lap mau cho tieu de cot, A, B, C ... là các cột trong excel sẽ được tô màu
            string[] array = { "A", "B", "C", "D", "E", "F" };
            foreach (string str in array)
            {
                Microsoft.Office.Interop.Excel.Range rowHead = exSheet.get_Range(str + 3);
                rowHead.Font.Bold = true;

                // Kẻ viền
                rowHead.Borders.LineStyle = Microsoft.Office.Interop.Excel.Constants.xlSolid;

                // Thiết lập màu nền
                rowHead.Interior.ColorIndex = 6;
                rowHead.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            }

            int n = dataGridView.Rows.Count;
            for (int i = 0; i < n; i++)
            {
                exSheet.get_Range("A" + (i + 4).ToString()).Value = dataGridView.Rows[i].Cells[0].Value;
                exSheet.get_Range("B" + (i + 4).ToString()).Value = dataGridView.Rows[i].Cells[1].Value;
                exSheet.get_Range("C" + (i + 4).ToString()).Value = dataGridView.Rows[i].Cells[2].Value;
                exSheet.get_Range("D" + (i + 4).ToString()).Value = dataGridView.Rows[i].Cells[3].Value;
                exSheet.get_Range("E" + (i + 4).ToString()).Value = dataGridView.Rows[i].Cells[4].Value;
                exSheet.get_Range("F" + (i + 4).ToString()).Value = dataGridView.Rows[i].Cells[5].Value;
            }
            exBook.Activate();
            SaveFileDialog saved = new SaveFileDialog();
            saved.ShowDialog();
            exBook.SaveAs(saved.FileName);
            exApp.Quit();
        }

    }
}
