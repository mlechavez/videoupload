using System.IO;
using System.Web.Hosting;
using System.Web.Mvc;

namespace VideoUpload.Web.Models
{
    public class VideoStreamResult : ActionResult
    {
        private string _fileName;
        public VideoStreamResult(string fileName)
        {
            _fileName = fileName;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            var path = HostingEnvironment.MapPath("~/Uploads/Videos/" + _fileName);

            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                context.HttpContext.Response.BufferOutput = false;

                byte[] buffer = new byte[1024];
                int bytesRead = 0;

                context.HttpContext.Response.AddHeader("Content-Type", "video/mp4");

                while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0)
                {                    
                    context.HttpContext.Response.OutputStream.Write(buffer, 0, bytesRead);                    
                    bytesRead--;
                }
                context.HttpContext.Response.End();
            }
        }
    }
}