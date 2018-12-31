namespace GraphQL.Privacy
{
    public class Skip : AuthorizationResult
    {

        public Skip(string reason) : base(reason)
        {}

        public Skip(object instance)
            : base(GetReason(instance))
        {}

        private static string GetReason(object instance)
        {
            if (instance == null) {
                return "Skipping authorization because subject is null";
            }
            return $"Skipping authorization for {instance.GetType().Name} because no policy or no rule returned a decision";
        }
    }
}