using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoUpload.Web.Models
{
    public interface IAsyncInitialization
    {
        Task Initialization { get; }           
    }
}
