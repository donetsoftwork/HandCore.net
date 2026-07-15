namespace ParseXmlTests.Supports;

public class Comment
{
    public string Summary { get; set; }
    public Dictionary<string, string> TypeParams { get; set; } = [];
    public Dictionary<string, string> Params { get; set; } = [];
    public string Returns { get; set; }
    public void AddTypeParam(string name, string value)
        => TypeParams.Add(name, value);
    public void AddParam(string name, string value)
        => Params.Add(name, value);
}
