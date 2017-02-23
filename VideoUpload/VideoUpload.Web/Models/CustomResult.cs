using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace VideoUpload.Web.Models
{
    public class CustomResult : ActionResult
    {
        private string _fileName;
        public CustomResult(string fileName)
        {
            _fileName = fileName;
        }
        public override void ExecuteResult(ControllerContext context)
        {                        
            //The File Path 
            var videoFilePath = HostingEnvironment.MapPath("~/Uploads/" + _fileName);
            //The header information 
            context.HttpContext.Response.AddHeader("Content-Disposition", "attachment; filename="+_fileName);
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