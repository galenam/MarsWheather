using System.IO;
using System.Threading.Tasks;

namespace Mars
{
    public interface INasaStream
    {
        Task<Stream> GetDataAsync();
    }
}