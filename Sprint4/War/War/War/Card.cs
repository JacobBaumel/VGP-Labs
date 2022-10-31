using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace War {
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

        

        public void draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, rect, Color.White);
        }

        public void draw(SpriteBatch spriteBatch, Rectangle r) {
            spriteBatch.Draw(texture, r, Color.White);
        }

        public enum Suits {
            HEART,
            SPADE,
            DIAMOND,
            CLUB,
            NONE
        }

        public static int cardFileNameToValue(string card) {
            return Convert.ToInt32(card.Substring(1));
        }

        public static Suits getSuitFromLetter(string letter) {
            switch(letter.Substring(0, 1)) {
                case "c":
                    return Suits.CLUB;

                case "s":
                    return Suits.SPADE;

                case "h":
                    return Suits.HEART;

                case "d":
                    return Suits.DIAMOND;

                default:
                    return Suits.NONE;
            }
        }
    }
}
