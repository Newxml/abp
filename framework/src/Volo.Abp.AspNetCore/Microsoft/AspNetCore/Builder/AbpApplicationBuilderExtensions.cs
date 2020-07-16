using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.RequestLocalization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Volo.Abp;
using Volo.Abp.AspNetCore.Auditing;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.Security.Claims;
using Volo.Abp.AspNetCore.Tracing;
using Volo.Abp.AspNetCore.Uow;
using Volo.Abp.DependencyInjection;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// abp应用构造器的扩展方法
    /// </summary>
    public static class AbpApplicationBuilderExtensions
    {
        //  异常处理中间件标记
        private const string ExceptionHandlingMiddlewareMarker = "_AbpExceptionHandlingMiddleware_Added";

        public static void InitializeApplication([NotNull] this IApplicationBuilder app)
        {
            Check.NotNull(app, nameof(app));

            app.ApplicationServices.GetRequiredService<ObjectAccessor<IApplicationBuilder>>().Value = app;

            //  初始化module
            var application = app.ApplicationServices.GetRequiredService<IAbpApplicationWithExternalServiceProvider>();
            //  应用生命周期
            var applicationLifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();
            
            //  当应用程序主机执行正常关闭时触发。 在此事件完成之前，关闭将被阻止。
            applicationLifetime.ApplicationStopping.Register(() =>
            {
                application.Shutdown();
            });

            //  当应用程序主机执行正常关闭时触发。 在此事件完成之前，关闭将被阻止。
            applicationLifetime.ApplicationStopped.Register(() =>
            {
                application.Dispose();
            });

            //  Q:注册应用服务
            application.Initialize(app.ApplicationServices);
        }

        /// <summary>
        /// 注册审计的中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseAuditing(this IApplicationBuilder app)
        {
            return app
                .UseMiddleware<AbpAuditingMiddleware>();
        }
        /// <summary>
        /// 注册工作单元的中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseUnitOfWork(this IApplicationBuilder app)
        {
            return app
                .UseAbpExceptionHandling()
                .UseMiddleware<AbpUnitOfWorkMiddleware>();
        }
        /// <summary>
        /// Q:
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder app)
        {
            return app
                .UseMiddleware<AbpCorrelationIdMiddleware>();
        }
        /// <summary>
        /// 注册本地化中间件
        /// </summary>
        /// <param name="app"></param>
        /// <param name="optionsAction"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseAbpRequestLocalization(this IApplicationBuilder app,
            Action<RequestLocalizationOptions> optionsAction = null)
        {
            app.ApplicationServices
                .GetRequiredService<IAbpRequestLocalizationOptionsProvider>()
                .InitLocalizationOptions(optionsAction);

            return app.UseMiddleware<AbpRequestLocalizationMiddleware>();
        }
        /// <summary>
        /// 注册异常拦截的中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseAbpExceptionHandling(this IApplicationBuilder app)
        {
            if (app.Properties.ContainsKey(ExceptionHandlingMiddlewareMarker))
            {
                return app;
            }

            app.Properties[ExceptionHandlingMiddlewareMarker] = true;
            return app.UseMiddleware<AbpExceptionHandlingMiddleware>();
        }
        /// <summary>
        /// 注册声明的中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseAbpClaimsMap(this IApplicationBuilder app)
        {
            return app.UseMiddleware<AbpClaimsMapMiddleware>();
        }
    }
}
