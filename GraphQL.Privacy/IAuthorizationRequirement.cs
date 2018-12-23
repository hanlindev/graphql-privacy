using System.Threading.Tasks;

namespace GraphQL.Privacy
{
    public interface IAuthorizationRule<TModel>
    {
        Task<AuthorizationResult> AuthorizeAsync(IAuthorizationContext<TModel> context);
    }
}