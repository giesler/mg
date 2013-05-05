using System;
using Microsoft.SPOT;
using Gsiot.Server;

namespace DripDuino
{
    class DripHttpServer
    {
        public static void Run()
        {
            HttpServer server = new HttpServer();

            server.RequestRouting.Add("GET /status",
                        context => 
                            {
                                context.SetResponse("zero", "text/plain");
                            });

            server.RequestRouting.Add("GET /toggle",
                        context =>
                            {
                                context.SetResponse("1", "text/plain");
                            });

            server.Run();
        }
    }
}
