"use client";

import { useState } from "react";
import { Plus } from "lucide-react";
import { TodoList, TodoItem as TodoItemType, AddTodoItemRequest, todoItemsApi } from "@/lib/api";
import { ScrollArea } from "@/components/ui/scroll-area";
import { Separator } from "@/components/ui/separator";
import TodoItem from "./TodoItem";
import AddItemForm from "./AddItemForm";
import AISuggestionsButton from "./AISuggestionsButton";

interface TodoItemsListProps {
  todoList: TodoList;
  onListUpdated: (list: TodoList) => void;
}

export default function TodoItemsList({ 
  todoList, 
  onListUpdated 
}: TodoItemsListProps) {
  const [isAddingItem, setIsAddingItem] = useState(false);

  const items = todoList.items || [];

  const handleItemAdded = async (data: AddTodoItemRequest) => {
    try {
      const newItem = await todoItemsApi.add(todoList.id, data);
      const updatedList = {
        ...todoList,
        items: [...items, newItem]
      };
      onListUpdated(updatedList);
      setIsAddingItem(false);
    } catch (err) {
      console.error("Failed to add todo item:", err);
    }
  };

  const handleItemDeleted = (deletedItemId: string) => {
    const updatedList = {
      ...todoList,
      items: items.filter(item => item.id !== deletedItemId)
    };
    onListUpdated(updatedList);
  };

  const handleAISuggestionAdded = async (suggestion: { title: string; description?: string }) => {
    try {
      const newItem = await todoItemsApi.add(todoList.id, suggestion);
      const updatedList = {
        ...todoList,
        items: [...items, newItem]
      };
      onListUpdated(updatedList);
    } catch (err) {
      console.error("Failed to add AI suggestion:", err);
    }
  };

  return (
    <div className="h-full flex flex-col">
      <ScrollArea className="flex-1">
        <div className="p-6">
          {items.length === 0 ? (
            <div className="text-center py-12">
              <div className="text-muted-foreground">
                <Plus className="h-12 w-12 mx-auto mb-4 opacity-50" />
                <h3 className="text-lg font-medium mb-2">No items yet</h3>
                <p className="text-sm mb-4">Add your first todo item to get started</p>
              </div>
            </div>
          ) : (
            <div className="space-y-3">
              {items.map((item, index) => (
                <div key={item.id}>
                  <TodoItem
                    item={item}
                    listId={todoList.id}
                    onItemDeleted={handleItemDeleted}
                  />
                  {index < items.length - 1 && (
                    <Separator className="my-3" />
                  )}
                </div>
              ))}
            </div>
          )}
        </div>
      </ScrollArea>

      <div className="border-t border-border bg-card">
        <div className="p-6 space-y-4">
          {/* Add Item Form */}
          <AddItemForm
            onSubmit={handleItemAdded}
            isOpen={isAddingItem}
            onToggle={setIsAddingItem}
          />

          {/* AI Suggestions Button - only show when there are 2 or more items */}
          {items.length >= 2 && (
            <AISuggestionsButton
              todoList={todoList}
              onSuggestionAdded={handleAISuggestionAdded}
            />
          )}
        </div>
      </div>
    </div>
  );
}