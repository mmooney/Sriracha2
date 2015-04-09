using Autofac;
using Common.Logging.Configuration;
using MMDB.Shared;
using Sriracha.Data.Deployment;
using Sriracha.Data.Deployment.DeploymentImpl;
using Sriracha.Data.Identity;
using Sriracha.Data.Identity.Impl;
using Sriracha.Data.Impersonation;
using Sriracha.Data.Impersonation.ImpersonationImpl;
using Sriracha.Data.Ioc;
using Sriracha.Data.Managers;
using Sriracha.Data.Managers.ManagersImpl;
using Sriracha.Data.Repository;
using Sriracha.Data.Utility;
using Sriracha.Data.Utility.UtilityImpl;
using Sriracha.Data.Validation;
using Sriracha.Data.Validation.ValidationImpl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.Ioc
{
    class SrirachaAutofacModule : Autofac.Module
    {
        private EnumIocMode _iocMode;

        public SrirachaAutofacModule(EnumIocMode iocMode)
        {
            this._iocMode = iocMode;
        }

        protected override void Load(Autofac.ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<AutofacIocFactory>().As<IIocFactory>();//.SingleInstance();

            //Deployment
            builder.RegisterType<DeployTaskRunner>().As<IDeployTaskRunner>();

            //Impersonation
            builder.RegisterType<Impersonator>().As<IImpersonator>();

            //Validation
            builder.RegisterType<DeployConfigurationValidator>().As<IDeployConfigurationValidator>();
            
            //Utility
            builder.RegisterType<Zipper>().As<IZipper>();
            builder.RegisterType<ProcessRunner>().As<IProcessRunner>();

            //Managers
            builder.RegisterType<UserManager>().As<IUserManager>();

            //NancyFX
            builder.RegisterType<Sriracha.Data.NancyFX.NancyUserMapper>().As<Nancy.Authentication.Forms.IUserMapper>();

            //Identity
            switch(_iocMode)
            {
                case EnumIocMode.DeploymentRunner:
                    builder.RegisterType<ConsoleSrirachaIdentity>().As<ISrirachaIdentity>();
                    break;
                case EnumIocMode.Service:
                case EnumIocMode.Web:
                    builder.RegisterType<ConsoleSrirachaIdentity>().As<ISrirachaIdentity>();
                    break;
                default:
                    throw new UnknownEnumValueException(_iocMode);
            }

            //Repositories
            string repositoryAssemblyName = AppSettingsHelper.GetRequiredSetting("RepositoryAssemblyName");
            this.RegisterRepositories(builder, repositoryAssemblyName);
            
            this.SetupLogging(builder);

            //http://stackoverflow.com/questions/2385370/cant-resolve-namevaluecollection-with-autofac
            builder.RegisterType<NameValueCollection>().UsingConstructor();
        }

        private void SetupLogging(ContainerBuilder builder)
        {
            //builder.Register(context =>
            //{
            //    //if ( != EnumDIMode.CommandLine)
            //    //{
            //    //    //This resolve operation has already ended.  
            //    //    //	When registering components using lambdas, the IComponentContext 'c' parameter to the lambda cannot be stored. 
            //    //    //	Instead, either resolve IComponentContext again from 'c', or resolve a Func<> based factory to create subsequent components from.
            //    //    var c = context.Resolve<IComponentContext>();
            //    //    var identity = context.Resolve<IUserIdentity>();
            //    //    var dbTarget = new NLogDBLogTarget(new AutofacDIFactory(c), identity);
            //    //    dbTarget.Layout = "${message} ${exception:format=message,stacktrace:separator=\r\n}";
            //    //    var loggingConfig = NLog.LogManager.Configuration;
            //    //    if (loggingConfig == null)
            //    //    {
            //    //        loggingConfig = new NLog.Config.LoggingConfiguration();
            //    //    }
            //    //    loggingConfig.AddTarget("dbTarget", dbTarget);
            //    //    var rule = new NLog.Config.LoggingRule("*", NLog.LogLevel.Warn, dbTarget);
            //    //    loggingConfig.LoggingRules.Add(rule);
            //    //    NLog.LogManager.Configuration = loggingConfig;
            //    //}
            //    var logger = NLog.LogManager.GetCurrentClassLogger();

            //    return logger;
            //})
            //        .As<NLog.Logger>()
            //        .SingleInstance();
            if(_iocMode == EnumIocMode.DeploymentRunner)
            {
                SetupColorConsoleLogging();
            }
            builder.RegisterType<LoggerDeployStatusReporter>().AsSelf();
            Common.Logging.LogManager.Adapter = new Common.Logging.NLog.NLogLoggerFactoryAdapter((Common.Logging.Configuration.NameValueCollection)null);
            builder.Register(ctx =>
            { return Common.Logging.LogManager.GetLogger(this.GetType()); })
                    .As<Common.Logging.ILog>()
                    .SingleInstance();
        }


        private static void SetupColorConsoleLogging()
        {
            var loggingConfig = NLog.LogManager.Configuration;
            if (loggingConfig == null)
            {
                loggingConfig = new NLog.Config.LoggingConfiguration();
            }
            var consoleTarget = new NLog.Targets.ColoredConsoleTarget();
            consoleTarget.Layout = "${longdate}:${message} ${exception:format=message,stacktrace=\r\n}";
            loggingConfig.AddTarget("consoleTarget", consoleTarget);
            var rule = new NLog.Config.LoggingRule("*", NLog.LogLevel.Trace, consoleTarget);
            loggingConfig.LoggingRules.Add(rule);
            NLog.LogManager.Configuration = loggingConfig;
        }

        private void RegisterRepositories(ContainerBuilder builder, string assemblyName)
        {
            var assembly = Assembly.Load(assemblyName.Replace(".dll", ""));
            var typeList = assembly.GetTypes()
                            .Where(p => typeof(ISrirachaRepositoryRegistar).IsAssignableFrom(p)
                                    && p.IsClass).ToList();
            if (typeList == null || typeList.Count == 0)
            {
                throw new Exception("Failed to find class implementing ISrirachaRepositoryRegistar in " + assemblyName);
            }
            if (typeList.Count > 1)
            {
                throw new Exception(string.Format("Found multiple classes implementing ISrirachaRepositoryRegistar in {0}: {1}", assemblyName, string.Join(",", typeList.Select(i => i.FullName))));
            }
            var registrar = (ISrirachaRepositoryRegistar)Activator.CreateInstance(typeList[0]);
            registrar.RegisterRepositories(new AutofacBuilderWrapper(builder));
        }
    }
}
