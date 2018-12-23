namespace GraphQL.Privacy
{
    public class Deny : AuthorizationResult
    {
        public Deny(string reason)
            : base(reason)
        {}
    }
}