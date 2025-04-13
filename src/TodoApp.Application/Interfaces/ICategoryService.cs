namespace TodoApp.Application.Interfaces;

public interface ICategoryService
{
    /// <summary>
    /// Gets all available categories.
    /// </summary>
    /// <returns>A list of all categories.</returns>
    List<string> GetAllCategories();
}