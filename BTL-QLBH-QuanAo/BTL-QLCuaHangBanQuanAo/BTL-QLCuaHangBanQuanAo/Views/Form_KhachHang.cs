using BTL_QLCuaHangBanQuanAo.Model.Class;
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
    public partial class Form_KhachHang : Form
    {
        string maKH;
        public Form_KhachHang()
        {
            InitializeComponent();
        }

        public void clear()
        {
            txbMaKhachHang.Text = "";
            txbTenKhachHang.Text = "";
            txbDiaChi.Text = "";
            txbDienThoai.Text = "";
        }

        public void EnableTextBox(Boolean enable)
        {
            txbMaKhachHang.Enabled = enable;
            txbTenKhachHang.Enabled = enable;
            txbDiaChi.Enabled = enable;
            txbDienThoai.Enabled = enable;
        }

        private void Form_KhachHang_Load(object sender, EventArgs e)
        {
            string query = @"select * from KhachHang";

            DataTable dt = DataProvider.Instance.ExecuteQuery(query);
            dgvShowKhachHang.DataSource = dt;

            EnableTextBox(false);
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            EnableTextBox(true);
            txbMaKhachHang.Focus();
        }

        private void dgvShowKhachHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvShowKhachHang.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvShowKhachHang.Rows[e.RowIndex];
                maKH = row.Cells["MaKhach"].Value.ToString();
                txbMaKhachHang.Text = row.Cells["MaKhach"].Value.ToString();
                txbTenKhachHang.Text = row.Cells["TenKhach"].Value.ToString();
                txbDiaChi.Text = row.Cells["DiaChi"].Value.ToString();
                txbDienThoai.Text = row.Cells["DienThoai"].Value.ToString();
            }

            EnableTextBox(true);
            txbMaKhachHang.Enabled = false;

        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            string maKhachHang = txbMaKhachHang.Text;
            string tenKhachHang = txbTenKhachHang.Text;
            string diaChi = txbDiaChi.Text;
            string soDienThoai = txbDienThoai.Text;
            Valid valid = new Valid();

            if (maKhachHang == "" || tenKhachHang == "" || diaChi == "" || soDienThoai == "")
            {
                MessageBox.Show("Bạn cần nhập đầy đủ thông tin!");
            }
            else
            {
                try
                {
                    if (valid.IsPhone(soDienThoai))
                    {
                        string query = $@"
                            update KhachHang set 
                                MaKhach = N'{maKhachHang}',
                                TenKhach = N'{tenKhachHang}',
                                DiaChi = N'{diaChi}',
                                DienThoai = N'{soDienThoai}'
                            where MaKhach = N'{maKH}'";
                        DataProvider.Instance.ExecuteNonQuery(query);
                        Form_KhachHang_Load(sender, e);
                        MessageBox.Show("Sửa đổi thành công!");
                    }
                    else
                    {
                        MessageBox.Show("Điện thoại không hợp lệ");
                        txbDienThoai.Focus();
                    }
                } catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string maKhachHang = txbMaKhachHang.Text;
            string tenKhachHang = txbTenKhachHang.Text;
            string diaChi = txbDiaChi.Text;
            string soDienThoai = txbDienThoai.Text;
            Valid valid = new Valid();


            if (maKhachHang == "" || tenKhachHang == "" || diaChi == "" || soDienThoai == "")
            {
                MessageBox.Show("Bạn cần nhập đầy đủ thông tin!");
            }
            else
            {
                try
                {
                    string queryCheckMaKhach = $"select * from KhachHang where MaKhach = N'{maKhachHang}'";
                    DataTable dt = DataProvider.Instance.ExecuteQuery(queryCheckMaKhach);

                    if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show("Mã khách đã tồn tại");
                    }
                    else
                    {
                        if (valid.IsPhone(soDienThoai))
                        {
                            string query = $@"
                                INSERT INTO KhachHang 
                                VALUES (
                                    N'{maKhachHang}', 
                                    N'{tenKhachHang}', 
                                    N'{diaChi}', 
                                    N'{soDienThoai}')";
                            DataProvider.Instance.ExecuteNonQuery(query);
                            Form_KhachHang_Load(sender, e);
                            MessageBox.Show("Thêm thành công!");
                        }
                        else
                        {
                            MessageBox.Show("Điện thoại không hợp lệ");
                            txbDienThoai.Focus();
                        }
                    }
                } catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Ban co chac muon xoa?", "Thong bao", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string query = $"delete from KhachHang where MaKhach = N'{maKH}'"; 
                DataProvider.Instance.ExecuteNonQuery(query);
                Form_KhachHang_Load(sender, e);
                MessageBox.Show("Xóa thành công!");
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string textSearch = txbSearch.Text;

            if(textSearch.Trim() == "")
            {
                Form_KhachHang_Load(sender, e);
            } else
            {
                string query = $"select * from KhachHang where TenKhach like N'%{textSearch}%' or MaKhach = N'{textSearch}'";
                DataTable dt = DataProvider.Instance.ExecuteQuery(query);

                dgvShowKhachHang.DataSource = dt;
            }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            DataProvider.Instance.ExportFileExcel(dgvShowKhachHang, "Danh sách khách hàng");
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            clear();
        }
    }
}
