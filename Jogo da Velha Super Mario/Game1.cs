using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System;

namespace Jogo_da_Velha_Super_Mario
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Texture2D bgMario, icone, cube, borda, transparente;
        private SpriteFont fontMario;
        private int largura, altura;
        private SoundEffect sndMenu;
        private SoundEffectInstance playSound;
        private UIButton btUmPlayer, btDoisPlayers, btComIA, btSemIA;
        private UIBotaoSelect btSelect, btSelectIA;
        private MouseState prevMouseState;

        private enum GameState { Null, MainMenu, DificuldadeMenu, PlayerTurn, ComputerTurn, ShowResults };
        private GameState currGameState = GameState.Null;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            largura = 800;
            altura = 600;
            graphics.PreferredBackBufferWidth = largura;
            graphics.PreferredBackBufferHeight = altura;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

        }


        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            fontMario = Content.Load<SpriteFont>("Fonts/Texture");
            bgMario = Content.Load<Texture2D>("Sprites/MenuMario_800_600");
            icone = Content.Load<Texture2D>("Sprites/icon");
            cube = Content.Load<Texture2D>("Sprites/cube_100_90");
            borda = Content.Load<Texture2D>("Sprites/borda");
            transparente = Content.Load<Texture2D>("Sprites/transparente");
            sndMenu = Content.Load<SoundEffect>("Sounds/smw_menu");

            btUmPlayer = new UIButton(new Vector2(251, 300), new Vector2(298, 29), transparente, "1 PLAYER GAME", fontMario);
            btDoisPlayers = new UIButton(new Vector2(251, 364), new Vector2(298, 29), transparente, "2 PLAYER GAME", fontMario);
            btComIA = new UIButton(new Vector2(320, 300), new Vector2(160, 29), transparente, "COM I.A.", fontMario);
            btSemIA = new UIButton(new Vector2(320, 364), new Vector2(160, 29), transparente, "SEM I.A.", fontMario);

            btSelect = new UIBotaoSelect(new Vector2(215, 306), new Vector2(20, 16), icone);
            btSelectIA = new UIBotaoSelect(new Vector2(280, 306), new Vector2(20, 16), icone);

            playSound = sndMenu.CreateInstance();
            playSound.IsLooped = true;
            playSound.Play();
            EnterGameState(GameState.MainMenu);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }


            UpdateGameState(gameTime);
            prevMouseState = Mouse.GetState();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied);

            DrawGameState(gameTime);

            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void EnterGameState(GameState newState)
        {
            LeaveGameState();
            currGameState = newState;

            switch (currGameState)
            {
                case GameState.MainMenu:
                    {
                        //board.Limpar();
                    }
                    break;

                case GameState.DificuldadeMenu:
                    {

                    }
                    break;
                case GameState.PlayerTurn:
                    {

                    }
                    break;

                case GameState.ComputerTurn:
                    {
                    }
                    break;


                case GameState.ShowResults:
                    { }
                    break;

            }
        }

        private void LeaveGameState()
        {
            switch (currGameState)
            {
                case GameState.MainMenu: { } break;
                case GameState.DificuldadeMenu: { } break;
                case GameState.PlayerTurn: { } break;
                case GameState.ComputerTurn: { } break;
                case GameState.ShowResults: { } break;
            }
        }

        public void DrawGameState(GameTime gameTime)
        {
            switch (currGameState)
            {
                case GameState.MainMenu:
                    {
                        float t = (float)(gameTime.TotalGameTime.TotalSeconds * MathHelper.Pi * 1);
                        float alpha = (float)Math.Abs(Math.Sin(t));

                        
                        spriteBatch.Draw(bgMario, new Vector2(0, 0));

                        btUmPlayer.Draw(spriteBatch);

                        btDoisPlayers.Draw(spriteBatch);

                        btSelect.Draw(spriteBatch);
                        
                        /*spriteBatch.Draw(borda, new Vector2(10, 10), new Color(1.0f, 1.0f, 1.0f, alpha));
                        //spriteBatch.Draw(borda, new Vector2(10, 10));
                        spriteBatch.Draw(cube, new Vector2(10, 10));
                        spriteBatch.Draw(cube, new Vector2(110, 10));
                        spriteBatch.Draw(cube, new Vector2(210, 10));

                        spriteBatch.Draw(cube, new Vector2(10, 100));
                        spriteBatch.Draw(cube, new Vector2(110, 100));
                        spriteBatch.Draw(cube, new Vector2(210, 100));

                        spriteBatch.Draw(cube, new Vector2(10, 190));
                        spriteBatch.Draw(cube, new Vector2(110, 190));
                        spriteBatch.Draw(cube, new Vector2(210, 190));*/

                        //int meiaAultura = altura / 2;
                        //int meialargura = largura / 2;
                        

                        //DrawString("1 PLAYER GAME", new Vector2(meialargura, meiaAultura + 16), new Color(1.0f, 1.0f, 1.0f, 1));
                        //DrawString("2 PLAYER GAME", new Vector2(meialargura, meiaAultura + 80), new Color(1.0f, 1.0f, 1.0f, 1));



                        //spriteBatch.Draw(icone, vectorP1, new Color(1.0f, 1.0f, 1.0f, alpha));
                        //spriteBatch.Draw(icone, vectorP2, new Color(1.0f, 1.0f, 1.0f, alpha));

                    }
                    break;

                case GameState.DificuldadeMenu:
                    {
                        spriteBatch.Draw(bgMario, new Vector2(0,0));

                        btSelectIA.Draw(spriteBatch);
                        btComIA.Draw(spriteBatch);
                        btSemIA.Draw(spriteBatch);
                    }
                    break;

                case GameState.PlayerTurn:
                    {
                    }
                    break;

                case GameState.ComputerTurn:
                    {
                    }
                    break;

                case GameState.ShowResults:
                    {
                    }
                    break;
            }
        }

        private void DrawString(string text, Vector2 pos, Color color)
        {
            Vector2 textSize = fontMario.MeasureString(text);

            spriteBatch.DrawString(
                fontMario,
                text,
                pos,                //position 
                Color.White,        //color
                0.0f,               //rotation
                textSize / 2.0f,    //origin (pivot)
                Vector2.One,        //scale
                SpriteEffects.None,
                0.0f
            );


        }

        private void DrawStringD(string text, Vector2 pos, Color color)
        {
            Vector2 textSize = fontMario.MeasureString(text);

            spriteBatch.DrawString(
                fontMario,
                text,
                pos,                //position 
                Color.White,        //color
                0.0f,               //rotation
                new Vector2(textSize.X,textSize.Y/2),    //origin (pivot)
                Vector2.One,        //scale
                SpriteEffects.None,
                0.0f
            );


        }

        public void UpdateGameState(GameTime gameTime)
        {
            switch (currGameState)
            {
                case GameState.MainMenu:{
                        if (btSelect.TesteSeta() == 0)
                        {
                            btSelect.SetPosition(215, 370);
                            btSelect.SetPrimeiro(false);
                        }
                        else if(btSelect.TesteSeta() == 1)
                        {
                            btSelect.SetPosition(215, 306);
                            btSelect.SetPrimeiro(true);
                        }

                        else if (btSelect.TesteSeta() == 2) // 1 player
                        {
                            EnterGameState(GameState.DificuldadeMenu);
                        }

                        else if (btSelect.TesteSeta() == 3) // 2 player
                        {
                            //Vai para a tela de jogo de 2 players
                            //EnterGameState(GameState.DificuldadeMenu);
                        }


                        if (Mouse.GetState().LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                        {
                            Vector2 pTeste = new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y);

                            if (btUmPlayer.TesteClick(pTeste))
                            {
                                EnterGameState(GameState.DificuldadeMenu);
                            }
                            else if(btDoisPlayers.TesteClick(pTeste)) // Selecionou o segundo
                             {
                                //EnterGameState(GameState.DificuldadeMenu);
                            }
                        }
                            break;

                }

                case GameState.DificuldadeMenu:
                    {
                        if(Keyboard.GetState().IsKeyDown(Keys.B) || Keyboard.GetState().IsKeyDown(Keys.Back))
                        {
                            EnterGameState(GameState.MainMenu);
                        }


                        if (btSelectIA.TesteSeta() == 0) // Down
                        {
                            btSelectIA.SetPosition(280, 370);
                            btSelectIA.SetPrimeiro(false);
                        }
                        else if (btSelectIA.TesteSeta() == 1)// UP
                        {
                            btSelectIA.SetPosition(280, 306);
                            btSelectIA.SetPrimeiro(true);
                        }

                        else if (btSelectIA.TesteSeta() == 2) //Enter com IA
                        {
                            //btSelect.setPosition(215, 306);
                        }

                        else if (btSelectIA.TesteSeta() == 3) // Enter sem IA
                        {
                            //Vai para a tela de jogo de 2 players
                            //EnterGameState(GameState.DificuldadeMenu);
                        }

                        if (Mouse.GetState().LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                        {
                            Vector2 pTeste = new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y);

                            if (btSelect.TesteClick(pTeste))
                            {
                                Vector2 pos = btSelect.GetPosition();

                                if (btComIA.TesteClick(pTeste))
                                {
                                    EnterGameState(GameState.DificuldadeMenu);
                                }
                                else if (btSemIA.TesteClick(pTeste)) // Selecionou o segundo
                                {
                                    //EnterGameState(GameState.DificuldadeMenu);
                                }

                            }
                        }
                        break;
                    }
            }
        }
    }
}
