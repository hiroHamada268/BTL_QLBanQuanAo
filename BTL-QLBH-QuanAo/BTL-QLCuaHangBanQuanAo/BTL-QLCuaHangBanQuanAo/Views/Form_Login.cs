using BTL_QLCuaHangBanQuanAo.Model.Class;
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
    public partial class Form_Login : Form
    {
        public Form_Login()
        {
            InitializeComponent();
        }

        private void ClearTextBox()
        {
            txbEmail.Text = "";
            txbPassword.Text = "";
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txbEmail.Text;
            string password = txbPassword.Text;
            Valid val = new Valid();

            if(email == "" || password == "")
            {
                MessageBox.Show("Bạn cần nhập đầy đủ thông tin!");
            } else
            {
                if(val.IsValid(email))
                {
                    if(password.Length >= 6)
                    {
                        string query = $"select * from NguoiDung where Email = N'{email}' and Password = N'{password}'";
                        DataTable dt = DataProvider.Instance.ExecuteQuery(query);
                        StaticData.datatable = dt;
                        Form1 f1 = new Form1();
                        this.Hide();
                        MessageBox.Show("Đăng nhập thành công");
                        ClearTextBox();
                        f1.ShowDialog();
                        this.Show();
                    } else
                    {
                        MessageBox.Show("Password tối thiểu 6 kí tự");
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form_Login_Load(object sender, EventArgs e)
        {

        }
    }
}
