# McWuT - Crime Generator Feature

## Overview

The CrimeGenerator is a new feature added to the McWuT application - a browser-based murder mystery investigation game that uses RandomUser API and LLM integration to generate procedural murder mysteries.

## Architecture

The implementation follows Clean Architecture principles with Domain-Driven Design (DDD):

### Domain Layer (`McWuT.Data.Models.CrimeGenerator`)
- **GameSession**: Tracks individual game instances with state, difficulty, and scoring
- **Character**: Base entity for all characters (victims, suspects, witnesses)
- **Victim**: Extends Character with death details and circumstances
- **Suspect**: Extends Character with motive, alibi, and guilt information
- **Witness**: Extends Character with statements and reliability scores
- **Clue**: Evidence items with descriptions, locations, and relevance
- **Relationship**: Connections between characters with type and strength
- **Location**: Crime scenes and other relevant places
- **Timeline**: Sequence of events with timestamps and actors

### Application Layer (`McWuT.Services.CrimeGenerator`)
- **GameService**: Core game lifecycle management (create, update, end games)
- **External API Services**:
  - **RandomUserService**: Integrates with RandomUser API for character generation
  - **LlmService**: Integrates with OpenAI API for AI-generated content

### Infrastructure Layer (`McWuT.Data`)
- Entity Framework Core configurations for all CrimeGenerator entities
- Repository pattern implementation using existing base repositories
- SQLite database for development environment

### Presentation Layer (`McWuT/Pages/CrimeGenerator`)
- **Index**: Main dashboard for starting new games and viewing history
- **Game**: Individual game interface with investigation tools
- Responsive UI with Bootstrap styling and interactive elements

## Key Features Implemented

### ✅ Core Game Mechanics
- Game session creation with difficulty levels (Easy, Medium, Hard)
- User-specific game tracking and history
- Game state management (InProgress, Solved, Failed, Abandoned)

### ✅ Domain Models
- Complete entity model hierarchy with relationships
- Soft delete pattern implementation
- Proper Entity Framework configurations

### ✅ External API Integration
- RandomUser API client for character profile generation
- OpenAI LLM integration with fallback responses
- Async HTTP client configuration with proper error handling

### ✅ User Interface
- Modern, responsive design matching existing app style
- Game dashboard with features overview
- Individual game interface with progress tracking
- Navigation integration with main application

### ✅ Infrastructure
- Database migration system
- Dependency injection configuration
- Service layer abstractions

## Technical Implementation Details

### Database Schema
- 9 new tables added to support the game entities
- Proper foreign key relationships and constraints
- Unique indexes for data integrity

### API Configuration
- OpenAI API integration with configurable endpoints
- RandomUser API client with rate limiting considerations
- Graceful fallback when external services are unavailable

### Security
- User authentication required for all game features
- User-specific data isolation
- Authorization checks on all game operations

## Getting Started

### Prerequisites
- .NET 8.0 SDK
- SQLite database (automatically created)

### Configuration
1. Update `appsettings.json` with your OpenAI API key:
```json
{
  "OpenAI": {
    "ApiKey": "your-openai-api-key-here"
  }
}
```

2. Run database migrations:
```bash
dotnet ef database update --project McWuT.Data --startup-project McWuT
```

3. Run the application:
```bash
dotnet run --project McWuT
```

### Usage
1. Navigate to the application in your browser
2. Register/login to access authenticated features
3. Click "Crime Generator" in the navigation or on the dashboard
4. Start a new mystery by selecting difficulty level
5. Investigate the generated case (character generation coming soon)

## Current Status

### ✅ Completed
- Domain model design and implementation
- Database schema and migrations  
- Core game services and business logic
- External API service foundations
- Basic user interface and navigation
- Game session management

### 🚧 In Progress / Future Enhancements
- Character generation using RandomUser API
- AI-powered story generation and dialogue
- Interactive crime scene exploration
- Evidence collection and analysis
- Suspect interrogation system
- Accusation and case resolution mechanics
- Multiplayer support with SignalR
- Advanced scoring and achievements

## Extension Points

The current implementation provides a solid foundation for extending the game with:
- More sophisticated AI prompts for varied storytelling
- Mini-games for evidence analysis
- Advanced character relationship dynamics
- Different crime types beyond murder mysteries
- Difficulty-based content variation
- Social features and leaderboards

## Dependencies Added

- **MediatR**: CQRS pattern support (future use)
- **AutoMapper**: DTO mapping (future use)  
- **FluentValidation**: Input validation (future use)
- **Serilog**: Advanced logging (future use)
- **SignalR**: Real-time features (future use)

The architecture is designed to be easily extensible and maintainable, following established patterns from the existing McWuT application.