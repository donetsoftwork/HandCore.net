namespace ParseXmlTests.Supports;

public class Rss
{
    public string Title { get; set; } = string.Empty;
    public RssImage Image { get; set; }

    public RssItem[] Items { get; set; }
}

public class RssImage
{
    public string Url { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

public class RssItem
{
    public string Title { get; set; } = string.Empty;
    public string Link { get; set; } = string.Empty;
}