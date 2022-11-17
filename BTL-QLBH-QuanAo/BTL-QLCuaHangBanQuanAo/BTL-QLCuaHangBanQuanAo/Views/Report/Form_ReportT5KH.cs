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
    public partial class Form_ReportT5KH : Form
    {
        string nam;

        public Form_ReportT5KH()
        {
            InitializeComponent();
        }

        public string Nam { get => nam; set => nam = value; }

        private void Form_ReportT5KH_Load(object sender, EventArgs e)
        {
            reportViewer1.LocalReport.ReportEmbeddedResource = @"BTL_QLCuaHangBanQuanAo.ReportT5KH.rdlc";
            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DataSetT5KH";
            reportDataSource.Value = DataProvider.Instance.ExecuteQuery($"select * from dbo.Top5KH({nam})");
            reportViewer1.LocalReport.DataSources.Add(reportDataSource);
            this.reportViewer1.RefreshReport();
        }
    }
}
