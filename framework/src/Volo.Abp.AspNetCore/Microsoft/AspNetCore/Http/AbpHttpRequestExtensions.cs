using JetBrains.Annotations;
using Volo.Abp;

namespace Microsoft.AspNetCore.Http
{
    /// <summary>
    /// 请求头扩展
    /// </summary>
    public static class AbpHttpRequestExtensions
    {
        private const string RequestedWithHeader = "X-Requested-With";
        private const string XmlHttpRequest = "XMLHttpRequest";

        /// <summary>
        /// 检查是否是AJAX的请求，不过还是可以伪造
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static bool IsAjax([NotNull]this HttpRequest request)
        {
            Check.NotNull(request, nameof(request));

            if (request.Headers == null)
            {
                return false;
            }

            return request.Headers[RequestedWithHeader] == XmlHttpRequest;
        }

        /// <summary>
        /// Headers["Accept"]，检查包含关系
        /// </summary>
        /// <param name="request"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static bool CanAccept([NotNull]this HttpRequest request, [NotNull] string contentType)
        {
            Check.NotNull(request, nameof(request));
            Check.NotNull(contentType, nameof(contentType));

            return request.Headers["Accept"].ToString().Contains(contentType);
        }
    }
}
