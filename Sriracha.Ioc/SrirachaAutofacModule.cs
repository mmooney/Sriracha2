using Autofac;
using Common.Logging.Configuration;
using Sriracha.Data.Deployment;
using Sriracha.Data.Deployment.DeploymentImpl;
using Sriracha.Data.Ioc;
using Sriracha.Data.Utility;
using Sriracha.Data.Utility.UtilityImpl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.Ioc
{
    class SrirachaAutofacModule : Module
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
            
            //Utility
            builder.RegisterType<Zipper>().As<IZipper>();
            builder.RegisterType<ProcessRunner>().As<IProcessRunner>();
            
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
            Common.Logging.LogManager.Adapter = new Common.Logging.NLog.NLogLoggerFactoryAdapter((Common.Logging.Configuration.NameValueCollection)null);
            builder.Register(ctx =>
            { return Common.Logging.LogManager.GetCurrentClassLogger(); })
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
    }
}
