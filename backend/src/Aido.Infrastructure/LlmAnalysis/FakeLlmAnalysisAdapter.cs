using Aido.Application.LlmAnalysis;
using Aido.Core;
using Aido.SharedKernel;

namespace Aido.Infrastructure.LlmAnalysis;

/// <summary>
/// Fake implementation of LlmAnalysisPort for testing and development purposes.
/// Generates simple continuation suggestions based on existing todo items.
/// </summary>
public class FakeLlmAnalysisAdapter : LlmAnalysisPort
{
    private readonly string[] _commonSuggestions = new[]
    {
        "Buy groceries",
        "Schedule meeting with team",
        "Review project documents",
        "Call customer support",
        "Update software",
        "Prepare presentation",
        "Send follow-up email",
        "Organize workspace",
        "Book appointment",
        "Plan weekend activities",
        "Complete expense report",
        "Read industry news",
        "Backup important files",
        "Exercise for 30 minutes",
        "Pay monthly bills"
    };

    public Task<Result<List<string>>> GetListContinuationSuggestions(TodoList todoList, int maxSuggestionCount = 3, 
        CancellationToken cancellationToken = default)
    {
        if (todoList == null)
            return Task.FromResult(Result<List<string>>.Failure("TodoList cannot be null"));

        if (maxSuggestionCount < 1)
            return Task.FromResult(Result<List<string>>.Failure("maxSuggestionCount must be at least 1"));

        List<string> suggestions = GenerateSuggestions(todoList, maxSuggestionCount).ToList();
        
        return Task.FromResult(Result<List<string>>.Success(suggestions));
    }

    private IEnumerable<string> GenerateSuggestions(TodoList todoList, int maxCount)
    {
        var existingTitles = todoList.Items.Select(item => item.Title.ToLowerInvariant()).ToHashSet();
        var suggestions = new List<string>();
        
        // Try to generate contextual suggestions based on existing items
        foreach (var item in todoList.Items.Take(3))
        {
            var contextualSuggestion = GenerateContextualSuggestion(item.Title, item.Description);
            if (!string.IsNullOrEmpty(contextualSuggestion) && 
                !existingTitles.Contains(contextualSuggestion.ToLowerInvariant()))
            {
                suggestions.Add(contextualSuggestion);
            }
        }
        
        // Fill remaining slots with common suggestions
        var random = new Random();
        var availableCommonSuggestions = _commonSuggestions
            .Where(s => !existingTitles.Contains(s.ToLowerInvariant()))
            .OrderBy(_ => random.Next())
            .ToList();
            
        suggestions.AddRange(availableCommonSuggestions.Take(maxCount - suggestions.Count));
        
        return suggestions.Take(maxCount);
    }

    private string GenerateContextualSuggestion(string title, string description)
    {
        var titleLower = title.ToLowerInvariant();
        var descriptionLower = description.ToLowerInvariant();
        
        // Generate contextual suggestions based on keywords
        if (titleLower.Contains("meeting") || descriptionLower.Contains("meeting"))
            return "Send meeting agenda";
        
        if (titleLower.Contains("project") || descriptionLower.Contains("project"))
            return "Update project status";
        
        if (titleLower.Contains("email") || descriptionLower.Contains("email"))
            return "Check email responses";
        
        if (titleLower.Contains("buy") || titleLower.Contains("purchase"))
            return "Compare prices online";
        
        if (titleLower.Contains("call") || titleLower.Contains("phone"))
            return "Add contact to phone book";
        
        if (titleLower.Contains("book") || titleLower.Contains("appointment"))
            return "Confirm appointment details";
        
        return string.Empty;
    }
}
