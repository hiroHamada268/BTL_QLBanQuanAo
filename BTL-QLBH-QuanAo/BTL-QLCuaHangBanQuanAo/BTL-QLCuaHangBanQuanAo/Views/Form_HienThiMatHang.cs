using BTL_QLCuaHangBanQuanAo.Model.Class;
using BTL_QLCuaHangBanQuanAo.Model.Database;
using BTL_QLCuaHangBanQuanAo.Views.Report;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTL_QLCuaHangBanQuanAo.Views
{
    public partial class Form_HienThiMatHang : Form
    {
        public Form_HienThiMatHang()
        {
            InitializeComponent();
        }

        private void Form_HienThiMatHang_Load(object sender, EventArgs e)
        {
            btnLoad_Click(sender, e);

            showMatHangDaMua();

            showPrice();

            showLoaiSp();

            showNhaCungCap();
        }

        private void showPrice()
        {
            DataTable dt = DataProvider.Instance.ExecuteQuery($"select * from NguoiDung");
            string query = $@"
                select sum(mh.SoLuong * sp.DonGiaBan) Price from MuaHang mh
                join SanPham sp on mh.MaQuanAo = sp.maQuanAo
                where mh.MaUser = N'{dt.Rows[0]["MaUser"]}'
            ";

            object price = DataProvider.Instance.ExecuteScalar(query);
            lblPrice.Text = price.ToString();
        }

        private void showProduct(DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dataRow = dt.Rows[i];
                Panel panel_FL = new Panel();
                panel_FL.Size = new Size(230, 230);

                PictureBox pb = new PictureBox();
                pb.Size = new Size(230, 160);

                // Xử lí đường dẫn tương đối

                // === pb.Image = new Bitmap(dataRow[3].ToString()); ===
                //string path = @"asset\ImgBTL" + $@"\{dataRow["image"]}";
                //pb.Image = Image.FromFile(
                //  Path.Combine(
                //     Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                //     path));
                //pb.SizeMode = PictureBoxSizeMode.StretchImage;
                if ((dt.Rows[i][12])!=null)
                {
                    try
                    {
                        byte[] bytes = (byte[])(dt.Rows[i][12]);
                        MemoryStream ms = new MemoryStream(bytes);
                        pb.Image = Image.FromStream(ms);
                        pb.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                    catch
                    {

                    }
                }





                Label lbl = new Label();
                if (dataRow[1].ToString().Length > 35)
                {
                    lbl.Text = dataRow[1].ToString().Substring(0, 20) + "...";
                }
                else
                {
                    lbl.Text = dataRow[1].ToString();
                }
                lbl.Location = new System.Drawing.Point(10, 180);
                lbl.AutoSize = true;
                lbl.Font = new Font("Calibri", 10);

                pb.MouseClick += new MouseEventHandler((o, a) =>
                {
                    StaticData.dataRowSp = dataRow;
                    Form_DetailSP dp = new Form_DetailSP();
                    dp.ShowDialog();
                });

                Button btnMua = new Button();
                btnMua.Text = "Mua";
                btnMua.Location = new Point(0, 200);

                btnMua.MouseClick += new MouseEventHandler((o, a) =>
                {
                    StaticData.dataRowSpMua = dataRow;
                    try
                    {
                        // Tim xem sp da duoc mua hay chua neu roi thi tang them so luong khi ma bam mu lai
                        string queryCheck = $"select * from MuaHang where MaUser = N'{StaticData.datatable.Rows[0]["MaUser"]}' and MaQuanAo = N'{dataRow["MaQuanAo"]}'";
                        DataTable dtMuaHang = DataProvider.Instance.ExecuteQuery(queryCheck);
                        if (dtMuaHang.Rows.Count > 0)
                        {
                            string queryUpdateSoLuong = $"update MuaHang set SoLuong = '{int.Parse(dtMuaHang.Rows[0]["SoLuong"].ToString()) + 1}' where MaUser = N'{StaticData.datatable.Rows[0]["MaUser"]}' and MaQuanAo = N'{dataRow["MaQuanAo"]}' ";
                            DataProvider.Instance.ExecuteNonQuery(queryUpdateSoLuong);
                            showMatHangDaMua();
                            showPrice();

                            MessageBox.Show("Bạn đã mua hàng thành công");
                        }
                        else
                        {
                            string sinhMaMua = $"select dbo.SinhMaMuaHang()";
                            object maSinh = DataProvider.Instance.ExecuteScalar(sinhMaMua);
                            string query = $@"
                                INSERT INTO MuaHang
                                VALUES(N'{maSinh}', N'{StaticData.datatable.Rows[0]["MaUser"]}', N'{dataRow["MaQuanAo"]}', '1', GetDate())";
                            DataProvider.Instance.ExecuteNonQuery(query);

                            showMatHangDaMua();
                            showPrice();
                            showPrice();
                            MessageBox.Show("Bạn đã mua hàng thành công");
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                });

                panel_FL.Controls.Add(pb);
                panel_FL.Controls.Add(lbl);
                panel_FL.Controls.Add(btnMua);
                panel_FL.BorderStyle = BorderStyle.FixedSingle;

                flowLayoutPanel1.Controls.Add(panel_FL);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            if (flowLayoutPanel1.Controls.Count == 0)
            {
                string query = "select * from SanPham";
                System.Data.DataTable dt = DataProvider.Instance.ExecuteQuery(query);
                showProduct(dt);
            }
        }

        private void showMatHangDaMua()
        {
            try
            {
                string query = $@"
                    select mh.MaMuaHang, sp.MaQuanAo MaSp, NguoiDung.TenDangNhap, sp.TenQuanAo, mh.SoLuong, mh.NgayMua  from MuaHang mh
                    join SanPham sp on mh.MaQuanAo = sp.MaQuanAo
                    join NguoiDung on mh.MaUser = NguoiDung.MaUser
                    where NguoiDung.MaUser = N'{StaticData.datatable.Rows[0]["MaUser"]}'";
                System.Data.DataTable dt = DataProvider.Instance.ExecuteQuery(query);
                dgvDsSpMua.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnApDung_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            string query;
            if (flowLayoutPanel1.Controls.Count == 0)
            {
                string search = txbSearch.Text;
                query = $@"
                    select * from SanPham sp 
                    join TheLoai tl on sp.MaLoai = tl.MaLoai
                    where sp.TenQuanAo like N'%{search}%' or sp.MaQuanAo = N'{search}'
                ";

                DataTable dt = DataProvider.Instance.ExecuteQuery(query);
                showProduct(dt);
            }
        }

        private void dgvDsSpMua_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDsSpMua.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvDsSpMua.Rows[e.RowIndex];
                string maSp = row.Cells["MaSp"].Value.ToString();
                txbMaSp.Text = maSp;

                if (maSp == null)
                {
                    MessageBox.Show("Bạn cần chọn một hàng để xem thông tin!");
                }
                else
                {
                    string query = $"select * from SanPham where maQuanAo = N'{maSp}'";
                    DataTable dt = DataProvider.Instance.ExecuteQuery(query);
                    StaticData.dataRowSp = dt.Rows[0];
                    Form_DetailSP dp = new Form_DetailSP();
                    dp.ShowDialog();
                }
            }
        }

        private void btnXoaSp_Click(object sender, EventArgs e)
        {
            if (txbMaSp.Text.Trim() == "")
            {
                MessageBox.Show("Bạn cần chọn một sản phẩm để xóa!");
            }
            else if (MessageBox.Show("Bạn có chắc muốn xóa sản phẩm!", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string query = $"delete from MuaHang where maQuanAo = N'{txbMaSp.Text}'";
                DataProvider.Instance.ExecuteNonQuery(query);
                txbMaSp.Text = "";

                showMatHangDaMua();
                showPrice();

            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            txbSoDienThoai.Text = "";
            txbTienKHTra.Text = "";
            txbTenKhachHang.Text = "";
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            Form_ReportThanhToan f = new Form_ReportThanhToan(txbSoDienThoai.Text, txbTenKhachHang.Text, txbTienKHTra.Text);
            f.MaUser = StaticData.datatable.Rows[0]["MaUser"].ToString();
            f.ShowDialog();
        }

        private void showLoaiSp()
        {
            string query = $"select * from TheLoai";
            DataTable dt = DataProvider.Instance.ExecuteQuery(query);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cbLoaiSP.Items.Add(dt.Rows[i]["TenLoai"]);
                
            }
        }

        private void showNhaCungCap()
        {
            string query = $"select * from NhaCungCap";
            DataTable dt = DataProvider.Instance.ExecuteQuery(query);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cbTenCC.Items.Add(dt.Rows[i]["TenNCC"]);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txbSearch.Text = "";
            cbLoaiSP.Text = "";
            cbTenCC.Text = "";
            string query = "select * from SanPham";
            System.Data.DataTable dt = DataProvider.Instance.ExecuteQuery(query);
            showProduct(dt);
        }

        private void cbLoaiSP_SelectedIndexChanged(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            if (cbLoaiSP.Text.Trim() != "")
            {
                txbSearch.Text = "";
                cbTenCC.Text = "";
                string query = $@"
                    select * from SanPham sp 
                    join TheLoai tl on sp.MaLoai = tl.MaLoai
                    where tl.TenLoai like N'%{cbLoaiSP.Text}%'
                ";

                DataTable dt = DataProvider.Instance.ExecuteQuery(query);
                showProduct(dt);
            }
        }

        private void cbTenCC_SelectedIndexChanged(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            if (cbTenCC.Text.Trim() != "")
            {
                txbSearch.Text = "";
                cbLoaiSP.Text = "";
                string query = $@"
                    select * from SanPham sp 
                    join TheLoai tl on sp.MaLoai = tl.MaLoai
                    join ChiTietHDN chdn on chdn.MaQuanAo = sp.MaQuanAo
                    join HoaDonNhap hdn on hdn.SoHDN = chdn.SoHDN
                    join NhaCungCap ncc on hdn.MaNCC = ncc.MaNCC
                    where ncc.TenNCC like N'%{cbTenCC.Text}%'
                ";

                DataTable dt = DataProvider.Instance.ExecuteQuery(query);
                showProduct(dt);
            }
        }

    }
}
