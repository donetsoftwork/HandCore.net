using Hand.Rule;

namespace Hand.ValidationTests;

public class IncludedRuleTests
{
    [Theory]
    [MemberData(nameof(TestDataSource))]
    [ClassData(typeof(TestDataProvider))]
    public void Validate(TestData data)
    {
        var rule = Logic.Included(data.Members);
        Assert.Equal(data.Expected, rule.Validate(data.Argument));
    }

    public static TheoryData<TestData> TestDataSource =
    [
        new TestData("Name", false, "Id"),
        new TestData("Id", true, "Id", "Name")
    ];
    public record TestData(string Argument, bool Expected, params string[] Members);
    public class TestDataProvider : TheoryData<TestData>
    {
        public TestDataProvider()
        {
            Add(new TestData("Id", false, "Name"));
            Add(new TestData("Name", true, "Id", "Name"));
        }
    }
}
