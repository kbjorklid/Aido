"use client";

import { useState } from "react";
import { Sparkles } from "lucide-react";
import { TodoList, AiSuggestion, todoListsApi } from "@/lib/api";
import { Button } from "@/components/ui/button";
import AISuggestionsPanel from "./AISuggestionsPanel";

interface AISuggestionsButtonProps {
  todoList: TodoList;
  onSuggestionAdded: (suggestion: AiSuggestion) => void;
  onSuggestionsGenerated?: () => void;
}

export default function AISuggestionsButton({ 
  todoList, 
  onSuggestionAdded,
  onSuggestionsGenerated
}: AISuggestionsButtonProps) {
  const [isOpen, setIsOpen] = useState(false);
  const [suggestions, setSuggestions] = useState<AiSuggestion[]>([]);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleGenerateSuggestions = async () => {
    try {
      setIsLoading(true);
      setError(null);
      setIsOpen(true);
      
      const generatedSuggestions = await todoListsApi.generateAiSuggestions(
        todoList.id,
        { maxSuggestions: 5 }
      );
      
      setSuggestions(generatedSuggestions);
      
      // Notify parent that suggestions have been generated
      if (onSuggestionsGenerated) {
        onSuggestionsGenerated();
      }
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to generate AI suggestions');
      console.error("Failed to generate AI suggestions:", err);
    } finally {
      setIsLoading(false);
    }
  };

  const handleSuggestionAdd = async (suggestion: AiSuggestion) => {
    await onSuggestionAdded(suggestion);
    // Remove the added suggestion from the list
    setSuggestions(prev => prev.filter(s => 
      s.title !== suggestion.title || s.description !== suggestion.description
    ));
  };

  return (
    <>
      <Button 
        onClick={handleGenerateSuggestions}
        disabled={isLoading}
        className="w-full"
        variant="secondary"
      >
        <Sparkles className="h-4 w-4 mr-2" />
        {isLoading ? "Generating..." : "Get AI Suggestions"}
      </Button>

      <AISuggestionsPanel
        isOpen={isOpen}
        onOpenChange={setIsOpen}
        suggestions={suggestions}
        isLoading={isLoading}
        error={error}
        onSuggestionAdd={handleSuggestionAdd}
        onRegenerateRequest={handleGenerateSuggestions}
      />
    </>
  );
}