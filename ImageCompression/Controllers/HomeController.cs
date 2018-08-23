using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ImageCompression.Models;
using System.Drawing;
using System.Web.Helpers;

namespace ImageCompression.Controllers
{
    //https://bobcravens.com/2009/10/17/image-compression-in-c-for-asp-net-mvc/
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            var imageList = new List<MyImage>();
            var images = Directory.GetFiles(Server.MapPath("~/Original"));
            foreach (var image in images)
            {
                var img = new MyImage();
                var fileInfo = new FileInfo(image);
                img.FileName = fileInfo.Name;
                img.FilePath = image;
                img.FileSize = fileInfo.Length;
                img.DateModified = fileInfo.LastWriteTime;
                imageList.Add(img);
            }
            ViewBag.IMGS = imageList;
            return View();
        }

        public ActionResult Compress()
        {
            var imageList = new List<MyImage>();
            var images = Directory.GetFiles(Server.MapPath("~/Compressed"));
            foreach (var image in images)
            {
                var img = new MyImage();
                var fileInfo = new FileInfo(image);
                img.FileName = fileInfo.Name;
                img.FilePath = image;
                img.FileSize = fileInfo.Length;
                img.DateModified = fileInfo.LastWriteTime;
                imageList.Add(img);
            }
            ViewBag.IMGS = imageList;

            return View();
        }

        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ProcessUpload()
        {
            var img = new MyImage();
            long quality =long.Parse(Request["quality"]);
            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase file = Request.Files[i];
                if (file.ContentLength != 0)
                {
                    var imgPath = Path.Combine(Server.MapPath("~/Original"), Path.GetFileName(file.FileName));
                    file.SaveAs(imgPath);
                    var tempImg = Image.FromFile(imgPath);
                    img.VariousQuality(tempImg,quality, Server.MapPath("~/Compressed"), Path.GetFileNameWithoutExtension(file.FileName));
                }
            }
            TempData["message"] = "Upload success" + quality;
            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        public ActionResult Resize()
        {
            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase file = Request.Files[i];
                if (file.ContentLength != 0)
                {
                    var imgPath = Path.Combine(Server.MapPath("~/Original"), Path.GetFileName(file.FileName));
                    file.SaveAs(imgPath);
                    var webImg = new WebImage(file.InputStream);
                    if (webImg.Width > 1000)
                        webImg.Resize(500, 500);
                    var imgCompressPath = Path.Combine(Server.MapPath("~/Compressed"), 
                        Path.GetFileNameWithoutExtension(file.FileName) + "_resize" + Path.GetExtension(file.FileName));
                    webImg.Save(imgCompressPath);
                }
            }
            TempData["message"] = "Upload success";
            return Redirect(Request.UrlReferrer.ToString());
        }

    }
}
