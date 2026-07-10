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
            .WithItem("name", nameof(RssImage.Name));
        var itemParser = config.Entity<RssItem>()
            .WithItem("title", nameof(RssItem.Title))
            .WithItem("link", nameof(RssItem.Link))
            .Repeat("item");
        var rssParser = config.Entity<Rss>()
            .WithItem<string>("title", nameof(Rss.Title))
            .WithItem(imageParser, "image", nameof(Rss.Image))
            .WithRepeat(itemParser, nameof(Rss.Items));
        Rss rss = rssParser.Get(xmlReader);
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
            .Repeat("item")
            .Convert(static list => list.ToArray());
        var items = itemParser.Parse(xmlReader);
        Assert.NotNull(items);
        Assert.Equal(10, items.Length);
    }
}
