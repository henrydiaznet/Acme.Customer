using System.Net;
using System.Net.Http.Json;
using Acme.Customers.API.Dtos;
using Acme.Customers.API.Extensions;
using Acme.Customers.API.Mediatr.Commands;
using Acme.Customers.Domain.Model;
using Acme.Customers.Infrastructure;
using Acme.Customers.IntegrationTests.Utilities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Acme.Customers.IntegrationTests.Controllers;

public class CustomerControllerTests: IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;
    
    public CustomerControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
        _client.DefaultRequestHeaders.Add("Acme-Api-Key", "test");
        
    }

    [Fact]
    public async Task GetAll_Customers()
    {
        // Arrange
        TestData.Init(_factory.Services);
        
        //Act
        var response = await _client.GetFromJsonAsync<IEnumerable<CustomerShortResponse>>("/customers");

        // Assert
        response.Should().NotBeNullOrEmpty();
    }
    
    [Fact]
    public async Task Find_Customer()
    {
        // Arrange
        TestData.Init(_factory.Services);
        var customerId = 2;

        //Act
        var response = await _client.GetFromJsonAsync<CustomerFullResponse>($"/customers/{customerId}");

        // Assert
        response.Should().NotBeNull();
        response.Email.Should().NotBeNull();
        response.Phone.Should().NotBeNull();
        response.Orders.Should().NotBeNull();
    }
    
    [Fact]
    public async Task Find_Customer_404()
    {
        // Arrange
        TestData.Init(_factory.Services);
        var customerId = 100;
        
        //Act
        var response = await _client.GetAsync($"/customers/{customerId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task Create_Customer()
    {
        // Arrange
        
        var customerToCreate = new CreateCustomerCommand
        {
            Address = "Address st., City, ZIP code",
            Email = "email@email.com",
            FullName = "John Smith",
            Phone = "(1400)123-1234"
        };

        //Act
        var response = await _client.PutAsJsonAsync("/customers", customerToCreate);
        var result = await response.Content.ReadFromJsonAsync<CustomerFullResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        result.Should().NotBeNull();
        result.Id.Should().NotBe(0);
    }
    
    [Fact]
    public async Task Update_Customer()
    {
        // Arrange
        TestData.Init(_factory.Services);
        var toUpdate = new UpdateCustomerCommand
        {
            Address = "Address st., City, ZIP code",
            FullName = "John Smith",
            Phone = "(1400)123-1234"
        };
        
        var customerId = 2;

        //Act
        var response = await _client.PutAsJsonAsync($"/customers/{customerId}", toUpdate);
        var result = await _client.GetFromJsonAsync<CustomerFullResponse>($"/customers/{customerId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
        result.Id.Should().Be(customerId);
        result.FullName.Should().Be(toUpdate.FullName);
        result.Phone.Should().Be(toUpdate.Phone);
        result.Address.Should().Be(toUpdate.Address);
    }

    [Fact] public async Task Delete_Customer()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var db = scopedServices.GetRequiredService<CustomerContext>();
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
        var toDelete = new Customer
        {
            FullName = "Delete Me"
        };
        var cust = db.Customers.Add(toDelete);
        db.SaveChanges();        
        
        var customerId = cust.Entity.Id;

        //Act
        var response = await _client.DeleteAsync($"/customers/{customerId}");
        var result = await _client.GetAsync($"/customers/{customerId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}