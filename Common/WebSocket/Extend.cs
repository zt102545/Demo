using Microsoft.AspNetCore.Builder;

namespace Common
{
    public static class Extend
    {
        /// <summary>  
        /// 路由绑定处理  
        /// </summary>  
        /// <param name="app"></param>  
        public static WebSocketServiceHost AddWebSocketService<T>(this IApplicationBuilder app, string path, int bufferSize = 4096)
            where T : WebSocketBehavior, new()
        {
            WebSocketServiceHost handler = new WebSocketServiceHost(path, bufferSize);
            app.Map(handler.Path, ap => { app.UseWebSockets(); ap.Use(handler.Accept<T>); });
            return handler;
        }
    }
}
