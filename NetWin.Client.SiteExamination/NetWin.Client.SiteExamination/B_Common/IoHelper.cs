using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NetWin.Client.SiteExamination.B_Common
{
    public class IoHelper
    {
        /// <summary>
        /// 转化为字节
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] StreamToBytes(Stream stream)
        {
            List<byte> bytes = new List<byte>();
            int temp = stream.ReadByte();
            while (temp != -1)
            {
                bytes.Add((byte)temp);
                temp = stream.ReadByte();
            }

            return bytes.ToArray();
        }

        /// <summary>
        /// 转化为流
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }
    }
}
