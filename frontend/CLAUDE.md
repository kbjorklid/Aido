# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Development Commands

- `npm run dev` - Start development server with Turbopack (opens http://localhost:3000)
- `npm run build` - Build production version
- `npm run start` - Start production server
- `npm run lint` - Run ESLint for code linting

## Project Architecture

This is a **Todo/Packing List App** built with Next.js 15 App Router, designed to integrate with a REST API for managing todo lists and items with AI-powered suggestions.

### Technology Stack
- **Next.js 15** with App Router and Turbopack
- **TypeScript** with strict configuration
- **Tailwind CSS v4** with custom theming and CSS variables
- **shadcn/ui** component library ("new-york" style, Zinc base color)
- **Lucide React** for icons

### Key File Structure
```
src/
├── app/                 # Next.js App Router pages
│   ├── layout.tsx      # Root layout with Geist fonts
│   ├── page.tsx        # Home page
│   └── globals.css     # Tailwind styles with theme variables
└── lib/
    └── utils.ts        # cn() utility for class merging
```

### Component System
- Uses shadcn/ui with path aliases configured in `components.json`
- Component aliases: `@/components`, `@/components/ui`, `@/lib`, `@/hooks`
- Install new shadcn components with: `npx shadcn@latest add [component-name]`

### API Integration
The app integrates with a REST API defined in `openapi-spec.yml`:
- **Todo Lists**: GET/POST `/api/todo-lists`, GET/DELETE `/api/todo-lists/{id}`
- **Todo Items**: POST `/api/todo-lists/{listId}/items`, DELETE `/api/todo-lists/{listId}/items/{itemId}`  
- **AI Suggestions**: POST `/api/todo-lists/{id}/ai-suggestions`
- All IDs are UUIDs, requests/responses use JSON

### Theming & Styling
- Dark mode support built-in with CSS variables
- Custom color palette using OKLCH color space
- Theme variables defined in `globals.css` (lines 46-113)
- Uses Tailwind CSS v4 with `@theme inline` configuration

### TypeScript Configuration
- Strict mode enabled with path mapping (`@/*` → `./src/*`)
- Next.js plugin configured for optimal development experience

## Backend address

Backend should be found in the address `http://localhost:5027/`