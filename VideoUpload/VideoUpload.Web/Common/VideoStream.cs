using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace VideoUpload.Web.Common
{
    public class VideoStream
    {
        private readonly string _fileName;
        public VideoStream(string fileName)
        {
            _fileName = fileName;
        }

        public async void WriteToStream(Stream outputStream)
        {
            try
            {
                var buffer = new byte[65536];
                using (var video = File.Open(_fileName, FileMode.Open, FileAccess.Read))
                {
                    var length = (int)video.Length;
                    var bytesRead = 1;

                    while (length > 0 && bytesRead > 0)
                    {
                        bytesRead = video.Read(buffer, 0, Math.Min(length, buffer.Length));
                        await outputStream.WriteAsync(buffer, 0, bytesRead);
                        length -= bytesRead;
                    }
                }
            }
            catch (HttpException)
            {
                return;                
            }
            finally
            {
                outputStream.Close();
            }
        }
    }
}