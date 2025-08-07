using Aido.Application.LlmAnalysis;
using Aido.Core;
using Aido.SharedKernel;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System.Text;

namespace Aido.Infrastructure.LlmAnalysis;

public class LlmAnalysisAdapter : LlmAnalysisPort
{
    private readonly Kernel _kernel;
    private readonly ILogger<LlmAnalysisAdapter> _logger;

    public LlmAnalysisAdapter(Kernel kernel, ILogger<LlmAnalysisAdapter> logger)
    {
        _kernel = kernel ?? throw new ArgumentNullException(nameof(kernel));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Result<List<string>>> GetListContinuationSuggestions(TodoList todoList, int maxSuggestionCount = 3,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        try
        {
            var prompt = BuildPrompt(todoList, maxSuggestionCount);
            
            var executionSettings = new OpenAIPromptExecutionSettings
            {
                MaxTokens = 500,
                Temperature = 0.7,
                TopP = 0.9
            };

            var kernelArguments = new KernelArguments(executionSettings);
            var result = await _kernel.InvokePromptAsync(prompt, kernelArguments);
            var response = result.GetValue<string>() ?? string.Empty;

            var suggestions = ParseSuggestions(response, maxSuggestionCount);
            
            _logger.LogInformation("Generated {Count} suggestions for todo list '{ListName}'", 
                suggestions.Count, todoList.Name);

            return Result<List<string>>.Success(suggestions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate AI suggestions for todo list '{ListName}'", todoList.Name);
            return Result<List<string>>.Failure($"Failed to generate suggestions: {ex.Message}");
        }
    }

    private static string BuildPrompt(TodoList todoList, int maxSuggestionCount)
    {
        var promptBuilder = new StringBuilder();
        
        promptBuilder.AppendLine("You are an AI assistant that helps users continue their todo lists with relevant suggestions.");
        promptBuilder.AppendLine("Based on the existing todo list below, suggest additional items that would logically fit.");
        promptBuilder.AppendLine();
        promptBuilder.AppendLine($"Todo List Name: {todoList.Name}");
        
        if (!string.IsNullOrWhiteSpace(todoList.Description))
        {
            promptBuilder.AppendLine($"Description: {todoList.Description}");
        }
        
        promptBuilder.AppendLine();
        promptBuilder.AppendLine("Existing items:");
        
        var completedItems = todoList.Items.Where(i => i.IsCompleted).ToList();
        var pendingItems = todoList.Items.Where(i => !i.IsCompleted).ToList();
        
        if (todoList.Items.Any())
        {
            foreach (var item in todoList.Items.Take(20))
            {
                promptBuilder.AppendLine($"- {item.Title}");
            }
        }
        
        promptBuilder.AppendLine();
        promptBuilder.AppendLine($"Please suggest up to {maxSuggestionCount} additional todo items that would naturally fit with this list.");
        promptBuilder.AppendLine("Consider the theme, context, and progression of the existing items.");
        promptBuilder.AppendLine("Format your response as a numbered list with only the item titles, one per line:");
        promptBuilder.AppendLine("1. First suggestion");
        promptBuilder.AppendLine("2. Second suggestion");
        promptBuilder.AppendLine("3. Third suggestion");
        
        return promptBuilder.ToString();
    }

    private static List<string> ParseSuggestions(string response, int maxSuggestionCount)
    {
        var suggestions = new List<string>();
        var lines = response.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        
        foreach (var line in lines)
        {
            var trimmedLine = line.Trim();
            
            // Look for numbered list items (1. 2. 3. etc.) or bullet points
            if (System.Text.RegularExpressions.Regex.IsMatch(trimmedLine, @"^\d+\.\s*(.+)"))
            {
                var match = System.Text.RegularExpressions.Regex.Match(trimmedLine, @"^\d+\.\s*(.+)");
                if (match.Success)
                {
                    suggestions.Add(match.Groups[1].Value.Trim());
                }
            }
            else if (trimmedLine.StartsWith("- ") || trimmedLine.StartsWith("• "))
            {
                suggestions.Add(trimmedLine.Substring(2).Trim());
            }
            
            if (suggestions.Count >= maxSuggestionCount)
                break;
        }
        
        // Fallback: if no numbered items found, try to extract meaningful lines
        if (suggestions.Count == 0)
        {
            foreach (var line in lines)
            {
                var trimmedLine = line.Trim();
                if (!string.IsNullOrEmpty(trimmedLine) && 
                    !trimmedLine.StartsWith("Here") && 
                    !trimmedLine.StartsWith("Based") &&
                    trimmedLine.Length > 3)
                {
                    suggestions.Add(trimmedLine);
                    if (suggestions.Count >= maxSuggestionCount)
                        break;
                }
            }
        }
        
        return suggestions.Take(maxSuggestionCount).ToList();
    }
}
