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
using BTL_QLCuaHangBanQuanAo.Model.Database;

namespace BTL_QLCuaHangBanQuanAo.Views.Report
{
    public partial class Form_ReportHDB : Form
    {
        string thang, nam;

        public Form_ReportHDB()
        {
            InitializeComponent();
        }

        public string Thang { get => thang; set => thang = value; }
        public string Nam { get => nam; set => nam = value; }

        private void Form_ReportHDB_Load(object sender, EventArgs e)
        {
            reportViewer1.LocalReport.ReportEmbeddedResource = @"BTL_QLCuaHangBanQuanAo.ReportHDB.rdlc";
            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DataSetHDB";
            reportDataSource.Value = DataProvider.Instance.ExecuteQuery($"select * from dbo.DSHDB({thang}, {nam})");
            reportViewer1.LocalReport.DataSources.Add(reportDataSource);
            this.reportViewer1.RefreshReport();
        }
    }
}
