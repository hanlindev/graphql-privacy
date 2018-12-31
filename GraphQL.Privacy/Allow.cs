namespace GraphQL.Privacy
{
    public class Allow<T> : AuthorizationResult
    {
        public Allow(string reason) : base(reason)
        {}
        public Allow(IAuthorizationRule<T> rule)
            : base(GetReason(rule))
            {}

        private static string GetReason(IAuthorizationRule<T> rule)
        {
            return $"Allowed by requirement - {rule.GetType().Name}";
        }
    }
}