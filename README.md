# Opaq.Net1

A modular, distributed game server architecture built with **C#**. This project is a personal sandbox for exploring high-scale networking and distributed system design.

> [!IMPORTANT]
> **Status: WIP/PoC.** This is a hobby project written in my spare time. It is far from a finished product.

### Components
```mermaid
graph TD
    Player[Player Client] -->|1. Try authorize| Login[Login Server]
    Login -->|2. Find server for player| Dispatcher[IDispatchers]
    Login -->|3. Redirect to Game Server| Player
    Dispatcher --> InstanceA[Game Instance: Lobby]
    Dispatcher --> InstanceB[Game Instance: Game]
```


---
*Developed for learning and fun.*