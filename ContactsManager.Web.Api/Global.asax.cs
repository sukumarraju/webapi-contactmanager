using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using ContactsManager.Web.Api.Controllers;
using Contacts.Data.SqlServer.Repository;
using Contacts.Data.SqlServer.Implementation;
using Microsoft.Practices.Unity;
using ContactManager;

using Contacts.Data.SqlServer.Entities;

namespace ContactsManager.Web.Api
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            this.ConfigureApi(GlobalConfiguration.Configuration);
        }
        void ConfigureApi(HttpConfiguration config)
        {
            var unity = new UnityContainer();
            unity.RegisterType<ContactsController>();
            unity.RegisterType<IContactsManagerRepository, ContactsManagerRepository>(new ContainerControlledLifetimeManager());
            
            config.DependencyResolver = new IoCContainer(unity);

        }
    }
}