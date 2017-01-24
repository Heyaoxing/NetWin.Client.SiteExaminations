using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using NetWin.Client.SiteExamination.A_Core.Model;
using NetWin.Client.SiteExamination.B_Common;
using NetWin.Client.SiteExamination.D_Data.Dto;
using NetWin.Client.SiteExamination.E_Services;

namespace NetWin.Client.TestTools
{
    //申名托管类型，对com是可见的
    [System.Runtime.InteropServices.ComVisible(true)]
    public partial class Tool : Form
    {
        ExaminationExecuteService examination = new ExaminationExecuteService();
        public Tool()
        {
            InitializeComponent();
            AddRegistry();
            ToolWb.ObjectForScripting = this;
            //修改webbrowser的属性使c#可以调用js方法： 
            ToolWb.ObjectForScripting = this;
            ToolWb.Navigate(ConfigurationManager.AppSettings["ExaminationWeb_Url"]);

            NetWin.Client.SiteExamination.A_Core.Config.SysConfig.LinkAmountLimit = Int32.Parse(ConfigurationManager.AppSettings["LinkAmountLimit"] ?? "50");
        }

        private void Tool_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        /// <summary>
        /// 检查注册表有没有指定ie版本,没有的话指定为ie 9
        /// </summary>
        private void AddRegistry()
        {
            try
            {
                string portName = RegistryHelper.GetRegistryData(Registry.LocalMachine, @"SOFTWARE\Microsoft\Internet Explorer\MAIN\FeatureControl\FEATURE_BROWSER_EMULATION", "NetWin.Client.TestTools.exe");
                if (string.IsNullOrWhiteSpace(portName))
                {
                    RegistryHelper.SetRegistryData(Registry.LocalMachine, @"SOFTWARE\Microsoft\Internet Explorer\MAIN\FeatureControl\FEATURE_BROWSER_EMULATION", "NetWin.Client.TestTools.exe", "9999");
                }
                LogHelper.Info("注册表:" + RegistryHelper.GetRegistryData(Registry.LocalMachine, @"SOFTWARE\Microsoft\Internet Explorer\MAIN\FeatureControl\FEATURE_BROWSER_EMULATION", "NetWin.Client.TestTools.exe"));
            }
            catch (Exception exception)
            {
                LogHelper.Error("检查注册表异常:" + exception.Message);
            }
        }


        #region 调用体检系统接口

        /// <summary>
        /// 体检资源登录检查
        /// 不通过初始检查的,则不往下执行
        /// </summary>
        /// <param name="siteUrl"></param>
        /// <param name="isReplace"></param>
        /// <returns></returns>
        public string CheckSite(string siteUrl, bool isReplace)
        {
            var result = examination.CheckSite(siteUrl, isReplace);
            var json = TextHelper.ObjectToJson(result);
            return json;
        }

        /// <summary>
        /// 查询需要体检的体检项
        /// </summary>
        /// <returns></returns>
        public string GetHistories(string siteUrl)
        {
            var result = ExaminationQueryService.GetHistories(siteUrl);
            var json = TextHelper.ObjectToJson(result);
            return json;
        }

        /// <summary>
        /// 获取资源检查记录
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public string GetSiteExaminationInfo(int siteId)
        {
            var result = ExaminationQueryService.GetSiteExaminationInfo(siteId);
            var json = TextHelper.ObjectToJson(result);
            return json;
        }

        /// <summary>
        /// 查询需要体检的体检项
        /// </summary>
        /// <returns></returns>
        public string GetExaminationItem()
        {
            var result = ExaminationQueryService.GetExaminationItem();
            var json = TextHelper.ObjectToJson(result);
            return json;
        }


        /// <summary>
        /// 查询体检结果
        /// </summary>
        /// <param name="detailId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public string GetDetailResult(int siteId, int detailId)
        {
            var result = ExaminationQueryService.GetDetailResult(siteId, detailId);
            var json = TextHelper.ObjectToJson(result);
            return json;
        }

        /// <summary>
        /// 启动体检
        /// </summary>
        /// <param name="siteUrl"></param>
        /// <returns></returns>
        public string Start(string siteUrl)
        {
            long userId = 10000; //TODO: 犀牛云帐号
            var result = examination.Start(userId, siteUrl);
            var json = TextHelper.ObjectToJson(result);
            return json;
        }

        /// <summary>
        /// 取消体检
        /// </summary>
        /// <returns></returns>
        public string Stop()
        {
            var result = examination.Stop();
            var json = TextHelper.ObjectToJson(result);
            return json;
        }

        /// <summary>
        /// 导出文件
        /// </summary>
        public void ExportFile(int siteId)
        {
            try
            {
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Guid.NewGuid() + ".doc");
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "word文件 (*.doc)|*.docx";
                saveFileDialog.FileName = "网站体检报告"+DateTime.Now.ToString("yyyyMMddHHmmss")+".doc";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ExaminationQueryService.GetExaminationReport(siteId, filePath);
                    string fileName = saveFileDialog.FileName;
                    File.Move(filePath, fileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("导出失败:" + ex.Message);
            }
        }

        #endregion
    }
}
