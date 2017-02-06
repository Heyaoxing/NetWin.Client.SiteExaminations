using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using NetWin.Client.SiteExamination.B_Common;
using NetWin.Client.SiteExamination.D_Data.Repository;
using NetWin.Client.SiteExamination.E_Services;

namespace NetWin.Client.UnitTest
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                StringBuilder SB = new StringBuilder();
                SB.Append("<table border=\"0.5\" width=\"100%\" >"); //边框可以定义浮点型 与HTML不同
                SB.Append("<tr>");
                SB.Append("<th style=\"width:15px;font-family:宋体;font-size:10pt;\" encoding=\"Identity-H\" rowspan=\"2\" bgcolor=\"rgb(255,153,204)\">发票</th>");
                SB.Append("<th colspan=\"6\" bgcolor=\"rgb(204,204,204)\" style=\"font-family:宋体;font-size:10pt;\" encoding=\"Identity-H\" >发票交接</th>");
                SB.Append("<th style=\"background-color: rgb(204,204,204);font-family:宋体;font-size:10pt;\" encoding=\"Identity-H\" bgcolor=\"rgb(204,204,204)\">备注</th>");
                SB.Append("</tr><tr>");
                SB.Append("<td colspan=\"6\" style=\"height:40px;font-family:宋体;font-size:10pt;\" encoding=\"Identity-H\" >&nbsp;&nbsp;发票___________张</td>");
                SB.Append("<td>&nbsp;</td>");
                SB.Append("</tr><tr>");
                SB.Append("<td style=\"font-family:宋体;font-size:10pt;\" colspan=\"2\" bgcolor=\"rgb(204,255,204)\" encoding=\"Identity-H\">配送站</td>");
                SB.Append("<td style=\"height: 40px;font-family:宋体;font-size:10pt;\" colspan=\"6\" encoding=\"Identity-H\">&nbsp;&nbsp;交接人:____________<span style=\"margin-left: 200;\">日期:_______________</span></td>");
                SB.Append("</tr><tr>");
                SB.Append("<td style=\"font-family:宋体;font-size:10pt;\" colspan=\"2\" bgcolor=\"rgb(204,255,204)\" encoding=\"Identity-H\" >小车司机</td>");
                SB.Append("<td style=\"height: 40px;font-family:宋体;font-size:10pt;\" colspan=\"6\" encoding=\"Identity-H\" ><img src=\"http://avatar.csdn.net/8/2/8/1_rocket5725.jpg\" height=\"150\" /></td>");
                SB.Append("</tr>");
                SB.Append("</table>");


                Document document = new Document();
                HTMLWorker HtmlW = new HTMLWorker(document);  // _Doc 是iTextSharp.text.Document对象
                PdfWriter.GetInstance(document, new FileStream("Chap0d101.pdf", FileMode.Create));
                document.Open();
                HtmlW.Parse(new StringReader(SB.ToString()));
                document.Add(HtmlW.CreateParagraph());
                document.Close();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            Console.WriteLine("结束");
            Console.Read();
        }
    }
}
