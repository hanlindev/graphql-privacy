namespace GraphQL.Privacy
{
    public class Allow<T> : AuthorizationResult
    {
        public Allow(string reason) : base(reason)
        {}
        public Allow(IAuthorizationRule<T> requirement)
            : base(GetReason(requirement))
            {}

        private static string GetReason(IAuthorizationRule<T> requirement)
        {
            return $"Allowed by requirement - {requirement.GetType().Name}";
        }
    }
}