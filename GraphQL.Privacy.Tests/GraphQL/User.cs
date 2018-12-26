using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphQL.Privacy.Test.GraphQL
{
    class User
    {
        public long Id { get; set; }
    }

    class UserType : ObjectGraphType<User>
    {
        public UserType()
        {
            Name = "User";
            Field(user => user.Id);
        }
    }
}
