
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using WebApiPractice.Data;
using WebApiPractice.Repository;
using WebApiPractice.Repository.IRepository;

namespace WebApiPractice
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("conStr"));
            });
            builder.Services.AddScoped<INationalParkRepository,NationalParkRepository>();
            builder.Services.AddScoped<ITrailRepository, TrailRepository>();
            builder.Services.AddScoped<IPaginationRepository, PaginationRepository>();
            builder.Services.AddScoped<IProductRepository,ProductRepository>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(app.Environment.ContentRootPath, "Uploads", "Products")),
                RequestPath = "/Uploads/Products"
            });
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
