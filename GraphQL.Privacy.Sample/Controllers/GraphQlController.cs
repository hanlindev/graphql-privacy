using GraphQL.DataLoader;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace GraphQL.Privacy.Sample.Controllers
{
    public class GraphQLController : Controller
    {
        private IDocumentExecuter _documentExecuter;
        private ISchema _schema;
        private DataLoaderDocumentListener _dataLoaderListener;
        private IDependencyResolver _dependencyResolver;

        public GraphQLController(
            IDocumentExecuter executer,
            ISchema schema,
            DataLoaderDocumentListener dataLoaderListener,
            IDependencyResolver dependencyResolver)
        {
            _documentExecuter = executer;
            _schema = schema;
            _dataLoaderListener = dataLoaderListener;
            _dependencyResolver = dependencyResolver;
        }

        [HttpPost]
        public virtual async Task<IActionResult> Post([FromBody] QueryPayload query)
        {
            if (query == null) { throw new ArgumentNullException(nameof(query)); }
            var inputs = query.Variables.ToInputs();
            var executionOptions = new ExecutionOptions
            {
                Schema = _schema,
                Query = query.Query,
                OperationName = query.OperationName,
                Inputs = inputs,
                UserContext = new GraphQLUserContext
                {
                    DependencyResolver = _dependencyResolver
                },
            };
            executionOptions.Listeners.Add(_dataLoaderListener);
            executionOptions.ThrowOnUnhandledException = true;

            var result = await _documentExecuter.ExecuteAsync(executionOptions).ConfigureAwait(false);

            if (result.Errors?.Count > 0)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}