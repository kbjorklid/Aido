"use client";

import { useState } from "react";
import { Plus, Trash2, List } from "lucide-react";
import { TodoList, CreateTodoListRequest, todoListsApi } from "@/lib/api";
import { Button } from "@/components/ui/button";
import { ScrollArea } from "@/components/ui/scroll-area";
import { Separator } from "@/components/ui/separator";
import CreateListModal from "./CreateListModal";

interface TodoListSidebarProps {
  todoLists: TodoList[];
  selectedListId: string | null;
  onListSelect: (id: string) => void;
  onListCreated: (list: TodoList) => void;
  onListDeleted: (id: string) => void;
  loading: boolean;
  error: string | null;
}

export default function TodoListSidebar({
  todoLists,
  selectedListId,
  onListSelect,
  onListCreated,
  onListDeleted,
  loading,
  error
}: TodoListSidebarProps) {
  const [isCreateModalOpen, setIsCreateModalOpen] = useState(false);
  const [deletingId, setDeletingId] = useState<string | null>(null);

  const handleCreateList = async (data: CreateTodoListRequest) => {
    try {
      const newList = await todoListsApi.create(data);
      onListCreated(newList);
      setIsCreateModalOpen(false);
    } catch (err) {
      console.error('Failed to create todo list:', err);
    }
  };

  const handleDeleteList = async (id: string) => {
    try {
      setDeletingId(id);
      await todoListsApi.delete(id);
      onListDeleted(id);
    } catch (err) {
      console.error('Failed to delete todo list:', err);
    } finally {
      setDeletingId(null);
    }
  };

  if (loading) {
    return (
      <div className="p-4 h-full flex items-center justify-center">
        <div className="text-muted-foreground">Loading...</div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="p-4 h-full flex items-center justify-center">
        <div className="text-destructive text-center">
          <p className="mb-2">Failed to load todo lists</p>
          <p className="text-sm">{error}</p>
        </div>
      </div>
    );
  }

  return (
    <div className="h-full flex flex-col">
      {/* Header */}
      <div className="p-4 border-b border-border">
        <div className="flex items-center justify-between">
          <h1 className="text-xl font-semibold flex items-center gap-2">
            <List className="h-5 w-5" />
            Todo Lists
          </h1>
          <Button 
            size="sm" 
            onClick={() => setIsCreateModalOpen(true)}
            className="h-8 w-8 p-0"
          >
            <Plus className="h-4 w-4" />
          </Button>
        </div>
      </div>

      {/* Lists */}
      <ScrollArea className="flex-1">
        <div className="p-2">
          {todoLists.length === 0 ? (
            <div className="text-center py-8 text-muted-foreground">
              <List className="h-8 w-8 mx-auto mb-2 opacity-50" />
              <p className="text-sm">No todo lists yet</p>
              <p className="text-xs mt-1">Create one to get started</p>
            </div>
          ) : (
            <div className="space-y-1">
              {todoLists.map((list) => (
                <div key={list.id} className="group relative">
                  <button
                    onClick={() => onListSelect(list.id)}
                    className={`w-full text-left p-3 rounded-lg transition-colors ${
                      selectedListId === list.id
                        ? 'bg-primary text-primary-foreground'
                        : 'hover:bg-accent hover:text-accent-foreground'
                    }`}
                  >
                    <div className="pr-8">
                      <h3 className="font-medium truncate">{list.name}</h3>
                      {list.description && (
                        <p className="text-xs opacity-70 truncate mt-1">
                          {list.description}
                        </p>
                      )}
                      <div className="text-xs opacity-60 mt-1">
                        {list.items?.length || 0} items
                      </div>
                    </div>
                  </button>
                  <Button
                    variant="ghost"
                    size="sm"
                    className="absolute right-2 top-1/2 transform -translate-y-1/2 h-6 w-6 p-0 opacity-0 group-hover:opacity-100 transition-opacity"
                    onClick={(e) => {
                      e.stopPropagation();
                      handleDeleteList(list.id);
                    }}
                    disabled={deletingId === list.id}
                  >
                    <Trash2 className="h-3 w-3" />
                  </Button>
                </div>
              ))}
            </div>
          )}
        </div>
      </ScrollArea>

      {/* Footer */}
      <div className="p-4 border-t border-border">
        <Button 
          className="w-full" 
          onClick={() => setIsCreateModalOpen(true)}
          variant="outline"
        >
          <Plus className="h-4 w-4 mr-2" />
          New List
        </Button>
      </div>

      {/* Create List Modal */}
      <CreateListModal
        open={isCreateModalOpen}
        onOpenChange={setIsCreateModalOpen}
        onSubmit={handleCreateList}
      />
    </div>
  );
}