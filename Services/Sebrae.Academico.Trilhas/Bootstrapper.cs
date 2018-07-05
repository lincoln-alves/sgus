using System.Diagnostics;
using log4net.Config;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Json;
using Nancy.Responses.Negotiation;
using Nancy.TinyIoc;
using Sebrae.Academico.BP.AutoMapper;

namespace Sebrae.Academico.Trilhas
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override NancyInternalConfiguration InternalConfiguration
        {
            get
            {
                return NancyInternalConfiguration.WithOverrides(
                (c) =>
                {
                    c.ResponseProcessors.Clear();
                    c.ResponseProcessors.Add(typeof(JsonProcessor));
                });
            }
        }

        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            pipelines.OnError += (ctx, exception) => Errors.Handle(ctx, exception);
            
            base.RequestStartup(container, pipelines, context);
        }
        
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            AutoMapperConfig.RegisterMappings();

            if (Debugger.IsAttached)
            {
                XmlConfigurator.Configure();
            }

            JsonSettings.MaxJsonLength = int.MaxValue;

            pipelines.AfterRequest += (ctx) =>
            {
                ctx.Response.Headers.Add("Access-Control-Allow-Origin", "*"); // TODO: REMOVE THIS AND ADD IN CONFIG FILE ALL DOMAINS FROM DEV, STAGE AND PROD
                ctx.Response.Headers.Add("Access-Control-Allow-Methods", "POST,GET,PUT,DELETE");
                ctx.Response.Headers.Add("Access-Control-Allow-Headers", "Accept, Origin, Content-type, Authorization");
            };

        }

        //
        // The bootstrapper enables you to reconfigure the composition of the framework,
        // by overriding the various methods and properties.
        // For more information https://github.com/NancyFx/Nancy/wiki/Bootstrapper
    }
}