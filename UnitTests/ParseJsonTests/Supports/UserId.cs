using Hand.Models;

namespace ParseJsonTests.Supports;

public readonly record struct UserId(long Original) : IEntityId;
