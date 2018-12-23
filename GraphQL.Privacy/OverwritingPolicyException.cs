using System;

namespace GraphQL.Privacy
{
    public class OverwritePolicyException : Exception
    {
        public OverwritePolicyException() : base("Overwriting existing policy is not allowed")
        {}
    }
}