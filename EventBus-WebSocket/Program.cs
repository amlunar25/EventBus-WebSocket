using EventBus_WebSocket.EventBusHandler;
using WebSocket_V4.WebSocketHandler;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton<EventBus>();
//builder.Services.AddSingleton<WebSocket_V4.WebSocketHandler.WebSocketManager>(); // Adding WebSocket
//builder.Services.AddHostedService<WebSocketBackgroundService>(); // Add Thread

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (!app.Environment.IsDevelopment())
// {
//     app.UseExceptionHandler("/Error");
//     // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//     app.UseHsts();
// }

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseWebSockets();
app.UseMiddleware<WebSocketMiddleware>();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapRazorPages();

app.Run();
