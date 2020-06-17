﻿using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.VirtualFileSystem;

namespace Microsoft.AspNetCore.Builder
{
    public static class VirtualFileSystemApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseVirtualFiles(this IApplicationBuilder app)
        {
            //  注册文件目录
            var a = app.ApplicationServices.GetRequiredService<IWebContentFileProvider>();
            return app.UseStaticFiles(
                new StaticFileOptions
                {
                    FileProvider = app.ApplicationServices.GetRequiredService<IWebContentFileProvider>()
                }
            );
        }
    }
}
