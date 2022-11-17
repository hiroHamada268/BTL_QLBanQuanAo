using BTL_QLCuaHangBanQuanAo.Model.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTL_QLCuaHangBanQuanAo.Views
{
    public partial class Form_NhanVien : Form
    {
        DataProvider connection = new DataProvider();
        DataTable dataNhanVien = new DataTable();
        public Form_NhanVien()
        {
            InitializeComponent();
        }
        private void LoadData()
        {
            dataNhanVien = connection.ExecuteQuery("SELECT * FROM NhanVien");
            dgvNhanVien.DataSource = dataNhanVien;

        }
    

        private void Form_NhanVien_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dgvNhanVien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvNhanVien.Rows.Count <= 0) return;

            foreach (Control ctr in grbThongTinNV.Controls)
            {
                if (ctr is TextBox || ctr is ComboBox || ctr is DateTimePicker)
                {
                    ctr.Enabled = false;
                }
            }
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnLuu.Enabled = false;
            btnBoQua.Enabled = false;
            
            txtMaNV.Text = dgvNhanVien.CurrentRow.Cells[0].Value.ToString();
            txtTenNV.Text = dgvNhanVien.CurrentRow.Cells[1].Value.ToString();
            txtGioiTinh.Text = dgvNhanVien.CurrentRow.Cells[2].Value.ToString();
            dtpNgaySinh.Value = DateTime.Parse(dgvNhanVien.CurrentRow.Cells[3].Value.ToString());
            txtDienThoai.Text = dgvNhanVien.CurrentRow.Cells[4].Value.ToString();
            txtDiaChi.Text = dgvNhanVien.CurrentRow.Cells[5].Value.ToString();
            cboMaCV.Text = dgvNhanVien.CurrentRow.Cells[6].Value.ToString();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            dgvNhanVien.DataSource = connection.ExecuteQuery($"SELECT * FROM NhanVien WHERE MaNV LIKE '%{txtTimKiem.Text}%' OR TenNV LIKE N'%{txtTimKiem.Text}%'");
        }
        private bool Check()
        {
            errorProvider1.Clear();
            string regexSDT = @"^[0]{1}[0-9]{9}$";
            if (txtMaNV.Text == "")
            {
                errorProvider1.SetError(txtMaNV, "Chưa Nhập Mã NV");
                txtMaNV.Focus();
                return false;
            }
            if (txtTenNV.Text == "")
            {
                errorProvider1.SetError(txtTenNV, "Chưa Nhập Tên NV");
                txtTenNV.Focus();
                return false;
            }
            if (txtGioiTinh.Text == "")
            {
                errorProvider1.SetError(txtGioiTinh, "Chưa Nhập Giới Tính");
                txtGioiTinh.Focus();
                return false;
            }
            if (txtDienThoai.Text == "")
            {
                errorProvider1.SetError(txtDienThoai, "Chưa Nhập SĐT");
                return false;
            }
            else
            if (Regex.IsMatch(txtDienThoai.Text, regexSDT) == false)
            {
                errorProvider1.SetError(txtDienThoai, "SĐT không hợp lệ");
                return false;
            }
            if (txtDiaChi.Text == "")
            {
                errorProvider1.SetError(txtDiaChi, "Chưa Nhập Địa Chỉ");
                return false;
            }
            if (DateTime.Now.Year - dtpNgaySinh.Value.Year < 18)
            {
                errorProvider1.SetError(dtpNgaySinh, "Chưa đủ độ tuổi làm việc");
                return false;
            }
            if (cboMaCV.Text == "")
            {
                errorProvider1.SetError(cboMaCV, "Chưa chọn Công Việc");
                return false;
            }
            return true;
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            foreach (Control ctr in grbThongTinNV.Controls)
            {
                if (ctr is TextBox || ctr is ComboBox||ctr is DateTimePicker)
                {
                    ctr.Enabled = true;
                }
            }
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnLuu.Enabled = true;
            btnBoQua.Enabled = true;
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            foreach (Control ctr in grbThongTinNV.Controls)
            {
                if (ctr is TextBox || ctr is ComboBox)
                {
                    ctr.Text = "";
                }
                if (ctr is DateTimePicker)
                {
                    ctr.Text = "";
                }
            }
            LoadData();
        }

        private void txbSearch_TextChanged(object sender, EventArgs e)
        {

        }

        private void cboMaCV_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cboMaCV_Click(object sender, EventArgs e)
        {
            DataTable dataMaCV = connection.ExecuteQuery("SELECT * FROM CongViec");
            cboMaCV.DataSource = dataMaCV;
            cboMaCV.ValueMember = "MaCV";
            cboMaCV.DisplayMember = "MaCV";
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnLuu.Enabled = true;
            btnBoQua.Enabled = true;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if(btnThem.Enabled == true)
            {
                if (Check())
                {
                    DataTable dataMaNV = connection.ExecuteQuery($"SELECT * FROM NhanVien WHERE MaNV = '{txtMaNV.Text}'");
                    if(dataMaNV.Rows.Count > 0)
                    {
                        MessageBox.Show("Mã NV đã tồn tại");
                        return;
                    }
                    if (MessageBox.Show("Bạn có muốn thêm nhân viên này không?", "Thông báo!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        try
                        {
                            string querry = $"INSERT INTO NhanVien VALUES('{txtMaNV.Text}', N'{txtTenNV.Text}',N'{txtGioiTinh.Text}', '{dtpNgaySinh.Value.ToString("yyyy-MM-dd")}', '{txtDienThoai.Text}', N'{txtDiaChi.Text}','{cboMaCV.Text}')";
                            connection.ExecuteNonQuery(querry);
                            LoadData();
                            MessageBox.Show("Thêm nhân viên thành công");
                        }
                        catch
                        {
                            MessageBox.Show("Thêm nhân viên không thành công");
                        }
                    }
                }
            }
            if (btnXoa.Enabled == true)
            {
                if (MessageBox.Show("Bạn có muốn xoá nhân viên này không?", "Thông báo!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (dgvNhanVien.SelectedRows.Count < 0)
                    {
                        return;
                    }
                    string manv = dgvNhanVien.CurrentRow.Cells[0].Value.ToString();

                    try
                    {
                        string querry = $"DELETE FROM NhanVien WHERE MaNV = '{manv}'";
                        connection.ExecuteNonQuery(querry);
                        LoadData();
                        MessageBox.Show("Xóa thành công");
                    }
                    catch
                    {
                        MessageBox.Show("Xóa thất bại");
                    }
                }
            }
            if(btnSua.Enabled == true)
            {
                if (Check())
                {
                    if (dgvNhanVien.CurrentRow.Cells[0].Value.ToString()!= txtMaNV.Text){
                        DataTable dataMaNV = connection.ExecuteQuery($"SELECT * FROM NhanVien WHERE MaNV = '{txtMaNV.Text}'");
                        if (dataMaNV.Rows.Count > 0)
                        {
                            MessageBox.Show("Mã NV đã tồn tại");
                            return;
                        } 
                    }
                    if (MessageBox.Show("Bạn có muốn sửa nhân viên này không?", "Thông báo!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {

                        try
                        {
                            string manv = dgvNhanVien.CurrentRow.Cells[0].Value.ToString();
                            string querry = $"UPDATE NhanVien SET MaNV='{txtMaNV.Text}', TenNV =N'{txtTenNV.Text}', GioiTinh =N'{txtGioiTinh.Text}', NgaySinh='{dtpNgaySinh.Value.ToString("yyyy-MM-dd")}',DienThoai='{txtDienThoai.Text}',DiaChi= N'{txtDiaChi.Text}',MaCV='{cboMaCV.Text}' WHERE MaNV ='{manv}'";
                            connection.ExecuteNonQuery(querry);
                            LoadData();
                            MessageBox.Show("Sửa nhân viên thành công");
                        }
                        catch
                        {
                            MessageBox.Show("Sửa nhân viên không thành công");
                        }
                    }
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            foreach (Control ctr in grbThongTinNV.Controls)
            {
                if (ctr is TextBox || ctr is ComboBox || ctr is DateTimePicker)
                {
                    ctr.Enabled = true;
                }
            }
            btnThem.Enabled = false;
            btnXoa.Enabled = false;
            btnLuu.Enabled = true;
            btnBoQua.Enabled = true;
        }

        private void btnBoQua_Click(object sender, EventArgs e)
        {
            foreach (Control ctr in grbThongTinNV.Controls)
            {
                if (ctr is TextBox || ctr is ComboBox || ctr is DateTimePicker)
                {
                    ctr.Enabled = false;
                }
            }
            btnBoQua.Enabled = false;
            btnLuu.Enabled = false;
            btnThem.Enabled = true;
            btnXoa.Enabled = false;
            btnSua.Enabled = false;

        }

        private void pnInput_Paint(object sender, PaintEventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            DataProvider.Instance.ExportFileExcel(dgvNhanVien, "Danh sách nhân viên");
        }

        private void btnReport_Click(object sender, EventArgs e)
        {

        }
    }
}
