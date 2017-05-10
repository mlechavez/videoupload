using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace VideoUpload.Web.Models
{
    public class DownloadResult : ActionResult
    {
        private string _fileName;
        public DownloadResult(string fileName)
        {
            _fileName = fileName;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            /*
            context.HttpContext.Response.Buffer = false;
            context.HttpContext.Response.BufferOutput = false;
            context.HttpContext.Response.Clear();
            */
            //The File Path 
            var videoFilePath = HostingEnvironment.MapPath("~/Uploads/Videos/" + _fileName);
            //The header information 
            context.HttpContext.Response.AddHeader("Content-Disposition", "attachment; filename=" + _fileName);
            context.HttpContext.Response.AddHeader("Content-Type", "video/mp4");

            var file = new FileInfo(videoFilePath);
            //Check the file exist,  it will be written into the response 

            if (file.Exists)
            {
                var stream = file.OpenRead();
                var bytesinfile = new byte[stream.Length];
                stream.Read(bytesinfile, 0, (int)file.Length);
                context.HttpContext.Response.BinaryWrite(bytesinfile);
            }            
        }
    }
}