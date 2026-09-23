# Bowling Blast Build Notes

## Implemented Required Features

- **XR setup:** The project uses XR Interaction Toolkit 3.4.1. The bowling scene contains an XR Origin, Direct Interactor, and teleportation areas for VR locomotion.
- **Interactables:** The scene includes an interactable bowling ball and bowling pins with Rigidbody and collider physics.
- **Ball reset:** The ball can be reset after a throw through the respawn trigger. Skybox and OOB areas can also be configured as ball-only reset areas.
- **Pin reset:** Fallen pins are detected, their settled state is checked, and the reset block animates down before the pins are restored and made visible again.
- **Pin audio:** Pin collision audio is supported through `CollisionAudio` and the BowlingPin audio asset.
- **Environment:** The project includes a complete virtual bowling alley environment with lane, pin area, walls, back area, lighting, materials, and imported environment assets.

## Implemented Optional Features

- **Ball audio:** The ball prefab includes an AudioSource and `CollisionAudio`, using the bowling collision sound resource.
- **Scoring:** `BowlingScoreManager` records each roll, counts fallen pins, preserves the fallen pin indices for the pin layout, and calculates strikes, spares, open frames, and running totals.
- **World Space scoreboard:** `BowlingScoreboard` displays the score through a TextMeshPro text component. It is intended to be placed on a World Space Canvas positioned in front of the scoreboard screen in column 2 of `SM_TVScreen1`.
- **Additional presentation:** The project uses URP lighting and baked environment/light data to improve the bowling alley presentation.

## Scoreboard Unity Setup

1. Create or select a GameObject with `BowlingScoreManager` in the bowling scene.
2. Select the `PinManager` and assign that score manager to its `Score Manager` field.
3. Select the `SM_TVScreen1` instance in column 2.
4. Create a child Canvas or a small plane with a child Canvas positioned just in front of the TV screen.
5. Set the Canvas to **World Space** and scale it to fit the screen.
6. Add a TextMeshPro text object under the Canvas.
7. Add `BowlingScoreboard` to the Canvas or scoreboard object.
8. Assign the `BowlingScoreManager` object to `Score Manager`.
9. Assign the TextMeshPro object to `Scoreboard Text`.
10. Set the Canvas and text orientation so the text faces the XR camera.

## Notes for Evaluation

The project records the pin layout by the order of the pins in the `PinManager.pins` array. Keep that array in a consistent lane-position order so saved fallen-pin patterns can be interpreted as splits, spares, and other pin arrangements.

For the reset behavior, configure the ball's reset-area arrays in the Inspector. Use the normal reset areas for the pin-reset procedure and the ball-only reset areas for OOB or Skybox surfaces.

The scripts currently compile without reported errors. Before demonstrating, verify the scene Inspector references for the PinManager, BowlingScoreManager, scoreboard TMP text, ball respawn point, and reset-area lists.
