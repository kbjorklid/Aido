"use client";

import { useState } from "react";
import { Trash2, MoreHorizontal } from "lucide-react";
import { TodoItem as TodoItemType, todoItemsApi } from "@/lib/api";
import { Checkbox } from "@/components/ui/checkbox";
import { Button } from "@/components/ui/button";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";

interface TodoItemProps {
  item: TodoItemType;
  listId: string;
  onItemDeleted: (itemId: string) => void;
}

export default function TodoItem({ 
  item, 
  listId, 
  onItemDeleted 
}: TodoItemProps) {
  const [isCompleted, setIsCompleted] = useState(false);
  const [isDeleting, setIsDeleting] = useState(false);

  const handleDelete = async () => {
    try {
      setIsDeleting(true);
      await todoItemsApi.delete(listId, item.id);
      onItemDeleted(item.id);
    } catch (err) {
      console.error("Failed to delete todo item:", err);
    } finally {
      setIsDeleting(false);
    }
  };

  const handleCompletedChange = (completed: boolean) => {
    setIsCompleted(completed);
  };

  return (
    <div className="group flex items-start gap-3 p-4 border border-border rounded-lg bg-card hover:bg-accent/50 transition-colors">
      <Checkbox
        checked={isCompleted}
        onCheckedChange={handleCompletedChange}
        className="mt-1"
      />
      <div className="flex-1 space-y-1">
        <h3 className={`font-medium leading-tight ${
          isCompleted ? 'line-through text-muted-foreground' : 'text-foreground'
        }`}>
          {item.title}
        </h3>
        {item.description && (
          <p className={`text-sm leading-relaxed ${
            isCompleted ? 'line-through text-muted-foreground/70' : 'text-muted-foreground'
          }`}>
            {item.description}
          </p>
        )}
      </div>
      <div className="opacity-0 group-hover:opacity-100 transition-opacity">
        <DropdownMenu>
          <DropdownMenuTrigger asChild>
            <Button variant="ghost" size="sm" className="h-8 w-8 p-0">
              <MoreHorizontal className="h-4 w-4" />
            </Button>
          </DropdownMenuTrigger>
          <DropdownMenuContent align="end">
            <DropdownMenuItem 
              onClick={handleDelete}
              disabled={isDeleting}
              className="text-destructive focus:text-destructive"
            >
              <Trash2 className="h-4 w-4 mr-2" />
              {isDeleting ? "Deleting..." : "Delete"}
            </DropdownMenuItem>
          </DropdownMenuContent>
        </DropdownMenu>
      </div>
    </div>
  );
}