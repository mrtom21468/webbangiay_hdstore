using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace WebApplication7.Middleware
{
    public class MaintenanceMiddleware
    {
        private readonly RequestDelegate _next;

        public MaintenanceMiddleware(RequestDelegate next)
            {
                _next = next;
            }
        public async Task Invoke(HttpContext context)
        {
            // Kiểm tra điều kiện để xác định xem ứng dụng có đang trong quá trình bảo trì hay không
            bool isMaintenanceMode = false; // Thay đổi thành true nếu ứng dụng đang trong quá trình bảo trì

            if (isMaintenanceMode)
            {
                // Trả về thông báo bảo trì
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                await context.Response.WriteAsync("<p>Ứng dụng đang trong quá trình bảo trì. Vui lòng thử lại sau.</p>");

            }
            else
            {
                // Nếu không trong quá trình bảo trì, chuyển yêu cầu cho middleware tiếp theo
                await _next(context);
            }
        }
    }

    public static class MaintenanceMiddlewareExtensions
    {
        public static IApplicationBuilder UseMaintenanceMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MaintenanceMiddleware>();
        }
    }
}

