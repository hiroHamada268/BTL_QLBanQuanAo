using BTL_QLCuaHangBanQuanAo.Model.Database;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTL_QLCuaHangBanQuanAo.Views.Report
{
    public partial class Form_ReportThanhToan : Form
    {
        string maUser;
        string sdtKH; string tenKH; string tienTraKH;

        public Form_ReportThanhToan(string sdtKH, string tenKH, string tienTraKH)
        {
            InitializeComponent();
            this.sdtKH = sdtKH;
            this.tenKH = tenKH;
            this.tienTraKH = tienTraKH;
        }

        public string MaUser { get => maUser; set => maUser = value; }

        private void Form_ReportThanhToan_Load(object sender, EventArgs e)
        {
            reportViewer1.LocalReport.ReportEmbeddedResource = @"BTL_QLCuaHangBanQuanAo.ReportThanhToan.rdlc";
            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DataSetThanhToan";
            reportDataSource.Value = DataProvider.Instance.ExecuteQuery($"select * from dbo.ThanhToan(N'{maUser}')");
            reportViewer1.LocalReport.DataSources.Add(reportDataSource);

            foreach(Control control in reportViewer1.Controls[0].Controls[0].Controls[2].Controls)
            {
                //reportViewer1.Controls[0].Controls.Add(control);
                //if(control.Name == "txbSDT")
                //{
                //    control.Text = sdtKH;
                //}
            }
            this.reportViewer1.RefreshReport();
        }
    }
}
