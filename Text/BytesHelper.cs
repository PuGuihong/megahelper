﻿#region Copyright © www.ef-automation.com 2015. All right reserved.

// ----------USER  : yu

#endregion Copyright © www.ef-automation.com 2015. All right reserved.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseUtils.Text
{
    public class BytesHelper
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BytesHelper()
        {
        }

        public static readonly Encoding GB2312 = Encoding.GetEncoding("GB2312");
        public static readonly Encoding ASCII = Encoding.ASCII;

        /// <summary>
        /// Bitmap 转化为 Byte[]
        /// </summary>
        /// <param name="BitReturn"></param>
        /// <returns></returns>
        public byte[] BitmapToByte(Bitmap BitReturn)
        {
            byte[] bReturn = null;
            MemoryStream ms = new MemoryStream();
            BitReturn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            bReturn = ms.GetBuffer();
            return bReturn;
        }

        public byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        //将  byte[] 转成 Stream

        public Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }

        #region 从byte转换成Boolean

        /// <summary>
        /// 从byte转换成Boolean,转换失败返回false。
        /// </summary>
        /// <param name="obj">对象。</param>
        /// <returns>Boolean值。</returns>
        public static bool ToBoolByByte(object obj)
        {
            try
            {
                string s = ToObjectString(obj).ToLower();
                return s == "1" || s == "true" ? true : false;
            }
            catch
            {
                return false;
            }
        }

        #region 数据转换obj为null时返回空值

        /// <summary>
        /// 返回对象obj的String值,obj为null时返回空值。
        /// </summary>
        /// <param name="obj">对象。</param>
        /// <returns>字符串。</returns>
        public static string ToObjectString(object obj)
        {
            return null == obj ? String.Empty : obj.ToString();
        }

        #endregion 数据转换obj为null时返回空值

        /// <summary>
        /// 从byte转换成Boolean。
        /// </summary>
        /// <param name="obj">对象。</param>
        /// <param name="returnValue">转换失败返回该值。</param>
        /// <returns>Boolean值。</returns>
        public static bool ToBoolByByte(object obj, bool returnValue)
        {
            try
            {
                string s = ToObjectString(obj).ToLower();
                return s == "1" || s == "true" ? true : false;
            }
            catch
            {
                return returnValue;
            }
        }

        #endregion 从byte转换成Boolean

        #region 半角转全角函数

        /// <summary>
        /// 半角转全角函数
        /// </summary>
        /// <param name="srcbuff"></param>
        /// <returns></returns>
        public static byte[] ToSBC(byte[] srcbuff)
        {
            List<byte> tmpbuff = new List<byte>();
            for (int i = 0; i < srcbuff.Length; )
            {
                if (srcbuff[i] == 0x20)
                {
                    tmpbuff.Add(0xA1);
                    tmpbuff.Add(0xA1);
                    i += 1;
                }
                else if (srcbuff[i] == 0x7E)
                {
                    tmpbuff.Add(0xA1);
                    tmpbuff.Add(0xAB);
                    i += 1;
                }
                else if (srcbuff[i] > 0x80)
                {
                    tmpbuff.Add(srcbuff[i]);
                    tmpbuff.Add(srcbuff[i + 1]);
                    i += 2;
                }
                else
                {
                    tmpbuff.Add(0xA3);
                    tmpbuff.Add((byte)(srcbuff[i] + 0x80));
                    i += 1;
                }
            }
            return tmpbuff.ToArray();
        }

        #endregion 半角转全角函数

        #region 从 byte[] 中截取子串

        /// <summary>
        /// 从 byte[] 中截取子串
        /// </summary>
        /// <param name="srcbuff"></param>
        /// <param name="start"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        internal static byte[] SubBuffer(byte[] srcbuff, int start, int len)
        {
            if (srcbuff.Length < start + len)
            {
                throw new ArgumentOutOfRangeException("SubBuffer");
            }
            byte[] retbuff = new byte[len];
            for (int i = 0; i < len; i++)
            {
                retbuff[i] = srcbuff[i + start];
            }
            return retbuff;
        }

        #endregion 从 byte[] 中截取子串

        #region 将 byte[] 顺序反转

        /// <summary>
        /// 将 byte[] 顺序反转
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] SwapBytes(byte[] bytes)
        {
            int l = bytes.Length;
            byte[] Newb = new byte[l];

            for (int i = 0; i < l; i++)
            {
                Newb[i] = bytes[l - i - 1];
            }
            return Newb;
        }

        #endregion 将 byte[] 顺序反转

        #region 获取 ushort 的高低位反转 byte[]

        /// <summary>
        /// 获取 ushort 的高低位反转 byte[]
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        internal static byte[] GetSwapBytes(ushort u)
        {
            return SwapBytes(BitConverter.GetBytes(u));
        }

        #endregion 获取 ushort 的高低位反转 byte[]

        #region 获取 int 的高低位反转 byte[]

        /// <summary>
        /// 获取 int 的高低位反转 byte[]
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        internal static byte[] GetSwapBytes(int i)
        {
            return SwapBytes(BitConverter.GetBytes(i));
        }

        #endregion 获取 int 的高低位反转 byte[]

        #region 转义特殊字符，即 '~'(0x7E)

        /// <summary>
        /// 转义特殊字符，即 '~'(0x7E)
        /// </summary>
        /// <param name="srcbuff"></param>
        /// <returns></returns>
        public static byte[] SpecCharConvert(byte[] srcbuff)
        {
            List<byte> tmpBuff = new List<byte>();
            foreach (byte b in srcbuff)
            {
                if (b == (byte)'~')
                {
                    // 转义可能出现的 ~
                    tmpBuff.Add(0x7D);// 之后转义 0x7D
                    tmpBuff.Add(0x5E);
                }
                else if (b == 0x7D)
                {
                    // 转义 0x7D
                    tmpBuff.Add(0x7D);
                    tmpBuff.Add(0x5D);
                }
                else
                {
                    tmpBuff.Add(b);
                }
            }
            return tmpBuff.ToArray();
        }

        #endregion 转义特殊字符，即 '~'(0x7E)

        #region 反转义特殊字符，即 '~'(0x7E) 0x7D0x5E->0x7E, 0x7D0x5D->0x7D

        /// <summary>
        /// 反转义特殊字符，即 '~'(0x7E)
        /// 0x7D0x5E->0x7E, 0x7D0x5D->0x7D
        /// </summary>
        /// <param name="srcbuff"></param>
        /// <returns></returns>
        public static byte[] SpecCharReverse(byte[] srcbuff)
        {
            List<byte> tmpBuff = new List<byte>();
            for (int i = 0; i < srcbuff.Length; )
            {
                if (srcbuff[i] == 0x7D)
                {
                    if (srcbuff[i + 1] == 0x5E)
                    {
                        tmpBuff.Add((byte)'~');
                    }
                    else if (srcbuff[i + 1] == 0x5D)
                    {
                        tmpBuff.Add(0x7D);
                    }
                    else
                    {
                        throw new ArgumentException("非法数据");
                    }
                    i += 2;
                }
                else
                {
                    tmpBuff.Add(srcbuff[i]);
                    i += 1;
                }
            }
            return tmpBuff.ToArray();
        }

        #endregion 反转义特殊字符，即 '~'(0x7E) 0x7D0x5E->0x7E, 0x7D0x5D->0x7D

        #region 在 srcbuff 中查找 subbuff 第一次出现的位置

        /// <summary>
        /// 在 srcbuff 中查找 subbuff 第一次出现的位置
        /// </summary>
        /// <param name="srcbuff"></param>
        /// <param name="subbuff"></param>
        /// <returns>找到则返回匹配开始的位置，没有找到返回 -1</returns>
        public static int BufferLookup(byte[] srcbuff, byte[] subbuff)
        {
            return BufferLookup(srcbuff, subbuff, 0);
        }

        #endregion 在 srcbuff 中查找 subbuff 第一次出现的位置

        #region 在 srcbuff 中查找 subbuff 第一次出现的位置

        /// <summary>
        /// 在 srcbuff 中查找 subbuff 第一次出现的位置
        /// </summary>
        /// <param name="srcbuff"></param>
        /// <param name="subbuff"></param>
        /// <param name="start">开始查找的位置</param>
        /// <returns>找到则返回匹配开始的位置，没有找到返回 -1</returns>
        public static int BufferLookup(byte[] srcbuff, byte[] subbuff, int start)
        {
            for (int i = start; i < srcbuff.Length - subbuff.Length + 1; i++)
            {
                for (int j = 0; j < subbuff.Length; j++)
                {
                    if (srcbuff[i + j] != subbuff[j])
                    {
                        break;
                    }
                    if (j == subbuff.Length - 1)
                    {
                        // 能运行到这里表明 subbuff 中的字节都已经被匹配过
                        return i;
                    }
                }
            }
            return -1;
        }

        #endregion 在 srcbuff 中查找 subbuff 第一次出现的位置

        #region 在 srcbuff 中查找 subchars 第一次出现的位置

        /// <summary>
        /// 在 srcbuff 中查找 subchars 第一次出现的位置
        /// </summary>
        /// <param name="srcbuff"></param>
        /// <param name="subchars">ASCII字符串</param>
        /// <returns>找到则返回匹配开始的位置，没有找到返回 -1</returns>
        public static int BufferLookup(byte[] srcbuff, string subchars)
        {
            return BufferLookup(srcbuff, subchars, 0);
        }

        #endregion 在 srcbuff 中查找 subchars 第一次出现的位置

        #region 在 srcbuff 中查找 subchars 第一次出现的位置

        /// <summary>
        /// 在 srcbuff 中查找 subchars 第一次出现的位置
        /// </summary>
        /// <param name="srcbuff"></param>
        /// <param name="subchars">ASCII字符串</param>
        /// <param name="start">开始查找的位置</param>
        /// <returns>找到则返回匹配开始的位置，没有找到返回 -1</returns>
        public static int BufferLookup(byte[] srcbuff, string subchars, int start)
        {
            byte[] subbuff = Encoding.ASCII.GetBytes(subchars);
            return BufferLookup(srcbuff, subbuff, start);
        }

        #endregion 在 srcbuff 中查找 subchars 第一次出现的位置

        #region 将二进制的数据转成Hex格式

        /// <summary>
        /// 将二进制的数据转成Hex格式
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string BytesToHex(byte[] bytes)
        {
            if (bytes == null)
                return "";
            string str = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                str += bytes[i].ToString("X2");
            }
            return str;
        }

        #endregion 将二进制的数据转成Hex格式

        #region 将日期时间格式的字符串转换到数据库使用的日期类型

        /// <summary>
        /// 将日期时间格式的字符串转换到数据库使用的日期类型
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static byte[] ToDBDate(DateTime dateTime)
        {
            Byte[] ret = new Byte[15]; //后面的字节没什么用，只是因为原先参考的定义使用了 15。
            ret[0] = (Byte)(dateTime.Year / 100 + 100);
            ret[1] = (Byte)(dateTime.Year % 100 + 100);
            ret[2] = (Byte)dateTime.Month;
            ret[3] = (Byte)dateTime.Day;
            ret[4] = (Byte)(dateTime.Hour + 1);
            ret[5] = (Byte)(dateTime.Minute + 1);
            ret[6] = (Byte)(dateTime.Second + 1);
            return ret;
        }

        #endregion 将日期时间格式的字符串转换到数据库使用的日期类型

        #region 将倒序的ushort字节还原为ushort

        /// <summary>
        /// 将倒序的ushort字节还原为ushort
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        internal static ushort GetSwappedUshort(byte[] buffer, int start)
        {
            byte[] tmp = new byte[2];
            tmp[0] = buffer[start];
            tmp[1] = buffer[start + 1];

            return BitConverter.ToUInt16(BytesHelper.SwapBytes(tmp), 0);
        }

        #endregion 将倒序的ushort字节还原为ushort

        #region 将倒序的uint字节还原为uint

        /// <summary>
        /// 将倒序的uint字节还原为uint
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        internal static uint GetSwappedUint(byte[] buffer, int start)
        {
            byte[] tmp = BytesHelper.SubBuffer(buffer, start, 4);
            return BitConverter.ToUInt32(BytesHelper.SwapBytes(tmp), 0);
        }

        #endregion 将倒序的uint字节还原为uint

        #region 转换时间格式

        /// <summary>
        /// 转换时间格式
        /// 原始数据：FFFF-FF-FF FF:FF:FF 每个F对应YYYY-MM-DD hh:mm:ss中的一个数字
        ///              共7个字节。比如 0x20 0x08 0x09 0x20 0x23 0x12 0x34 表示 2008-09-20 23:12:34
        /// </summary>
        /// <param name="timeBuff"></param>
        /// <returns></returns>
        internal static DateTime Revert7BDateTime(byte[] timeBuff)
        {
            string timestr = "";
            for (int i = 0; i < 7; i++)
            {
                timestr += timeBuff[i].ToString("X2");
            }
            timestr = timestr.Substring(0, 4) + "-" + timestr.Substring(4, 2) + "-" + timestr.Substring(6, 2) + " " + timestr.Substring(8, 2) + ":" + timestr.Substring(10, 2) + ":" + timestr.Substring(12, 2);
            try
            {
                return Convert.ToDateTime(timestr);
            }
            catch
            {
                return new DateTime(2000, 01, 01);
            }
        }

        #endregion 转换时间格式

        #region Hex进制转二进制

        /// <summary>
        /// Hex进制转二进制
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] HexToBytes(string input)
        {
            int i = input.Length % 2;
            if (i != 0)
            {
                throw new Exception("字符串的长度必需是偶数");
            }
            List<byte> ls = new List<byte>();
            for (int j = 0; j < input.Length; j += 2)
            {
                byte b = Convert.ToByte(input.Substring(j, 2), 16);
                ls.Add(b);
            }
            return ls.ToArray();
        }

        #endregion Hex进制转二进制

        /// <summary>
        /// Determine if two byte arrays are equal.
        /// </summary>
        /// <param name="byte1">The first byte array to compare.</param>
        /// <param name="byte2">The byte array to compare to the first.</param>
        /// <returns><see langword="true"/> if the two byte arrays are equal; otherwise <see langword="false"/>.</returns>
        public static bool Compare(byte[] byte1, byte[] byte2)
        {
            if (byte1 == null)
            {
                return false;
            }

            if (byte2 == null)
            {
                return false;
            }

            if (byte1.Length != byte2.Length)
            {
                return false;
            }

            bool result = true;
            for (int i = 0; i < byte1.Length; i++)
            {
                if (byte1[i] != byte2[i])
                {
                    result = false;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Combines two byte arrays into one.
        /// </summary>
        /// <param name="byte1">The prefixed bytes.</param>
        /// <param name="byte2">The suffixed bytes.</param>
        /// <returns>The combined byte arrays.</returns>
        public static byte[] Combine(byte[] byte1, byte[] byte2)
        {
            if (byte1 == null)
            {
                throw new ArgumentNullException("byte1");
            }

            if (byte2 == null)
            {
                throw new ArgumentNullException("byte2");
            }

            byte[] combinedBytes = new byte[byte1.Length + byte2.Length];
            Buffer.BlockCopy(byte1, 0, combinedBytes, 0, byte1.Length);
            Buffer.BlockCopy(byte2, 0, combinedBytes, byte1.Length, byte2.Length);

            return combinedBytes;
        }

        /// <summary>
        /// Copys a byte arrays into new one.
        /// </summary>
        /// <param name="byte1">The byte arrays to which new array is created.</param>
        /// <returns>The cloned byte arrays.</returns>
        public static byte[] Clone(byte[] byte1)
        {
            if (byte1 == null)
            {
                throw new ArgumentNullException("byte1");
            }

            byte[] copyBytes = new byte[byte1.Length];

            Buffer.BlockCopy(byte1, 0, copyBytes, 0, byte1.Length);

            return copyBytes;
        }

        /// <summary>
        /// <para>Returns a byte array from a string representing a hexidecimal number.</para>
        /// </summary>
        /// <param name="hexadecimalNumber">
        /// <para>The string containing a valid hexidecimal number.</para>
        /// </param>
        /// <returns><para>The byte array representing the hexidecimal.</para></returns>
        public static byte[] GetBytesFromHexString(string hexadecimalNumber)
        {
            string InvalidHexString = "String must represent a valid hexadecimal (e.g. : 0F99DD)";
            if (String.IsNullOrEmpty(hexadecimalNumber))
            {
                throw new ArgumentNullException("hexadecimalNumber");
            }

            StringBuilder sb = new StringBuilder(hexadecimalNumber.ToUpper(CultureInfo.CurrentCulture));

            if (sb[0].Equals('0') && sb[1].Equals('X'))
            {
                sb.Remove(0, 2);
            }

            if (sb.Length % 2 != 0)
            {
                throw new ArgumentException(InvalidHexString);
            }

            byte[] hexBytes = new byte[sb.Length / 2];
            try
            {
                for (int i = 0; i < hexBytes.Length; i++)
                {
                    int stringIndex = i * 2;
                    hexBytes[i] = Convert.ToByte(sb.ToString(stringIndex, 2), 16);
                }
            }
            catch (FormatException ex)
            {
                throw new ArgumentException(InvalidHexString, ex);
            }

            return hexBytes;
        }

        /// <summary>
        /// <para>Returns a string from a byte array represented as a hexidecimal number (eg: 0F351A).</para>
        /// </summary>
        /// <param name="bytes">
        /// <para>The byte array to convert to forat as a hexidecimal number.</para>
        /// </param>
        /// <returns>
        /// <para>The formatted representation of the bytes as a hexidecimal number.</para>
        /// </returns>
        public static string GetHexStringFromBytes(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }

            if (bytes.Length == 0)
            {
                throw new ArgumentOutOfRangeException("bytes", "The value must be greater than 0 bytes.");
            }

            StringBuilder sb = new StringBuilder(bytes.Length * 2);
            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(bytes[i].ToString("X2", CultureInfo.CurrentCulture));
            }
            return sb.ToString();
        }

        #region 使用指定字符集将string转换成byte[]

        /// <summary>
        /// 将string转换成byte[]
        /// </summary>
        /// <param name="text">要转换的字符串</param>
        public static byte[] StringToBytes(string text)
        {
            return Encoding.Default.GetBytes(text);
        }

        /// <summary>
        /// 使用指定字符集将string转换成byte[]
        /// </summary>
        /// <param name="text">要转换的字符串</param>
        /// <param name="encoding">字符编码</param>
        public static byte[] StringToBytes(string text, Encoding encoding)
        {
            return encoding.GetBytes(text);
        }

        #endregion 使用指定字符集将string转换成byte[]

        #region 使用指定字符集将byte[]转换成string

        /// <summary>
        /// 将byte[]转换成string
        /// </summary>
        /// <param name="bytes">要转换的字节数组</param>
        public static string BytesToString(byte[] bytes)
        {
            return Encoding.Default.GetString(bytes);
        }

        /// <summary>
        /// 使用指定字符集将byte[]转换成string
        /// </summary>
        /// <param name="bytes">要转换的字节数组</param>
        /// <param name="encoding">字符编码</param>
        public static string BytesToString(byte[] bytes, Encoding encoding)
        {
            return encoding.GetString(bytes);
        }

        #endregion 使用指定字符集将byte[]转换成string
    }
}