using BenchmarkDotNet.Attributes;
using Hand.ParseXml;
using Hand.ParseXml.Move;

namespace ParseXmlBench;

[MemoryDiagnoser, SimpleJob(launchCount: 2, warmupCount: 10, iterationCount: 10, invocationCount: 1000000)]
public class ElementContentBench
{
    private static readonly FirstReader<string> _firstReader = HandXml.Default.Content()
        .Element("summary")
        .First();
    private static readonly MoveToParser<string> _moveToReader = HandXml.Default.Content()
        .MoveTo("summary");
    private static readonly string _text = @"
        <member name = ""F:GenerateConvertTests.Supports.ColumnType.Identity"">
            <summary>
            自增列
            </summary>
        </member>";

    [Benchmark(Baseline = true)]
    public string First()
        => _firstReader.Parse(_text);

    [Benchmark]
    public string MoveTo()
        => _moveToReader.Parse(_text);
}
