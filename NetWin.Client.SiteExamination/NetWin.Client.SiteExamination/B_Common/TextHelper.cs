using System;
using System.Collections.Generic;
using System.IO;

using System.Text;

namespace NetWin.Client.SiteExamination.B_Common
{
    public class TextHelper
    {
        #region 得到字符串长度，一个汉字长度为2
        /// <summary>
        /// 得到字符串长度，一个汉字长度为2
        /// </summary>
        /// <param name="inputString">参数字符串</param>
        /// <returns></returns>
        public static int StrLength(string inputString)
        {
            System.Text.ASCIIEncoding ascii = new System.Text.ASCIIEncoding();
            int tempLen = 0;
            byte[] s = ascii.GetBytes(inputString);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                    tempLen += 2;
                else
                    tempLen += 1;
            }
            return tempLen;
        }
        #endregion

        #region 截取指定长度字符串
        /// <summary>
        /// 截取指定长度字符串
        /// </summary>
        /// <param name="inputString">要处理的字符串</param>
        /// <param name="len">指定长度</param>
        /// <returns>返回处理后的字符串</returns>
        public static string ClipString(string inputString, int len)
        {
            bool isShowFix = false;
            if (len % 2 == 1)
            {
                isShowFix = true;
                len--;
            }
            System.Text.ASCIIEncoding ascii = new System.Text.ASCIIEncoding();
            int tempLen = 0;
            string tempString = "";
            byte[] s = ascii.GetBytes(inputString);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                    tempLen += 2;
                else
                    tempLen += 1;

                try
                {
                    tempString += inputString.Substring(i, 1);
                }
                catch
                {
                    break;
                }

                if (tempLen > len)
                    break;
            }

            byte[] mybyte = System.Text.Encoding.Default.GetBytes(inputString);
            if (isShowFix && mybyte.Length > len)
                tempString += "…";
            return tempString;
        }
        #endregion

        /// <summary>
        /// 检查字符串是否存在与一个,组合到一起的字符串数组中
        /// </summary>
        /// <param name="strSplit">未分割的字符串</param>
        /// <param name="split">分割符号</param>
        /// <param name="targetValue">目标字符串</param>
        /// <returns></returns>
        public static bool CheckStringHasValue(string strSplit, char split, string targetValue)
        {
            string[] strList = strSplit.Split(split);
            foreach (string str in strList)
            {
                if (targetValue == str)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 截取从开头到结尾之间的字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="startStr"></param>
        /// <param name="endStr"></param>
        /// <returns></returns>
        public static string Truncation(string str, string startStr, string endStr)
        {
            try
            {
                int startIndex = str.IndexOf(startStr, StringComparison.OrdinalIgnoreCase) + startStr.Length;
                int endIndex = str.IndexOf(endStr, StringComparison.OrdinalIgnoreCase);
                return str.Substring(startIndex, endIndex - startIndex);
            }
            catch
            {
                // ignored
            }
            return string.Empty;
        }

      
        /// <summary>
        /// 查找字符数组在另一个字符数组中匹配的位置
        /// </summary>
        /// <param name="source">源字符数组</param>
        /// <param name="match">匹配字符数组</param>
        /// <returns>匹配的位置，未找到匹配则返回-1</returns>
        private static int IndexOf(char[] source, char[] match)
        {
            int idx = -1;
            for (int i = 0; i < source.Length - match.Length; i++)
            {
                if (source[i] == match[0])
                {
                    bool isMatch = true;
                    for (int j = 0; j < match.Length; j++)
                    {
                        if (source[i + j] != match[j])
                        {
                            isMatch = false;
                            break;
                        }
                    }
                    if (isMatch)
                    {
                        idx = i;
                        break;
                    }
                }
            }
            return idx;
        }

        /// <summary>
        /// 截取从开头到指定字符之间的字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="endStr"></param>
        /// <returns></returns>
        public static string Truncation(string str, string endStr)
        {
            try
            {
                int endIndex = str.IndexOf(endStr, StringComparison.OrdinalIgnoreCase);
                return str.Substring(0, endIndex);
            }
            catch
            {
                // ignored
            }
            return string.Empty;
        }



        /// <summary>
        /// 字符串每隔多少位数插入置顶字符
        /// </summary>
        /// <param name="str">需要处理的字符</param>
        /// <param name="middle">指定插入的字符</param>
        /// <param name="gap">间隔数</param>
        /// <returns></returns>
        public static string InsertString(string str, string middle, int gap)
        {
            if (string.IsNullOrEmpty(str))
                return str;
            StringBuilder sb = new StringBuilder();
            var array = str.ToCharArray();
            for (int i = 0; i < array.Length; i++)
            {
                sb.Append(array[i]);
                if (i % gap == 0 && i != 0)
                    sb.Append(middle);
            }
            return sb.ToString();
        }


    }
}
