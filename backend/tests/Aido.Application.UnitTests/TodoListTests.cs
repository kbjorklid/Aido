using Aido.Core;

namespace Aido.Application.UnitTests;

public class TodoListTests
{

    [Fact]
    public void AddItem()
    {
        // Arrange
        TodoList list = new TodoList("name", "description"); 
        
        // Act
        list.AddItem("item1", "description1", false);
        
        // Assert
        TodoItem todoItem = Assert.Single(list.Items);
        Assert.Equal("item1", todoItem.Title);
        Assert.Equal("description1", todoItem.Description);
    }
}
