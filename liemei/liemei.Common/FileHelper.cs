using liemei.Common.common;
using liemei.Common.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace liemei.Common
{
    public class FileHelper
    {
        /// <summary>
        /// 获取图片文件存放位置
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetPicFilePath(string fileName)
        {
            
            return Path.Combine(SystemSet.ResourcesPath, fileName);
        }
        /// <summary>
        /// 获取图片文件下载链接
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetPicFileURL(string fileName)
        {
            return string.Format("{0}/{1}", SystemSet.WebResourcesSite, fileName);
        }

        public static string GetFileContent(string fileName)
        {
            string result = string.Empty;
            try
            {
                if (File.Exists(fileName))
                {
                    result = File.ReadAllText(fileName);
                }
            } catch (Exception ex)
            {
                
            }

            return result;
        }
        /// <summary>
        /// 根据性别随机获取头像
        /// </summary>
        /// <param name="sex">1.男；2.女</param>
        /// <returns></returns>
        public static string GetHeadimgurl(int sex)
        {
            Random ran = new Random();
            int i = 2;
            if (sex == 1)
                i = ran.Next(1, 5);
            else
                i = ran.Next(6,12);
            return string.Format("{0}/images/{1}.jpg", SystemSet.WebResourcesSite,i);
        }
        /// <summary>
        /// 随机获取头像
        /// </summary>
        /// <returns></returns>
        public static string GetHeadimgurl()
        {
            Random ran = new Random();
            int i = ran.Next(1, 12);
            return string.Format("{0}/images/{1}.jpg", SystemSet.WebResourcesSite, i);
        }
        /// <summary>
        /// 根据文字和图片获取验证码图片
        /// </summary>
        /// <param name="content"></param>
        /// <param name="picFileName"></param>
        /// <returns></returns>
        public static VerCodePic GetVerCodePic(string content,string picFileName,int fontSize=20)
        {
            ClassLoger.Info("FileHelper.GetVerCodePic","开始生成二维码");
            Bitmap bmp = new Bitmap(picFileName);
            List<int> hlist = new List<int>();
            VerCodePic codepic = new VerCodePic();
            int i = Utils.GetRandom(0, SystemSet.FontPoint.Count - 1);
            codepic.Font1 = SystemSet.FontPoint[i] as FontPoint;
            hlist.Add(i);

            A: int i2 = Utils.GetRandom(0, SystemSet.FontPoint.Count - 1);
            if (hlist.Contains(i2))
                goto A;
            codepic.Font2 = SystemSet.FontPoint[i2] as FontPoint;
            hlist.Add(i2);

            B: int i3 = Utils.GetRandom(0, SystemSet.FontPoint.Count - 1);
            if (hlist.Contains(i3))
                goto B;
            hlist.Add(i3);
            codepic.Font3 = SystemSet.FontPoint[i3] as FontPoint;

            C: int i4 = Utils.GetRandom(0, SystemSet.FontPoint.Count - 1);
            if (hlist.Contains(i4))
                goto C;
            hlist.Add(i4);
            codepic.Font4 = SystemSet.FontPoint[i4] as FontPoint;

            ClassLoger.Info("FileHelper.GetVerCodePic", "xxxxxxxxxxxxxxxxxxxxx");
            string fileName = (content + "-" + picFileName+"-"+i+"|"+i2+"|"+i3+"|"+i4).MD5()+Path.GetExtension(picFileName);
            string dir = Path.Combine(SystemSet.ResourcesPath, SystemSet.VerCodePicPath);
            string filePath = Path.Combine(dir, fileName);
            if (File.Exists(filePath))
            {
                codepic.PicURL = string.Format("{0}/{1}/{2}", SystemSet.WebResourcesSite, SystemSet.VerCodePicPath, fileName);
                return codepic;
            }
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            
            Graphics g = Graphics.FromImage(bmp);
            Font font = new Font("微软雅黑", fontSize, GraphicsUnit.Pixel);
            SolidBrush sbrush = new SolidBrush(Color.Black);
            SolidBrush sbrush1 = new SolidBrush(Color.Peru);
            SolidBrush sbrush2 = new SolidBrush(Color.YellowGreen);
            SolidBrush sbrush3 = new SolidBrush(Color.SkyBlue);
            List<char> fontlist = content.ToList();
            ClassLoger.Info("FileHelper.GetVerCodePic", fontlist.Count.ToString());
            g.DrawString(fontlist[0].TryToString(), font, sbrush, new PointF(codepic.Font1.X, codepic.Font1.Y));
            g.DrawString(fontlist[1].TryToString(), font, sbrush1, new PointF(codepic.Font2.X, codepic.Font2.Y));
            g.DrawString(fontlist[2].TryToString(), font, sbrush2, new PointF(codepic.Font3.X, codepic.Font3.Y));
            g.DrawString(fontlist[3].TryToString(), font, sbrush3, new PointF(codepic.Font4.X, codepic.Font4.Y));

            bmp.Save(filePath, ImageFormat.Jpeg);
            codepic.PicURL = string.Format("{0}/{1}/{2}",SystemSet.WebResourcesSite, SystemSet.VerCodePicPath, fileName);
            return codepic;
        }
        /// <summary>
        /// 获取验证码图片资源文件
        /// </summary>
        /// <returns></returns>
        public static string GetVerCodePicResource()
        {
            int i = Utils.GetRandom(1,9);
            return Path.Combine(SystemSet.ResourcesPath, SystemSet.VerCodePicSourcePath, i+ ".jpg");
        }
        /// <summary>
        /// 按比例缩放图片
        /// </summary>
        /// <param name="b"></param>
        /// <param name="destHeight"></param>
        /// <param name="destWidth"></param>
        /// <returns></returns>
        private static Bitmap GetThumbnail(Bitmap b, int destHeight, int destWidth)
        {
            System.Drawing.Image imgSource = b;
            System.Drawing.Imaging.ImageFormat thisFormat = imgSource.RawFormat;
            int sW = 0, sH = 0;
            // 按比例缩放           
            int sWidth = imgSource.Width;
            int sHeight = imgSource.Height;
            if (sHeight > destHeight || sWidth > destWidth)
            {
                if ((sWidth * destHeight) > (sHeight * destWidth))
                {
                    sW = destWidth;
                    sH = (destWidth * sHeight) / sWidth;
                }
                else
                {
                    sH = destHeight;
                    sW = (sWidth * destHeight) / sHeight;
                }
            }
            else
            {
                sW = sWidth;
                sH = sHeight;
            }
            Bitmap outBmp = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage(outBmp);
            g.Clear(Color.Transparent);
            // 设置画布的描绘质量         
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(imgSource, new Rectangle((destWidth - sW) / 2, (destHeight - sH) / 2, sW, sH), 0, 0, imgSource.Width, imgSource.Height, GraphicsUnit.Pixel);
            g.Dispose();
            // 以下代码为保存图片时，设置压缩质量     
            EncoderParameters encoderParams = new EncoderParameters();
            long[] quality = new long[1];
            quality[0] = 100;
            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            encoderParams.Param[0] = encoderParam;
            imgSource.Dispose();
            return outBmp;
        }
        /// <summary>
        /// 根据图片生成缩略图，并返回缩略图相对路径
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="destHeight"></param>
        /// <param name="destWidth"></param>
        /// <returns></returns>
        public static string ThumbnailPic(string FilePath,int destHeight,int destWidth)
        {
            string fileName = string.Format("{0}-{1}X{2}{3}", Path.GetFileNameWithoutExtension(FilePath), destHeight, destWidth, Path.GetExtension(FilePath));
            string dic = Path.Combine(SystemSet.ResourcesPath, SystemSet.Thumbnail);
            string _filepath = Path.Combine(dic, fileName);
            if (File.Exists(_filepath))
            {
                return string.Format("{0}/{1}", SystemSet.Thumbnail, fileName);
            }
            if (!Directory.Exists(dic))
            {
                Directory.CreateDirectory(dic);
            }
            Bitmap bmp = new Bitmap(FilePath);
            Bitmap outbmp = GetThumbnail(bmp, destHeight, destWidth);
            outbmp.Save(_filepath);
            return string.Format("{0}/{1}", SystemSet.Thumbnail, fileName);
        }

        public static string GetScaleDll(int scaleId)
        {
            var scaleFolder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scales");
            string filePath = System.IO.Path.Combine(scaleFolder, scaleId + ".dll");
            return filePath;
        }
    }
    
}
