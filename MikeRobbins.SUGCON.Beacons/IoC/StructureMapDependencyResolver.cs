﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StructureMap;
using System.Web.Http.Dependencies;
using IDependencyResolver = System.Web.Http.Dependencies.IDependencyResolver;

namespace MikeRobbins.SUGCON.Beacons.Website.IoC
{
    public class StructureMapDependencyResolver : IDependencyResolver
    {
        private readonly IContainer _container;

        public StructureMapDependencyResolver(IContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }
            this._container = container;
        }


        public void Dispose()
        {
            _container.Dispose();
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == null) return null;

            try
            {
                if (serviceType.IsAbstract || serviceType.IsInterface)
                {
                    return _container.TryGetInstance(serviceType);
                }
                else
                {
                    return _container.GetInstance(serviceType);
                }
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _container.GetAllInstances<object>().Where(s => s.GetType() == serviceType);
        }

        public IDependencyScope BeginScope()
        {
            IContainer child = _container.GetNestedContainer();
            return new StructureMapDependencyResolver(child);
        }
    }
}