using Microsoft.AspNetCore.Builder; // Đã kiểm tra và đảm bảo có đủ using
using Microsoft.EntityFrameworkCore; 
// Hoặc Microsoft.EntityFrameworkCore.Sqlite; (Tùy phiên bản)
using DemoMvc551.Data;

   
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddDbContext<ApplicationDbContext>
        (options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new
         InvalidOperationException("Connection string 'DefaultConnection' not found.")));


        // Add services to the container.
        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

      
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Demo}/{action=Index}/{id?}");
       


        app.Run();
    }
