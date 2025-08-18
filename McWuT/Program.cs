using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using McWuT.Data.Contexts;
using McWuT.Services.Notes;
using McWuT.Services;
using McWuT.Data.Repositories.Base;
using McWuT.Services.PasswordVault;
using McWuT.Services.Shopping;
using McWuT.Common.Converters;
using McWuT.Services.CrimeGenerator;
using McWuT.Services.CrimeGenerator.External;
using McWuT.Data.Models.CrimeGenerator;

namespace McWuT.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            
            // Add Blazor Server for interactive UI components
            builder.Services.AddServerSideBlazor();

            // Add SignalR for real-time updates
            builder.Services.AddSignalR();

            // Add MediatR for CQRS
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                 options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=mcwut.db"));

            builder.Services.AddScoped<INotesService, NotesService>();
            builder.Services.AddScoped<IPasswordVaultService, PasswordVaultService>();
            builder.Services.AddScoped<IShoppingListService, ShoppingListService>();
            builder.Services.AddScoped<IShoppingItemService, ShoppingItemService>();
            builder.Services.AddScoped(typeof(IEntityRepository<>), typeof(EntityRepository<>));
            builder.Services.AddScoped(typeof(IUserEntityRepository<>), typeof(UserEntityRepository<>));

            // CrimeGenerator services
            builder.Services.AddScoped<IGameService, GameService>();
            builder.Services.AddScoped<IRandomUserService, RandomUserService>();
            builder.Services.AddScoped<ILlmService, LlmService>();

            // HttpClient for external APIs
            builder.Services.AddHttpClient<IRandomUserService, RandomUserService>();
            builder.Services.AddHttpClient<ILlmService, LlmService>();

            // Converters
            builder.Services.AddSingleton<IJsonCSharpConversionService, JsonCSharpConversionService>();

            // Add Data Protection
            //builder.Services.AddDataProtection();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                            .AddRoles<IdentityRole>()
                            .AddEntityFrameworkStores<ApplicationDbContext>();

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

            app.UseStaticFiles();
            app.MapRazorPages();

            app.Run();
        }
    }
}