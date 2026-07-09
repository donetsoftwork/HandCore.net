using Hand.Models;

namespace ParseXmlTests.Supports;

public readonly record struct UserId(long Original) : IEntityId;
