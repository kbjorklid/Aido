"use client";

import { useState } from "react";
import { Plus, X } from "lucide-react";
import { AddTodoItemRequest } from "@/lib/api";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Textarea } from "@/components/ui/textarea";

interface AddItemFormProps {
  onSubmit: (data: AddTodoItemRequest) => void;
  isOpen: boolean;
  onToggle: (open: boolean) => void;
}

export default function AddItemForm({ 
  onSubmit, 
  isOpen, 
  onToggle 
}: AddItemFormProps) {
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [isSubmitting, setIsSubmitting] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!title.trim()) return;

    setIsSubmitting(true);
    try {
      await onSubmit({
        title: title.trim(),
        description: description.trim() || undefined,
      });
      
      // Reset form
      setTitle("");
      setDescription("");
    } catch (err) {
      console.error("Failed to add item:", err);
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleCancel = () => {
    setTitle("");
    setDescription("");
    onToggle(false);
  };

  if (!isOpen) {
    return (
      <Button 
        onClick={() => onToggle(true)}
        variant="outline" 
        className="w-full"
      >
        <Plus className="h-4 w-4 mr-2" />
        Add Item
      </Button>
    );
  }

  return (
    <div className="border border-border rounded-lg p-4 bg-card">
      <form onSubmit={handleSubmit} className="space-y-3">
        <div>
          <Input
            placeholder="What do you need to do?"
            value={title}
            onChange={(e) => setTitle(e.target.value)}
            required
            autoFocus
          />
        </div>
        <div>
          <Textarea
            placeholder="Add a description (optional)"
            value={description}
            onChange={(e) => setDescription(e.target.value)}
            rows={2}
            className="resize-none"
          />
        </div>
        <div className="flex justify-end space-x-2">
          <Button
            type="button"
            variant="ghost"
            size="sm"
            onClick={handleCancel}
            disabled={isSubmitting}
          >
            <X className="h-4 w-4 mr-1" />
            Cancel
          </Button>
          <Button 
            type="submit" 
            size="sm"
            disabled={isSubmitting || !title.trim()}
          >
            <Plus className="h-4 w-4 mr-1" />
            {isSubmitting ? "Adding..." : "Add Item"}
          </Button>
        </div>
      </form>
    </div>
  );
}