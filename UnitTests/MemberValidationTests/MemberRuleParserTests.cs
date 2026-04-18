using Hand.Rule;
using Hand.Rule.Logics;

namespace MemberValidationTests;

public class MemberRuleParserTests
{
    [Theory]
    [InlineData("", typeof(TrueLogic<string>))]
    [InlineData(null, typeof(TrueLogic<string>))]
    [InlineData("ALL", typeof(TrueLogic<string>))]
    [InlineData("all", typeof(TrueLogic<string>))]
    [InlineData("All", typeof(TrueLogic<string>))]
    [InlineData("Empty", typeof(FalseLogic<string>))]
    [InlineData("empty", typeof(FalseLogic<string>))]
    [InlineData("Include: Id Name", typeof(IncludedRule<string>))]
    [InlineData("Id Name", typeof(IncludedRule<string>))]
    [InlineData("Exclude: Id", typeof(NotLogic<string>))]
    public void Parse(string? text, Type expected)
    {
        var rule = MemberRuleParser.Default.Parse(text);
        Assert.NotNull(rule);
        Assert.IsType(expected, rule);
    }
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("ALL")]
    [InlineData("all")]
    [InlineData("All")]
    public void Default(string? text)
    {
        var rule = MemberRuleParser.Default.Parse(text);
        Assert.NotNull(rule);
        Assert.IsType<TrueLogic<string>>(rule);
        Assert.True(rule.Validate("Id"));
    }
    [Theory]
    [InlineData("Empty")]
    [InlineData("empty")]
    public void Empty(string? text)
    {
        var rule = MemberRuleParser.Default.Parse(text);
        Assert.NotNull(rule);
        Assert.IsType<FalseLogic<string>>(rule);
        Assert.False(rule.Validate("Id"));
    }
    [Fact]
    public void Include()
    {
        var rule = MemberRuleParser.Default.Parse("Include: Id Name");
        Assert.NotNull(rule);
        Assert.True(rule.Validate("Id"));
        Assert.True(rule.Validate("Name"));
        Assert.False(rule.Validate("Other"));
    }
    [Fact]
    public void Exclude()
    {
        var rule = MemberRuleParser.Default.Parse("Exclude: Id Name");
        Assert.NotNull(rule);
        Assert.False(rule.Validate("Id"));
        Assert.False(rule.Validate("Name"));
        Assert.True(rule.Validate("Other"));
    }
    [Fact]
    public void IncludePrefix()
    {
        var parser = new MemberRuleParser("I:", "E:", [' '], StringComparer.Ordinal);
        var rule = parser.Parse("I: Id Name"); 
        Assert.NotNull(rule);
        Assert.True(rule.Validate("Id"));
        Assert.True(rule.Validate("Name"));
        Assert.False(rule.Validate("Other"));
    }
    [Fact]
    public void ExcludePrefix()
    {
        var parser = new MemberRuleParser("I:", "E:", [' '], StringComparer.Ordinal);
        var rule = parser.Parse("E: Id Name");
        Assert.NotNull(rule);
        Assert.False(rule.Validate("Id"));
        Assert.False(rule.Validate("Name"));
        Assert.True(rule.Validate("Other"));
    }
    [Fact]
    public void Separators()
    {
        var parser = new MemberRuleParser("I:", "E:", [',', ' '], StringComparer.Ordinal);
        var rule = parser.Parse("I: Id, Name");
        Assert.NotNull(rule);
        Assert.True(rule.Validate("Id"));
        Assert.True(rule.Validate("Name"));
        Assert.False(rule.Validate("Other"));
    }
}
