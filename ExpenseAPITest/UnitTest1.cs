namespace ExpenseAPITest;
// 這個是用來測試ExpenseAPI/Controllers/ExpenseController.cs的單元測試
// 只需要測試POST方法
// 使用AAA模式（Arrange, Act, Assert）
// 每一個方法至少提供 3 個測試案例，分別是正常情況、異常情況和邊界情況
using ExpenseAPI.Controllers;
using ExpenseAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Xunit;
public class ExpenseControllerTests
{
    private readonly Mock<ILogger<ExpenseController>> _loggerMock;
    private readonly ExpenseController _controller;
    private readonly DbContextOptions<ExpenseContext> _options;

    public ExpenseControllerTests()
    {
        _loggerMock = new Mock<ILogger<ExpenseController>>();
        _options = new DbContextOptionsBuilder<ExpenseContext>()
            .UseInMemoryDatabase(databaseName: "ExpenseDB")
            .Options;

        var context = new ExpenseContext(_options);
        _controller = new ExpenseController(context, _loggerMock.Object);
    }

    [Fact]
    public async Task Post_ValidExpense_ReturnsCreatedAtActionResult()
    {
        // Arrange
        var expense = new Expense
        {
            Description = "Lunch",
            Amount = 15.00M,
            Date = DateTime.Now,
            Category = "Food"
        };

        // Act
        var result = await _controller.PostExpense(expense);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal("GetExpense", createdAtActionResult.ActionName);
        Assert.Equal(expense, createdAtActionResult.Value);
    }

    [Fact]
    public async Task Post_DescriptionLunchBefore11AM_ReturnsBadRequest()
    {
        // Arrange
        var expense = new Expense
        {
            Description = "Lunch",
            Amount = 15.00M,
            Date = DateTime.Today.AddHours(10), // Before 11 AM
            Category = "Food"
        };

        // Act
        var result = await _controller.PostExpense(expense);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("This Time Can not buy Lunch", badRequestResult.Value);
    }

    [Fact]
    public async Task Post_InvalidExpense_ReturnsBadRequest()
    {
        // Arrange
        var expense = new Expense
        {
            Description = null, // Invalid description
            Amount = 15.00M,
            Date = DateTime.Now,
            Category = "Food"
        };

        // Act
        var result = await _controller.PostExpense(expense);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("The Description field is required.", badRequestResult.Value);
    }
}

internal class Mock<T>
{
    public ILogger<ExpenseController> Object { get; internal set; }
}