// See https://aka.ms/new-console-template for more information


using System.Net.Http;
using Tcp_Server;

// create the builder
var builder = WebApplicationfactory.CreateBuilder();
builder.Services.AddControllers();

// build the app
var app = builder.Build();
app.MapControllers();

app.MapGet("/codecamp",
    ctx => $"We are codecamp batch 3 ffdd dfrrgf . Route: {ctx.Path}");




// run the server
await app.RunAsync(5005);


