using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace KarbowskiAstro
{
    public class Rakieta
    {
        private Texture2D texture;
        private Vector2 position;
        private Texture2D pocisk2D;
        private int nrKlatki;
        private int szerokośćKlatki;
        private Pocisk strzał;
        public Rakieta(Texture2D texture, Texture2D pocisk2D)
        {
            this.texture = texture;
            this.pocisk2D = pocisk2D;
            position = new Vector2(210, 480);
            nrKlatki = 0;
            strzał.wystrzelony = false;
        }
        public Vector2 GetPocisk()
        {
            return strzał.position;
        }
        public void Wystrzel()
        {
            strzał.wystrzelony = true;
            strzał.position = position;
        }
        public void deletePocisk()
        {
            strzał.wystrzelony = false;
            strzał.position = new Vector2(1000, 1000);
        }
        private struct Pocisk
        {
            public Vector2 position;
            public bool wystrzelony;

        }
        public void LotPocisku()
        {
            if (strzał.wystrzelony)
            {
                strzał.position.Y -= 10;
                if (strzał.position.Y < 0)
                    strzał.wystrzelony = false;

            }
        }
        public Vector2 GetPosition()
        {
            return position;
        }
        public Vector2 GetSize()
        {
            return new Vector2(texture.Width / 6, texture.Height);
        }
        public void MoveL() 
        {
            if (position.X <= 400)
            {
                position.X -= 5;
                if (position.X < 0) 
                    position.X = 0;
            }
        }
        public void MoveR() 
        {
            if (position.X >= 0)
            {
                position.X += 5;
                if (position.X > 400) 
                    position.X = 400;
            }
        }
        public void MoveU() 
        {
            if (position.Y >= 0)
            {
                position.Y -= 5;
                if (position.Y < 0) 
                    position.Y = 0;
            }
        }
        public void MoveD() 
        {
            if (position.Y <= 477)
            {
                position.Y += 5;
                if (position.Y > 477) 
                    position.Y = 477;
            }
        }
        public void Draw(Texture2D rakieta, SpriteBatch spriteBatch) 
        {
            szerokośćKlatki = texture.Width / 6;
            Rectangle klatka = new Rectangle(nrKlatki * szerokośćKlatki, 0, szerokośćKlatki, texture.Height);
            Rectangle rectGracza = new Rectangle((int)position.X, (int)position.Y, klatka.Width, klatka.Height);
            spriteBatch.Draw(texture, rectGracza, klatka, Color.White);
            if (strzał.wystrzelony)
            {
                spriteBatch.Draw(pocisk2D, strzał.position, Color.White);
            }
            nrKlatki++;
            if (nrKlatki == 6)
                nrKlatki = 0;
        }
    }
}
