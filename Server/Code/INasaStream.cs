using System;
using System.IO;
using System.Threading.Tasks;

namespace Mars
{
    public interface INasaStream
    {
        Task<Stream> GetDataAsync();
        Task<Stream> GetPhotoAsync(RoverName name, DateTime date);
    }
}