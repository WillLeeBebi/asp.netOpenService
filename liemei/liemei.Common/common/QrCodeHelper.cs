using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace liemei.Common.common
{
    public class QrCodeHelper
    {
        /// <summary>
        /// 生成二维码图片
        /// </summary>
        /// <param name="codeContent"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool CreateImgCode(string codeContent, string fileName)
        {
            bool flag = false;
            try
            {
                string path = Path.GetDirectoryName(fileName);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                //创建二维码生成类  
                QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.M);
                QrCode qrCode = new QrCode();


                qrEncoder.TryEncode(codeContent, out qrCode);

                GraphicsRenderer gRender = new GraphicsRenderer(new FixedModuleSize(30, QuietZoneModules.Four));
                BitMatrix matrix = qrCode.Matrix; //qrCodeGraphicControl1.GetQrMatrix();
                using (FileStream stream = new FileStream(fileName, FileMode.Create))
                {
                    gRender.WriteToStream(matrix, System.Drawing.Imaging.ImageFormat.Png, stream, new Point(600, 600));
                }
                flag = true;
            }
            catch (Exception ex)
            {
                ClassLoger.Error("QrCodeHelper.Create_ImgCode");
            }

            return flag;
        }
    }
}
