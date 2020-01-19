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


namespace TurboSpaceChase
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        KeyboardState keyboardState;
        KeyboardState oldKeyboardState;
        GameScreen activeScreen;
        MenuInicial menuinicial;
        Level1 level1;
        ConfiguraNaves configuranaves;
        SoundEffect somtiro;
        SoundEffect rocket;
        SoundEffect colisao;
        SoundEffect explode;
        SoundEffect flare;
        Song musica;
        Texture2D comandosp1;
        Texture2D comandosp2;
        int playsong = 0;

        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            graphics.IsFullScreen = false;
            IsMouseVisible = false;
            graphics.ApplyChanges();
            Window.Title = "TURBO SPACE CHASE";
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            menuinicial = new MenuInicial(
                this,
                spriteBatch,
                Content.Load<SpriteFont>("Tela/menufont"),
                Content.Load<Texture2D>("Tela/01_menu"));

            comandosp1 = Content.Load<Texture2D>("imagens/posp1");
            comandosp2 = Content.Load<Texture2D> ("imagens/posp2");
            somtiro = Content.Load<SoundEffect>("audios/somtiro");
            rocket = Content.Load<SoundEffect>("audios/rocket");
            colisao = Content.Load<SoundEffect>("audios/colisao");
            explode = Content.Load<SoundEffect>("audios/explode");
            flare = Content.Load<SoundEffect>("audios/flare");
            musica = Content.Load<Song>("audios/musica");

            Components.Add(menuinicial);
            menuinicial.Hide();

            configuranaves = new ConfiguraNaves(
                this,
                spriteBatch,
                Content.Load<Texture2D>("Tela/02_telaEscolha"));
            Components.Add(configuranaves);
            configuranaves.Hide();

            level1 = new Level1(
                this,
                spriteBatch,
                Content.Load<Texture2D>("Tela/03_jogo"),
                configuranaves);
            Components.Add(level1);
            level1.Hide();
            activeScreen = menuinicial;
            activeScreen.Show();
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (playsong == 0)
            {
                MediaPlayer.Play(musica);
                playsong = 1;
            }

            if (activeScreen == menuinicial)
            {
                if (CheckKey(Keys.Enter))
                {
                    explode.Play();
                    configuranaves.Limpar();
                    if (menuinicial.SelectedIndex == 0)
                    {
                        activeScreen.Hide();
                        activeScreen = configuranaves;
                        activeScreen.Show();
                    }
                    if (menuinicial.SelectedIndex == 1)
                    {
                        this.Exit();
                    }
                }
                if (CheckKey(Keys.Up))
                    flare.Play();
                if (CheckKey(Keys.Down))
                    flare.Play();
            }
            if (activeScreen == configuranaves)
             {
                if (configuranaves.iniciolistanave > 1)
                {
                    level1.Limpar();
                    level1.configuranaves = this.configuranaves;
                    activeScreen.Hide();
                    activeScreen = level1;
                    activeScreen.Show();
                }
            }
            if (activeScreen == level1)
            {
                if (CheckKey(Keys.Escape))
                {  
                    configuranaves.Hide();
                    activeScreen = menuinicial;
                    activeScreen.Show();
                }

                if (CheckKey(Keys.P))
                    somtiro.Play();
                if (CheckKey(Keys.R))
                    somtiro.Play();
                if (CheckKey(Keys.T))
                    rocket.Play();
                if (CheckKey(Keys.Down))
                    flare.Play();
            }
           base.Update(gameTime);
            oldKeyboardState = keyboardState;
        }

        private bool CheckKey(Keys theKey)
        {
            return keyboardState.IsKeyUp(theKey) &&
                oldKeyboardState.IsKeyDown(theKey);
        }

        protected override void Draw(GameTime gameTime)
        {

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            if (activeScreen == level1)
            {
                spriteBatch.Draw(comandosp1, new Vector2(50.0f, 450.0f), Color.White);
                spriteBatch.Draw(comandosp2, new Vector2(630.0f, 450.0f), Color.White);
            }

            base.Draw(gameTime);
            spriteBatch.End();
        }
    }
}
