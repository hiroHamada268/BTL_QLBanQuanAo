using BTL_QLCuaHangBanQuanAo.Model.Class;
using BTL_QLCuaHangBanQuanAo.Model.Database;
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
    public partial class Form_DetailSP : Form
    {
        public Form_DetailSP()
        {
            InitializeComponent();
        }

        private void Form_DetailSP_Load(object sender, EventArgs e)
        {
            if(StaticData.dataRowSp != null)
            {
                //pictureBox1.Image = new Bitmap(StaticData.dataRowSp["Anh"].ToString());

                //string path = @"asset\ImgBTL" + $@"\{StaticData.dataRowSp["Anh"]}";
                //pictureBox1.Image = Image.FromFile(
                //  Path.Combine(
                //     Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                //     path));

                if ((StaticData.dataRowSp[12]) != null)
                {
                    try
                    {
                        byte[] bytes = (byte[])(StaticData.dataRowSp[12]);
                        MemoryStream ms = new MemoryStream(bytes);
                        pictureBox1.Image = Image.FromStream(ms);
                        pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                    catch
                    {

                    }
                }


                if (int.Parse(StaticData.dataRowSp["SoLuong"].ToString()) > 0)
                {
                    lblTrangThai.Text = "Còn Hàng";
                }
                else
                {
                    lblTrangThai.Text = "Hết Hàng";
                } 

                lblTieuDe.Text = StaticData.dataRowSp["TenQuanAo"].ToString();
                lblSoLuong.Text = StaticData.dataRowSp["SoLuong"].ToString();

                string queryCo = $"select * from SanPham sp join Co c on sp.MaCo = c.MaCo where c.MaCo = N'{StaticData.dataRowSp["MaCo"]}'";
                DataTable dtCo = DataProvider.Instance.ExecuteQuery(queryCo);
                if (dtCo != null)
                {
                    lblMaCo.Text = dtCo.Rows[0]["TenCo"].ToString();
                }
                lblCo.Text = StaticData.dataRowSp["MaCo"].ToString();

                lblDonGiaBan.Text = StaticData.dataRowSp["DonGiaBan"].ToString();

                string query = $"select * from SanPham sp join ChatLieu cl on sp.MaChatLieu = cl.MaChatLieu where cl.MaChatLieu = N'{StaticData.dataRowSp["MaChatLieu"]}'";
                DataTable dt = DataProvider.Instance.ExecuteQuery(query);
                if(dt != null)
                {
                    lblChatLieu.Text = dt.Rows[0]["TenChatLieu"].ToString();
                }
                lblDonGiaBan.Text = StaticData.dataRowSp["DonGiaBan"].ToString();
            }
        }
    }
}
