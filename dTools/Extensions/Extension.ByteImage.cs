using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dTools
{
    /// <summary>
    /// Image扩展
    /// </summary>
    public static class ExtensionByteImage
    {
        /// <summary>
        /// 图片转换成Byte
        /// </summary>
        /// <param name="Image"></param>
        /// <param name="imageFormat"></param>
        /// <returns></returns>
        public static byte[] ImageToByte(this Image Image, ImageFormat imageFormat)
        {
            if (Image == null) { return null; }
            byte[] data = null;
            using (MemoryStream oMemoryStream = new MemoryStream())
            {
                using (Bitmap oBitmap = new Bitmap(Image))
                {
                    oBitmap.Save(oMemoryStream, imageFormat);
                    oMemoryStream.Position = 0;
                    data = new byte[oMemoryStream.Length];
                    oMemoryStream.Read(data, 0, Convert.ToInt32(oMemoryStream.Length));
                    oMemoryStream.Flush();
                }
            }
            return data;
        }

        /// <summary>
        ///Byte转换成Image
        /// </summary>
        /// <param name="Buffer"></param>        
        public static Image ByteToImage(this byte[] Buffer)
        {
            if (Buffer == null || Buffer.Length == 0) { return null; }
            try
            {
                MemoryStream oMemoryStream = new MemoryStream(Buffer);
                oMemoryStream.Position = 0;
                Image oImage = Image.FromStream(oMemoryStream);
                return new Bitmap(oImage);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Icon转成Image
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static Image ToImage(this Icon @this)
        {
            if (@this == null)
            {
                return null;
            }
            return Image.FromHbitmap(@this.ToBitmap().GetHbitmap());

            //using (MemoryStream mStream = new MemoryStream())
            //{
            //    @this.Save(mStream);
            //    return Image.FromStream(mStream);
            //}
        }

        /// <summary>
        /// Image转换成Icon
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static Icon ToIcon(this Image @this)
        {
            if (@this == null)
            {
                return null;
            }
            using (MemoryStream msImg = new MemoryStream(), msIco = new MemoryStream())
            {
                @this.Save(msImg, ImageFormat.Png);
                using (var bin = new BinaryWriter(msIco))
                {
                    //写图标头部
                    bin.Write((short)0);           //0-1保留
                    bin.Write((short)1);           //2-3文件类型。1=图标, 2=光标
                    bin.Write((short)1);           //4-5图像数量（图标可以包含多个图像）

                    bin.Write((byte)@this.Width);  //6图标宽度
                    bin.Write((byte)@this.Height); //7图标高度
                    bin.Write((byte)0);            //8颜色数（若像素位深>=8，填0。这是显然的，达到8bpp的颜色数最少是256，byte不够表示）
                    bin.Write((byte)0);            //9保留。必须为0
                    bin.Write((short)0);           //10-11调色板
                    bin.Write((short)32);          //12-13位深
                    bin.Write((int)msImg.Length);  //14-17位图数据大小
                    bin.Write(22);                 //18-21位图数据起始字节

                    //写图像数据
                    bin.Write(msImg.ToArray());
                    bin.Flush();
                    bin.Seek(0, SeekOrigin.Begin);
                    return new Icon(msIco);
                }
            }
        }
    }
}