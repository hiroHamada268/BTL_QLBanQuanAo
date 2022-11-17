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
    public partial class Form_ReportDSSPKhongBanDuoc : Form
    {
        string thang, nam;

        public Form_ReportDSSPKhongBanDuoc()
        {
            InitializeComponent();
        }

        public string Thang { get => thang; set => thang = value; }
        public string Nam { get => nam; set => nam = value; }

        private void Form_ReportDSSPKhongBanDuoc_Load(object sender, EventArgs e)
        {
            reportViewer1.LocalReport.ReportEmbeddedResource = @"BTL_QLCuaHangBanQuanAo.ReportDSSPKhongBanDuoc.rdlc";
            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DataSetDSSPKBanDuoc";
            reportDataSource.Value = DataProvider.Instance.ExecuteQuery($"select * from dbo.DSSPKhongBanDuoc({thang}, {nam})");
            reportViewer1.LocalReport.DataSources.Add(reportDataSource);
            this.reportViewer1.RefreshReport();
            this.reportViewer1.RefreshReport();
        }
    }
}
