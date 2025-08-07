"use client";

import { useState, useEffect } from "react";
import { TodoList } from "@/lib/api";
import { todoListsApi } from "@/lib/api";
import TodoListSidebar from "@/components/TodoListSidebar";
import TodoListHeader from "@/components/TodoListHeader";
import TodoItemsList from "@/components/TodoItemsList";

export default function Home() {
  const [todoLists, setTodoLists] = useState<TodoList[]>([]);
  const [selectedListId, setSelectedListId] = useState<string | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const selectedList = todoLists.find(list => list.id === selectedListId);

  // Load todo lists on mount
  useEffect(() => {
    loadTodoLists();
  }, []);

  const loadTodoLists = async () => {
    try {
      setLoading(true);
      const lists = await todoListsApi.getAll();
      setTodoLists(lists);
      setError(null);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to load todo lists');
    } finally {
      setLoading(false);
    }
  };

  const handleListCreated = (newList: TodoList) => {
    setTodoLists(prev => [...prev, newList]);
    setSelectedListId(newList.id);
  };

  const handleListDeleted = (deletedListId: string) => {
    setTodoLists(prev => prev.filter(list => list.id !== deletedListId));
    if (selectedListId === deletedListId) {
      setSelectedListId(null);
    }
  };

  const handleListUpdated = (updatedList: TodoList) => {
    setTodoLists(prev => prev.map(list => 
      list.id === updatedList.id ? updatedList : list
    ));
  };

  return (
    <div className="flex h-screen bg-background">
      {/* Sidebar */}
      <div className="w-80 border-r border-border bg-card">
        <TodoListSidebar
          todoLists={todoLists}
          selectedListId={selectedListId}
          onListSelect={setSelectedListId}
          onListCreated={handleListCreated}
          onListDeleted={handleListDeleted}
          loading={loading}
          error={error}
        />
      </div>

      {/* Main Content */}
      <div className="flex-1 flex flex-col">
        {selectedList ? (
          <>
            <TodoListHeader 
              todoList={selectedList}
              onListUpdated={handleListUpdated}
              onListDeleted={handleListDeleted}
            />
            <div className="flex-1">
              <TodoItemsList 
                todoList={selectedList}
                onListUpdated={handleListUpdated}
              />
            </div>
          </>
        ) : (
          <div className="flex-1 flex items-center justify-center">
            <div className="text-center text-muted-foreground">
              <h2 className="text-2xl font-semibold mb-2">Select a todo list</h2>
              <p>Choose a list from the sidebar to view and manage your items</p>
              {todoLists.length === 0 && !loading && (
                <p className="mt-4">Create your first todo list to get started</p>
              )}
            </div>
          </div>
        )}
      </div>
    </div>
  );
}
