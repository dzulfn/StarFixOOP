# 🚀 StarFix

A web-based quiz & survival game built in C# for our OOP group project. You pilot a spaceship, answer space trivia, and try to keep your ship intact long enough to rescue a stranded crew.

## How to Run

```bash
dotnet build
dotnet run
```

Opens at **http://localhost:5050** in your browser.

## How It Works

The backend is an ASP.NET Core server that exposes two API endpoints (`GET /api/state` and `POST /api/action`). The frontend is plain HTML/CSS/JS in `wwwroot/` — it calls the API, gets back JSON, and renders the game screen.

The game controller is a state machine. Each "phase" (main menu, quiz, result, etc.) maps to a screen. When the player clicks a button, the browser sends an action, the controller updates the phase, and returns the new state.

## Gameplay

1. Enter your name and start the mission.
2. Each level has 5 quiz questions about space.
3. Correct answers earn points and items. Wrong answers cost a life and damage the ship.
4. Use Oxygen Tanks to restore lives and Repair Kits to fix the ship between questions.
5. Pass at least half the questions in each level to advance. Clear both levels to win.

## Design Notes

- **Why a state machine?** The browser sends one action at a time and waits for a response. A switch-based state machine fits this pattern naturally — each phase knows what inputs to accept and what phase comes next.
- **Why not use the abstract `Execute()` method?** The quiz flow needs to return JSON to the browser, not print to console. So `WebGameController` handles the quiz logic directly, and `AlienEncounter.Execute()` just throws `NotSupportedException`. It's there because `Challenge` is abstract.
- **Fuel was removed.** We originally planned a fuel mechanic but it added complexity without making the game more fun, so we cut it.

## Project Structure

```
StarFix.csproj
Main.cs
Interfaces/
    IRepairable.cs
    IDisplayable.cs
Exceptions/
    InvalidSelectionException.cs
Models/
    Player.cs
    Spaceship.cs
    Question.cs
    Items/
        Item.cs          (abstract)
        OxygenTank.cs
        RepairKit.cs
    Challenges/
        Challenge.cs     (abstract)
        AlienEncounter.cs
    Levels/
        Level.cs         (base)
        Level1.cs
        Level2.cs
Controllers/
    WebGameController.cs
wwwroot/
    index.html
    css/style.css
    js/app.js
docs/
    uml-final.puml
```

## OOP Concepts Used

| Concept | Where |
|---------|-------|
| Encapsulation | Private fields with validated properties in every model class |
| Inheritance | `Challenge` → `AlienEncounter`, `Item` → `OxygenTank`/`RepairKit`, `Level` → `Level1`/`Level2` |
| Polymorphism | `item.Use(player, spaceship)` — each item subclass does something different |
| Interfaces | `IRepairable` (Spaceship), `IDisplayable` (Player, Spaceship, Level) |
| Abstract classes | `Challenge` and `Item` define the shape; subclasses fill in the details |
| Collections | `List<Item>` and `List<Challenge>` — exposed as `IReadOnlyList<T>` where needed |
| Exception handling | `InvalidSelectionException` thrown on bad quiz input, caught in controller |
