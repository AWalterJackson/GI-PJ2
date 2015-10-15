// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using SharpDX;
using SharpDX.Toolkit;
using System;
using System.Collections.Generic;
using Windows.UI.Input;
using Windows.UI.Core;
using Windows.Devices.Sensors;

namespace Project
{
    // Use this namespace here in case we need to use Direct3D11 namespace as well, as this
    // namespace will override the Direct3D11.
    using SharpDX.Toolkit.Graphics;
    using SharpDX.Toolkit.Input;

    public class LabGame : Game
    {
        private GraphicsDeviceManager graphicsDeviceManager;
        public List<GameObject> gameObjects;
        private Stack<GameObject> addedGameObjects;
        private Stack<GameObject> removedGameObjects;
        private KeyboardManager keyboardManager;
        public KeyboardState keyboardState;
        private Player player;
        public AccelerometerReading accelerometerReading;
        public GameInput input;
        public int score;
        public bool gameOver;
        public int size;
        public int edgemax;
        private Land worldBase;
        private Ocean ocean;
        public MainPage mainPage;
        public bool powerups = true;

        // TASK 4: Use this to represent difficulty
        public float difficulty;

        // Represents the camera's position and orientation
        public Camera camera;

        // Graphics assets
        public Assets assets;

        // Random number generator
        public Random random;

        // World boundaries that indicate where the edge of the screen is for the camera.
        public float boundaryLeft;
        public float boundaryRight;
        public float boundaryTop;
        public float boundaryBottom;

        public bool started = false;
        /// <summary>
        /// Initializes a new instance of the <see cref="LabGame" /> class.
        /// </summary>
        public LabGame(MainPage mainPage)
        {
            // Creates a graphics manager. This is mandatory.
            graphicsDeviceManager = new GraphicsDeviceManager(this);

            // Setup the relative directory to the executable directory
            // for loading contents with the ContentManager
            Content.RootDirectory = "Content";

            // Create the keyboard manager
            keyboardManager = new KeyboardManager(this);
            assets = new Assets(this);
            random = new Random();
            input = new GameInput();

            // Set boundaries.
            boundaryLeft = -4.5f;
            boundaryRight = 4.5f;
            boundaryTop = 4;
            boundaryBottom = -4.5f;

            // Initialise event handling.
            input.gestureRecognizer.Tapped += Tapped;
            input.gestureRecognizer.ManipulationStarted += OnManipulationStarted;
            input.gestureRecognizer.ManipulationUpdated += OnManipulationUpdated;
            input.gestureRecognizer.ManipulationCompleted += OnManipulationCompleted;

            this.mainPage = mainPage;

            score = 0;
            difficulty = 1;
            gameOver = false;
            size = 3;
            edgemax = (int)Math.Pow(2, size);
        }

		/// <summary>
		/// Load game content.
		/// </summary>
        protected override void LoadContent()
        {
            // Initialise game object containers.
            gameObjects = new List<GameObject>();
            addedGameObjects = new Stack<GameObject>();
            removedGameObjects = new Stack<GameObject>();

            // Create game objects.
            worldBase = new Land(this, this.size);
            gameObjects.Add(worldBase);
            ocean = new Ocean(this, this.size);
            gameObjects.Add(ocean);
            player = new Player(this);
            gameObjects.Add(player);
            //Enemy enemy = new Enemy(this, new Vector3(1, 1, -5));
            //gameObjects.Add(enemy);
            //gameObjects.Add(new EnemyController(this));

            // Create an input layout from the vertices

            base.LoadContent();
        }

		/// <summary>
		/// Initialize method.
		/// </summary>
        protected override void Initialize()
        {
            Window.Title = "Lab 4";
            camera = new Camera(this);

            base.Initialize();
        }

		/// <summary>
		/// Frame update method.
		/// </summary>
		/// <param name="gameTime">Time since last update.</param>
        protected override void Update(GameTime gameTime)
        {
            if (started)
            {
                if (gameOver == true)
                {
                    this.Exit();
                    this.Dispose();
                    App.Current.Exit();
                    return;
                }
                keyboardState = keyboardManager.GetState();
                flushAddedAndRemovedGameObjects();
                camera.Update();
                accelerometerReading = input.accelerometer.GetCurrentReading();
                for (int i = 0; i < gameObjects.Count; i++)
                {
                    gameObjects[i].Update(gameTime);
                }

                mainPage.UpdateScore(score);

                if (keyboardState.IsKeyDown(Keys.Escape))
                {
                    this.Exit();
                    this.Dispose();
                    App.Current.Exit();
                }
                // Handle base.Update
            }
            base.Update(gameTime);

        }

		/// <summary>
		/// Render everything that needs to be rendered.
		/// </summary>
		/// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            if (started)
            {
                // Clears the screen with the Color.CornflowerBlue
                GraphicsDevice.Clear(Color.CornflowerBlue);

                for (int i = 0; i < gameObjects.Count; i++)
                {
                    gameObjects[i].Draw(gameTime);
                    gameObjects[i].basicEffect.View = camera.View;
                    gameObjects[i].basicEffect.Projection = camera.Projection;
                }
            }
            // Handle base.Draw
            base.Draw(gameTime);
        }

		/// <summary>
		/// Count the number of game objects for a certain type.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
        public int Count(GameObjectType type)
        {
            int count = 0;
            foreach (var obj in gameObjects)
            {
                if (obj.type == type) { count++; }
            }
            return count;
        }

		/// <summary>
		/// Add a new game object.
		/// </summary>
		/// <param name="obj"></param>
        public void Add(GameObject obj)
        {
            if (!gameObjects.Contains(obj) && !addedGameObjects.Contains(obj))
            {
                addedGameObjects.Push(obj);
            }
        }

		/// <summary>
		/// Remove a game object.
		/// </summary>
		/// <param name="obj"></param>
        public void Remove(GameObject obj)
        {
            if (gameObjects.Contains(obj) && !removedGameObjects.Contains(obj))
            {
                removedGameObjects.Push(obj);
            }
        }

		/// <summary>
		/// Process the buffers of game objects that need to be added/removed.
		/// </summary>
        private void flushAddedAndRemovedGameObjects()
        {
            while (addedGameObjects.Count > 0) { gameObjects.Add(addedGameObjects.Pop()); }
            while (removedGameObjects.Count > 0) { gameObjects.Remove(removedGameObjects.Pop()); }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
        public void OnManipulationStarted(GestureRecognizer sender, ManipulationStartedEventArgs args)
        {
            // Pass Manipulation events to the game objects.

        }

		/// <summary>
		/// Initiate event upon recieving a tap input.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
        public void Tapped(GestureRecognizer sender, TappedEventArgs args)
        {
            // Pass Manipulation events to the game objects.
            foreach (var obj in gameObjects)
            {
                obj.Tapped(sender, args);
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
        public void OnManipulationUpdated(GestureRecognizer sender, ManipulationUpdatedEventArgs args)
        {
            camera.pos.Z = camera.pos.Z * args.Delta.Scale;
            // Update camera position for all game objects
            foreach (var obj in gameObjects)
            {
                if (obj.basicEffect != null) { obj.basicEffect.View = camera.View; }
                obj.OnManipulationUpdated(sender, args);
            }
        }

        public void OnManipulationCompleted(GestureRecognizer sender, ManipulationCompletedEventArgs args)
        {

        }

        public void start(float playerSpeed, float playerAcceleration, bool powerups, float difficulty)
        {
            started = true;
            player.maxspeed = playerSpeed;
            player.maxaccel = playerAcceleration;
            this.powerups = powerups;
            this.difficulty = difficulty;
        }

    }
}
