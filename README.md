# Viral Spread

Viral Spread is an Android game developed using the Unity engine. This repository contains the complete project files and resources necessary to build and run the game. The game challenges players to control the spread of a virtual virus through strategic navigation and quick decision-making.

## Table of Contents
1. [Introduction](#introduction)
2. [Features](#features)
3. [Installation](#installation)
4. [Usage](#usage)
5. [Project Structure](#project-structure)
6. [Gameplay](#gameplay)
7. [Scripts Overview](#scripts-overview)
8. [Contributing](#contributing)
9. [License](#license)
10. [Contact](#contact)

## Introduction

Viral Spread is an engaging and interactive game where players navigate through various levels to control the spread of a virtual virus. The game is built with Unity and designed for Android devices, providing a fun and educational experience.

<div style="display: flex; justify-content: space-between;">
  <img src="Game%20Screenshot/VS1.jpg" alt="Game Screenshot 1" style="width: 32%;">
  <img src="Game%20Screenshot/VS2.jpg" alt="Game Screenshot 2" style="width: 32%;">
  <img src="Game%20Screenshot/VS3.jpg" alt="Game Screenshot 3" style="width: 32%;">
</div>

## Features
- **Intuitive Controls**: Easy touch controls for smooth gameplay.
- **Multiple Levels**: Various levels with increasing difficulty to keep players challenged.
- **Engaging Graphics**: High-quality graphics and animations.
- **Sound Effects**: Immersive sound effects to enhance the gaming experience.
- **Real-time Feedback**: Instant scoring and feedback based on player performance.
- **Educational Aspect**: Learn about virus spread and control measures.

## Installation

To get a local copy up and running, follow these simple steps:

### Prerequisites
- Unity Hub installed.
- Unity Editor version 2020.3.0f1 or later installed.
- Android SDK setup.

### Steps
1. Clone the repository:
    ```sh
    git clone https://github.com/adibakshi28/Viral_Spread-Android.git
    ```
2. Open Project in Unity:
    - Open Unity Hub.
    - Click on "Add" and select the cloned project directory.
    - Open the project.
3. Configure Build Settings for Android:
    - Go to `File > Build Settings`.
    - Select `Android` and click on `Switch Platform`.
    - Configure player settings, including package name, version, and other relevant settings.
4. Build the Project:
    - Connect your Android device or configure an emulator.
    - Click on `Build and Run` to generate the APK and install it on the device.

## Usage

After successfully building the project, install the APK on your Android device. Launch the game and follow the on-screen instructions to start playing. Use touch controls to navigate and complete the objectives of each level, aiming to control the spread of the virus effectively.

## Project Structure
- **Assets**: Contains all game assets, including:
    - **Scenes**: Different levels and menus.
    - **Scripts**: C# scripts for game logic.
    - **Prefabs**: Pre-configured game objects.
    - **Animations**: Animation controllers and clips.
    - **Audio**: Sound effects and music files.
    - **UI**: User interface elements.
- **Packages**: Unity packages used in the project.
- **ProjectSettings**: Project settings including input, tags, layers, and build settings.
- **.gitignore**: Specifies files and directories to be ignored by Git.
- **Build Notes.txt**: Contains notes and information related to building the project.
- **LICENSE**: The license under which the project is distributed.
- **README.md**: This readme file.

## Gameplay

Players must navigate through different environments to control the spread of a virus. The game includes:
- **Levels**: Each level presents unique challenges and obstacles.
- **Objectives**: Clear objectives that must be completed to progress.
- **Scoring**: Points awarded based on performance and efficiency.
- **Feedback**: Real-time feedback to help players improve.

<div style="display: flex; justify-content: space-between;">
  <img src="Game%20Screenshot/VS4.jpg" alt="Game Screenshot 4" style="width: 32%;">
  <img src="Game%20Screenshot/VS5.jpg" alt="Game Screenshot 5" style="width: 32%;">
  <img src="Game%20Screenshot/VS6.jpg" alt="Game Screenshot 6" style="width: 32%;">
</div>

## Scripts Overview

The `Assets/Scripts` directory contains the C# scripts responsible for the game's functionality. Here is a detailed overview of the key scripts:

### ColorButtonScript.cs
This script handles the functionality of color buttons used in the game. It manages user interactions with the buttons and triggers corresponding actions, such as changing the color state in the game.

### GameController.cs
The central script that manages the overall game state. It controls game flow, including starting and ending the game, tracking the player's progress, and updating the user interface with scores and other information.

### Hexagon.cs
This script defines the behavior of hexagonal game objects. It includes logic for handling interactions, animations, and the properties of the hexagons, such as their color and state.

### LevelController.cs
Manages the setup and control of individual game levels. This script initializes level-specific elements, tracks the player's progress within a level, and handles transitions between levels.

### LevelGenerator.cs
Responsible for generating the game levels dynamically. It includes algorithms to create the game grid, place hexagons, and set up initial conditions for each level.

### LevelSelectButton.cs
Handles the functionality of buttons in the level selection menu. It manages user interactions, allowing players to select and navigate to different levels.

### MenuController.cs
Manages the game's main menu and other UI screens. It handles user interactions with menu buttons, such as starting the game, accessing settings, and quitting the game.

### ParticleEffect.cs
Controls particle effects used in the game. This script handles the creation, animation, and destruction of particle effects to enhance the visual experience.

## Contributing

Contributions are what make the open-source community such an amazing place to learn, inspire, and create. Any contributions you make are greatly appreciated.

1. Fork the Project:
    - Click the "Fork" button at the top right of the repository page.
2. Create your Feature Branch:
    ```sh
    git checkout -b feature/AmazingFeature
    ```
3. Commit your Changes:
    ```sh
    git commit -m 'Add some AmazingFeature'
    ```
4. Push to the Branch:
    ```sh
    git push origin feature/AmazingFeature
    ```
5. Open a Pull Request:
    - Navigate to your forked repository.
    - Click on the "Pull Request" button and submit your changes for review.

## License

This project is licensed under the MIT License. See the LICENSE file for more information.

## Contact

For any inquiries or support, feel free to contact me or raise an issue

<div style="display: flex; justify-content: space-between;">
  <img src="Game%20Screenshot/VS7.jpg" alt="Game Screenshot 7" style="width: 47%;">
  <img src="Game%20Screenshot/VS8.jpg" alt="Game Screenshot 8" style="width: 47%;">
</div>
