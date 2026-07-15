using Hand.Creational;

namespace ParseXmlTests.Supports;

public class CommentBuilder
    : IMemberBuilder<Comment>
{
    private readonly Comment _original = new();
    /// <summary>
    /// 原始对象
    /// </summary>
    public Comment Original
        => _original;
    /// <inheritdoc />
    public Comment Build()
        => _original;
    /// <inheritdoc />
    public void Save<TMember>(string name, TMember value)
    {
        switch (name)
        {
            case "summary":
                if (value is string summaryValue)
                    _original.Summary = summaryValue;
                break;
            case "typeparam":
                if (value is KeyValuePair<string, string> typeParam)
                    _original.TypeParams[typeParam.Key]= typeParam.Value;
                break;
            case "param":
                if (value is KeyValuePair<string, string> param)
                    _original.Params[param.Key] = param.Value;
                break;
            case "returns":
                if (value is string returnsValue)
                    _original.Returns = returnsValue;
                break;
        }
    }
    /// <summary>
    /// 工厂模式
    /// </summary>
    public static readonly ICreator<CommentBuilder> Creater = new DefaultCreater<CommentBuilder>();
}
