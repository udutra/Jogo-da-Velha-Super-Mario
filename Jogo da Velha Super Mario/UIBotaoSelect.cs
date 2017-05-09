using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Jogo_da_Velha_Super_Mario
{
    class UIBotaoSelect
    {

        private Vector2 position, size;
        private Texture2D background;
        private bool primeiro;

        public UIBotaoSelect(Vector2 position, Vector2 size, Texture2D background)
        {
            this.position = position;
            this.size = size;
            this.background = background;
            primeiro = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, position, null, Color.White, 0.0f, Vector2.Zero, new Vector2(size.X / background.Width, size.Y / background.Height), SpriteEffects.None, 0.0f);
        }

        public bool TesteClick(Vector2 pTeste)
        {
            if (((pTeste.X > position.X) && (pTeste.X < position.X + size.X)) && ((pTeste.Y > position.Y) && (pTeste.Y < position.Y + size.Y)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SetPosition(int x, int y)
        {
            position = new Vector2(x, y);
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public void SetPrimeiro(bool prim)
        {
            primeiro = prim;
        }

        public bool GetPrimeiro()
        {
            return primeiro;
        }

        public int TesteSeta()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                return 0;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                return 1;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Enter) && primeiro == true){
                return 2;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Enter) && primeiro == false)
            {
                return 3;
            }
            else
            {
                return -1;
            }
        }
    }
}