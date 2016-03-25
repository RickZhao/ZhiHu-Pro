using System;
using System.Text;

namespace Zhihu.Common.Helper
{
    /* ----------------------------------------------------------
  文件名称：UnicodeConverter.h

  作者：秦建辉

  MSN：splashcn@msn.com

  当前版本：V1.0

  历史版本：
      V1.0	2011年05月12日
              完成正式版本。

  功能描述：
      Unicode内码转换器。用于utf-8、utf-16（UCS2）、utf-32（UCS4）之间的编码转换
   ------------------------------------------------------------ */

    /// <summary>
    /// UTF8、UTF16（UCS2）、UTF32（UCS4）编码转换器
    /// </summary>
    public sealed class UnicodeConverter
    {
        /// <summary>
        /// 转换UCS4编码到UTF8编码
        /// </summary>
        /// <param name="UCS4">要转换的UCS4编码</param>
        /// <returns>存储UTF8编码的字节数组</returns> 
        public static Byte[] UCS4ToUTF8(UInt32 UCS4)
        {
            UInt32[] CodeUp =
            {
                0x80, // U+00000000 ～ U+0000007F
                0x800, // U+00000080 ～ U+000007FF
                0x10000, // U+00000800 ～ U+0000FFFF
                0x200000, // U+00010000 ～ U+001FFFFF
                0x4000000, // U+00200000 ～ U+03FFFFFF
                0x80000000 // U+04000000 ～ U+7FFFFFFF
            };

            // 根据UCS4编码范围确定对应的UTF8编码字节数
            Int32 Len = -1;
            for (Int32 i = 0; i < CodeUp.Length; i++)
            {
                if (UCS4 < CodeUp[i])
                {
                    Len = i + 1;
                    break;
                }
            }

            if (Len == -1) return null; // 无效的UCS4编码

            // 转换为UTF8编码
            Byte[] UTF8 = new Byte[Len];
            Byte[] Prefix = {0, 0xC0, 0xE0, 0xF0, 0xF8, 0xFC};
            for (Int32 i = Len - 1; i > 0; i--)
            {
                UTF8[i] = (Byte) ((UCS4 & 0x3F) | 0x80);
                UCS4 >>= 6;
            }

            UTF8[0] = (Byte) (UCS4 | Prefix[Len - 1]);

            return UTF8;
        }

        /// <summary>
        /// 转换UTF8编码到UCS4编码
        /// </summary>
        /// <param name="UTF8">UTF8编码的字节数组</param>
        /// <param name="index">要转换的起始索引位置</param>
        /// <param name="UCS4">要输出的UCS4编码</param>
        /// <returns>字节数组中参与编码转换的字节长度</returns> 
        public static Int32 UTF8ToUCS4(Byte[] UTF8, Int32 index, out UInt32 UCS4)
        {
            if (index < 0 || index >= UTF8.Length)
            {
                UCS4 = 0xFFFFFFFF;
                return 0;
            }

            Byte b = UTF8[index];
            if (b < 0x80)
            {
                UCS4 = b;
                return 1;
            }

            if (b < 0xC0 || b > 0xFD)
            {
                // 非法UTF8
                UCS4 = 0xFFFFFFFF;
                return 0;
            }

            Int32 Len;
            if (b < 0xE0)
            {
                UCS4 = (UInt32) (b & 0x1F);
                Len = 2;
            }
            else if (b < 0xF0)
            {
                UCS4 = (UInt32) (b & 0x0F);
                Len = 3;
            }
            else if (b < 0xF8)
            {
                UCS4 = (UInt32) (b & 7);
                Len = 4;
            }
            else if (b < 0xFC)
            {
                UCS4 = (UInt32) (b & 3);
                Len = 5;
            }
            else
            {
                UCS4 = (UInt32) (b & 1);
                Len = 6;
            }

            if (index + Len > UTF8.Length)
            {
                // 非法UTF8
                UCS4 = 0xFFFFFFFF;
                return 0;
            }

            for (Int32 i = 1; i < Len; i++)
            {
                b = UTF8[index + i];
                if (b < 0x80 || b > 0xBF)
                {
                    // 非法UTF8
                    UCS4 = 0xFFFFFFFF;
                    return 0;
                }

                UCS4 = (UInt32) ((UCS4 << 6) + (b & 0x3F));
            }

            return Len;
        }

