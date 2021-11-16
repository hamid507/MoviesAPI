using Business.Abstractions.Services;
using Business.Mappings;
using Business.ServiceImplementations;
using DataAccess.Abstractions;
using DataAccess.Persistence;
using DataAccess.Persistence.DBContext;
using Hangfire;
using Hangfire.Common;
using Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            AppSettings.Initialize(configuration);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = AppSettings.DefaultConnection;

            services.AddControllers();
            services.AddSwaggerGen();
            services.AddHangfire(configuration =>configuration.UseSqlServerStorage(connectionString));
            services.AddHangfireServer();
            services.AddDbContext<InterviewDBContext>(options =>options.UseSqlServer(connectionString, builder => builder.MigrationsAssembly("Api")));

            services.AddAutoMapper(typeof(MappingProfile));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISchedulerService, SchedulerService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IRecurringJobManager jobManager, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "MoviesAPI V1");
            });

            var schedulerService = serviceProvider.GetService<ISchedulerService>();
            string cron = Cron.Weekly(DayOfWeek.Sunday, 19, 30);

            jobManager.AddOrUpdate("email_sender_job", Job.FromExpression(() => schedulerService.CheckWatchlistForEmails()), cron, TimeZoneInfo.Utc);
        }
    }
}
