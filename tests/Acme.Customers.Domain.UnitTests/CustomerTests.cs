using Acme.Customers.Domain.Exceptions;

namespace Acme.Customers.Domain.UnitTests;

public class CustomerTests
{
    private static readonly Fixture Fixture = new();
    
    static CustomerTests()
    {
        Fixture.Customize<Order>(c => c.FromFactory(() => new Order(Fixture.Build<OrderItem>()
            .Without(x => x.Order).CreateMany(5))));
    }

    [Theory]
    [MemberData(nameof(GetCustomersValidToAddOrders))]
    public void Customer_AddOrder_Ok(Customer sut, Order order)
    {
        //arrange

        //act
        sut.AddOrder(order);

        //assert
        sut.Orders.Should().NotBeEmpty();
        sut.Orders.Where(x => x.Status == OrderStatus.Processing).Should().NotBeEmpty();
    }

    [Fact]
    public void Customer_AddOrder_Throws_AddingSameOrder()
    {
        //arrange
        var duplicateOrder = new Order { Id = 1 };
        duplicateOrder.FulfillOrder();
        var sut = new Customer(Fixture.Create<string>(), Fixture.Create<ContactInformation>(), new List<Order>
        {
            duplicateOrder
        });
        
        //act
        
        //assert
        Assert.Throws<OrderException>(() => sut.AddOrder(duplicateOrder));
    }
    
    [Fact]
    public void Customer_AddOrder_Throws_AlreadyProcessingOrder()
    {
        //arrange
        var processingOrder = new Order();
        var sut = new Customer(Fixture.Create<string>(), Fixture.Create<ContactInformation>(), new List<Order>
        {
            processingOrder
        });
        var newOrder = new Order();
        
        //act
        
        //assert
        Assert.Throws<OrderException>(() => sut.AddOrder(newOrder));
    }
    
    [Fact]
    public void Customer_FulfillOrder_Ok()
    {
        //arrange
        var processingOrder = new Order { Id = 1 };
        var sut = new Customer(Fixture.Create<string>(), Fixture.Create<ContactInformation>(), new List<Order>
        {
            processingOrder
        });
        
        //act
        sut.FulfillOrder(1);
        
        //assert
        sut.Orders.All(x => x.Status == OrderStatus.Fulfilled).Should().BeTrue();
    }
    
    [Fact]
    public void Customer_FulfillOrder_Throws_OnAlreadyCanceled()
    {
        //arrange
        var alreadyCanceledOrder = new Order { Id = 1 };
        alreadyCanceledOrder.FulfillOrder();
        var sut = new Customer(Fixture.Create<string>(), Fixture.Create<ContactInformation>(), new List<Order>
        {
            alreadyCanceledOrder
        });
        
        //act
        
        //assert
        Assert.Throws<OrderException>(() => sut.FulfillOrder(1));
    }
    
    [Fact]
    public void Customer_FulfillOrder_Throws_NonExistentOrder()
    {
        //arrange
        var alreadyCanceledOrder = new Order { Id = 1 };
        alreadyCanceledOrder.FulfillOrder();
        var sut = new Customer(Fixture.Create<string>(), Fixture.Create<ContactInformation>(), new List<Order>
        {
            alreadyCanceledOrder
        });
        
        //act
        
        //assert
        Assert.Throws<OrderException>(() => sut.FulfillOrder(2));
    }
    
    [Fact]
    public void Customer_CancelOrder_Ok()
    {
        //arrange
        var processingOrder = new Order { Id = 1 };
        var sut = new Customer(Fixture.Create<string>(), Fixture.Create<ContactInformation>(), new List<Order>
        {
            processingOrder
        });
        
        //act
        sut.CancelOrder(1);
        
        //assert
        sut.Orders.All(x => x.Status == OrderStatus.Cancelled).Should().BeTrue();
    }
    
    [Fact]
    public void Customer_CancelOrder_Throws_OnAlreadyCanceled()
    {
        //arrange
        var alreadyCanceledOrder = new Order { Id = 1 };
        alreadyCanceledOrder.CancelOrder();
        var sut = new Customer(Fixture.Create<string>(), Fixture.Create<ContactInformation>(), new List<Order>
        {
            alreadyCanceledOrder
        });
        
        //act
        
        //assert
        Assert.Throws<OrderException>(() => sut.CancelOrder(1));
    }
    
    [Fact]
    public void Customer_CancelOrderOrder_Throws_NonExistentOrder()
    {
        //arrange
        var processingOrder = new Order { Id = 1 };
        processingOrder.FulfillOrder();
        var sut = new Customer(Fixture.Create<string>(), Fixture.Create<ContactInformation>(), new List<Order>
        {
            processingOrder
        });
        
        //act
        
        //assert
        Assert.Throws<OrderException>(() => sut.CancelOrder(2));
    }

    public static IEnumerable<object[]> GetCustomersValidToAddOrders()
    {
        yield return new object[]
        {
            new Customer(),
            Fixture.Create<Order>()
        };

        var cancelledOrder = Fixture.Create<Order>();
        cancelledOrder.CancelOrder();

        var fullFilledOrder = Fixture.Create<Order>();
        fullFilledOrder.FulfillOrder();
        
        var customerWithOrder = new Customer(Fixture.Create<string>(), Fixture.Create<ContactInformation>(), new List<Order>
        {
            cancelledOrder,
            fullFilledOrder
        });
        
        yield return new object[]
        {
            customerWithOrder,
            Fixture.Create<Order>()
        };
    }
}