// See https://aka.ms/new-console-template for more information


using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Net.Http;
using System.Net.Sockets;
using System.Xml.Linq;
using Tcp_Server;

// create the builder
var builder = WebApplicationFactory.CreateBuilder();
builder.Services.AddControllers();

// register dependencies
//builder.Services.AddTransient<TestService>();
//builder.Services.AddTransient<TestChildService>();


//Singleton


builder.Services.AddSingleton<TestService>();
builder.Services.AddSingleton<TestChildService>();


//Scoped
//builder.Services.AddScoped<TestService>();
//builder.Services.AddScoped<TestChildService>();





// build the app
var app = builder.Build();
app.MapControllers();

// resolving dependencies
//var testService = app.Services.GetService<TestService>();
//testService.print();



//singleton

var a = app.Services.GetService<TestService>();
var b = app.Services.GetService<TestService>();

Console.WriteLine($"Hashcode of a is {a.GetHashCode()}");
Console.WriteLine($"Hashcode of b is {b.GetHashCode()}");

Console.WriteLine(ReferenceEquals(a, b));



// Scoped
var services = new List<ServiceDescriptor>()
{
    new ServiceDescriptor(typeof(TestService), typeof(TestService), ServiceLifetime.Scoped),
    new ServiceDescriptor(typeof(TestChildService), typeof(TestChildService), ServiceLifetime.Scoped)
};

var provider = new ServiceProvider(services);



// Scope 1
var scope1 = provider.CreateScope();
var c = provider.GetService(typeof(TestService), scope1);

var e = provider.GetService(typeof(TestService), scope1);


Console.WriteLine($"Hashcode of Testservice is {c.GetHashCode()}");
Console.WriteLine($"Hashcode of TestService is {e.GetHashCode()}");



// Scope 2
var scope2 = provider.CreateScope();
var d = app.Services.GetService(typeof(TestService), scope2);

Console.WriteLine($"Hashcode of TestService is {d.GetHashCode()}");


Console.WriteLine(ReferenceEquals(c, d));

app.MapGet("/codecamp",
    ctx => $"We are codecamp batch 3 Route: {ctx.Path}");



// run the server
await app.RunAsync(5006);


