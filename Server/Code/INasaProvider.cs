using System.Threading.Tasks;

namespace Mars {
    public interface INasaProvider {
        Task<MarsWheather> GetAsync();
    }
}