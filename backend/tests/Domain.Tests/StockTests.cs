using UGetMore.Domain.Entities;
using Xunit;

namespace UGetMore.Domain.Tests;

public class StockTests
{
    [Fact]
    public void NewStock_HasFullQuantityAvailable()
    {
        var stock = new Stock(Guid.NewGuid(), quantityOnHand: 10);

        Assert.Equal(10, stock.QuantityAvailable);
    }

    [Fact]
    public void Reserve_ReducesQuantityAvailable_ButNotQuantityOnHand()
    {
        var stock = new Stock(Guid.NewGuid(), quantityOnHand: 10);

        stock.Reserve(4);

        Assert.Equal(10, stock.QuantityOnHand);
        Assert.Equal(4, stock.QuantityReserved);
        Assert.Equal(6, stock.QuantityAvailable);
    }

    [Fact]
    public void Reserve_MoreThanAvailable_Throws()
    {
        var stock = new Stock(Guid.NewGuid(), quantityOnHand: 5);

        var ex = Assert.Throws<InvalidOperationException>(() => stock.Reserve(6));
        Assert.Contains("only 5 available", ex.Message);
    }

    [Fact]
    public void Reserve_ExactlyAllAvailable_Succeeds()
    {
        var stock = new Stock(Guid.NewGuid(), quantityOnHand: 5);

        stock.Reserve(5);

        Assert.Equal(0, stock.QuantityAvailable);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Reserve_NonPositiveQuantity_Throws(int quantity)
    {
        var stock = new Stock(Guid.NewGuid(), quantityOnHand: 10);

        Assert.Throws<ArgumentOutOfRangeException>(() => stock.Reserve(quantity));
    }

    [Fact]
    public void Release_GivesBackReservedQuantity()
    {
        var stock = new Stock(Guid.NewGuid(), quantityOnHand: 10);
        stock.Reserve(4);

        stock.Release(4);

        Assert.Equal(0, stock.QuantityReserved);
        Assert.Equal(10, stock.QuantityAvailable);
    }

    [Fact]
    public void Release_MoreThanReserved_Throws()
    {
        var stock = new Stock(Guid.NewGuid(), quantityOnHand: 10);
        stock.Reserve(2);

        Assert.Throws<InvalidOperationException>(() => stock.Release(3));
    }

    [Fact]
    public void Commit_RemovesFromBothReservedAndOnHand()
    {
        var stock = new Stock(Guid.NewGuid(), quantityOnHand: 10);
        stock.Reserve(4);

        stock.Commit(4);

        Assert.Equal(6, stock.QuantityOnHand);
        Assert.Equal(0, stock.QuantityReserved);
        Assert.Equal(6, stock.QuantityAvailable);
    }

    [Fact]
    public void Commit_MoreThanReserved_Throws()
    {
        var stock = new Stock(Guid.NewGuid(), quantityOnHand: 10);
        stock.Reserve(2);

        Assert.Throws<InvalidOperationException>(() => stock.Commit(3));
    }

    [Fact]
    public void Constructor_NegativeQuantityOnHand_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Stock(Guid.NewGuid(), quantityOnHand: -1));
    }
}
