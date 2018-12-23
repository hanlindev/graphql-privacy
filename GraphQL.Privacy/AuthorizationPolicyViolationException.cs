using System;

namespace GraphQL.Privacy
{
    public class AuthorizationPolicyViolationException : Exception
    {
        public AuthorizationPolicyViolationException(AuthorizationResult result)
            : base(result.Reason)
        {}
    }
}