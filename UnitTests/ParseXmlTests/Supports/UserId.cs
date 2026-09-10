using Hand.Primitives;

namespace ParseXmlTests.Supports;

public readonly record struct UserId(long Original) : IEntityId;
