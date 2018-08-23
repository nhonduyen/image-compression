using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ImageCompression.Models
{
    public class MyImage
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public long FileSize { get; set; }
        public DateTime DateModified { get; set; }

        public MyImage()
        {
        }

      
        public void VariousQuality(Image original, long quality, string path, string filename)
        {
            ImageCodecInfo jpgEncoder = null;
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (var codec in codecs)
            {
                if (codec.FormatID == ImageFormat.Jpeg.Guid)
                {
                    jpgEncoder = codec;
                    break;
                }
            }
            if (jpgEncoder != null)
            {
                var encoder = Encoder.Quality;
                var encoderParameters = new EncoderParameters(1);
                var encoderParameter = new EncoderParameter(encoder, quality);
                encoderParameters.Param[0] = encoderParameter;
                string fileOut = Path.Combine(path,filename+ "_" + quality + ".jpeg");
                using (var ms = new FileStream(fileOut, FileMode.Create, FileAccess.Write))
                {
                    original.Save(ms, jpgEncoder, encoderParameters);
                }
            }
        }
    }
}