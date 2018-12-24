﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQL.Privacy.Sample.Models.Abstractions
{
    public interface ICanBeHidden
    {
        bool IsHidden { get; }
    }
}
