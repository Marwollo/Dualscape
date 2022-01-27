using Amazon.DynamoDBv2;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dualscape.Repository.Classes;
using Dualscape.Repository.Interfaces;
using Dualscape.Service.Classes;
using Dualscape.Service.Interfaces;
using Dualscape.RTC.Hubs;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.SignalR;
using Owin;
using Microsoft.AspNetCore.Http;
using System.Net.WebSockets;
using System.Net;
using System.Text;

namespace Dualscape.API
{
    public class Startup
    {
        public List<WebSocket> websocketConnections = new List<WebSocket>();
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                    });
            });

            services.AddControllers();

            services.AddSingleton<IGameStateRepository, GameStateRepository>();


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Dualscape.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dualscape.API v1"));
            }
            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            var wsOptions = new WebSocketOptions
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
            };
            app.UseWebSockets(wsOptions);
            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/rtc")
                {
                    if (context.WebSockets.IsWebSocketRequest)
                    {
                        using (WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync())
                        {
                            await SocketHandler(context, webSocket);
                        }
                    }
                    else
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                     }
                }
            });
        }

        public async Task SocketHandler(HttpContext context, WebSocket webSocket)
        {
            websocketConnections.Add(webSocket);
            byte[] buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket
                .ReceiveAsync(new ArraySegment<byte>(buffer), System.Threading.CancellationToken.None);

            if (result != null)
            {
                while (!result.CloseStatus.HasValue)
                {
                    string msg = Encoding.UTF8.GetString(new ArraySegment<byte>(buffer, 0, result.Count));
                    for (int i = 0; i < websocketConnections.Count; i++)
                    {
                        await websocketConnections[i]
                        .SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(msg)),
                        result.MessageType,
                        result.EndOfMessage,
                        System.Threading.CancellationToken.None);
                        result = await websocketConnections[i]
                            .ReceiveAsync(new ArraySegment<byte>(buffer), System.Threading.CancellationToken.None);
                    }
                    
                }
            }
            /*await webSocket.CloseAsync(result.CloseStatus.Value, 
                result.CloseStatusDescription, 
                System.Threading.CancellationToken.None);*/
        }
    }
}
