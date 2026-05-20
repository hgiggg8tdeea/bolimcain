# Development Notes & Progress Tracker

## Current Status
- **Phase**: Pre-Production / Prototype
- **Last Updated**: May 20, 2026
- **Perspective**: 2D Top-Down ✅ DECIDED
- **Next Focus**: Core mechanics implementation

---

## Design Decisions

### Perspective Decision (LOCKED)
- ✅ 2D Top-Down view (Pokémon-style)
- Isometric view: Not selected
- Camera: Fixed overhead, follows player

**Rationale**: Faster solo development, fits Pokémon fan game aesthetic, great for strategic pod placement visibility

---

## Technical Decisions

### 2D Specifics
- **Sprite Size**: TBD (recommend 32x32 or 16x16 grid)
- **Camera**: Orthographic, top-down
- **Collision**: 2D BoxColliders
- **Rendering**: Sprite Renderer with sorting layers
- **Input**: Keyboard (WASD) + Mouse (pod placement)

### Physics
- 2D Rigidbody for player movement
- 2D colliders for obstacles and creatures
- Simple physics, no complex gravity

### Networking
- Solo single-player only (no multiplayer)

### Save System
- TBD: File-based or cloud saves

### Input System
- **Movement**: WASD or Arrow Keys
- **Interact**: E key
- **Pod Placement**: Left Mouse Click
- **Pause**: ESC
- **Consider**: Controller support (optional)

---

## Asset Pipeline (2D Specific)

### Sprites Needed (Phase 1)
- [ ] Player character (4 directional sprites or 1 with animator)
- [ ] Basic creature type (1 invader type for prototype)
- [ ] Bubble pod (placed and active states)
- [ ] Tileset (grass, ground, obstacles)
- [ ] UI icons (health, pod count, objectives)

### Animation Requirements
- [ ] Player walk cycle (4 directions)
- [ ] Creature walk/patrol cycle
- [ ] Pod deployment animation
- [ ] Creature containment animation

### Placeholder Strategy
- Use simple colored rectangles/circles initially
- Replace with proper sprites later
- Focus on mechanics first, visuals second

---

## Core Scripts to Create (Phase 1)

- [ ] PlayerController.cs - Movement and interaction
- [ ] BubblePodSystem.cs - Pod placement and containment
- [ ] CreatureAI.cs - Enemy behavior
- [ ] GameManager.cs - Level management
- [ ] UIManager.cs - HUD and menus

---

## Known Limitations (Solo Dev)

1. Sprite art creation will be time-intensive
2. Voice acting likely out of scope
3. Complex physics not needed (2D is simpler)
4. QA limited to personal testing
5. Animation frame-heavy if doing pixel art

---

## Recommended Art Style for Solo

- **Pixel Art**: Retro, smaller file size, easier animation
- **Simple Vector**: Clean lines, scalable
- **Hybrid**: Simple pixel sprites with vector UI

**Suggestion**: Start with 32x32 pixel art or simple vector shapes

---

## Quick Reference

**Main Mechanic**: Bubble pod strategic placement  
**Core Loop**: Explore → Identify threats → Deploy pods → Contain → Progress  
**Target Players**: Pokémon fans who like strategy  
**Unique Hook**: Extinction-level invasion + pod containment twist  
**Perspective**: 2D Top-Down (like classic Pokémon)

---

## Phase 1 Checklist

- [ ] Unity 2022 LTS project created and configured
- [ ] Scenes folder structure ready
- [ ] Player movement script implemented
- [ ] Basic creature spawner
- [ ] Bubble pod placement system
- [ ] Simple test level created
- [ ] Core loop playtested

---

Update this file as you make progress and decisions!
