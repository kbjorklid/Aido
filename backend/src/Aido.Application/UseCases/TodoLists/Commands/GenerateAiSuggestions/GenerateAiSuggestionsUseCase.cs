using Aido.Application.LlmAnalysis;
using Aido.Application.Repositories;
using Aido.SharedKernel;

namespace Aido.Application.UseCases.TodoLists.Commands.GenerateAiSuggestions;

public class GenerateAiSuggestionsUseCase
{
    private readonly ITodoListRepository _todoListRepository;
    private readonly LlmAnalysisPort _llmAnalysisPort;

    public GenerateAiSuggestionsUseCase(ITodoListRepository todoListRepository, LlmAnalysisPort llmAnalysisPort)
    {
        _todoListRepository = todoListRepository;
        _llmAnalysisPort = llmAnalysisPort;
    }

    public async Task<Result<GenerateAiSuggestionsResponse>> ExecuteAsync(GenerateAiSuggestionsRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            // Fetch the todo list
            var todoList = await _todoListRepository.GetByIdAsync(request.TodoListId, cancellationToken);
            if (todoList == null)
            {
                return Result.Failure<GenerateAiSuggestionsResponse>("Todo list not found");
            }

            // Get AI suggestions
            var suggestionsResult = await _llmAnalysisPort.GetListContinuationSuggestions(todoList, request.MaxSuggestions, cancellationToken);
            if (suggestionsResult.IsFailure)
            {
                return Result.Failure<GenerateAiSuggestionsResponse>($"Failed to get AI suggestions: {suggestionsResult.Error}");
            }

            // Add suggestions as todo items
            var addedSuggestions = new List<string>();
            foreach (var suggestion in suggestionsResult.Value)
            {
                if (!string.IsNullOrWhiteSpace(suggestion))
                {
                    todoList.AddItem(suggestion, string.Empty, isSuggestionFromAi: true);
                    addedSuggestions.Add(suggestion);
                }
            }

            // Update the todo list
            await _todoListRepository.UpdateAsync(todoList, cancellationToken);

            var response = new GenerateAiSuggestionsResponse(addedSuggestions.Count, addedSuggestions);
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            return Result.Failure<GenerateAiSuggestionsResponse>($"An error occurred: {ex.Message}");
        }
    }
}