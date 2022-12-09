using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IIWGTI {

    enum Subjects {
        CompSci,
        English,
        Math,
        PE
    }

    class Subject {
        Rectangle source;
        Texture2D texture;
        SpriteFont font;
        public int value;
        Subjects s;

        public Subject(Rectangle _source, Texture2D _texture, int _value, Subjects _s, SpriteFont _font) {
            value = _value;
            source = _source;
            texture = _texture;
            s = _s;
            font = _font;
        }

        public void draw(SpriteBatch spriteBatch, ref Rectangle pos) {
            pos.Width = (source.Width / 4) * 2;
            pos.Height = (source.Height / 4) * 2;
            spriteBatch.Draw(texture, pos, source, Color.White);
            spriteBatch.DrawString(font, stringFromSubject(s) + value, new Vector2(pos.Left, pos.Bottom + 10), Color.White);
            pos.X += pos.Width + 60;
        }


        public static Subjects subjectFromString(string s) {
            switch(s) {
                case "Computer Science":
                    return Subjects.CompSci;

                case "English":
                    return Subjects.English;

                case "Math":
                    return Subjects.Math;

                case "PE":
                    return Subjects.PE;

                default:
                    throw new System.Exception();
            }
        }

        public static string stringFromSubject(Subjects s) {
            switch(s) {
                case Subjects.CompSci:
                    return "Computer\nScience ";

                case Subjects.English:
                    return "English ";

                case Subjects.Math:
                    return "Math ";

                case Subjects.PE:
                    return "PE ";

                default:
                    throw new System.Exception();
            }
        }

    }
}
