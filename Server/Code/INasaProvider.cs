using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mars {
    public interface INasaProvider {
        Task<IEnumerable<MarsWheather>> GetAsync();
    }
}