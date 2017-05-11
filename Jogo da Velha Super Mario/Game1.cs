using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System;
using Jogo_da_Velha_Super_Mario.Content;
using System.Collections.Generic;

namespace Jogo_da_Velha_Super_Mario
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Texture2D bgMario, icone, cube, borda, transparente, pena, flor, final, fundoJogo, transparente_borda;
        private SpriteFont fontMario;
        private int largura, altura;
        private SoundEffect sndMenu, sndJogada, sndFimDojogo;
        private SoundEffectInstance playSound, jogadaSound, fimDojogoSound;
        private UIButton btUmPlayer, btDoisPlayers, btComIA, btSemIA, btUserJogaPrimeiro, btIAJogaPrimeiro, btReiniciarPartida, btNaoReiniciarPartida;
        private UIBotaoSelect btSelect, btSelectIA, btSelectPlayer;
        private MouseState prevMouseState;
        private KeyboardState prevKeyState;
        private bool musica, musicaJogada;
        private Board board;
        private Vector2 cellPos;
        private float stateTimer;

        private bool umPlayer, comIA, jogaPrimeiro; // para funcionar o jogo verificar se true or false

        private enum GameState { Null, MainMenu, DificuldadeMenu, SelectPlayer, PlayerTurn, ComputerTurn, ShowResults, PlayerTurn2, ComputerTurnSemIA, FinalGame};
        private GameState currGameState = GameState.Null;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            largura = 800;
            altura = 600;
            graphics.PreferredBackBufferWidth = largura;
            graphics.PreferredBackBufferHeight = altura;

            this.Window.Title = "Jogo Da Velha - Super Mario World";
            this.Window.Position = new Point((1920/2 - 400), (1080/2 - 300));
            board = new Board();
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
            pena = Content.Load<Texture2D>("Sprites/pena_100_90");
            flor = Content.Load<Texture2D>("Sprites/flor_100_90");
            fundoJogo = Content.Load<Texture2D>("Sprites/fundo_jogo_800_600");
            final = Content.Load<Texture2D>("Sprites/final");
            borda = Content.Load<Texture2D>("Sprites/borda");
            transparente = Content.Load<Texture2D>("Sprites/transparente");
            transparente_borda = Content.Load<Texture2D>("Sprites/transparente_borda");
            sndMenu = Content.Load<SoundEffect>("Sounds/smw_menu");
            sndJogada = Content.Load<SoundEffect>("Sounds/click_no_jogo");
            
            btUmPlayer = new UIButton(new Vector2(251, 300), new Vector2(298, 29), transparente, "1 PLAYER GAME", fontMario);
            btDoisPlayers = new UIButton(new Vector2(251, 364), new Vector2(298, 29), transparente, "2 PLAYER GAME", fontMario);
            btComIA = new UIButton(new Vector2(320, 300), new Vector2(260, 29), transparente, "IMPOSSIVEL", fontMario);
            btSemIA = new UIButton(new Vector2(320, 364), new Vector2(130, 29), transparente, "MEDIO", fontMario);
            btUserJogaPrimeiro = new UIButton(new Vector2(251, 300), new Vector2(378, 29), transparente, "COMECAR JOGANDO", fontMario);
            btIAJogaPrimeiro = new UIButton(new Vector2(251, 364), new Vector2(410, 29), transparente,   "PC COMECA JOGANDO", fontMario);
            btReiniciarPartida = new UIButton(new Vector2(520, 565), new Vector2(80, 29), transparente,  "SIM", fontMario);
            btNaoReiniciarPartida = new UIButton(new Vector2(640, 565), new Vector2(80, 29), transparente, "NAO", fontMario);

            btSelect = new UIBotaoSelect(new Vector2(215, 306), new Vector2(20, 16), icone);
            btSelectIA = new UIBotaoSelect(new Vector2(280, 306), new Vector2(20, 16), icone);
            btSelectPlayer = new UIBotaoSelect(new Vector2(215, 306), new Vector2(20, 16), icone);

            playSound = sndMenu.CreateInstance();
            playSound.IsLooped = true;
            playSound.Play();
            musica = true;
            musicaJogada = true;

            jogadaSound = sndJogada.CreateInstance();
            

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
            if (Keyboard.GetState().IsKeyDown(Keys.M) && prevKeyState.IsKeyUp(Keys.M))
            {
                if (musica == true)
                {
                    playSound.Stop();
                    musica = false;
                }
                else
                {
                    playSound.Play();
                    musica = true;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.N) && prevKeyState.IsKeyUp(Keys.N))
            {
                if (musicaJogada == true)
                {
                    jogadaSound.Volume = 0.0f;
                    musicaJogada = false;
                }
                else
                {
                    jogadaSound.Volume = 1.0f;
                    musicaJogada = true;
                }
            }

            UpdateGameState(gameTime);
            prevMouseState = Mouse.GetState();
            prevKeyState = Keyboard.GetState();
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
                        board.Limpar();
                    }
                    break;

                case GameState.DificuldadeMenu:
                    {
                        playSound.Volume = 1.0f;
                    }
                    break;
                case GameState.PlayerTurn:
                    {
                        playSound.Volume = 0.5f;
                    }
                    break;

                case GameState.PlayerTurn2:
                    {
                        playSound.Volume = 0.5f;
                    }
                    break;

                case GameState.SelectPlayer:
                    {
                        playSound.Volume = 1.0f;
                    }
                    break; 
                case GameState.ComputerTurn:
                    {
                        playSound.Volume = 0.5f;
                        List<Board> possibilidades = board.GetPossibilidades(1);

                        int bestScore = -1;

                        List<Board> bestBoard = new List<Board>();

                        foreach (Board p in possibilidades)
                        {
                            int aux = board.Minimax(p, 9, 2);

                            System.Console.WriteLine("Aux: " + aux);

                            if (aux == bestScore)
                            {
                                bestBoard.Add(p);
                            }
                            else if (aux > bestScore)
                            {
                                bestScore = aux;
                                bestBoard.Clear();
                                bestBoard.Add(p);
                            }

                        }

                        int tam = bestBoard.Count;

                        Random rnd = new Random();
                        int val = rnd.Next(0, tam - 1);

                        

                        board = bestBoard[val];

                        if (board.EhFimDeJogo())
                        {
                            EnterGameState(GameState.ShowResults);
                        }
                        else
                        {
                            EnterGameState(GameState.PlayerTurn);
                        }
                    }
                    
                    break;

                case GameState.ComputerTurnSemIA:
                    {
                        playSound.Volume = 0.5f;
                        List<Board> possibilidades = board.GetPossibilidades(1);
                        int tam = possibilidades.Count;

                        Random rnd = new Random();
                        int val = rnd.Next(0, tam - 1);
                        
                        board = possibilidades[val];

                        if (board.EhFimDeJogo())
                        {
                            EnterGameState(GameState.ShowResults);
                        }
                        else
                        {
                            EnterGameState(GameState.PlayerTurn);
                        }
                    }
                    break;
                case GameState.ShowResults:
                    {

                        playSound.Volume = 1.0f;
                        stateTimer = 3;
                    }
                    break;
                case GameState.FinalGame:
                    {
                        if (board.GetVencedor() == 1)
                        {
                            if (umPlayer == true)
                            {
                                sndFimDojogo = Content.Load<SoundEffect>("Sounds/jogador_perdeu");
                            }
                            else
                            {
                                sndFimDojogo = Content.Load<SoundEffect>("Sounds/jogador_perdeu");
                            }
                        }
                        else if (board.GetVencedor() == 2)
                        {
                            sndFimDojogo = Content.Load<SoundEffect>("Sounds/jogador_ganhou");

                        }
                        else
                        {
                            
                            sndFimDojogo = Content.Load<SoundEffect>("Sounds/jogo_empatado");
                        }
                        
                        fimDojogoSound = sndFimDojogo.CreateInstance();
                        playSound.Stop();
                        fimDojogoSound.Play();
                        stateTimer = 8;
                    }
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
                case GameState.PlayerTurn2: { } break;
                case GameState.SelectPlayer: { } break;
                case GameState.ComputerTurn: { } break;
                case GameState.ComputerTurnSemIA: { } break;
                case GameState.ShowResults: { } break;
                case GameState.FinalGame: { } break;
            }
        }

        public void DrawGameState(GameTime gameTime)
        {
            switch (currGameState)
            {
                case GameState.MainMenu:
                    {
                        spriteBatch.Draw(bgMario, new Vector2(0, 0));

                        btUmPlayer.Draw(spriteBatch);

                        btDoisPlayers.Draw(spriteBatch);

                        btSelect.Draw(spriteBatch);
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

                case GameState.SelectPlayer: {
                        spriteBatch.Draw(bgMario, new Vector2(0, 0));

                        btSelectPlayer.Draw(spriteBatch);
                        btUserJogaPrimeiro.Draw(spriteBatch);
                        btIAJogaPrimeiro.Draw(spriteBatch);
                    }
                    break;

                case GameState.PlayerTurn:
                    {
                        DrawBoard();
                    }
                    break;

                case GameState.PlayerTurn2:
                    {
                        DrawBoard();
                    }
                    break;

                case GameState.ComputerTurn:
                    {
                        DrawBoard();
                    }
                    break;

                case GameState.ComputerTurnSemIA:
                    {
                        DrawBoard();
                    }
                    break;

                case GameState.ShowResults:
                    {
                        DrawBoard();
                        string titulo = "FIM DO JOGO - ";

                        if (board.GetVencedor() == 1)
                        {
                            if (umPlayer == true)
                            {
                                titulo = titulo + "COMPUTADOR VENCEU";
                            }
                            else
                            {
                                titulo = titulo + "JOGADOR 2 VENCEU";
                            }
                        }
                        else if (board.GetVencedor() == 2)
                        {
                            titulo = titulo + "JOGADOR 1 VENCEU";

                        }
                        else
                        {
                            titulo = titulo + "JOGO EMPATADO";
                        }
                        DrawString(titulo, new Vector2(largura / 2, 30), Color.Yellow);

                        
                        
                        DrawString2("REINICIAR PARTIDA?", new Vector2(40, 568), Color.Yellow);
                        btReiniciarPartida.Draw(spriteBatch);
                        btNaoReiniciarPartida.Draw(spriteBatch);
                    }
                    break;

                case GameState.FinalGame:
                    {
                        spriteBatch.Draw(final, new Vector2(0, 0));

                        string titulo = "FIM DO JOGO - ";

                        if (board.GetVencedor() == 1)
                        {
                            if (umPlayer == true)
                            {
                                titulo = titulo + "COMPUTADOR VENCEU";
                            }
                            else
                            {
                                titulo = titulo + "JOGADOR 2 VENCEU";
                            }
                        }
                        else if (board.GetVencedor() == 2)
                        {
                            titulo = titulo + "JOGADOR 1 VENCEU";

                        }
                        else
                        {
                            titulo = titulo + "JOGO EMPATADO";
                        }
                        DrawString(titulo, new Vector2(largura / 2, 30), Color.Yellow);
                        
                    }
                    break;
            }
        }

        public void UpdateGameState(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            switch (currGameState)
            {
                case GameState.MainMenu: {
                        
                        if (Keyboard.GetState().IsKeyDown(Keys.Down) && prevKeyState.IsKeyUp(Keys.Down))
                        {
                            btSelect.SetPosition(215, 370);
                            btSelect.SetPrimeiro(false);
                        }
                        else if (Keyboard.GetState().IsKeyDown(Keys.Up) && prevKeyState.IsKeyUp(Keys.Up))
                        {
                            btSelect.SetPosition(215, 306);
                            btSelect.SetPrimeiro(true);
                        }

                        else if (Keyboard.GetState().IsKeyDown(Keys.Enter) && btSelect.GetPrimeiro() == true && prevKeyState.IsKeyUp(Keys.Enter)) // 1 player
                        {
                            umPlayer = true;
                            EnterGameState(GameState.DificuldadeMenu);
                        }

                        else if (Keyboard.GetState().IsKeyDown(Keys.Enter) && btSelect.GetPrimeiro() == false && prevKeyState.IsKeyUp(Keys.Enter)) // 2 player
                        {
                            //Vai para a tela de jogo de 2 players
                            umPlayer = false;
                            EnterGameState(GameState.PlayerTurn);
                        }


                        if (Mouse.GetState().LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                        {
                            Vector2 pTeste = new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y);

                            if (btUmPlayer.TesteClick(pTeste))
                            {
                                umPlayer = true;
                                EnterGameState(GameState.DificuldadeMenu);
                            }
                            else if (btDoisPlayers.TesteClick(pTeste)) // Selecionou o segundo
                            {
                                umPlayer = false;
                                EnterGameState(GameState.PlayerTurn);
                            }
                        }

                        
                            
                        break;
                        
                    }

                case GameState.DificuldadeMenu:
                    {
                        
                        if (Keyboard.GetState().IsKeyDown(Keys.B) || Keyboard.GetState().IsKeyDown(Keys.Back))
                        {
                            EnterGameState(GameState.MainMenu);
                        }


                        if (Keyboard.GetState().IsKeyDown(Keys.Down) && prevKeyState.IsKeyUp(Keys.Down)) // Down
                        {
                            btSelectIA.SetPosition(280, 370);
                            btSelectIA.SetPrimeiro(false);
                        }
                        else if (Keyboard.GetState().IsKeyDown(Keys.Up) && prevKeyState.IsKeyUp(Keys.Up))// UP
                        {
                            btSelectIA.SetPosition(280, 306);
                            btSelectIA.SetPrimeiro(true);
                        }

                        else if (Keyboard.GetState().IsKeyDown(Keys.Enter) && btSelectIA.GetPrimeiro() == true && prevKeyState.IsKeyUp(Keys.Enter)) //Enter com IA
                        {
                            comIA = true;
                            EnterGameState(GameState.SelectPlayer);
                            
                        }

                        else if (Keyboard.GetState().IsKeyDown(Keys.Enter) && btSelectIA.GetPrimeiro() == false && prevKeyState.IsKeyUp(Keys.Enter)) // Enter sem IA
                        {
                            comIA = false;
                            EnterGameState(GameState.SelectPlayer);
                            
                        }

                        if (Mouse.GetState().LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                        {
                            Vector2 pTeste = new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y);


                            Vector2 pos = btSelectIA.GetPosition();

                            if (btComIA.TesteClick(pTeste))
                            {
                                comIA = true;
                                EnterGameState(GameState.SelectPlayer);
                                
                            }
                            else if (btSemIA.TesteClick(pTeste)) // Selecionou o segundo
                            {
                                comIA = false;
                                EnterGameState(GameState.SelectPlayer);
                                
                            }
                        }
                        break;
                    }

                case GameState.SelectPlayer:
                    {
                        
                        if ((Keyboard.GetState().IsKeyDown(Keys.B) && prevKeyState.IsKeyUp(Keys.B)) || (Keyboard.GetState().IsKeyDown(Keys.Back) && prevKeyState.IsKeyUp(Keys.Back)))
                        {
                            EnterGameState(GameState.MainMenu);
                        }


                        if (Keyboard.GetState().IsKeyDown(Keys.Down) && prevKeyState.IsKeyUp(Keys.Down)) // Down
                        {
                            btSelectPlayer.SetPosition(215, 370);
                            btSelectPlayer.SetPrimeiro(false);
                        }
                        else if (Keyboard.GetState().IsKeyDown(Keys.Up) && prevKeyState.IsKeyUp(Keys.Up))// UP
                        {
                            btSelectPlayer.SetPosition(215, 306);
                            btSelectPlayer.SetPrimeiro(true);
                        }

                        else if (Keyboard.GetState().IsKeyDown(Keys.Enter) && btSelectPlayer.GetPrimeiro() == true && prevKeyState.IsKeyUp(Keys.Enter)) //Enter com Jogador Começa
                        {
                            jogaPrimeiro = true;
                            EnterGameState(GameState.PlayerTurn);

                        }

                        else if (Keyboard.GetState().IsKeyDown(Keys.Enter) && btSelectPlayer.GetPrimeiro() == false && prevKeyState.IsKeyUp(Keys.Enter)) // Enter com PC Começa
                        {
                            jogaPrimeiro = false;
                            if(comIA == true)
                            {
                                EnterGameState(GameState.ComputerTurn);
                            }
                            else
                            {
                                EnterGameState(GameState.ComputerTurnSemIA);
                            }
                            
                        }

                        if (Mouse.GetState().LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                        {
                            Vector2 pTeste = new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y);


                            Vector2 pos = btSelect.GetPosition();

                            if (btUserJogaPrimeiro.TesteClick(pTeste))
                            {
                                jogaPrimeiro = true;
                                EnterGameState(GameState.PlayerTurn);
                            }
                            else if (btIAJogaPrimeiro.TesteClick(pTeste)) // Selecionou o segundo
                            {
                                jogaPrimeiro = false;
                                if (comIA == true)
                                {
                                    EnterGameState(GameState.ComputerTurn);
                                }
                                else
                                {
                                    EnterGameState(GameState.ComputerTurnSemIA);
                                }
                            }
                        }

                        break;
                    }


                case GameState.PlayerTurn:
                    {
                        
                        if (Mouse.GetState().LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                        {
                            Vector2 pTeste = new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y);

                            for (int y = 0; y < board.GetAlturaArray(); y++)
                            {
                                for (int x = 0; x < board.GetAlturaArray(); x++)
                                {
                                    Vector2 pMin = new Vector2(x * 100 + (largura - 100 * board.GetAlturaArray()) / 2, y * 90 + (altura - 90 * board.GetComprimentoArray()) / 2);
                                    Vector2 pMax = pMin + new Vector2(100, 90);

                                    if (((pTeste.X > pMin.X) && (pTeste.X < pMax.X)) && ((pTeste.Y > pMin.Y) && (pTeste.Y < pMax.Y)) && board.RetornaValorCelula(x, y) == 0)
                                    {
                                        board.AtribuiValorCelula(x, y, 2);
                                        jogadaSound.Play();
                                        if (board.EhFimDeJogo())
                                        {
                                            EnterGameState(GameState.ShowResults);
                                        }
                                        else
                                        {
                                            if(umPlayer == true)
                                            {
                                                if (comIA == true)
                                                {
                                                    EnterGameState(GameState.ComputerTurn);
                                                }
                                                else
                                                {
                                                    Random rnd = new Random();
                                                    int val = rnd.Next(2,4);

                                                    if (val % 2 == 0)
                                                    {
                                                        EnterGameState(GameState.ComputerTurn);
                                                    }
                                                    else
                                                    {
                                                        EnterGameState(GameState.ComputerTurnSemIA);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                EnterGameState(GameState.PlayerTurn2);
                                            }
                                            
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;

                case GameState.PlayerTurn2:
                    {
                        
                        if (Mouse.GetState().LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                        {
                            Vector2 pTeste = new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y);

                            for (int y = 0; y < board.GetAlturaArray(); y++)
                            {
                                for (int x = 0; x < board.GetAlturaArray(); x++)
                                {
                                    Vector2 pMin = new Vector2(x * 100 + (largura - 100 * board.GetAlturaArray()) / 2, y * 90 + (altura - 90 * board.GetComprimentoArray()) / 2);
                                    Vector2 pMax = pMin + new Vector2(100, 90);

                                    if (((pTeste.X > pMin.X) && (pTeste.X < pMax.X)) && ((pTeste.Y > pMin.Y) && (pTeste.Y < pMax.Y)) && board.RetornaValorCelula(x, y) == 0)
                                    {
                                        board.AtribuiValorCelula(x, y, 1);
                                        jogadaSound.Play();
                                        if (board.EhFimDeJogo())
                                        {
                                            EnterGameState(GameState.ShowResults);
                                        }
                                        else
                                        {
                                            EnterGameState(GameState.PlayerTurn);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;

                case GameState.ComputerTurn: { } break;
                case GameState.ComputerTurnSemIA: { } break;
                case GameState.ShowResults:
                    {
                        if (Mouse.GetState().LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                        {
                            Vector2 pTeste = new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y);

                            if (btReiniciarPartida.TesteClick(pTeste))
                            {
                                if(umPlayer == true)
                                {
                                    if(jogaPrimeiro == true)
                                    {
                                        board.Limpar();
                                        EnterGameState(GameState.PlayerTurn);
                                    }
                                    else
                                    {
                                        if (comIA == true)
                                        {
                                            board.Limpar();
                                            EnterGameState(GameState.ComputerTurn);
                                        }
                                        else
                                        {
                                            board.Limpar();
                                            EnterGameState(GameState.ComputerTurnSemIA);
                                        }
                                    }
                                    
                                }
                                else
                                {
                                    if (jogaPrimeiro == true)
                                    {
                                        board.Limpar();
                                        EnterGameState(GameState.PlayerTurn2);
                                    }
                                    else
                                    {
                                        board.Limpar();
                                        EnterGameState(GameState.PlayerTurn);
                                    }
                                }
                                
                            }
                            else if (btNaoReiniciarPartida.TesteClick(pTeste)) // Selecionou o segundo
                            {
                                EnterGameState(GameState.FinalGame);
                            }
                        }

                        stateTimer -= dt;
                        if (stateTimer <= 0)
                        {
                            EnterGameState(GameState.FinalGame);
                        }
                    }
                    break;

                case GameState.FinalGame:
                    {
                        stateTimer -= dt;
                        if (stateTimer <= 0)
                        {
                            fimDojogoSound.Stop();
                            EnterGameState(GameState.MainMenu);

                        }
                    }
                    break;
            }
        }

        public void DrawBoard()
        {
            spriteBatch.Draw(fundoJogo, new Vector2(0,0));

            for (int x = 0; x < board.GetAlturaArray(); x++)
            {
                for (int y = 0; y < board.GetComprimentoArray(); y++)
                {
                    cellPos = new Vector2(x * 100 + (largura - 100 * board.GetAlturaArray()) / 2, y * 100 + (altura - 100 * board.GetComprimentoArray()) / 2);

                    if (board.RetornaValorCelula(x, y) == 0)
                    {
                        spriteBatch.Draw(cube, cellPos);

                    }
                    //if(board.cell[x,y] == 1){
                    if (board.RetornaValorCelula(x, y) == 1)
                    {
                        spriteBatch.Draw(flor, cellPos);


                    };
                    //if (board.cell[x, y] == 2){
                    if (board.RetornaValorCelula(x, y) == 2)
                    {
                        spriteBatch.Draw(pena, cellPos);
                    }
                }
            }
        }

        private void DrawString(string text, Vector2 pos, Color color)
        {
            Vector2 textSize = fontMario.MeasureString(text);

            spriteBatch.DrawString(
                fontMario,
                text,
                pos,                //position 
                color,              //color
                0.0f,               //rotation
                textSize / 2.0f,    //origin (pivot)
                Vector2.One,        //scale
                SpriteEffects.None,
                0.0f
            );


        }
        private void DrawString2(string text, Vector2 pos, Color color)
        {
            Vector2 textSize = fontMario.MeasureString(text);

            spriteBatch.DrawString(
                fontMario,
                text,
                pos,                //position 
                color,              //color
                0.0f,               //rotation
                new Vector2(0,0),               //origin (pivot)
                Vector2.One,        //scale
                SpriteEffects.None,
                0.0f
            );


        }

    }
}
