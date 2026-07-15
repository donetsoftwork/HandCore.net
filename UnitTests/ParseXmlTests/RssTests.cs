using Hand.ParseXml;
using ParseXmlTests.Supports;
using System.Xml;

namespace ParseXmlTests;

public class RssTests
{
    [Fact]
    public void Complex()
    {
        using var fs = new FileStream("rss.xml", FileMode.Open, FileAccess.Read);
        using var xmlReader = XmlReader.Create(fs);
        var config = HandXml.Default;
        var imageParser = config.Entity<RssImage>()
            .WithItem("url", nameof(RssImage.Url))
            .WithItem("title", nameof(RssImage.Title));
        var itemParser = config.Entity<RssItem>()
            .WithItem("title", nameof(RssItem.Title))
            .WithItem("link", nameof(RssItem.Link))
            .Element("item")
            .Each()
            .ToArray();
        var rssParser = config.Entity<Rss>()
            .WithItem<string>("title", nameof(Rss.Title))
            .WithItem(imageParser, "image", nameof(Rss.Image))
            .WithItem(itemParser, "item", nameof(Rss.Items))
            .First();
        Rss rss = rssParser.Parse(xmlReader);
        Assert.NotNull(rss);
    }
    [Fact]
    public void Convert()
    {
        using var fs = new FileStream("rss.xml", FileMode.Open, FileAccess.Read);
        using var xmlReader = XmlReader.Create(fs);
        var config = HandXml.Default;
        var itemParser = config.Entity<RssItem>()
            .WithItem("title", nameof(RssItem.Title))
            .WithItem("link", nameof(RssItem.Link))
            .Element("item")
            .Each()
            .Convert(static list => list.ToArray());
        var items = itemParser.Parse(xmlReader);
        Assert.NotNull(items);
        Assert.Equal(10, items.Length);
    }
}
