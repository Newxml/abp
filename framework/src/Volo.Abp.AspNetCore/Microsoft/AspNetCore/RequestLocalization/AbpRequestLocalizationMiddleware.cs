using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Microsoft.AspNetCore.RequestLocalization
{
    /// <summary>
    /// 请求解析本地化的http请求中间件
    /// </summary>
    public class AbpRequestLocalizationMiddleware : IMiddleware, ITransientDependency
    {
        private readonly IAbpRequestLocalizationOptionsProvider _requestLocalizationOptionsProvider;
        private readonly ILoggerFactory _loggerFactory;


        public AbpRequestLocalizationMiddleware(
            IAbpRequestLocalizationOptionsProvider requestLocalizationOptionsProvider,
            ILoggerFactory loggerFactory)
        {
            _requestLocalizationOptionsProvider = requestLocalizationOptionsProvider;
            _loggerFactory = loggerFactory;
        }

        /// <summary>
        /// 委托可以处理HTTP请求的方法。
        /// </summary>
        /// <param name="context">http上下文</param>
        /// <param name="next">下一个处理方法</param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var middleware = new RequestLocalizationMiddleware(
                next,
                new OptionsWrapper<RequestLocalizationOptions>(
                    await _requestLocalizationOptionsProvider.GetLocalizationOptionsAsync()
),
                _loggerFactory
            );

            await middleware.Invoke(context);
        }
    }
}