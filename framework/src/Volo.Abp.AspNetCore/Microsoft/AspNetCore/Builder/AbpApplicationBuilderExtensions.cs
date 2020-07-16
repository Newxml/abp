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
    /// abpӦ�ù���������չ����
    /// </summary>
    public static class AbpApplicationBuilderExtensions
    {
        //  �쳣�����м�����
        private const string ExceptionHandlingMiddlewareMarker = "_AbpExceptionHandlingMiddleware_Added";

        public static void InitializeApplication([NotNull] this IApplicationBuilder app)
        {
            Check.NotNull(app, nameof(app));

            app.ApplicationServices.GetRequiredService<ObjectAccessor<IApplicationBuilder>>().Value = app;

            //  ��ʼ��module
            var application = app.ApplicationServices.GetRequiredService<IAbpApplicationWithExternalServiceProvider>();
            //  Ӧ����������
            var applicationLifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();
            
            //  ��Ӧ�ó�������ִ�������ر�ʱ������ �ڴ��¼����֮ǰ���رս�����ֹ��
            applicationLifetime.ApplicationStopping.Register(() =>
            {
                application.Shutdown();
            });

            //  ��Ӧ�ó�������ִ�������ر�ʱ������ �ڴ��¼����֮ǰ���رս�����ֹ��
            applicationLifetime.ApplicationStopped.Register(() =>
            {
                application.Dispose();
            });

            //  Q:ע��Ӧ�÷���
            application.Initialize(app.ApplicationServices);
        }

        /// <summary>
        /// ע����Ƶ��м��
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseAuditing(this IApplicationBuilder app)
        {
            return app
                .UseMiddleware<AbpAuditingMiddleware>();
        }
        /// <summary>
        /// ע�Ṥ����Ԫ���м��
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
        /// ע�᱾�ػ��м��
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
        /// ע���쳣���ص��м��
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
        /// ע���������м��
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseAbpClaimsMap(this IApplicationBuilder app)
        {
            return app.UseMiddleware<AbpClaimsMapMiddleware>();
        }
    }
}
