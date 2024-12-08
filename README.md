
# Air Hockey Multiplayer Game

Welcome to the **Air Hockey Multiplayer Game**! This project is built using Unity and integrates multiplayer functionality using Photon Unity Networking 2 (PUN2). Challenge your friends or players from around the globe in a fast-paced, competitive air hockey game.

---

## Features

- **Multiplayer Support**: Play with 2-4 players in real-time using PUN2.
- **Dynamic Gameplay**: Realistic physics for the puck and paddles to create an immersive gaming experience.
- **Cross-Platform**: Supports both desktop and mobile platforms.
- **Smooth Synchronization**: Real-time synchronization of player actions and puck movements.
- **User-Friendly UI**: Intuitive controls and simple interface for a seamless gaming experience.

---

## Prerequisites

To run or modify this project, ensure you have the following installed:

- [Unity 2022.x or newer](https://unity.com/)
- [Photon Unity Networking 2 (PUN2)](https://assetstore.unity.com/packages/tools/network/pun-2-free-119922)

---

## How to Play

1. **Start the Game**:
   - Launch the game and log in using your unique player name.
2. **Host or Join a Room**:
   - Create a new room to host a game or join an existing one.
3. **Gameplay**:
   - Use your paddle to hit the puck and score against your opponent.
   - First player to reach the score limit wins the game.
4. **Exit Game**:
   - Leave the room or quit the game at any time from the menu.

---

## Installation

### Clone the Repository
```bash
git clone https://github.com/username/air-hockey-multiplayer.git
```

### Open in Unity
1. Open Unity Hub.
2. Add the cloned project folder to your projects.
3. Open the project in Unity Editor.

### Import PUN2
1. Go to the Unity Asset Store.
2. Download and import the **Photon Unity Networking 2 (PUN2)** package.

---

## Project Structure

- **Assets/**
  - Game Scripts: Contains all the logic for multiplayer, physics, and game mechanics.
  - UI: Includes menus, buttons, and score displays.
  - Prefabs: Contains pre-configured game objects such as paddles, puck, and the game board.
- **Photon/**:
  - Networking scripts and configurations for multiplayer functionality.
- **Scenes/**:
  - Main Menu: The starting screen of the game.
  - Game Scene: The actual air hockey arena.

---

## Multiplayer Integration Details

This project leverages PUN2 for networking. Key components include:
- **Room Management**: Players can create or join rooms dynamically.
- **Synchronization**: Real-time updates for puck position, paddle movements, and scores.
- **RPCs (Remote Procedure Calls)**: Ensures seamless communication between players.
- **Custom Matchmaking**: Supports quick matches or private rooms.

---

## Future Enhancements

- AI Opponent: Add a single-player mode against an AI.
- Spectator Mode: Allow users to watch ongoing matches.
- Power-Ups: Introduce special abilities like speed boosts or puck modifiers.
- Customization: Enable paddle and puck customization.

---

## Contributing

Feel free to contribute to the project! Fork the repository and submit a pull request for any improvements or bug fixes.

---

## License

This project is licensed under the [MIT License](LICENSE).

---

## Acknowledgments

- **Unity Technologies**: For providing the amazing Unity Engine.
- **Exit Games**: For the Photon Unity Networking 2 (PUN2) framework.

---
