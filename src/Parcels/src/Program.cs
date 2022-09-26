using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Parcels.Commands;
using Parcels.DAL;
using Parcels.DAL.Projections;
using Parcels.DAL.Repositories.Abstract;
using Parcels.Domain;
using Prometheus;

namespace Parcels
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddTransient<IParcelsRepository, ParcelsRepository>();

            builder.Services.AddProjection<Parcel, ParcelInlineProjection>();
            
            builder.Services.AddMediatR(typeof(Program));
            builder.Services.AddDbContext<ParcelsContext>(x =>
            {
                x.UseSqlServer(builder.Configuration.GetSection("ConnectionStrings:Default").Value);
                x.EnableSensitiveDataLogging();
            });

            var app = builder.Build();

            using (var scope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<ParcelsContext>().Database.Migrate();
            }

            app.UseMetricServer();
            app.UseHttpMetrics();

            app.MapControllers();

            app.Run();
        }
    }
}

//migrationBuilder.Sql(@"
//CREATE FUNCTION [dbo].[CalculateVersion](@StreamId AS UNIQUEIDENTIFIER)
//RETURNS INT
//AS
//BEGIN

//DECLARE @ret INT;

//SELECT @ret = MAX([Version]) FROM[Events] WHERE [StreamId] = @StreamId;

//RETURN @ret;
//END
//GO		
//");