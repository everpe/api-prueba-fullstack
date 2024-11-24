using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using MediatR;
using AutoMapper;
using PruebaIngresoBibliotecario.Api.Interfaces;
using PruebaIngresoBibliotecario.Api.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using PruebaIngresoBibliotecario.Api.Mediators.Behaviors;
using PruebaIngresoBibliotecario.Api.Infraestructure;



namespace PruebaIngresoBibliotecario.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
            Trace.AutoFlush = true;
            Trace.Indent();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSwaggerDocument();

            services.AddDbContext<PersistenceContext>(opt =>
            {
                opt.UseInMemoryDatabase("PruebaIngreso");
            });


            services.AddScoped<IProductService, ProductService>();
            services.AddAutoMapper(typeof(Startup));
            services.AddMediatR(typeof(Startup).Assembly);
            services.AddValidatorsFromAssembly(typeof(Program).Assembly); // Registra los validadores
            services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionHandlingBehavior<,>));

            services.AddControllers();


        }


        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseOpenApi();
            app.UseSwaggerUi3();
            // Ejecuta los seeds al inicio
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<PersistenceContext>();
            SeedData.SeedAsync(context).Wait();
        }

    }
}
