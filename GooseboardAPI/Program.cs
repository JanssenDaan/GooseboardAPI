using SignalRChat.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("http://gooseboard.ngrok.io")
                .AllowAnyHeader()
                .WithMethods("GET", "POST")
                .AllowCredentials().SetIsOriginAllowed(_ => true);
            
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
// UseCors must be called before MapHub.
app.UseCors();
//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseHttpLogging();
app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapHub<GooseHub>("/chatHub");
//app.UseWelcomePage();

app.Run();
