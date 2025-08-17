using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using McWuT.Data.Contexts;
using McWuT.Services.Notes;
using McWuT.Services;
using McWuT.Data.Repositories.Base;
using McWuT.Services.PasswordVault;
using McWuT.Services.Shopping;
using McWuT.Common.Converters;

namespace McWuT.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                 options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<INotesService, NotesService>();
            builder.Services.AddScoped<IPasswordVaultService, PasswordVaultService>();
            builder.Services.AddScoped<IShoppingListService, ShoppingListService>();
            builder.Services.AddScoped<IShoppingItemService, ShoppingItemService>();
            builder.Services.AddScoped(typeof(IEntityRepository<>), typeof(EntityRepository<>));
            builder.Services.AddScoped(typeof(IUserEntityRepository<>), typeof(UserEntityRepository<>));

            // Converters
            builder.Services.AddSingleton<IJsonCSharpConversionService, JsonCSharpConversionService>();

            // Add Data Protection
            //builder.Services.AddDataProtection();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                            .AddRoles<IdentityRole>()
                            .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Configuration["ConnectionStrings:DefaultConnection"] = "Server=localhost;Database=mydb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true";

      
        
                var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapRazorPages()
               .WithStaticAssets();

            app.Run();
        }
    }
}