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
    public partial class Form_ReportNCC : Form
    {
        string maNCC;
        public Form_ReportNCC()
        {
            InitializeComponent();
        }

        public string MaNCC { get => maNCC; set => maNCC = value; }

        private void Form_ReportNCC_Load(object sender, EventArgs e)
        {
            reportViewer1.LocalReport.ReportEmbeddedResource = @"BTL_QLCuaHangBanQuanAo.ReportNCC.rdlc";
            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DataSetNCC";
            reportDataSource.Value = DataProvider.Instance.ExecuteQuery($"select * from dbo.DSHDN(N'{MaNCC}')");
            reportViewer1.LocalReport.DataSources.Add(reportDataSource);
            this.reportViewer1.RefreshReport();
        }
    }
}
