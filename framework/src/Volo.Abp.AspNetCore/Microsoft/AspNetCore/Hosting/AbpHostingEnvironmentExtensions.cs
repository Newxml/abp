using System;
using Microsoft.Extensions.Configuration;

namespace Microsoft.AspNetCore.Hosting
{
    public static class AbpHostingEnvironmentExtensions
    {
        /// <summary>
        /// 好像被VirtualFileSystemApplicationBuilderExtensions替代了
        /// </summary>
        /// <param name="env"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IConfigurationRoot BuildConfiguration(
            this IWebHostEnvironment env,
            AbpConfigurationBuilderOptions options = null)
        {
            options = options ?? new AbpConfigurationBuilderOptions();

            if (options.BasePath.IsNullOrEmpty())
            {
                options.BasePath = env.ContentRootPath;
            }

            if (options.EnvironmentName.IsNullOrEmpty())
            {
                options.EnvironmentName = env.EnvironmentName;
            }

            return ConfigurationHelper.BuildConfiguration(options);
        }
    }
}
