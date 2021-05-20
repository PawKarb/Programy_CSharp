using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Threading;
using Windows.ApplicationModel.Core;

namespace KarbowskiAstro
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Rakieta gracz;
        private Meteor wrog;
        private Meteor wrogDrugi;
        private Texture2D rakieta;
        private Texture2D meteor;
        private Texture2D control;
        private Texture2D niebo;
        private Texture2D pocisk;
        private DateTime timeOfGameOver;
        private SpriteFont wykrytoKolizje, wykrytoKolizjePocisk;
        public int score = 0;
        private bool isGameOver = false;
        SoundEffectInstance wybuchRaz;
        SoundEffect wybuch;

        enum States
        {
            GameOver,
            Game,
        }

        States _state;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

        }

        protected override void Initialize()
        {
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 480;
            graphics.PreferredBackBufferHeight = 800;
            graphics.ApplyChanges();
            _state = States.Game;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            niebo = Content.Load<Texture2D>("niebo");
            rakieta = Content.Load<Texture2D>("AnimRakiety");
            meteor = Content.Load<Texture2D>("meteor");
            pocisk = Content.Load<Texture2D>("pocisk2D");
            control = Content.Load<Texture2D>("control");
            wykrytoKolizje = Content.Load<SpriteFont>("Kolizja");
            wykrytoKolizjePocisk = Content.Load<SpriteFont>("KolizjaPocisk");
            wybuch = Content.Load<SoundEffect>("wybuch");
            wrog = new Meteor(meteor, 10);
            wrogDrugi = new Meteor(meteor, 20);
            gracz = new Rakieta(rakieta, pocisk);
            wybuchRaz = wybuch.CreateInstance();
        }
        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            switch (_state)
            {
                case States.Game:
                    wrog.Update();
                    wrogDrugi.Update();
                    gracz.LotPocisku();
                    if (wrog.Kolizja(gracz) || wrogDrugi.Kolizja(gracz))
                    {
                        wybuchRaz.Play();
                        isGameOver = true;
                        _state = States.GameOver; 
                        timeOfGameOver = DateTime.Now;
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.X))
                    {
                        CoreApplication.Exit(); 
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.W))
                    {
                        gracz.MoveU();
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.S))
                    {
                        gracz.MoveD();
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.A))
                    {
                        gracz.MoveL();
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.D))
                    {
                        gracz.MoveR();
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        gracz.Wystrzel();
                    }

                    TouchCollection mscaDotknięte = TouchPanel.GetState();
                    foreach (TouchLocation dotyk in mscaDotknięte)
                    {
                        Vector2 pozDotyku = dotyk.Position;
                        if (dotyk.State == TouchLocationState.Moved)
                        {
                            if (Math.Pow(pozDotyku.X - 110, 2) + (Math.Pow(pozDotyku.Y - 645, 2)) <= 40 * 40) 
                            {
                                gracz.MoveU();
                            }
                            if (Math.Pow(pozDotyku.X - 110, 2) + (Math.Pow(pozDotyku.Y - 740, 2)) <= 40 * 40) 
                            {
                                gracz.MoveD();
                            }
                            if (Math.Pow(pozDotyku.X - 60, 2) + (Math.Pow(pozDotyku.Y - 690, 2)) <= 40 * 40) 
                            {
                                gracz.MoveL();
                            }
                            if (Math.Pow(pozDotyku.X - 160, 2) + (Math.Pow(pozDotyku.Y - 690, 2)) <= 40 * 40) 
                            {
                                gracz.MoveR();
                            }
                        }
                        if (dotyk.State == TouchLocationState.Pressed)
                        {
                            if (Math.Pow(pozDotyku.X - 375, 2) + (Math.Pow(pozDotyku.Y - 695, 2)) <= 40 * 40) 
                            {
                                gracz.Wystrzel();
                            }
                        }
                    }
                    break;
                case States.GameOver:
                    if ((DateTime.Now - timeOfGameOver).TotalSeconds >= 3) 
                    {
                        wybuchRaz.Stop(); 
                        Thread.Sleep(500); 
                        CoreApplication.Exit(); 
                    }
                    break;
            }
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            spriteBatch.Draw(niebo, new Vector2(0, 0), Color.White); 
            gracz.Draw(rakieta, spriteBatch); 
            wrog.Draw(spriteBatch);
            wrogDrugi.Draw(spriteBatch);
            spriteBatch.Draw(control, new Vector2(0, 583), Color.White); 
            if (isGameOver == true)
            {
                spriteBatch.DrawString(wykrytoKolizje, "Game Over!!!", new Vector2(90, 300), Color.White); 
            }
            spriteBatch.DrawString(wykrytoKolizjePocisk, "Score:" + (wrog.GetScore() + wrogDrugi.GetScore()), new Vector2(410, 780), Color.White); 

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
