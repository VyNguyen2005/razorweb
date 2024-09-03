using Microsoft.EntityFrameworkCore;
using razor09_razorweb.models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<MyBlogContext>((options) => {
    string connectionString = builder.Configuration.GetConnectionString("MyWebBlog");
    options.UseSqlServer(connectionString);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

/*
  dotnet aspnet-codegenerator razorpage -m razor09_razorweb.models.Article -dc razor09_razorweb.models.MyBlogContext -outDir Pages/Blog -udl --referenceScriptLibraries
  // Microsoft.VisualStudio.Web.CodeGeneration.Design
  // Microsoft.EntityFrameworkCore.Tools
*/