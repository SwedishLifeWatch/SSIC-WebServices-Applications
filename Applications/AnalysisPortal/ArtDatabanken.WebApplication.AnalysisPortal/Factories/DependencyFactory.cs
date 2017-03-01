﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Factories
{
    /// <summary>
    /// Simple wrapper for unity resolution.
    /// </summary>
    public class DependencyFactory
    {
        private static IUnityContainer _container;

        /// <summary>
        /// Public reference to the unity container which will 
        /// allow the ability to register instrances or take 
        /// other actions on the container.
        /// </summary>
        public static IUnityContainer Container
        {
            get
            {
                return _container;
            }
            private set
            {
                _container = value;
            }
        }

        /// <summary>
        /// Static constructor for DependencyFactory which will 
        /// initialize the unity container.
        /// </summary>
        static DependencyFactory()
        {
            var container = new UnityContainer();

            var section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            if (section != null)
            {
                section.Configure(container);
            }
            RegisterTypes(container);
            _container = container;
        }

        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IDITaxon, DITaxon>();
            container.RegisterType<IDITaxonViewManager, DITaxonViewManager>(new InjectionConstructor(typeof(IDITaxon)));                
        }

        /// <summary>
        /// Resolves the type parameter T to an instance of the appropriate type.
        /// </summary>
        /// <typeparam name="T">Type of object to return</typeparam>
        public static T Resolve<T>()
        {
            T ret = default(T);

            if (Container.IsRegistered(typeof(T)))
            {
                ret = Container.Resolve<T>();
            }

            return ret;
        }
    }

    public class DITaxon : IDITaxon
    {
        public string GetName()
        {
            return "TaxonName";            
        }
    }

    public interface IDITaxon
    {
        string GetName();
    }

    public class DITaxonViewManager : IDITaxonViewManager
    {
        IDITaxon taxon;

        public DITaxonViewManager(IDITaxon taxon)
        {
            this.taxon = taxon;
        }

        public string GetName()
        {
            return taxon.GetName();
        }
    }

    public interface IDITaxonViewManager
    {
        string GetName();
    }
}
