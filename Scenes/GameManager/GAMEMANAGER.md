# GameManager.cs Documentation

## Class: GameManager

The `GameManager` class is a partial class in the Godot Engine that manages the game scenes and transitions. It is responsible for initializing, replacing, and managing various game screens.

### Properties

1. **dashBoardScene (PackedScene)**
   - Description: The scene to be loaded for the dashboard screen.

2. **quizScene (PackedScene)**
   - Description: The scene to be loaded for the quiz screen.

3. **resultsScene (PackedScene)**
   - Description: The scene to be loaded for the results screen.

4. **startScene (PackedScene)**
   - Description: The scene to be loaded for the start screen.

5. **ESCMenuOverlay (PackedScene)**
   - Description: The scene to be loaded for the ESC menu overlay.

6. **questionDataManagerScene (PackedScene)**
   - Description: The scene containing the `QuestionDataManager` used for managing game questions.

7. **userDataManagerScene (PackedScene)**
   - Description: The scene containing the `UserDataManager` used for managing user data.

8. **questionDataManager (QuestionDataManager)**
   - Description: An instance of `QuestionDataManager` initialized from `questionDataManagerScene`.

9. **userDataManager (UserDataManager)**
   - Description: An instance of `UserDataManager` initialized from `userDataManagerScene`.

10. **gameManager (GameManager)**
    - Description: A reference to the current `GameManager` instance.

11. **currentLoadedScene (Node)**
    - Description: The currently loaded scene.

### Methods

1. **_Ready()**
   - Description: Called when the node is added to the scene tree. Initializes the game manager by instantiating and adding the `questionDataManager`, `userDataManager`, and calling `StartScreen` and `ESCMenuScreen`.

2. **StartScreen()**
   - Description: Instantiates and loads the start screen scene, initializing it with the current `GameManager`.

3. **DashBoardScreen()**
   - Description: Instantiates and loads the dashboard screen scene, initializing it with the current `GameManager` and `userDataManager`.

4. **QuizScreen()**
   - Description: Instantiates and loads the quiz screen scene, initializing it with the current `GameManager`, `questionDataManager`, and setting the number of questions to 100.

5. **DailyQuizScene()**
   - Description: Instantiates and loads the quiz screen scene, initializing it with the current `GameManager`, `questionDataManager`, and setting the number of questions to 5.

6. **ResultScreen(int score, int rate)**
   - Description: Instantiates and loads the results screen scene, initializing it with the current `GameManager`, the given score, and rate.

7. **ESCMenuScreen()**
   - Description: Instantiates and loads the ESC menu overlay scene, initializing it with the current `GameManager`.

8. **ReplaceScene(Node scene)**
   - Description: Replaces the current scene with the provided scene. Frees the old scene and adds the new scene to the scene tree.
   - Parameters:
     - `scene (Node)`: The new scene to be loaded.

## Notes

- The `GameManager` class uses the `[Export]` attribute to allow these properties to be configured in the Godot editor.
- The `ReplaceScene` method ensures that only one scene is loaded at a time by freeing the previous scene and adding the new scene to the scene tree.
- The `StartScreen`, `DashBoardScreen`, `QuizScreen`, `DailyQuizScene`, and `ResultScreen` methods instantiate and load specific game scenes, initializing them with the necessary parameters.
- The `ESCMenuScreen` method loads the ESC menu overlay, which can be used to handle ESC key interactions.
- The `questionDataManager` and `userDataManager` are initialized once when the `GameManager` is created and are used throughout the game to manage questions and user data, respectively.
- The `currentLoadedScene` property keeps track of the currently loaded scene, allowing it to be replaced dynamically.

This documentation provides an overview of the `GameManager` class, its properties, and methods, and how they are used to manage game scenes in the Godot Engine.