const API_BASE_URL = 'http://localhost:5027';

// Type definitions based on OpenAPI spec
export interface TodoList {
  id: string;
  name: string;
  description?: string;
  items?: TodoItem[];
}

export interface TodoItem {
  id: string;
  title: string;
  description?: string;
}

export interface CreateTodoListRequest {
  name: string;
  description?: string;
}

export interface AddTodoItemRequest {
  title: string;
  description?: string;
}

export interface GenerateAiSuggestionsRequest {
  maxSuggestions: number;
}

export interface AiSuggestion {
  title: string;
  description?: string;
}

// API Service Functions
export const todoListsApi = {
  // Get all todo lists
  async getAll(): Promise<TodoList[]> {
    const response = await fetch(`${API_BASE_URL}/api/todo-lists`);
    if (!response.ok) throw new Error('Failed to fetch todo lists');
    return response.json();
  },

  // Get a specific todo list by ID
  async getById(id: string): Promise<TodoList> {
    const response = await fetch(`${API_BASE_URL}/api/todo-lists/${id}`);
    if (!response.ok) throw new Error('Failed to fetch todo list');
    return response.json();
  },

  // Create a new todo list
  async create(data: CreateTodoListRequest): Promise<TodoList> {
    const response = await fetch(`${API_BASE_URL}/api/todo-lists`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(data),
    });
    if (!response.ok) throw new Error('Failed to create todo list');
    return response.json();
  },

  // Delete a todo list
  async delete(id: string): Promise<void> {
    const response = await fetch(`${API_BASE_URL}/api/todo-lists/${id}`, {
      method: 'DELETE',
    });
    if (!response.ok) throw new Error('Failed to delete todo list');
  },

  // Generate AI suggestions for a todo list
  async generateAiSuggestions(id: string, data: GenerateAiSuggestionsRequest): Promise<AiSuggestion[]> {
    const response = await fetch(`${API_BASE_URL}/api/todo-lists/${id}/ai-suggestions`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(data),
    });
    if (!response.ok) throw new Error('Failed to generate AI suggestions');
    return response.json();
  },
};

export const todoItemsApi = {
  // Add an item to a todo list
  async add(listId: string, data: AddTodoItemRequest): Promise<TodoItem> {
    const response = await fetch(`${API_BASE_URL}/api/todo-lists/${listId}/items`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(data),
    });
    if (!response.ok) throw new Error('Failed to add todo item');
    return response.json();
  },

  // Delete an item from a todo list
  async delete(listId: string, itemId: string): Promise<void> {
    const response = await fetch(`${API_BASE_URL}/api/todo-lists/${listId}/items/${itemId}`, {
      method: 'DELETE',
    });
    if (!response.ok) throw new Error('Failed to delete todo item');
  },
};