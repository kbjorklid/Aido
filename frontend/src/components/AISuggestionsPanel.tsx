"use client";

import { Sparkles, Plus, RefreshCw, X, AlertCircle } from "lucide-react";
import { AiSuggestion } from "@/lib/api";
import {
  Sheet,
  SheetContent,
  SheetHeader,
  SheetTitle,
  SheetDescription,
} from "@/components/ui/sheet";
import { Button } from "@/components/ui/button";
import { ScrollArea } from "@/components/ui/scroll-area";
import { Separator } from "@/components/ui/separator";

interface AISuggestionsPanelProps {
  isOpen: boolean;
  onOpenChange: (open: boolean) => void;
  suggestions: AiSuggestion[];
  isLoading: boolean;
  error: string | null;
  onSuggestionAdd: (suggestion: AiSuggestion) => void;
  onRegenerateRequest: () => void;
}

export default function AISuggestionsPanel({
  isOpen,
  onOpenChange,
  suggestions,
  isLoading,
  error,
  onSuggestionAdd,
  onRegenerateRequest
}: AISuggestionsPanelProps) {
  return (
    <Sheet open={isOpen} onOpenChange={onOpenChange}>
      <SheetContent className="sm:max-w-[400px] w-full">
        <SheetHeader>
          <SheetTitle className="flex items-center gap-2">
            <Sparkles className="h-5 w-5" />
            AI Suggestions
          </SheetTitle>
          <SheetDescription>
            AI-powered suggestions to help complete your todo list
          </SheetDescription>
        </SheetHeader>

        <div className="mt-6">
          {error && (
            <div className="p-4 border border-destructive/20 bg-destructive/10 rounded-lg mb-4">
              <div className="flex items-start gap-2">
                <AlertCircle className="h-5 w-5 text-destructive mt-0.5 flex-shrink-0" />
                <div>
                  <p className="text-sm font-medium text-destructive">Failed to generate suggestions</p>
                  <p className="text-xs text-destructive/80 mt-1">{error}</p>
                  <Button
                    size="sm"
                    variant="outline"
                    onClick={onRegenerateRequest}
                    className="mt-2 h-7"
                  >
                    <RefreshCw className="h-3 w-3 mr-1" />
                    Try Again
                  </Button>
                </div>
              </div>
            </div>
          )}

          {isLoading && (
            <div className="flex items-center justify-center py-8">
              <div className="flex flex-col items-center gap-3">
                <div className="animate-spin">
                  <Sparkles className="h-8 w-8 text-primary" />
                </div>
                <p className="text-sm text-muted-foreground">Generating suggestions...</p>
              </div>
            </div>
          )}

          {!isLoading && !error && suggestions.length > 0 && (
            <div>
              <div className="flex items-center justify-between mb-4">
                <p className="text-sm text-muted-foreground">
                  {suggestions.length} suggestion{suggestions.length !== 1 ? 's' : ''}
                </p>
                <Button
                  size="sm"
                  variant="outline"
                  onClick={onRegenerateRequest}
                >
                  <RefreshCw className="h-3 w-3 mr-1" />
                  Regenerate
                </Button>
              </div>

              <ScrollArea className="h-[calc(100vh-300px)]">
                <div className="space-y-3">
                  {suggestions.map((suggestion, index) => (
                    <div key={index} className="border border-border rounded-lg p-4 bg-card">
                      <div className="space-y-2">
                        <h4 className="font-medium text-sm">{suggestion.title}</h4>
                        {suggestion.description && (
                          <p className="text-xs text-muted-foreground leading-relaxed">
                            {suggestion.description}
                          </p>
                        )}
                        <Button
                          size="sm"
                          onClick={() => onSuggestionAdd(suggestion)}
                          className="h-7 w-full"
                        >
                          <Plus className="h-3 w-3 mr-1" />
                          Add to List
                        </Button>
                      </div>
                    </div>
                  ))}
                </div>
              </ScrollArea>
            </div>
          )}

          {!isLoading && !error && suggestions.length === 0 && (
            <div className="text-center py-8">
              <Sparkles className="h-12 w-12 mx-auto mb-4 text-muted-foreground/50" />
              <p className="text-sm text-muted-foreground mb-4">
                No suggestions available
              </p>
              <Button
                size="sm"
                variant="outline"
                onClick={onRegenerateRequest}
              >
                <RefreshCw className="h-3 w-3 mr-1" />
                Try Generating
              </Button>
            </div>
          )}
        </div>

        <div className="absolute bottom-4 left-6 right-6">
          <Button 
            variant="outline" 
            onClick={() => onOpenChange(false)}
            className="w-full"
          >
            <X className="h-4 w-4 mr-2" />
            Close
          </Button>
        </div>
      </SheetContent>
    </Sheet>
  );
}