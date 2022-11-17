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

namespace BTL_QLCuaHangBanQuanAo.Views
{
    public partial class Form_NoiSanXuat : Form
    {
        DataProvider connection = new DataProvider();
        DataTable data = new DataTable();
        public Form_NoiSanXuat()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            data = connection.ExecuteQuery("SELECT * FROM NoiSanXuat");
            dgvDanhSach.DataSource = data;

        }
        private void Form_NoiSanXuat_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            dgvDanhSach.DataSource = connection.ExecuteQuery($"SELECT * FROM NoiSanXuat WHERE MaNSX LIKE '%{txtTimKiem.Text}%' OR TenNSX LIKE N'%{txtTimKiem.Text}%'");

        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            foreach (Control ctr in grbThongTin.Controls)
            {
                if (ctr is TextBox || ctr is ComboBox)
                {
                    ctr.Text = "";
                }
            }
            LoadData();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            foreach (Control ctr in grbThongTin.Controls)
            {
                if (ctr is TextBox || ctr is ComboBox || ctr is DateTimePicker)
                {
                    ctr.Enabled = true;
                }
            }
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnLuu.Enabled = true;
            btnBoQua.Enabled = true;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            foreach (Control ctr in grbThongTin.Controls)
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

        private void btnXoa_Click(object sender, EventArgs e)
        {
            btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnLuu.Enabled = true;
            btnBoQua.Enabled = true;
        }
        private bool Check()
        {
            errorProvider1.Clear();
            if (txtMa.Text.Trim() == "")
            {
                txtMa.Focus();
                errorProvider1.SetError(txtMa, "Chưa nhập mã");
                return false;
            }
            if (txtTen.Text.Trim() == "")
            {
                txtTen.Focus();
                errorProvider1.SetError(txtTen, "Chưa nhập tên");
                return false;
            }
            return true;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (btnThem.Enabled == true)
            {
                if (Check())
                {
                    DataTable dataMa = connection.ExecuteQuery($"SELECT * FROM NoiSanXuat WHERE MaNSX = '{txtMa.Text}'");
                    if (dataMa.Rows.Count > 0)
                    {
                        MessageBox.Show("Mã đã tồn tại");
                        return;
                    }
                    if (MessageBox.Show("Bạn có muốn thêm  không?", "Thông báo!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        try
                        {
                            string querry = $"INSERT INTO NoiSanXuat VALUES('{txtMa.Text}', N'{txtTen.Text}')";
                            connection.ExecuteNonQuery(querry);
                            LoadData();
                            MessageBox.Show("Thêm thành công");
                        }
                        catch
                        {
                            MessageBox.Show("Thêm không thành công");
                        }
                    }
                }
            }
            if (btnXoa.Enabled == true)
            {
                if (MessageBox.Show("Bạn có muốn xoá không?", "Thông báo!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (dgvDanhSach.SelectedRows.Count < 0)
                    {
                        return;
                    }
                    string ma = dgvDanhSach.CurrentRow.Cells[0].Value.ToString();

                    try
                    {
                        string querry = $"DELETE FROM NoiSanXuat WHERE MaNSX = '{ma}'";
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

            if (btnSua.Enabled == true)
            {
                if (Check())
                {
                    if (dgvDanhSach.CurrentRow.Cells[0].Value.ToString() != txtMa.Text)
                    {
                        DataTable dataMa = connection.ExecuteQuery($"SELECT * FROM NoiSanXuat WHERE MaNSX = '{txtMa.Text}'");
                        if (dataMa.Rows.Count > 0)
                        {
                            MessageBox.Show("Mã đã tồn tại");
                            return;
                        }
                    }
                    if (MessageBox.Show("Bạn có muốn sửa không?", "Thông báo!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {

                        try
                        {
                            string ma = dgvDanhSach.CurrentRow.Cells[0].Value.ToString();
                            string querry = $"UPDATE NoiSanXuat SET MaNSX='{txtMa.Text}', TenNSX =N'{txtTen.Text}' WHERE MaNSX ='{ma}'";
                            connection.ExecuteNonQuery(querry);
                            LoadData();
                            MessageBox.Show("Sửa thành công");
                        }
                        catch
                        {
                            MessageBox.Show("Sửa không thành công");
                        }
                    }
                }
            }
        }

        private void btnBoQua_Click(object sender, EventArgs e)
        {
            foreach (Control ctr in grbThongTin.Controls)
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

        private void dgvDanhSach_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDanhSach.Rows.Count <= 0) return;

            foreach (Control ctr in grbThongTin.Controls)
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

            txtMa.Text = dgvDanhSach.CurrentRow.Cells[0].Value.ToString();
            txtTen.Text = dgvDanhSach.CurrentRow.Cells[1].Value.ToString();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            DataProvider.Instance.ExportFileExcel(dgvDanhSach, "Danh sách nơi sản xuất");
        }
    }
}
