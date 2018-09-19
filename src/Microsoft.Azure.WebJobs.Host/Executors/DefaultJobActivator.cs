// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Azure.WebJobs.Host.Executors
{
    /// <summary>
    /// The default <see cref="IJobActivator"/> integrates with DI,
    /// supporting constructor injection for registered services.
    /// </summary>
    internal class DefaultJobActivator : IJobActivator
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<Type, ObjectFactory> _factories;

        public DefaultJobActivator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _factories = new Dictionary<Type, ObjectFactory>();
        }

        public T CreateInstance<T>()
        {
            var jobType = typeof(T);
            if (!_factories.TryGetValue(jobType, out ObjectFactory factory))
            {
                factory = ActivatorUtilities.CreateFactory(jobType, Type.EmptyTypes);
                _factories[jobType] = factory;
            }

            return (T)factory(_serviceProvider, null);
        }
    }
}
