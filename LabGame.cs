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

    using SharpDX.Toolkit.Graphics;
    using SharpDX.Toolkit.Input;

    public class LabGame : Game
    {
        private GraphicsDeviceManager graphicsDeviceManager;    //Graphics device manager
        public List<GameObject> gameObjects;                    //List of active game objects
        private Stack<GameObject> addedGameObjects;             //game objects to be added to the active list
        private Stack<GameObject> removedGameObjects;           //game objects to be removed from the active list
        private KeyboardManager keyboardManager;                //Keyboard input manager
        public KeyboardState keyboardState;                     //Keyboard state to read input from
        private Player player;                                  //Player entity
        public AccelerometerReading accelerometerReading;       //Input from accelerometers
        public GameInput input;                                 //Input class into the game
        private EnemyController controller;                     //Enemy spawning and AI controller
        public LightingController lighting;                     //Lighting and shading controller
        public int score;                                       //Player score
        public bool gameOver;                                   //Game exits when you lose, no second chances
        public int size;                                        //Map size
        public int edgemax;                                     //Maximum map coordinate value
        public Land worldBase;                                  //Land under the ocean
        private Ocean ocean;                                    //The ocean
        public MainPage mainPage;                               //Main XAML interface page
        public int windowHeight, windowWidth;                   //Window height and width
        public bool lightingSystemOn = true;                    //Control variable for basiceffect lighting on/off

        //Difficulty representation
        public float difficulty;

        // Represents the camera's position and orientation
        public Camera camera;

        // Graphics assets
        public Assets assets;

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
            //Assets utility class
            assets = new Assets(this);
            //Input parser
            input = new GameInput();

            // Initialise event handling.
            input.gestureRecognizer.Tapped += Tapped;
            input.gestureRecognizer.ManipulationStarted += OnManipulationStarted;
            input.gestureRecognizer.ManipulationUpdated += OnManipulationUpdated;
            input.gestureRecognizer.ManipulationCompleted += OnManipulationCompleted;

            //Create main XAML interface
            this.mainPage = mainPage;

            //Initialise game state
            score = 0;
            difficulty = 1;
            gameOver = false;
            size = 7;
			edgemax = (int)Assets.WORLD_WIDTH;
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
            lighting = new LightingController(this);
            gameObjects.Add(lighting);
            player = new Player(this);
            gameObjects.Add(player);
            controller = new EnemyController(this);
            gameObjects.Add(controller);

            base.LoadContent();
        }

		/// <summary>
		/// Initialize method.
		/// </summary>
        protected override void Initialize()
        {
            Window.Title = "Piracy!";
            camera = new Camera(this);

            base.Initialize();
        }

		/// <summary>
		/// Frame update method.
		/// </summary>
		/// <param name="gameTime">Time since last update.</param>
        protected override void Update(GameTime gameTime)
        {
            if (started) //Do nothing if the game has not started
            {
                if (gameOver == true)   //Exit if you lose. NO SECOND CHANCES
                {
                    this.Exit();
                    this.Dispose();
                    App.Current.Exit();
                    return;
                }

                keyboardState = keyboardManager.GetState(); //Get keyboard state
                flushAddedAndRemovedGameObjects();          //Add and remove waiting objects
                camera.Update();                            //Update the camera first
                accelerometerReading = input.accelerometer.GetCurrentReading(); //Read accelerometers
                for (int i = 0; i < gameObjects.Count; i++) //Update all other game objects and entities
                {
                    gameObjects[i].Update(gameTime);
                }

                mainPage.UpdateScore(score);    //Increment the player score
                mainPage.UpdateHitpoints(player.hitpoints); //Adjust the hitpoints display

                if (keyboardState.IsKeyDown(Keys.Escape)) //Exit the game when escape is mashed
                {
                    this.Exit();
                    this.Dispose();
                    App.Current.Exit();
                }

            }
            else
            {
                windowHeight = Window.ClientBounds.Height;
                windowWidth = Window.ClientBounds.Width;
            }

            // Handle base.Update
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
                // Clears the screen with Black
                GraphicsDevice.Clear(Color.Black);

                for (int i = 0; i < gameObjects.Count; i++)
                {
                    gameObjects[i].Draw(gameTime);
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

        //Get the player position for other classes
        public Vector3 getPlayerPos()
        {
            return this.player.pos;
        }

        //Get the player velocity for other classes
        public Vector3 getPlayerVel()
        {
            return this.player.velocity;
        }

        //Get the player acceleration for other classes
        public Vector3 getPlayeraccel()
        {
            return this.player.acceleration;
        }

        //Allow other classes to add a light to the lighting queue
        public void addLight(LightingController.LightSource light)
        {
            lighting.Add(light);
        }

        //Allow other classes to remove their light from the lighting queue
        public void removeLight(LightingController.LightSource light)
        {
            lighting.Remove(light);
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
            player.Tapped(sender, args); //Only the player can react to taps
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
        public void OnManipulationUpdated(GestureRecognizer sender, ManipulationUpdatedEventArgs args)
        {
            camera.pos.Z = camera.pos.Z * args.Delta.Scale; //Only the camera responds to pinch/stretch
        }

        public void OnManipulationCompleted(GestureRecognizer sender, ManipulationCompletedEventArgs args)
        {
            //Not Used
        }

        //Start the game from the main menu
        public void start(float playerSpeed, float playerAcceleration, float difficulty)
        {
            started = true;
            player.maxspeed = player.maxspeed * playerSpeed;
            player.maxaccel = player.maxaccel * playerAcceleration;
            player.hitpoints = (int)(100/difficulty);
        }

    }
}
