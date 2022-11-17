using BTL_QLCuaHangBanQuanAo.Model.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace BTL_QLCuaHangBanQuanAo.Views
{
    public partial class Form_SanPham : Form
    {
        string MaQA = "";
        DataProvider sqlConnect = new DataProvider();
        string img = "";
        public Form_SanPham()
        {
            InitializeComponent();
        }

        private void Form_SanPham_Load(object sender, EventArgs e)
        {
            string Query = "SELECT * FROM SanPham";
            DataTable data = sqlConnect.ExecuteQuery(Query);
            dgvData.DataSource = data;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (img == "" && CheckDuLieu())
            {
                if (MessageBox.Show("Bạn có muốn thêm dữ liệu mà không có ảnh không ?", "Thông báo!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        string sql = $"INSERT INTO SanPham (MaQuanAo, TenQuanAo,SoLuong,DonGiaBan,DonGiaNhap,MaLoai,MaCo,MaChatLieu,MaMau,MaDoiTuong,MaMua,MaNSX) Values('{txtMaQA.Text}',N'{txtTen.Text}',{txtSL.Text},{txtDonGiaBan.Text},{txtDonGiaNhap.Text},'{txtMaLoai.Text}','{txtMaCo.Text}','{txtMaCL.Text}','{txtMaMau.Text}','{txtMaDoiTuong.Text}','{txtMaMua.Text}','{txtMaNSX.Text}')";
                        sqlConnect.ExecuteNonQuery(sql);
                        dgvData.DataSource = sqlConnect.ExecuteQuery("SELECT * FROM SanPham");
                        MessageBox.Show("Thêm Thành Công");
                    }
                    catch
                    {
                        MessageBox.Show("Thêm thất bại");
                    }
                    return;

                }
                return;
            }
            if (!CheckDuLieu())
            {
                return;
            }
            try
            {
                byte[] transfer = null;
                FileStream file = new FileStream(img, FileMode.Open, FileAccess.Read);
                BinaryReader reader = new BinaryReader(file);
                transfer = reader.ReadBytes((int)file.Length);

                string sql = $@"
                    INSERT INTO SanPham (
                        MaQuanAo, 
                        TenQuanAo,
                        SoLuong,
                        DonGiaBan,
                        DonGiaNhap,
                        MaLoai,
                        MaCo,
                        MaChatLieu,
                        MaMau,MaDoiTuong,MaMua,MaNSX,Img) 
                    Values('{txtMaQA.Text}',N'{txtTen.Text}',{txtSL.Text},{txtDonGiaBan.Text},{txtDonGiaNhap.Text},'{txtMaLoai.Text}','{txtMaCo.Text}','{txtMaCL.Text}','{txtMaMau.Text}','{txtMaDoiTuong.Text}','{txtMaMua.Text}','{txtMaNSX.Text}',@Img)";
                sqlConnect.ExecuteNonQuery(sql, transfer);
                dgvData.DataSource = sqlConnect.ExecuteQuery("SELECT * FROM SanPham");
                MessageBox.Show("Thêm Thành Công");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Thêm Thất Bại");
            }
        }
        bool CheckDuLieu()
        {
            if (txtMaQA.Text == "")
            {
                MessageBox.Show("Chưa điền mã quần áo");
                return false;
            }
            if (txtTen.Text == "")
            {
                MessageBox.Show("Chưa điền mã tên sản phẩm");
                return false;
            }
            if (txtSL.Text == "")
            {
                MessageBox.Show("Chưa điền số lượng");
                return false;
            }
            if (txtDonGiaBan.Text == "")
            {
                MessageBox.Show("Chưa điền đơn giá bán");
                return false;
            }
            if (txtDonGiaNhap.Text == "")
            {
                MessageBox.Show("Chưa điền đơn giá nhập");
                return false;
            }
            if (txtMaLoai.Text == "")
            {
                MessageBox.Show("Chưa điền mã loại");
                return false;
            }
            if (txtMaCo.Text == "")
            {
                MessageBox.Show("Chưa điền mã cỡ");
                return false;
            }
            if (txtMaCL.Text == "")
            {
                MessageBox.Show("Chưa điền mã chất liệu");
                return false;
            }
            if (txtMaMau.Text == "")
            {
                MessageBox.Show("Chưa điền mã mầu");
                return false;
            }
            if (txtMaDoiTuong.Text == "")
            {
                MessageBox.Show("Chưa điền mã đối tượng");
                return false;
            }
            if (txtMaMua.Text == "")
            {
                MessageBox.Show("Chưa điền mã mùa");
                return false;
            }
            if (txtMaNSX.Text == "")
            {
                MessageBox.Show("Chưa điền mã nhà sản xuất");
                return false;
            }
            return true;
        }
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                PicSanPham.Image = Image.FromFile(ofd.FileName);
                img = ofd.FileName;
            }
        }

        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow data = dgvData.Rows[e.RowIndex];
            MaQA = data.Cells[0].Value.ToString();
            txtMaQA.Text = data.Cells[0].Value.ToString();
            txtTen.Text = data.Cells[1].Value.ToString();
            txtSL.Text = data.Cells[2].Value.ToString();
            txtDonGiaBan.Text = data.Cells[3].Value.ToString();
            txtDonGiaNhap.Text = data.Cells[4].Value.ToString();
            txtMaLoai.Text = data.Cells[5].Value.ToString();
            txtMaCo.Text = data.Cells[6].Value.ToString();
            txtMaCL.Text = data.Cells[7].Value.ToString();
            txtMaMau.Text = data.Cells[8].Value.ToString();
            txtMaDoiTuong.Text = data.Cells[9].Value.ToString();
            txtMaMua.Text = data.Cells[10].Value.ToString();
            txtMaNSX.Text = data.Cells[11].Value.ToString();
            //MessageBox.Show(data.Cells[12].Value.ToString())
            if (data.Cells[12].Value.ToString() != "")
            {
                try
                {
                    byte[] bytes = (byte[])(data.Cells[12].Value);
                    MemoryStream ms = new MemoryStream(bytes);
                    PicSanPham.Image = Image.FromStream(ms);
                }
                catch   
                {

                }
                //PicSanPham.Image = Image.FromFile(data.Cells[12].Value.ToString());
                //PicSanPham.ImageLocation = data.Cells[12].Value.ToString();
            }
            else
            {
                PicSanPham.Image = null;
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = $"DELETE FROM SanPham WHERE MaQuanAo = '{txtMaQA.Text}'";
                sqlConnect.ExecuteNonQuery(sql);
                dgvData.DataSource = sqlConnect.ExecuteQuery("SELECT * FROM SanPham");
                MessageBox.Show("Xoá Thành Công");
            }
            catch
            {
                MessageBox.Show("Xoá Thất Bại");
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            if (txtMaQA.Text == "")
            {
                MessageBox.Show("Nhập mã sản phẩm đi");
                txtMaQA.Focus();
                return;
            }
            string sql = "SELECT* FROM SanPham WHERE MaQuanAo LIKE  '%" + txtMaQA.Text + "%'";
            dgvData.DataSource = sqlConnect.ExecuteQuery(sql);
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Title = "Export Excel";
            save.Filter = "ALL FILE |*.*|Excel|*.xlsx";
            save.FilterIndex = 2;
            if (save.ShowDialog() == DialogResult.OK)
            {
                try
                {

                    ExportExcel(dgvData, save.FileName);
                    MessageBox.Show("Xuất FILE thành công !");
                }
                catch
                {
                    MessageBox.Show("Xuất FILE thất bại !");
                }
            }
        }
        private void ExportExcel(DataGridView data, string Path)
        {
            string sql = "SELECT * FROM SanPham";
            dgvData.DataSource = sqlConnect.ExecuteQuery(sql);
            //khai báo thư viện hỗ trợ Microsoft.Office.Interop.Excel
            //Microsoft.Office.Interop.Excel.Application 
            Excel.Application excel;
            Excel.Workbook workbook;
            Excel.Worksheet worksheet;
            try
            {
                //Tạo đối tượng COM.
                excel = new Excel.Application();
                excel.Visible = false;
                excel.DisplayAlerts = false;
                //tạo mới một Workbooks bằng phương thức add()
                workbook = excel.Workbooks.Add(Type.Missing);
                worksheet = (Excel.Worksheet)workbook.Sheets[1];
                //đặt tên cho sheet
                worksheet.Name = "Data";
                // export header trong DataGridView
                //Range range 
                Excel.Range TieuDe = worksheet.get_Range("A1", "F1");
                TieuDe.Merge();
                TieuDe.Font.Size = 20;
                TieuDe.Font.Bold = true;
                TieuDe.Font.Name = "Times New Roman";
                TieuDe.Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                TieuDe.Value2 = "Thống kê sản phẩm";
                TieuDe.Font.Color = Color.Red;
                for (int i = 0; i < data.ColumnCount; i++)
                {
                    worksheet.Cells[2, i + 1] = data.Columns[i].HeaderText;
                }
                // export nội dung trong DataGridView
                for (int i = 0; i < data.RowCount - 1; i++)
                {
                    for (int j = 0; j < data.ColumnCount; j++)
                    {
                        worksheet.Cells[i + 3, j + 1] = data.Rows[i].Cells[j].Value.ToString();
                    }
                }
                // sử dụng phương thức SaveAs() để lưu workbook với filename
                workbook.SaveAs(Path);
                //đóng workbook
                workbook.Close();
                excel.Quit();
                //MessageBox.Show("Xuất dữ liệu ra Excel thành công!");
                System.Diagnostics.Process.Start(Path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                workbook = null;
                worksheet = null;
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (MaQA == "")
            {
                MessageBox.Show("Chưa nhập mã sản phẩm");
                return;
            }
            try
            {
                if (!CheckDuLieu())
                {
                    return;
                }
                if (img == "")
                {
                    string sql = $"UPDATE SanPham SET MaQuanAo = '{txtMaQA.Text}',TenQuanAo = N'{txtTen.Text}',SoLuong = {txtSL.Text},DonGiaBan = {txtDonGiaBan.Text},DonGiaNhap = {txtDonGiaNhap.Text},MaLoai = '{txtMaLoai.Text}',MaCo = '{txtMaCo.Text}',MaChatLieu = '{txtMaCL.Text}',MaMau = '{txtMaMau.Text}',MaDoiTuong = '{txtMaDoiTuong.Text}',MaMua = '{txtMaMua.Text}',MaNSX = '{txtMaNSX.Text}'  WHERE MaQuanAo = '{MaQA}'";
                    //,Img = N'{img}'
                    if (MessageBox.Show($"Bạn có chắc chắn muốn sửa sản phẩm {MaQA} này không", "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        sqlConnect.ExecuteQuery(sql);

                        dgvData.DataSource = sqlConnect.ExecuteQuery("Select * from SanPham");
                        MessageBox.Show("Sửa Thành Công");
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    string sql = $"UPDATE SanPham SET MaQuanAo = '{txtMaQA.Text}',TenQuanAo = N'{txtTen.Text}',SoLuong = {txtSL.Text},DonGiaBan = {txtDonGiaBan.Text},DonGiaNhap = {txtDonGiaNhap.Text},MaLoai = '{txtMaLoai.Text}',MaCo = '{txtMaCo.Text}',MaChatLieu = '{txtMaCL.Text}',MaMau = '{txtMaMau.Text}',MaDoiTuong = '{txtMaDoiTuong.Text}',MaMua = '{txtMaMua.Text}',MaNSX = '{txtMaNSX.Text}',Img=@Img  WHERE MaQuanAo = '{MaQA}'";
                    //,Img = N'{img}'
                    if (MessageBox.Show($"Bạn có chắc chắn muốn sửa sản phẩm {MaQA} này không", "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        byte[] transfer = null;
                        FileStream file = new FileStream(img, FileMode.Open, FileAccess.Read);
                        BinaryReader reader = new BinaryReader(file);
                        transfer = reader.ReadBytes((int)file.Length);
                        sqlConnect.ExecuteNonQuery(sql, transfer);

                        dgvData.DataSource = sqlConnect.ExecuteQuery("Select * from SanPham");
                        MessageBox.Show("Sửa Thành Công");
                    }
                    else
                    {
                        return;
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Sửa Thất Bại");
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Title = "Export Excel";
            save.Filter = "ALL FILE |*.*|Excel|*.xlsx";
            save.FilterIndex = 2;
            if (save.ShowDialog() == DialogResult.OK)
            {
                try
                {

                    ExportExcel(dgvData, save.FileName);
                    MessageBox.Show("Xuất FILE thành công !");
                }
                catch
                {
                    MessageBox.Show("Xuất FILE thất bại !");
                }
            }
        }
    }
}
