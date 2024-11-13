using Game2D.core;
using Game2D.game.scenes;

ConfigurationSpecification.SetConsoleVisibility();

Game game = new Game(960, 540, "MyGame");
game.AddLayer(new SandboxScene());
game.Run();
