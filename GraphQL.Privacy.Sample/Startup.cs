using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphiQl;
using GraphQL.DataLoader;
using GraphQL.Privacy.Sample.GraphQL;
using GraphQL.Types;
using GraphQL.Types.Relay;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GraphQL.Privacy.Sample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddDbContext<SampleDbContext>();
            services.AddHttpContextAccessor();
            ConfigureGraphQL(services);
        }

        private void ConfigureGraphQL(IServiceCollection services)
        {
            // Register graphql types here
            services
                .AddSingleton<PageInfoType>()
                .AddSingleton<AlbumType>()
                .AddSingleton<AlbumQuery>()
                .AddSingleton<ConnectionType<AlbumType>>()
                .AddSingleton<EdgeType<AlbumType>>()
                .AddSingleton<UserType>()
                .AddSingleton<UserQuery>();

            // This is required dependency injection helpers
            services
                .AddSingleton<IHttpContextResolverService, HttpContextResolverService>()
                .AddSingleton(typeof(ITypedResolverService<>), typeof(TypedResolverService<>))
                .AddSingleton<IDependencyResolver>(s => new FuncDependencyResolver(s.GetRequiredService));

            services
                .AddSingleton<IDataLoaderContextAccessor, DataLoaderContextAccessor>()
                .AddSingleton<DataLoaderDocumentListener>()
                // Unlike the normal DocumentExecutor implementation, use this one to enable authorization
                .AddSingleton<IDocumentExecuter, AuthorizationEnabledDocumentExecuter>()
                .AddSingleton<SampleQuery>()
                .AddSingleton<SampleMutation>()
                .AddSingleton<ISchema, SampleSchema>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseGraphiQl();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "graphql",
                    template: "/graphql");
            });
        }
    }
}
