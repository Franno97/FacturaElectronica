using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Mre.Visas.FacturaElectronica.Application;
using Mre.Visas.FacturaElectronica.Infrastructure;
using Mre.Visas.FacturaElectronica.Api.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Mre.Sb.Auditar;
using Mre.Visas.FacturaElectronica.Infrastructure.Persistence.Contexts;

namespace Mre.Visas.FacturaElectronica.Api
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; private set; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddHttpContextAccessor();

      // crea un HttpClient que se usa para acceder al IDP
      services.AddHttpClient("API_IDP", client =>
      {
        client.BaseAddress = new Uri(@"https://idpdev.cancilleria.gob.ec:44318/");
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
      });

      // crea un HttpClient que se usa para acceder al gestor de servicios
      services.AddHttpClient("API_FAC", client =>
      {
        client.BaseAddress = new Uri(@"https://ocelotdev.cancilleria.gob.ec:6050/");
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
      });


      services.AddInfrastructureLayer(Configuration);
      services.AddApplicationLayer();

      services.AddControllers().AddNewtonsoftJson(options =>
      {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
      });

      //ADD CROSS
      services.AddCors();

      services.AddSwaggerExtension("Mre.Visas.FacturaElectronica.Api", "v1");
      services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
      services.AddMvc();

      services.AgregarAuditoria(Configuration);

      //Servicios remotos
      //RemoteServicesExtensions.ConfigureHttpClient(services, Configuration);
      services.Configure<RemoteServices>(Configuration.GetSection("RemoteServices"));
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        //app.UseSwagger();
        //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mre.Visas.FacturaElectronica.Api v1"));
      }

      app.UseHttpsRedirection();
      app.UseRouting();
      app.UseSwaggerExtension("Mre.Visas.FacturaElectronica.Api");
      app.UseApiExceptionMiddleware();
      
      app.UseAuthorization();

      //ADD CROSS
      // global cors policy
      app.UseCors(x => x
          .AllowAnyMethod()
          .AllowAnyHeader()
          .SetIsOriginAllowed(origin => true) // allow any origin
          .AllowCredentials()); // allow credentials

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
      app.UsarAuditoria<ApplicationDbContext>();
    }
  }
}
