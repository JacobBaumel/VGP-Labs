using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace Cards {
    class Card {
        public Texture2D texture;
        public int value;
        public Suits suit;
        public Rectangle rect;

        public Card(Texture2D _texture, int _value, Suits _suit, int startX, int startY) {
            texture = _texture;
            value = _value;
            suit = _suit;
            rect = new Rectangle(startX, startY, texture.Width, texture.Height);
        }

        public enum Suits {
            HEART,
            SPADE,
            DIAMOND,
            CLUB
        }

        public void draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, rect, Color.White);
        }
    }
}
