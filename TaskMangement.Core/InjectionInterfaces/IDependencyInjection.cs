﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Core.InjectionInterfaces
{
    public interface IDependencyInjection
    {
        void Register(IServiceCollection serviceCollection, IConfiguration configuration);
        int Order { get; }
    }
}
