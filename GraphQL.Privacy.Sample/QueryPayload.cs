using Newtonsoft.Json.Linq;

namespace GraphQL.Privacy.Sample
{
    public class QueryPayload
    {
        public string OperationName { get; set; }
        public string NamedQuery { get; set; }
        public string Query { get; set; }
        public JObject Variables { get; set; }
    }
}