using System;
using JetBrains.Annotations;

namespace Volo.Abp
{
    /// <summary>
    /// ʹ���ⲿ�����ṩ�����Ӧ�ó���
    /// </summary>
    public interface IAbpApplicationWithExternalServiceProvider : IAbpApplication
    {
        void Initialize([NotNull] IServiceProvider serviceProvider);
    }
}