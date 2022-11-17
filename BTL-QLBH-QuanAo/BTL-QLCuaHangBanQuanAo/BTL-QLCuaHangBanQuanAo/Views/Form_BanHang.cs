using BTL_QLCuaHangBanQuanAo.Model.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace BTL_QLCuaHangBanQuanAo.Views
{
    public partial class Form_BanHang : Form
    {
        DataProvider data = new DataProvider();
        DataTable table = new DataTable();
        double Tien = 0;
        double OldTien = 0;
        List<string> listMaQA = new List<string>();
        public Form_BanHang()
        {
            InitializeComponent();
        }

        private void Form_BanHang_Load(object sender, EventArgs e)
        {
            table.Columns.Add(new DataColumn("MaQuanAo"));
            table.Columns.Add(new DataColumn("SoLuong"));
            table.Columns.Add(new DataColumn("DonGia"));
            table.Columns.Add(new DataColumn("GiamGia"));
            table.Columns.Add(new DataColumn("ThanhTien"));
            dgvLapHD.DataSource = table;
            StartPr();
        }
        void StartPr()
        {
            Tien = 0;
            OldTien = 0;
            listMaQA.Clear();
            table.Rows.Clear();
            //txtMaHD.Text = data.ExecuteFunction("SinhMaHDB");
            txtMaHD.Text = data.SinhMaHDB();
            string qr = $"select distinct MaNV from NhanVien";
            data.FillCBO(qr, cboMaNV, "MaNV");
            qr = $"select distinct MaKhach from KhachHang";
            data.FillCBO(qr, cboKhachhang, "MaKhach");
            qr = $"select distinct MaQuanAo from SanPham";
            data.FillCBO(qr, cboSP, "MaQuanAo");
        }

        private void cboMaNV_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboMaNV.SelectedIndex != -1)
            {
                try
                {
                    data.FillTxt($"SELECT * from NhanVien WHERE MaNV = '{cboMaNV.SelectedValue.ToString()}'", txtTenNV, "TenNV");
                }
                catch
                {
                    //MessageBox.Show("looixi");
                }

            }
            else
            {
                txtTenNV.Text = "";
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!checkDuLieu1())
            {
                MessageBox.Show("Chưa điền đủ thông tin");
                return;
            }
            if (listMaQA.Contains(cboSP.SelectedValue.ToString()))
            {
                MessageBox.Show("Sản phẩm này đã có trong đơn hàng !\nNếu bạn muốn thêm vui lòng chọn sửa !");
                return;
            }
            DataRow row;
            row = table.NewRow();
            row["MaQuanAo"] = cboSP.SelectedValue.ToString();
            row["SoLuong"] = txtSoluong.Text;
            row["DonGia"] = txtDongia.Text;
            row["GiamGia"] = txtGiamgia.Text;
            row["ThanhTien"] = txtThanhTien.Text;
            Tien += double.Parse(txtThanhTien.Text);
            txtTong.Text = Tien.ToString();
            table.Rows.Add(row);
            dgvLapHD.DataSource = table;
            listMaQA.Add(cboSP.SelectedValue.ToString());
            txtTrakhach.Text = (double.Parse(txtKhachtra.Text) - double.Parse(txtTong.Text)).ToString();
        }

        bool checkDuLieu1()
        {
            if (cboSP.SelectedIndex < 0)
            {
                cboSP.Focus();
                return false;
            }
            if (txtSoluong.Text.Trim() == "")
            {
                txtSoluong.Focus();
                return false;
            }
            if (txtGiamgia.Text.Trim() == "")
            {
                txtGiamgia.Focus();
                return false;
            }
            return true;
        }
        bool checkDuLieu2()
        {
            if (!checkDuLieu1())
            {
                return false;
            }
            if (cboKhachhang.SelectedIndex < 0)
            {
                cboKhachhang.Focus();
                return false;
            }
            if (cboMaNV.SelectedIndex < 0)
            {
                cboMaNV.Focus();
                return false;
            }
            return true;
        }
        private void cboSP_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSP.SelectedIndex != -1)
            {
                try
                {
                    data.FillTxt($"SELECT * from SanPham WHERE MaQuanAo = '{cboSP.SelectedValue.ToString()}'", txtTenSP, "TenQuanAo");
                    data.FillTxt($"SELECT * from SanPham WHERE MaQuanAo = '{cboSP.SelectedValue.ToString()}'", txtDongia, "DonGiaBan");
                }
                catch
                {
                    //MessageBox.Show("looixi");
                }

            }
            else
            {
                txtTenSP.Text = "";
            }
        }

        private void txtSoluong_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtThanhTien.Text = (double.Parse(txtSoluong.Text) * double.Parse(txtDongia.Text) * (1 - (double.Parse(txtGiamgia.Text) / 100))).ToString();
            }
            catch
            {

            }
            if (txtSoluong.Text == "" || txtDongia.Text == "" || txtGiamgia.Text == "")
            {
                txtThanhTien.Text = "";
            }
            txtTrakhach.Text = (double.Parse(txtKhachtra.Text) - double.Parse(txtTong.Text)).ToString();
        }

        private void txtGiamgia_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtThanhTien.Text = (double.Parse(txtSoluong.Text) * double.Parse(txtDongia.Text) * (1 - (double.Parse(txtGiamgia.Text) / 100))).ToString();
            }
            catch
            {

            }
            if (txtSoluong.Text == "" || txtDongia.Text == "" || txtGiamgia.Text == "")
            {
                txtThanhTien.Text = "";
            }
            txtTrakhach.Text = (double.Parse(txtKhachtra.Text) - double.Parse(txtTong.Text)).ToString();
        }

        private void txtGiamgia_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || Convert.ToInt16(e.KeyChar) == 8)
            {
                if (Convert.ToInt16(e.KeyChar) != 8)
                {
                    double check = double.Parse(txtGiamgia.Text + Convert.ToChar(e.KeyChar).ToString());
                    if (check > 100)
                    {
                        e.Handled = true;

                    }
                }
            }
            else
            {
                e.Handled = true;
            }
            txtTrakhach.Text = (double.Parse(txtKhachtra.Text) - double.Parse(txtTong.Text)).ToString();
        }

        private void txtDongia_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtThanhTien.Text = (double.Parse(txtSoluong.Text) * double.Parse(txtDongia.Text) * (1 - (double.Parse(txtGiamgia.Text) / 100))).ToString();
            }
            catch
            {

            }
            if (txtSoluong.Text == "" || txtDongia.Text == "" || txtGiamgia.Text == "")
            {
                txtThanhTien.Text = "";
            }
            txtTrakhach.Text = (double.Parse(txtKhachtra.Text) - double.Parse(txtTong.Text)).ToString();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            txtTrakhach.Text = (double.Parse(txtKhachtra.Text) - double.Parse(txtTong.Text)).ToString();
            int i = dgvLapHD.CurrentRow.Index;
            listMaQA.Remove(dgvLapHD.Rows[i].Cells[0].Value.ToString());
            Tien -= OldTien;
            txtTong.Text = Tien.ToString();
            dgvLapHD.Rows.RemoveAt(i);
            btnXoa.Enabled = false;
        }

        private void txtKhachtra_TextChanged(object sender, EventArgs e)
        {
            txtTrakhach.Text = (double.Parse(txtKhachtra.Text) - double.Parse(txtTong.Text)).ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!checkDuLieu2())
            {
                MessageBox.Show("Bạn chưa điền đủ thông tin");
                return;
            }
            if (double.Parse(txtTrakhach.Text) < 0)
            {
                MessageBox.Show($"Khách cần trả thêm {Math.Abs(double.Parse(txtTrakhach.Text))} tiền");
            }
            else if (double.Parse(txtTrakhach.Text) > 0)
            {
                MessageBox.Show($"Cần trả cho khách {Math.Abs(double.Parse(txtTrakhach.Text))} tiền");
            }
            try
            {
                string sql = $"INSERT INTO HoaDonBan VALUES('{txtMaHD.Text}','{dtpTime.Value.ToString("yyyy-MM-dd")}',{txtTong.Text},'{cboMaNV.Text}','{cboKhachhang.Text}')";
                data.ExecuteNonQuery(sql);
                for (int i = 0; i < dgvLapHD.Rows.Count - 1; i++)
                {
                    sql = $"INSERT INTO ChiTietHDB VALUES({dgvLapHD.Rows[i].Cells[1].Value.ToString()},{dgvLapHD.Rows[i].Cells[3].Value.ToString()},{dgvLapHD.Rows[i].Cells[4].Value.ToString()},'{dgvLapHD.Rows[i].Cells[0].Value.ToString()}','{txtMaHD.Text}')";
                    data.ExecuteNonQuery(sql);
                }
                StartPr();
                MessageBox.Show("Thêm thành công");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi");
            }

            StartPr();
        }

        private void btnThemnhom_Click(object sender, EventArgs e)
        {

        }

        private void dgvLapHD_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            DataProvider.Instance.ExportFileExcel(dgvLapHD, "Danh sách hóa đơn");
        }
    }
}
