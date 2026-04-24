using Hand;
using Hand.Maping.Recognizers;
using Hand.Rule;
using Hand.Rule.Logics;

namespace MemberValidationTests;

public class RecognizeValidationParserTests
{
    [Theory]
    [InlineData("Filter: Include: Id Name", typeof(IncludedRule<string>))]
    [InlineData("Filter: Id Name", typeof(IncludedRule<string>))]
    [InlineData("Filter: Exclude: Id", typeof(NotLogic<string>))]
    public void ParseValidation(string? text, Type expected)
    {
        if (MemberRecognizeParser.Default.Parse(text) is not ValidationRecognizer<string> recognizer)
        {
            Assert.Fail();
            return;
        }
        var rule = recognizer.Validation;
        Assert.NotNull(rule);
        Assert.IsType(expected, rule);
    }
    [Theory]
    [InlineData("Include: Id Name", typeof(IncludedRule<string>))]
    [InlineData("Id Name", typeof(IncludedRule<string>))]
    [InlineData("Exclude: Id", typeof(NotLogic<string>))]
    public void Parse(string? text, Type expected)
    {
        if (MemberRecognizeParser.Default.Parse(text) is not ValidationRecognizer<string> recognizer)
        {
            Assert.Fail();
            return;
        }
        var rule = recognizer.Validation;
        Assert.NotNull(rule);
        Assert.IsType(expected, rule);
    }
    [Fact]
    public void Include()
    {
        if (MemberRecognizeParser.Default.Parse("Include: Id Name") is not ValidationRecognizer<string> recognizer)
        {
            Assert.Fail();
            return;
        }
        var rule = recognizer.Validation;
        Assert.NotNull(rule);
        Assert.True(rule.Validate("Id"));
        Assert.True(rule.Validate("Name"));
        Assert.False(rule.Validate("Other"));
    }
    [Fact]
    public void Exclude()
    {
        if (MemberRecognizeParser.Default.Parse("Exclude: Id Name") is not ValidationRecognizer<string> recognizer)
        {
            Assert.Fail();
            return;
        }
        var rule = recognizer.Validation;
        Assert.NotNull(rule);
        Assert.False(rule.Validate("Id"));
        Assert.False(rule.Validate("Name"));
        Assert.True(rule.Validate("Other"));
    }
    [Fact]
    public void IncludePrefix()
    {
        var parser = MemberRecognizeParser.CreateDefault("C:", "T:", "F:","I:", "E:", [' '], StringComparison.Ordinal);
        if (parser.Parse("I: Id Name") is not ValidationRecognizer<string> recognizer)
        {
            Assert.Fail();
            return;
        }
        var rule = recognizer.Validation;
        Assert.NotNull(rule);
        Assert.True(rule.Validate("Id"));
        Assert.True(rule.Validate("Name"));
        Assert.False(rule.Validate("Other"));
    }
    [Fact]
    public void ExcludePrefix()
    {
        var parser = MemberRecognizeParser.CreateDefault("C:", "T:", "F:","I:", "E:", [' '], StringComparison.Ordinal);
        if (parser.Parse("E: Id Name") is not ValidationRecognizer<string> recognizer)
        {
            Assert.Fail();
            return;
        }
        var rule = recognizer.Validation;
        Assert.NotNull(rule);
        Assert.False(rule.Validate("Id"));
        Assert.False(rule.Validate("Name"));
        Assert.True(rule.Validate("Other"));
    }
    [Fact]
    public void Separators()
    {
        var parser = MemberRecognizeParser.CreateDefault("C:", "T:", "F:", "I:", "E:", [',', ' '], StringComparison.Ordinal);
        if (parser.Parse("I: Id, Name") is not ValidationRecognizer<string> recognizer)
        {
            Assert.Fail();
            return;
        }
        var rule = recognizer.Validation;
        Assert.NotNull(rule);
        Assert.True(rule.Validate("Id"));
        Assert.True(rule.Validate("Name"));
        Assert.False(rule.Validate("Other"));
    }
}
