using System;
using JetBrains.Annotations;

namespace Volo.Abp
{
    /// <summary>
    /// 使用外部服务提供程序的应用程序
    /// </summary>
    public interface IAbpApplicationWithExternalServiceProvider : IAbpApplication
    {
        void Initialize([NotNull] IServiceProvider serviceProvider);
    }
}