        /// <summary>
        /// 转换UCS4编码到UTF16编码
        /// </summary>
        /// <param name="UCS4">要转换的UCS4编码</param>
        /// <returns>存储UTF16编码的数组</returns> 
        public static UInt16[] UCS4ToUTF16(UInt32 UCS4)
        {
            if (UCS4 <= 0xFFFF)
            {
                // 基本平面字符
                UInt16[] UTF16 = {(UInt16) UCS4};
                return UTF16;
            }
            else if (UCS4 <= 0xEFFFF)
            {
                // 对于辅助平面字符，使用代理项对
                UInt16[] UTF16 = new UInt16[2];

                UTF16[0] = (UInt16) (0xD800 + (UCS4 >> 10) - 0x40);
                UTF16[1] = (UInt16) (0xDC00 + (UCS4 & 0x03FF));
                return UTF16;
            }
            else
            {
                // 超出编码范围  
                return null;
            }
        }

        /// <summary>
        /// 转换UTF16编码到UCS4编码
        /// </summary>
        /// <param name="UTF16">要转换的UTF16编码数组</param>
        /// <param name="index">要转换的起始索引位置</param>
        /// <param name="UCS4">要输出的UCS4编码</param>
        /// <returns>数组中参与编码转换的单元数</returns> 
        public static Int32 UTF16ToUCS4(UInt16[] UTF16, Int32 index, out UInt32 UCS4)
        {
            if (index < 0 || index >= UTF16.Length)
            {
                UCS4 = 0xFFFFFFFF;
                return 0;
            }

            UInt16 CodeA = UTF16[index];
            if (CodeA >= 0xD800 && CodeA <= 0xDFFF)
            {
                // 代理项区域（Surrogate Area）
                if (CodeA < 0xDC00 && index + 2 <= UTF16.Length)
                {
                    UInt16 CodeB = UTF16[index + 1];
                    if (CodeB >= 0xDC00 && CodeB <= 0xDFFF)
                    {
                        UCS4 = (UInt32) ((CodeB & 0x03FF) + (((CodeA & 0x03FF) + 0x40) << 10));
                        return 2;
                    }
                }

                // 非法UTF16
                UCS4 = 0xFFFFFFFF;
                return 0;
            }
            else
            {
                UCS4 = CodeA;
                return 1;
            }
        }

        /// <summary>
        /// 转换UCS4编码到Unicode字符串
        /// </summary>
        /// <param name="UCS4">要转换的UCS4编码</param>
        /// <returns>转换后的Unicode字符串</returns> 
        public static String UCS4ToString(UInt32 UCS4)
        {
            StringBuilder sb = new StringBuilder(2);
            if (UCS4 <= 0xFFFF)
            {
                // 基本平面字符
                sb.Append(Convert.ToChar(UCS4));
                return sb.ToString();
            }
            else if (UCS4 <= 0xEFFFF)
            {
                // 对于辅助平面字符，使用代理项对
                sb.Append(Convert.ToChar(0xD800 + (UCS4 >> 10) - 0x40));
                sb.Append(Convert.ToChar(0xDC00 + (UCS4 & 0x03FF)));
                return sb.ToString();
            }
            else
            {
                // 超出编码范围
                return null;
            }
        }

