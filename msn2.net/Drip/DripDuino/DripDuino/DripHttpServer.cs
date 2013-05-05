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
                                context.SetResponse(Dripper.IsOn.ToString(), "text/plain");
                            });

            server.RequestRouting.Add("GET /toggle",
                        context =>
                            {
                                context.SetResponse("not yet implemented", "text/plain");
                            });

            server.Run();
        }
    }
}
