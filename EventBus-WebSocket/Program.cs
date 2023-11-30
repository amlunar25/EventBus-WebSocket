using EventBus_WebSocket.EventBusHandler;
using EventBus_WebSocket.Process;
using WebSocket_V4.WebSocketHandler;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton(new EventBus()); // Crea una unica instancia si ya fue creada para ser utilizada en cualquier parte del programa

//builder.Services.AddSingleton<WebSocket_V4.WebSocketHandler.WebSocketManager>(); // Adding WebSocket
builder.Services.AddHttpClient();
builder.Services.AddHostedService<BackgroundTasksService>(); // Add Thread

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
