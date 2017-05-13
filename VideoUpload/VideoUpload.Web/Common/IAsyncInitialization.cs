using System.Threading.Tasks;

namespace VideoUpload.Web.Common
{
    public interface IAsyncInitialization
    {
        Task Initialization { get; }           
    }
}
