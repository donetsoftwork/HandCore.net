using System;

namespace Hand.Generators.GenerateProperty;

/// <summary>
/// 生成代码块
/// </summary>
public class GenerateBlock : IDisposable
{
    private readonly MemberBuilder _builder;
    /// <summary>
    /// 生成代码块
    /// </summary>
    /// <param name="builder"></param>
    public GenerateBlock(MemberBuilder builder)
    {        
        _builder = builder;
        _builder.GenerateBlockBeigin();
    }
    /// <summary>
    /// 生成代码块
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static GenerateBlock Create(MemberBuilder builder)
        => new(builder);
    /// <summary>
    /// 代码块结束
    /// </summary>
    public void Dispose()
    {
        _builder.GenerateBlockEnd();
    }
}
