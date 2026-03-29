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
    ctx => $"We are codecamp batch 3 Route: {ctx.Path}");


app.MapGet("/codecamp/{batch}/{name}",
    (int batch, string name) =>
        $"Welcome {name} from batch {batch}");

app.MapGet("/user/{id}", (int id) => $"User ID: {id}");

app.MapGet("/userinfo/{name}", (string name) => $"User ID: {name}");

// run the server
await app.RunAsync(5006);


