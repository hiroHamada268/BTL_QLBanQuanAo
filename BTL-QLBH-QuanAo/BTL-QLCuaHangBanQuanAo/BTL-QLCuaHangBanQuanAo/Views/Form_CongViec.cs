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
    public partial class Form_CongViec : Form
    {
        DataProvider connection = new DataProvider();
        DataTable dataCongViec = new DataTable();
        public Form_CongViec()
        {
            InitializeComponent();
        }
        private void LoadData()
        {
            dataCongViec = connection.ExecuteQuery("SELECT * FROM CongViec");
            dgvCongViec.DataSource = dataCongViec;

        }
        private void Form_CongViec_Load(object sender, EventArgs e)
        {
            LoadData();
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

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            dgvCongViec.DataSource = connection.ExecuteQuery($"SELECT * FROM CongViec WHERE MaCV LIKE '%{txtTimKiem.Text}%' OR TenCV LIKE N'%{txtTimKiem.Text}%'");

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
            if (txtMaCV.Text.Trim()=="") 
            {
                txtMaCV.Focus();
                errorProvider1.SetError(txtMaCV,"Chưa nhập mã");
                return false;
            }
            if (txtTenCV.Text.Trim()=="")
            {
                txtTenCV.Focus();
                errorProvider1.SetError(txtTenCV, "Chưa nhập tên");
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
                    DataTable dataMaCV = connection.ExecuteQuery($"SELECT * FROM CongViec WHERE MaCV = '{txtMaCV.Text}'");
                    if (dataMaCV.Rows.Count > 0)
                    {
                        MessageBox.Show("Mã CV đã tồn tại");
                        return;
                    }
                    if (MessageBox.Show("Bạn có muốn thêm công việc này không?", "Thông báo!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        try
                        {
                            string querry = $"INSERT INTO CongViec VALUES('{txtMaCV.Text}', N'{txtTenCV.Text}')";
                            connection.ExecuteNonQuery(querry);
                            LoadData();
                            MessageBox.Show("Thêm công việc thành công");
                        }
                        catch
                        {
                            MessageBox.Show("Thêm công việc không thành công");
                        }
                    }
                }
            }
            if (btnXoa.Enabled == true)
            {
                if (MessageBox.Show("Bạn có muốn xoá công việc này không?", "Thông báo!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (dgvCongViec.SelectedRows.Count < 0)
                    {
                        return;
                    }
                    string macv = dgvCongViec.CurrentRow.Cells[0].Value.ToString();

                    try
                    {
                        string querry = $"DELETE FROM CongViec WHERE MaCV = '{macv}'";
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
                    if (dgvCongViec.CurrentRow.Cells[0].Value.ToString() != txtMaCV.Text)
                    {
                        DataTable dataMaCV = connection.ExecuteQuery($"SELECT * FROM CongViec WHERE MaCV = '{txtMaCV.Text}'");
                        if (dataMaCV.Rows.Count > 0)
                        {
                            MessageBox.Show("Mã CV đã tồn tại");
                            return;
                        }
                    }
                    if (MessageBox.Show("Bạn có muốn sửa công việc này không?", "Thông báo!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {

                        try
                        {
                            string macv = dgvCongViec.CurrentRow.Cells[0].Value.ToString();
                            string querry = $"UPDATE CongViec SET MaCV='{txtMaCV.Text}', TenCV =N'{txtTenCV.Text}' WHERE MaCV ='{macv}'";
                            connection.ExecuteNonQuery(querry);
                            LoadData();
                            MessageBox.Show("Sửa công việc thành công");
                        }
                        catch
                        {
                            MessageBox.Show("Sửa công việc không thành công");
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

        private void dgvCongViec_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCongViec.Rows.Count <= 0) return;

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

            txtMaCV.Text = dgvCongViec.CurrentRow.Cells[0].Value.ToString();
            txtTenCV.Text = dgvCongViec.CurrentRow.Cells[1].Value.ToString();
           
        }
    }
}
