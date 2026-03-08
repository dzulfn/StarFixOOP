# 🚀 Space Rescue Mission

A console-based educational quiz/survival game built in **C#** for the OOP group project.

## Overview

The player commands a spaceship on a rescue mission across two increasingly dangerous levels. Each level features asteroid-dodge challenges and alien quiz encounters that affect the player's lives, score, spaceship health, and fuel. Items are collected and used strategically to survive.

## How to Build & Run

```bash
# Requires .NET 8 SDK
dotnet build
dotnet run
```

## Gameplay

1. **Start New Game** — Enter your name and play Level 1 → Level 2 sequentially.
2. **Select Level** — Practice a specific level independently.
3. **Challenges** — Dodge asteroid fields (pick a direction) and answer alien quiz questions.
4. **Inventory** — Collect items (Oxygen Tanks, Repair Kits) and use them between challenges.
5. **Win/Lose** — Clear both levels with your ship intact to win. Lose all lives or your ship = game over.

## Project Structure

```
OOP_project/
├── SpaceRescueMission.csproj
├── Program.cs
├── README.md
├── Interfaces/
│   ├── IRepairable.cs
│   └── IDisplayable.cs
├── Exceptions/
│   └── InvalidSelectionException.cs
├── Models/
│   ├── Player.cs
│   ├── Spaceship.cs
│   ├── Question.cs
│   ├── Items/
│   │   ├── Item.cs          (abstract)
│   │   ├── OxygenTank.cs
│   │   └── RepairKit.cs
│   ├── Challenges/
│   │   ├── Challenge.cs      (abstract)
│   │   ├── AsteroidField.cs
│   │   └── AlienEncounter.cs
│   └── Levels/
│       ├── Level.cs          (base)
│       ├── Level1.cs
│       └── Level2.cs
├── Controllers/
│   └── GameController.cs
└── docs/
    └── uml-final.puml
```

## OOP Concepts Demonstrated

| Concept | Where |
|---------|-------|
| **Encapsulation** | Every class uses private fields with validated public properties |
| **Inheritance** | `Challenge` → `AsteroidField`, `AlienEncounter`; `Item` → `OxygenTank`, `RepairKit`; `Level` → `Level1`, `Level2` |
| **Polymorphism** | `challenge.Execute(player, spaceship)` iterates base-type list; `item.Use(player, spaceship)` via abstract override |
| **Interfaces** | `IRepairable` (Spaceship repair), `IDisplayable` (Player, Spaceship, Level status display) |
| **Abstract classes** | `Challenge` and `Item` with abstract methods |
| **Collections** | `List<Item>` inventory, `List<Challenge>`, `List<Question>`, `List<Level>` — exposed as `IReadOnlyList<T>` |
| **Exception handling** | `InvalidSelectionException` for invalid menu/level/item/answer input |

## Class Summary

| Class | Responsibility |
|-------|---------------|
| `Player` | Name, score, lives, inventory management |
| `Spaceship` | Health, fuel, repair (via IRepairable) |
| `Question` | Multiple-choice quiz question with answer-checking |
| `Item` (abstract) | Base for usable items |
| `OxygenTank` | Restores player lives |
| `RepairKit` | Repairs spaceship health |
| `Challenge` (abstract) | Base for game challenges |
| `AsteroidField` | Directional evasion challenge affecting ship/fuel |
| `AlienEncounter` | Quiz challenge with input validation |
| `Level` (base) | Challenge loop, item distribution, completion criteria |
| `Level1` | Earth Orbit — easier challenges |
| `Level2` | Deep Space — harder challenges |
| `GameController` | Main menu, game loop, level progression, win/lose |
| `InvalidSelectionException` | Custom exception for bad user input |
# StarFix