        /// <summary>
        /// 转换Unicode字符串到UCS4编码
        /// </summary>
        /// <param name="Unicode">用于转换的Unicode字符串</param>
        /// <param name="index">要转换的起始索引位置</param>
        /// <param name="UCS4">要输出的UCS4编码</param>
        /// <returns>参与编码转换的字符个数</returns> 
        public static Int32 StringToUCS4(String Unicode, Int32 index, out UInt32 UCS4)
        {
            if (index < 0 || index >= Unicode.Length)
            {
                UCS4 = 0xFFFFFFFF;
                return 0;
            }

            UInt16 CodeA = Convert.ToUInt16(Unicode[index]);
            if (CodeA >= 0xD800 && CodeA <= 0xDFFF)
            {
                // 代理项区域（Surrogate Area）
                if (CodeA < 0xDC00 && index + 2 <= Unicode.Length)
                {
                    UInt16 CodeB = Convert.ToUInt16(Unicode[index + 1]);
                    if (CodeB >= 0xDC00 && CodeB <= 0xDFFF)
                    {
                        UCS4 = (UInt32) ((CodeB & 0x03FF) + (((CodeA & 0x03FF) + 0x40) << 10));
                        return 2;
                    }
                }

                // 非法UTF16
                UCS4 = 0xFFFFFFFF;
                return 0;
            }
            else
            {
                UCS4 = CodeA;
                return 1;
            }
        }

        /// <summary>
        /// 转换UTF8编码到Unicode字符串
        /// </summary>
        /// <param name="UTF8">用于转换的UTF8编码字节数组</param>
        /// <param name="index">要转换的起始索引位置</param>
        /// <param name="count">要转换的字节数</param>
        /// <returns>转换后的Unicode字符串</returns> 
        public static String UTF8ToString(Byte[] UTF8, Int32 index, Int32 count)
        {
            if (index < 0 || index >= UTF8.Length || count == 0)
            {
                return null;
            }

            // 计算实际能够转换的字节数
            if (count < 0 || index + count > UTF8.Length)
            {
                count = UTF8.Length - index;
            }

            StringBuilder sb = new StringBuilder();
            Int32 i = 0;
            do
            {
                UInt32 UCS4;
                Int32 Len = UTF8ToUCS4(UTF8, index, out UCS4);
                if (Len == 0 || i + Len > count)
                {
                    // 转换失败
                    return null;
                }

                String Unicode = UCS4ToString(UCS4);
                if (Unicode == null)
                {
                    // 转换失败
                    return null;
                }

                sb.Append(Unicode);

                i += Len;
                index += Len;
            } while (i < count);

            return sb.ToString();
        }

        /// <summary>
        /// 转换Unicode字符串到UTF8编码
        /// </summary>
        /// <param name="Unicode">要转换的Unicode字符串</param>
        /// <param name="index">要转换的起始索引位置</param>
        /// <param name="count">要转换的字符个数</param>
        /// <returns>存储UTF8编码的字节数组</returns> 
        public static Byte[] StringToUTF8(String Unicode, Int32 index, Int32 count)
        {
            if (String.IsNullOrEmpty(Unicode) || index < 0 || index >= Unicode.Length || count == 0)
            {
                return null;
            }

            // 计算实际能够转换的字符数
            if (count < 0 || index + count > Unicode.Length)
            {
                count = Unicode.Length - index;
            }

            // 基本平面字符，其UTF8编码不超过3个字节，扩展平面字符则不会超过6个字节
            Byte[] UTF8Package = new Byte[Unicode.Length*3];
            Int32 AppendIndex = 0;
            Int32 i = 0;
            do
            {
                UInt32 UCS4;
                Int32 Len = StringToUCS4(Unicode, index, out UCS4);
                if (Len == 0 || i + Len > count)
                {
                    // 转换失败
                    return null;
                }

                Byte[] UTF8 = UCS4ToUTF8(UCS4);
                if (UTF8 == null)
                {
                    // 转换失败
                    return null;
                }

                UTF8.CopyTo(UTF8Package, AppendIndex);
                AppendIndex += UTF8.Length;

                i += Len;
                index += Len;
            } while (i < count);

            // 调整到实际大小
            Array.Resize(ref UTF8Package, AppendIndex);

            return UTF8Package;
        }
    }
}