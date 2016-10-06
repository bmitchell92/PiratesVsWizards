using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TowerDefenceMap
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        public bool DEBUG = false;
        public MapManager mapManager;
        public WizardManager wizardManager;

        public PirateManager pirateManager;

        public Texture2D selectionTexture;

        public GamePadState currentgamePadState;
        public GamePadState previousgamePadState;

        public GamePadState currentgamePadStatep2;
        public GamePadState previousgamePadStatep2;

        public KeyboardState currentKeyboardState;
        public KeyboardState previousKeyboardState;

        public Random random;

        public AudioEngine engine;
        public SoundBank soundBank;
        public WaveBank waveBank;

        public Cue music;
        public Cue summon;

        private Sprite startMenu;
        private Sprite pauseMenu;
        private Sprite winScreen;
        private Sprite loseScreen;

        int GameState = 1;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            this.graphics.PreferredBackBufferHeight = 820;
            this.graphics.PreferredBackBufferWidth = 861;
            this.graphics.IsFullScreen = false;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            random = new Random();
            
            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.

            // Initialize audio objects.
            engine = new AudioEngine("Content\\Audio\\PiratesVsWizardsSounds.xgs");
            soundBank = new SoundBank(engine, "Content\\Audio\\Sound Bank.xsb");
            waveBank = new WaveBank(engine, "Content\\Audio\\Wave Bank.xwb");

            music = soundBank.GetCue("Alestorm - You Are a Pirate!");
            summon = soundBank.GetCue("summon");

            selectionTexture = Content.Load<Texture2D>(@"Images/select");
            spriteBatch = new SpriteBatch(GraphicsDevice);
            mapManager = new MapManager(this);
            wizardManager = new WizardManager(this);
            pirateManager = new SPPirateManager(this);

            startMenu = new Sprite(this, Vector2.Zero, "images/startmenu");
            pauseMenu = new Sprite(this, Vector2.Zero, "images/pausemenu");
            winScreen = new Sprite(this, Vector2.Zero, "images/wizardwinscreen");
            loseScreen = new Sprite(this, Vector2.Zero, "images/wizardlostscreen");

            music.Play();

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || currentKeyboardState.IsKeyUp(Keys.Escape) && previousKeyboardState.IsKeyDown(Keys.Escape))
                this.Exit();

            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            previousgamePadState = currentgamePadState;
            currentgamePadState = GamePad.GetState(PlayerIndex.One);

            previousgamePadStatep2 = currentgamePadStatep2;
            currentgamePadStatep2 = GamePad.GetState(PlayerIndex.Two);

            if (GameState == 1)
            {
                if (currentKeyboardState.IsKeyUp(Keys.S) && previousKeyboardState.IsKeyDown(Keys.S) || currentgamePadState.IsButtonUp(Buttons.Start) && previousgamePadState.IsButtonDown(Buttons.Start))
                {
                    GameState = 2;
                }

                base.Update(gameTime);
            }
            else if (GameState == 2)
            {
                if (currentKeyboardState.IsKeyUp(Keys.P) && previousKeyboardState.IsKeyDown(Keys.P) || currentgamePadState.IsButtonUp(Buttons.Start) && previousgamePadState.IsButtonDown(Buttons.Start))
                {
                    GameState = 3;
                }

                mapManager.Update(gameTime);

                pirateManager.Update(gameTime);
                wizardManager.Update(gameTime);

                if (wizardManager.castle.health <= 0 || pirateManager.pirateShip.health <= 0)
                {
                    GameState = 4;
                }

                base.Update(gameTime);
            }
            else if (GameState == 3)
            {
                if (currentKeyboardState.IsKeyUp(Keys.P) && previousKeyboardState.IsKeyDown(Keys.P) || currentgamePadState.IsButtonUp(Buttons.Start) && previousgamePadState.IsButtonDown(Buttons.Start))
                {
                    GameState = 2;
                }
                base.Update(gameTime);
            }
            else if (GameState == 4)
            {
                base.Update(gameTime);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            if (GameState == 1)
            {
                startMenu.Draw();
            }
            else if (GameState == 2)
            {

                mapManager.Draw();


                pirateManager.Draw();
                wizardManager.Draw();
            }
            else if (GameState == 3)
            {
                pauseMenu.Draw();
            }
            else if (GameState == 4)
            {
                if (wizardManager.castle.health <= 0)
                {
                    music.Pause();
                    loseScreen.Draw();
                }
                else
                {
                    winScreen.Draw();
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
