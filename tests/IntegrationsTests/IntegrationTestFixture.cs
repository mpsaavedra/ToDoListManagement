//using Microsoft.VisualStudio.TestPlatform.TestHost;
using Bootler.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler.Tests.IntegrationTest;

public class IntegrationTestFixture : WebApplicationFactory<Program>
{
    public HttpClient HttpClient { get; private set; }

    public IntegrationTestFixture()
    {
        HttpClient = CreateClient();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            //var dbContextDescriptor = services.SingleOrDefault(
            //    d => d.ServiceType ==
            //        typeof(DbContextOptions<AppDbContext>));

            //if(dbContextDescriptor != null)
            //    services.Remove(dbContextDescriptor);

            //var dbContextFactory = services.SingleOrDefault(
            //    d => d.ServiceType == typeof(IDbContextFactory<AppDbContext>));

            //if (dbContextFactory != null)
            //    services.Remove(dbContextFactory);

            //services.AddDbContext<AppDbContext>(options =>
            //{
            //    options.UseInMemoryDatabase("InMemoryAppDb");
            //});

            //services.AddAuthentication("TestScheme")
            //    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestScheme", options => { });

            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated();
                //db.SeedData();
                db.SaveChanges();
            }
        });
    }
}
