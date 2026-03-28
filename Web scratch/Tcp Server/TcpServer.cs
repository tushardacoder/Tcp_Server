using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Tcp_Server
{

   public class RequestContext()
    {
        public String Method { get; set; }=String.Empty; // get , post

        public String Path {  get; set; }=String.Empty;// request path  e.g: /hello
    }

    // Tcp server class that listens for incoming tcp connections
    public class TcpServer
    {

        private readonly int _port;
        private readonly Router _router;

        public TcpServer(int port, Router router)
        {
            _port = port;
            _router = router;

        }

        //method to run tcp server asynchronously

        public async  Task StartAsync()
        {
            // Create a TCP listener bound to localhost (127.0.0.1) on the specified port
            var listener = new TcpListener(IPAddress.Loopback,_port);
            listener.Start();

            Console.WriteLine($"Server started on port {_port}");

            // Infinite loop to continuously accept new clients
            while (true)
            {
                // Accept a new client asynchronously
                var client = await listener.AcceptTcpClientAsync();


                // Handle the client connection in a background task so
                // the server can immediately accept other clients

                _ = Task.Run(() => HandleClinet(client));

            }

        }


        //method to handle a single client 

        private async Task HandleClinet(TcpClient client)
        {
            //get the network stream to read write data
            using var stream = client.GetStream();

            //create a buffer to hold incoming bytes
            var buffer = new byte[1024];

            // Read data from the client into the buffer asynchronously
            var bytecount = await stream.ReadAsync(buffer);


            // Convert the bytes to a string using UTF-8 encoding

            var requesttext = Encoding.UTF8.GetString(buffer,0,bytecount);


            // Naive parsing of HTTP request:
            // Split the request into lines by CRLF
            var lines = requesttext.Split("\r\n");

            // The first line is usually like: "GET /path HTTP/1.1"
            var requestLine = lines[0].Split();

            // Create a RequestContext object to store method and path
            var context = new RequestContext
            {
                Method = requestLine[0],
                Path = requestLine[1]
            };


            // Create a simple response that just echoes the requested path
            //var responseText = $"You requested {context.Path}";
            var responseText = _router.Resolve(context);

            // Convert the response to bytes
            // Include basic HTTP response headers
            var responseBytes = Encoding.UTF8.GetBytes(
                "HTTP/1.1 200 OK\r\n" +
                "Content-Length: " + responseText.Length + "\r\n\r\n" +
                responseText
            );


            // Send the response back to the client
            await stream.WriteAsync(responseBytes);



            // Close the connection after sending the response
            client.Close();


        }

    }
}
