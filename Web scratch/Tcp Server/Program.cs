// See https://aka.ms/new-console-template for more information


using Tcp_Server;

var server = new TcpServer(5005);
await server.StartAsync();
