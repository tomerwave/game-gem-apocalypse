# Gem Apocalypse - Unity Game Project

## Project Overview

A first-person exploration/adventure game built in Unity using the Universal Render Pipeline (URP). Features a state machine-based player controller with various movement states and environmental interactions.

## Architecture

### Player Controller (State Machine Pattern)

Located in: `Assets/EasyPeasyFirstPersonController/Scripts/`

- **FirstPersonController.cs** - Main controller orchestrating movement, camera, and state transitions
- **PlayerBaseState.cs** - Abstract base class for all player states
- **PlayerStateFactory.cs** - Factory for creating state instances
- **InputManagerOld.cs** - Handles input (WASD, mouse, E for interact, Space for jump, Shift for sprint, Ctrl for crouch/slide)

### Player States (7 total)

Located in: `Assets/EasyPeasyFirstPersonController/Scripts/States/`

- PlayerGroundedState, PlayerJumpingState, PlayerFallState, PlayerSlidingState, PlayerCrouchingState, PlayerLedgeGrabState, PlayerSwimmingState

### Game Scripts

Located in: `Assets/Scripts/`

- **BoxInteraction.cs** - Player component for detecting and pushing interactable boxes (Press E to toggle)
- **InteractableBox.cs** - Attach to pushable boxes (requires Rigidbody, "Interactable" tag and layer)
- **SwitchScript.cs** - Interactive switches that toggle on/off with E key
- **SwitchInteractive.cs** - Objects that respond to switch state changes
- **InteractionPromptUI.cs** - Centralized UI system for showing interaction prompts (singleton)
- **ShallowWater.cs** - Ambient shallow water zones with optional splash effects
- **InventoryManagment.cs** - 9-slot hotbar inventory system
- **ToggleTimeScript.cs** - Objects that switch between past/future states
- **LaserEmitter.cs** - Emits a laser beam from its position, stops on collision

## Key Controls

| Action       | Key          |
| ------------ | ------------ |
| Movement     | WASD         |
| Look         | Mouse        |
| Jump         | Space        |
| Sprint       | Left Shift   |
| Crouch/Slide | Left Control |
| Interact     | E            |
| Inventory    | 1-9          |

## Setup Requirements

### For Interactable Boxes

1. Create "Interactable" tag and layer in Project Settings
2. Add `InteractableBox` component to box
3. Set tag to "Interactable", layer to "Interactable"
4. Ensure box has Rigidbody and Collider

### For Switches

1. Add `SwitchScript` component
2. Ensure player has "Player" tag
3. Assign objects to `interacteWith` array

### For Interaction UI

1. Create Canvas with Panel and Text (Legacy)
2. Add `InteractionPromptUI` to a manager GameObject
3. Assign Panel to `promptPanel`, Text to `promptText`

## Movement Speeds

- Walk: 3 m/s
- Sprint: 5 m/s
- Crouch: 1.5 m/s
- Slide: 6 m/s
- Holding box: 50% speed multiplier

## Important Notes

- Player must have "Player" tag for interactions to work
- Interaction requires both being in range AND looking at the object (raycast check)
- Box interaction uses toggle (press E to start/stop pushing)
- Switch interaction shows dynamic "Turn On/Off" based on current state
