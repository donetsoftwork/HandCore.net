using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hand.Generators.GenerateProperty;

/// <summary>
/// 成员处理
/// </summary>
public class MemberBuilder(StringBuilder builder, int indent = 0)
{
    private readonly StringBuilder _builder = builder;
    private int _indent = indent;
    private readonly HashSet<string> _using = [];
    private readonly HashSet<string> _methodNames = [];
    private readonly HashSet<string> _methodPartialNames = [];

    private readonly HashSet<string> _fieldNames = [];
    private readonly HashSet<string> _propertyNames = [];
    private readonly List<string[]> _constructors = [];

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parameterTypes"></param>
    /// <returns></returns>
    public bool CheckConstructor(params string[] parameterTypes)
    {
        return _constructors.Any(types => ArrayEquals(types, parameterTypes));
    }

    public static bool ArrayEquals(string[] list1, string[] list2)
    {
        var count = list1.Length;
        if (count != list2.Length)
            return false;
        for (int i = 0; i < count; i++)
        {
            if (!string.Equals(list1[i], list2[i]))
                return false;
        }
        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool MethodContains(string name)
    {
        return _methodNames.Contains(name);
    }
    #region TryBuild
    /// <summary>
    /// 尝试建方法
    /// </summary>
    /// <param name="method"></param>
    /// <param name="methodType"></param>
    /// <param name="modifiers"></param>
    /// <returns></returns>
    public bool TryBuildMethod(string method, string methodType, params IEnumerable<string> modifiers)
    {
        if (MethodContains(method))
            return false;
        BuildMethod(method, methodType, modifiers);
        return true;
    }

    public void BuildMethod(string method, string methodType, params IEnumerable<string> modifiers)
    {
        var builder = GenerateIndent();
        if (_methodPartialNames.Contains(method))
        {
            modifiers = modifiers.Append("partial")
                .Distinct();
        }
        GenerateModifiers(builder, modifiers);
        builder.Append($"{methodType} {method}");
        AddMethod(method);
    }

    public void BuildConstructor(string name,params IEnumerable<string> modifiers)
    {
        var builder = GenerateIndent();
        GenerateModifiers(builder, modifiers);
        builder.Append(name);
    }
    /// <summary>
    /// 尝试建字段
    /// </summary>
    /// <param name="name"></param>
    /// <param name="propertyType"></param>
    /// <returns></returns>
    public bool TryBuildField(string name, string propertyType, params string[] modifiers)
    {
        if (_fieldNames.Contains(name))
            return false;
        var builder = GenerateIndent();
        GenerateModifiers(builder, modifiers);
        builder.AppendLine($"{propertyType} {name};");
        _fieldNames.Add(name);
        return true;
    }
    /// <summary>
    /// 尝试建属性
    /// </summary>
    /// <param name="name"></param>
    /// <param name="propertyType"></param>
    /// <returns></returns>
    public bool TryBuildProperty(string name, string propertyType, bool canWrite, params string[] modifiers)
    {
        if (_propertyNames.Contains(name))
            return false;
        var builder = GenerateIndent();
        GenerateModifiers(builder, modifiers);
        builder.Append($"{propertyType} {name}");
        if (canWrite)
            builder.AppendLine($" {{ get; set; }}");
        else
            builder.AppendLine($" {{ get; }}");
        _propertyNames.Add(name);
        return true;
    }
    /// <summary>
    /// 按字段建属性
    /// </summary>
    /// <param name="name"></param>
    /// <param name="propertyType"></param>
    /// <param name="canWrite"></param>
    /// <param name="field"></param>
    /// <param name="modifiers"></param>
    /// <returns></returns>   
    public bool TryBuildPropertyByField(string name, string propertyType, bool canWrite, string field, params string[] modifiers)
    {
        if (_propertyNames.Contains(name))
            return false;
        var builder = GenerateIndent();
        GenerateModifiers(builder, modifiers);
        builder.AppendLine($"{propertyType} {name}");
        using (GenerateBlock.Create(this))
        {
            AppendLine($"get => _value;");
            if (canWrite)
                AppendLine($"set => _value = value;");
        }

        _propertyNames.Add(name);
        return true;
    }
    #endregion
    #region Add
    /// <summary>
    /// 
    /// </summary>
    /// <param name="ns"></param>
    public void AddUsing(string ns)
    {
        if (_using.Contains(ns))
            return;
        _using.Add(ns);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="members"></param>
    public void AddRange(IEnumerable<MemberDeclarationSyntax> members)
    {
        foreach (var member in members)
        {
            if(member is ConstructorDeclarationSyntax constructor)
            {
                Add(constructor);
            }
            else if (member is MethodDeclarationSyntax method)
            {
                Add(method);
            }
            else if (member is PropertyDeclarationSyntax property)
            {
                Add(property);
            }
        }
    }
    public void Add(ConstructorDeclarationSyntax constructor)
    {
        var parameterTypes = GetParameterTypes(constructor.ParameterList.Parameters)
            .ToArray();
        _constructors.Add(parameterTypes);
    }
    public void Add(MethodDeclarationSyntax method)
    {
        string name = method.Identifier.ValueText;
        if (method.Body is null)
        {
            AddPartialMethod(name);
        }
        else
        {
            AddMethod(name);
        }
    }
    public void Add(PropertyDeclarationSyntax property)
    {
        string name = property.Identifier.ValueText;
        AddProperty(name);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    public void AddMethod(string name)
    {
        _methodNames.Add(name);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    public void AddPartialMethod(string name)
    {
        _methodPartialNames.Add(name);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    public void AddProperty(string name)
    {
        _propertyNames.Add(name);
    }
    #endregion
    /// <summary>
    /// 
    /// </summary>
    public void AppendLine(string value)
    {
        GenerateIndent()
            .AppendLine(value);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    public void AppendLineNoIndent(string value)
    {
        _builder.AppendLine(value);
    }
    public void GenerateSummary(params IEnumerable<string> summarys)
    {
        AppendLine("/// <summary>");
        foreach (var summary in summarys)
        {
            AppendLine($"/// {summary}");
        }
        AppendLine("/// </summary>");
    }
    #region Generate
    public void GenerateUsing()
    {
        foreach (var ns in _using)
        {
            AppendLine($"using {ns};");
        }
    }
    public void GenerateNamespace(INamedTypeSymbol namedTypeSymbol)
    {
        string ns = namedTypeSymbol.ContainingNamespace.ToDisplayString();
        GenerateNamespace(ns);
    }
    public void GenerateNamespace(string ns)
    {
        AppendLine($"namespace {ns}");
    }
    /// <summary>
    /// 代码段开始(花括号)
    /// </summary>
    public void GenerateBlockBeigin()
    {
        GenerateBlockBeigin(_builder, ref _indent);
    }
    /// <summary>
    /// 代码段结束(花括号)
    /// </summary>
    public void GenerateBlockEnd()
    {
        GenerateBlockEnd(_builder, ref _indent);
    }
    /// <summary>
    /// 缩进
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public StringBuilder GenerateIndent()
    {
        return GenerateIndent(_builder, _indent);
    }
    /// <summary>
    /// 生成修饰符
    /// </summary>
    /// <param name="modifiers"></param>
    /// <returns></returns>
    public StringBuilder GenerateModifiers(params IEnumerable<string> modifiers)
    {
        return GenerateModifiers(_builder, modifiers);
    }
    #endregion
    #region staticGenerate
    /// <summary>
    /// 代码段开始(花括号)
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="indent">缩进数量</param>
    public static void GenerateBlockBeigin(StringBuilder builder, ref int indent)
    {
        GenerateIndent(builder, indent)
            .AppendLine("{");
        indent += 4;
    }
    /// <summary>
    /// 代码段结束(花括号)
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="indent">缩进数量</param>
    public static void GenerateBlockEnd(StringBuilder builder, ref int indent)
    {
        indent -= 4;
        GenerateIndent(builder, indent)
            .AppendLine("}");        
    }
    /// <summary>
    /// 缩进
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="indent"></param>
    /// <returns></returns>
    public static StringBuilder GenerateIndent(StringBuilder builder, int indent)
    {
        if (indent > 0)
            builder.Append(' ', indent);
        return builder;
    }
    /// <summary>
    /// 生成修饰符
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="modifiers"></param>
    /// <returns></returns>
    public static StringBuilder GenerateModifiers(StringBuilder builder, params IEnumerable<string> modifiers)
    {
        foreach (string modifier in modifiers)
        {
            builder.Append(modifier)
                .Append(' ');
        }
        return builder;
    }
    #endregion
    static IEnumerable<string> GetParameterTypes(IEnumerable<ParameterSyntax> parameters)
    {
        foreach (var parameter in parameters)
        {
            if (parameter.Type is PredefinedTypeSyntax typeSyntax)
                yield return typeSyntax.Keyword.ValueText;
        }
    }
    /// <summary>
    /// 拼接全部字符
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return _builder.ToString();
    }
}
