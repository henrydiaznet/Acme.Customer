using System.Net;
using System.Net.Http.Json;
using Acme.Customers.API.Dtos;
using Acme.Customers.API.Mediatr.Commands;
using Acme.Customers.Domain.Model;
using Acme.Customers.Infrastructure;
using Acme.Customers.IntegrationTests.Utilities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Acme.Customers.IntegrationTests.Controllers;

public class OrderControllerTests: IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly HttpClient _client;

    public OrderControllerTests(CustomWebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper)
    {
        _factory = factory;
        _testOutputHelper = testOutputHelper;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
        _client.DefaultRequestHeaders.Add("Acme-Api-Key", "test");
    }
    
    [Fact]
    public async Task GetAll_Orders_ForCustomer()
    {
        // Arrange
        TestData.Init(_factory.Services);
        var orderId = 2;

        //Act
        var response = await _client.GetFromJsonAsync<IEnumerable<OrderResponse>>($"/Orders/customer/{orderId}");

        // Assert
        response.Should().NotBeNull();
        response.Count().Should().Be(2);
    }
    
    [Fact]
    public async Task Find_Order()
    {
        // Arrange
        TestData.Init(_factory.Services);
        var orderId = 4;

        //Act
        var response = await _client.GetFromJsonAsync<OrderResponse>($"/Orders/{orderId}");

        // Assert
        response.Should().NotBeNull();
        response.Id.Should().Be(orderId);
    }
    
    [Fact]
    public async Task Find_Order_404()
    {
        // Arrange
        TestData.Init(_factory.Services);
        var orderId = 1111;
        
        //Act
        var response = await _client.GetAsync($"/Orders/{orderId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact(DisplayName = "Create on Customer without current orders")]
    public async Task Create_Order_Ok()
    {
        TestData.Init(_factory.Services);

        // Arrange
        var toCreate = new CreateOrderCommand
        {
            CustomerId = 4,
            Items = new List<BasketItem> { new(1, 1), new(2, 1), new(3, 2) }
        };
        
        //Act
        var response = await _client.PutAsJsonAsync("/Orders", toCreate);
        var content = await response.Content.ReadFromJsonAsync<OrderResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        content.Should().NotBeNull();
        content.Id.Should().NotBe(0);
    }
    
    [Fact(DisplayName = "Create on Customer with existing unfulfilled orders")]
    public async Task Create_Order_NotOk()
    {
        // Arrange
        TestData.Init(_factory.Services);
        var toCreate = new CreateOrderCommand
        {
            CustomerId = 3,
            Items = new List<BasketItem> { new(1, 1), new(2, 1), new(3, 2) }
        };
        
        //Act
        var response = await _client.PutAsJsonAsync("/Orders", toCreate);
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        content.Should().NotBeNull();
        _testOutputHelper.WriteLine(content);
    }
    
    [Fact]
    public async Task Cancel_Order_Ok()
    {
        // Arrange
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var db = scopedServices.GetRequiredService<CustomerContext>();
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
        var customerWithOrder = new Customer
        {
            FullName = "I have Order"
        };
        customerWithOrder.AddOrder(new Order(new List<OrderItem> { new() {ItemId = 1, Quantity = 2}}));
        var cust = db.Customers.Add(customerWithOrder);
        db.SaveChanges();
        
        var customerId = cust.Entity.Id;
        var orderId = cust.Entity.Orders.FirstOrDefault().Id;

        var cancel = new CancelOrderCommand
        {
            CustomerId = customerId,
            OrderId = orderId
        };
        
        //Act
        var response = await _client.PostAsJsonAsync("/Orders", cancel);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}