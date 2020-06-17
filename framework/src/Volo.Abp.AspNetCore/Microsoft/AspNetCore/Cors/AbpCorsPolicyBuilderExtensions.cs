using Microsoft.AspNetCore.Cors.Infrastructure;

namespace Microsoft.AspNetCore.Cors
{
    /// <summary>
    /// 跨域策略扩展
    /// </summary>
    /// <param name="corsPolicyBuilder"></param>
    /// <returns></returns>
    public static class AbpCorsPolicyBuilderExtensions
    {
        public static CorsPolicyBuilder WithAbpExposedHeaders(this CorsPolicyBuilder corsPolicyBuilder)
        {
            return corsPolicyBuilder.WithExposedHeaders("_AbpErrorFormat");
        }
    }
}