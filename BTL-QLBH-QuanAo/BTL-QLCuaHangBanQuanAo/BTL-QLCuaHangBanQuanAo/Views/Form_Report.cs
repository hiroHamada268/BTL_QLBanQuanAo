using BTL_QLCuaHangBanQuanAo.Model.Database;
using BTL_QLCuaHangBanQuanAo.Views.Report;
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
    public partial class Form_Report : Form
    {
        public Form_Report()
        {
            InitializeComponent();
        }

        private void btnReportHDB_Click(object sender, EventArgs e)
        {
            string thang = txbThang.Text;
            string nam = txbNam.Text;

            if(thang == "" || nam == "")
            {
                MessageBox.Show("Cần điền đầy đủ thông tin");

            } else
            {
                if(int.Parse(thang) <= 0 || int.Parse(thang) >12)
                {
                    MessageBox.Show("Tháng không tồn tại");
                } else
                {
                    Form_ReportHDB f1 = new Form_ReportHDB();
                    f1.Thang = thang;
                    f1.Nam = nam;
                    f1.ShowDialog();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string thang = txbThang1.Text;
            string nam = txbNam1.Text;

            if (thang == "" || nam == "")
            {
                MessageBox.Show("Cần điền đầy đủ thông tin");

            }
            else
            {
                if (int.Parse(thang) <= 0 || int.Parse(thang) > 12)
                {
                    MessageBox.Show("Tháng không tồn tại");
                }
                else
                {
                    Form_ReportDSSPKhongBanDuoc f1 = new Form_ReportDSSPKhongBanDuoc();
                    f1.Thang = thang;
                    f1.Nam = nam;
                    f1.ShowDialog();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string tenNhaCungCap = cbMaNhaCungCap.Text;
            if(tenNhaCungCap == "")
            {
                MessageBox.Show("Can nhap day du thong tin");
            } else
            {
                string query = $"select * from NhaCungCap where TenNCC like N'%{tenNhaCungCap}%'";
                DataTable dt = DataProvider.Instance.ExecuteQuery(query);
                Form_ReportNCC f2 = new Form_ReportNCC();
                f2.MaNCC = dt.Rows[0]["MaNCC"].ToString();
                f2.ShowDialog();
            }
        }

        private void cbMaNhaCungCap_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form_Report_Load(object sender, EventArgs e)
        {
            string query = $"select * from NhaCungCap";
            DataTable dt = DataProvider.Instance.ExecuteQuery(query);
            for(int i = 0; i < dt.Rows.Count; i++)
            {
                cbMaNhaCungCap.Items.Add(dt.Rows[i]["TenNCC"]);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string nam = textBoxNam.Text;

            if (nam == "")
            {
                MessageBox.Show("Cần điền đầy đủ thông tin");
            }
            else
            {
                Form_ReportT5KH f1 = new Form_ReportT5KH();
                f1.Nam = nam;
                f1.ShowDialog();
            }
        }
    }
}
