using GraphQL.Types;
using System;
using System.Threading.Tasks;

namespace GraphQL.Privacy.Rules
{
    public class DelegateToFieldRule<T, TModel, TDelegateType, TDelegate, TDelegateID> : IAuthorizationRule<TModel>
        where T : IComplexGraphType
        where TModel : class
        where TDelegateType : IComplexGraphType
        where TDelegate : class
    {
        public T Field { get; private set; }
        public string FieldName { get; private set; }
        public Func<TModel, TDelegateID> IDGetter { get; private set; }

        public DelegateToFieldRule(
            T field,
            string fieldName,
            Func<TModel, TDelegateID> idGetter)
        {
            Field = field;
            FieldName = fieldName;
            IDGetter = idGetter;
        }

        public async Task<AuthorizationResult> AuthorizeAsync(IAuthorizationContext<TModel> authContext)
        {
            var delegatedFieldType = Field.GetField(FieldName);
            var executionStragyHelpers = authContext.Resolve<IExecutionStrategyHelpers>();
            var delegatedNode = executionStragyHelpers.BuildExecutionNode(
                authContext.ExecutionNode,
                delegatedFieldType.ResolvedType,
                authContext.ExecutionNode.Field,
                delegatedFieldType
            );

            var delegateID = IDGetter(authContext.Subject);
            var modelLoader = authContext.Resolve<IModelLoader>();
            var delegateResult = await modelLoader.FindAsync<TDelegate, TDelegateID>(delegateID);
            delegatedNode.Result = delegateResult;
            var result = await authContext
                .Resolve<TDelegateType>()
                .AuthorizeAsync<TDelegate>(authContext.ExecutionContext, delegatedNode);
            if (result is Allow<TDelegate>)
            {
                return new Allow<TModel>(this);
            }
            return result;
        }
    }
}
