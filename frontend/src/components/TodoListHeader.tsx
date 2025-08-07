"use client";

import { useState } from "react";
import { MoreHorizontal, Edit, Trash2 } from "lucide-react";
import { TodoList, todoListsApi } from "@/lib/api";
import { Button } from "@/components/ui/button";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";

interface TodoListHeaderProps {
  todoList: TodoList;
  onListUpdated: (list: TodoList) => void;
  onListDeleted: (id: string) => void;
}

export default function TodoListHeader({
  todoList,
  onListUpdated,
  onListDeleted
}: TodoListHeaderProps) {
  const [isDeleting, setIsDeleting] = useState(false);

  const handleDelete = async () => {
    if (!confirm("Are you sure you want to delete this todo list? This action cannot be undone.")) {
      return;
    }

    try {
      setIsDeleting(true);
      await todoListsApi.delete(todoList.id);
      onListDeleted(todoList.id);
    } catch (err) {
      console.error("Failed to delete todo list:", err);
    } finally {
      setIsDeleting(false);
    }
  };

  const itemCount = todoList.items?.length || 0;

  return (
    <div className="border-b border-border bg-card">
      <div className="p-6">
        <div className="flex items-start justify-between">
          <div className="flex-1">
            <h1 className="text-2xl font-bold text-foreground">{todoList.name}</h1>
            {todoList.description && (
              <p className="text-muted-foreground mt-1">{todoList.description}</p>
            )}
            <div className="flex items-center gap-4 mt-3 text-sm text-muted-foreground">
              <span>{itemCount} {itemCount === 1 ? 'item' : 'items'}</span>
            </div>
          </div>
          <div className="flex items-center gap-2">
            <DropdownMenu>
              <DropdownMenuTrigger asChild>
                <Button variant="ghost" size="sm" className="h-8 w-8 p-0">
                  <MoreHorizontal className="h-4 w-4" />
                </Button>
              </DropdownMenuTrigger>
              <DropdownMenuContent align="end">
                <DropdownMenuItem>
                  <Edit className="h-4 w-4 mr-2" />
                  Edit
                </DropdownMenuItem>
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
      </div>
    </div>
  );
}