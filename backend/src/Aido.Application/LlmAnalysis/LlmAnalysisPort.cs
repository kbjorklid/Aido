using Aido.Core;
using Aido.SharedKernel;

namespace Aido.Application.LlmAnalysis;

public interface LlmAnalysisPort
{
    Task<Result<List<string>>> GetListContinuationSuggestions(TodoList todoList, int maxSuggestionCount = 3,
            CancellationToken cancellationToken = default(CancellationToken));
}
