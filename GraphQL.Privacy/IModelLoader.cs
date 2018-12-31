using System.Threading.Tasks;

namespace GraphQL.Privacy
{
    public interface IModelLoader
    {
        Task<T> FindAsync<T, TID>(TID id) where T : class;
    }
}
