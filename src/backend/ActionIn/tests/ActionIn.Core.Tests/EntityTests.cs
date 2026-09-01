using ActionIn.Core.DomainObjects;
using Xunit;

namespace ActionIn.Core.Tests;

public class EntityTests
{
    private class FakeEntity : Entity
    {
    }

    private class AnotherFakeEntity : Entity
    {
    }

    [Fact]
    public void Constructor_WhenCalled_GeneratesNonEmptyId()
    {
        var entity = new FakeEntity();
        Assert.NotEqual(Guid.Empty, entity.Id);
    }

    [Fact]
    public void Equals_SameReference_ReturnsTrue()
    {
        var entity = new FakeEntity();
        var result = entity.Equals(entity);
        Assert.True(result);
    }

    [Fact]
    public void Equals_SameIdDifferentInstance_ReturnsTrue()
    {
        var entity1 = new FakeEntity();
        var entity2 = new FakeEntity { Id = entity1.Id };

        var result = entity1.Equals(entity2);
        Assert.True(result);
    }

    [Fact]
    public void Equals_DifferentId_ReturnsFalse()
    {
        var entity1 = new FakeEntity();
        var entity2 = new FakeEntity();

        var result = entity1.Equals(entity2);
        Assert.False(result);
    }

    [Fact]
    public void Equals_ComparedToNull_ReturnsFalse()
    {
        var entity = new FakeEntity();
        var result = entity.Equals(null);
        Assert.False(result);
    }

    [Fact]
    public void Equals_DifferentTypeSameId_ReturnsTrue()
    {
        var entity1 = new FakeEntity();
        var entity2 = new AnotherFakeEntity { Id = entity1.Id };

        var result = entity1.Equals(entity2);
        Assert.True(result);
    }

    [Fact]
    public void EqualityOperator_BothNull_ReturnsTrue()
    {
        FakeEntity? entity1 = null;
        FakeEntity? entity2 = null;

        var result = entity1 == entity2;
        Assert.True(result);
    }

    [Fact]
    public void EqualityOperator_BothNonNullSameId_ReturnsTrue()
    {
        var entity1 = new FakeEntity();
        var entity2 = new FakeEntity { Id = entity1.Id };

        var result = entity1 == entity2;
        Assert.True(result);
    }

    [Fact]
    public void EqualityOperator_BothNonNullDifferentId_ReturnsFalse()
    {
        var entity1 = new FakeEntity();
        var entity2 = new FakeEntity();

        var result = entity1 == entity2;
        Assert.False(result);
    }

    [Fact]
    public void EqualityOperator_OneNull_ReturnsFalse()
    {
        var entity1 = new FakeEntity();
        FakeEntity? entity2 = null;

        var result = entity1 == entity2;
        Assert.False(result);
    }

    [Fact]
    public void EqualityOperator_SameId_ReturnsTrue()
    {
        var entity1 = new FakeEntity();
        var entity2 = new FakeEntity { Id = entity1.Id };

        var result = entity1 == entity2;
        Assert.True(result);
    }

    [Fact]
    public void InequalityOperator_DifferentId_ReturnsTrue()
    {
        var entity1 = new FakeEntity();
        var entity2 = new FakeEntity();

        var result = entity1 != entity2;
        Assert.True(result);
    }

    [Fact]
    public void InequalityOperator_SameId_ReturnsFalse()
    {
        var entity1 = new FakeEntity();
        var entity2 = new FakeEntity { Id = entity1.Id };

        var result = entity1 != entity2;
        Assert.False(result);
    }

    [Fact]
    public void GetHashCode_SameId_ReturnsSameHashCode()
    {
        var entity1 = new FakeEntity();
        var entity2 = new FakeEntity { Id = entity1.Id };

        var hashCode1 = entity1.GetHashCode();
        var hashCode2 = entity2.GetHashCode();

        Assert.Equal(hashCode1, hashCode2);
    }

    [Fact]
    public void GetHashCode_DifferentId_ReturnsDifferentHashCodes()
    {
        var entity1 = new FakeEntity();
        var entity2 = new FakeEntity();

        var hashCode1 = entity1.GetHashCode();
        var hashCode2 = entity2.GetHashCode();

        Assert.NotEqual(hashCode1, hashCode2);
    }

    [Fact]
    public void ToString_ReturnsExpectedFormat()
    {
        var entity = new FakeEntity();
        var expectedString = $"FakeEntity [Id={entity.Id}]";

        var result = entity.ToString();

        Assert.Equal(expectedString, result);
    }
}
