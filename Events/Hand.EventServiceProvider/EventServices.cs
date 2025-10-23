using Hand.EventHandlers;
using Hand.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Hand.Events;

/// <summary>
/// 事件相关服务注册
/// </summary>
public static class EventServices
{
    /// <summary>
    /// 扫描注册事件处理器
    /// </summary>
    /// <param name="services"></param>
    /// <param name="lifetime"></param>
    /// <param name="assemblies"></param>
    /// <returns></returns>
    public static IServiceCollection ScanEventHandler(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped, params Assembly[] assemblies)
    {
        var interfaceTypes = new Type[]
        {
            typeof(IEventHandler<>),
            typeof(ITaskEventHandler<>)
        };
        void register(Type serviceType, Type implType)
            => services.Add(new ServiceDescriptor( serviceType, implType, lifetime));
        ReflectionType.ScanInterfaceImp(assemblies, register, interfaceTypes);
        services.Add(new ServiceDescriptor(typeof(IEventHandlerProvider), typeof(EventHandlerServiceProvider), lifetime));
        return services;
    }
    /// <summary>
    /// 注册事件总线
    /// </summary>
    /// <typeparam name="TEventBus"></typeparam>
    /// <param name="services"></param>
    /// <param name="lifetime"></param>
    /// <returns></returns>
    public static IServiceCollection AddEventBus<TEventBus>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        where TEventBus : IEventBus
    {
        services.Add(new ServiceDescriptor(typeof(IEventBus), typeof(TEventBus), lifetime));
        return services;
    }
    /// <summary>
    /// 注册事件异步总线
    /// </summary>
    /// <typeparam name="TEventAsyncBus"></typeparam>
    /// <param name="services"></param>
    /// <param name="lifetime"></param>
    /// <returns></returns>
    public static IServiceCollection AddEventAsyncBus<TEventAsyncBus>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        where TEventAsyncBus : IEventAsyncBus
    {
        services.Add(new ServiceDescriptor(typeof(IEventAsyncBus), typeof(TEventAsyncBus), lifetime));
        return services;
    }
}
