using System;

namespace VideoUpload.Web.Common
{
    public interface IExceptionLogger
    {
        void Log(Exception ex);
    }
}
