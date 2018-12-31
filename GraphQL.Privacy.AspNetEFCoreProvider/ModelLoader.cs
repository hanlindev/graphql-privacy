using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GraphQL.Privacy
{
    public class ModelLoader<TDB> : IModelLoader
        where TDB : DbContext
    {
        private TDB _context;

        public ModelLoader(TDB context)
        {
            _context = context;
        }

        public async Task<T> FindAsync<T, TID>(TID id)
            where T : class
        {
            return await _context.Set<T>().FindAsync(id);
        }
    }
}
