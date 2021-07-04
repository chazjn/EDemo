using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmailNotificationSystem;
using EquipmentAvailabiltySystem;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using AppointmentApi.Db;
using AppointmentApi.Validation;
using AppointmentApi.Db.Models;

namespace AppointmentApi
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
            services.AddControllers();

            services.AddSwaggerGen(o =>
            {
                o.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Appointment Service Api"
                });
            });

            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(o =>
            {
                o.GroupNameFormat = "'v'VVV";
                o.SubstituteApiVersionInUrl = true;
            });

            var emailConfigurationSection = Configuration.GetSection("EmailNotificationSystem");
            var smtpClientSettings = emailConfigurationSection.Get<SmtpClientSettings>();
            services.AddScoped<ISmtpClient>(serviceProvider => new SmtpClientAdapter(smtpClientSettings));

            services.AddDbContext<AppointmentsContext>(options => options
                                .UseSqlServer(Configuration.GetConnectionString("AppointmentsContext")));

            services.AddSingleton<IEquipmentAvailabilityService, EquipmentAvailabilityService>();
            services.AddScoped<IAppointmentsRepository, AppointmentsRepository>();
            services.AddScoped<IAppointmentParameters, StandardAppointmentParameters>();
            services.AddScoped<IValidatorFactory, ValidatorFactory>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AppointmentsContext appointmentsContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            app.UseSwaggerUI(o =>
            {
                o.SwaggerEndpoint("/swagger/v1/swagger.json", "Appointment Service Api v1");
            });

            appointmentsContext.Database.Migrate();
            appointmentsContext.Patients.Add(new Patient { EmailAddress = "test1@example.com" });
            appointmentsContext.Patients.Add(new Patient { EmailAddress = "test2@example.com" });
            appointmentsContext.SaveChanges();
        }
    }
}
