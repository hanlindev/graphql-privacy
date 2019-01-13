using GraphQL.Builders;
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
        public IComplexGraphType ResolvedType { get; private set; }
        public string DelegateFieldName { get; private set; }
        public Func<TModel, TDelegateID> IDGetter { get; private set; }

        public DelegateToFieldRule(
            T resolvedType,
            string delegateFieldName,
            Func<TModel, TDelegateID> idGetter)
        {
            ResolvedType = resolvedType;
            DelegateFieldName = delegateFieldName;
            IDGetter = idGetter;
        }

        public DelegateToFieldRule(
            IComplexGraphType graphType,
            string fieldName,
            string delegateFieldName,
            Func<TModel, TDelegateID> idGetter)
        {
            ResolvedType = graphType.GetField(fieldName).ResolvedType as IComplexGraphType;
            if (ResolvedType == null)
            {
                throw new Exception($"ResolvedType of an object authorized by {GetType().Name} must be an IComplexGraphType");
            }
            DelegateFieldName = delegateFieldName;
            IDGetter = idGetter;
        }

        public async Task<AuthorizationResult> AuthorizeAsync(IAuthorizationContext<TModel> authContext)
        {
            var delegatedFieldType = ResolvedType.GetField(DelegateFieldName);
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
