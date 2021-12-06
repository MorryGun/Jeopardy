using Jeopardy_Backend;
using Jeopardy_Backend_TestCommon.DataSetup;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Jeopardy_Backend_TestCommon
{
    public class TestStartUp : Startup
    {
        public TestStartUp(IConfiguration configuration) : base(configuration)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            // Add in memory database for EF core
            services.AddMvc().AddApplicationPart(typeof(Startup).Assembly);
            services.AddDbContext<JeopardyContext>(options => options.UseInMemoryDatabase("Jeopardy"));
            services.AddDbContext<UserContext>(options => options.UseInMemoryDatabase("User"));

            // Call base service configuration
            base.ConfigureServices(services);
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Get the context from services
            var jeopardyContext = app.ApplicationServices.GetRequiredService<JeopardyContext>();
            app.ApplicationServices.GetRequiredService<UserContext>();

            // Setup test data
            var testDataManager = new TestDataManager(jeopardyContext);
            testDataManager.AddTestData();

            // Call base middleware configuration
            base.Configure(app, env);
        }
    }
}
