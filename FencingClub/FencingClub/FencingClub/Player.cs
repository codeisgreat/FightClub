using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;

namespace WindowsGame1
{

    enum SpriteID { Attack, Defense, Wait }

    public class Player
    {
        //Testar ännu lite skit
        //GIT COMMENT SKIT FÖR ATT TESTA OSV!
        float speed;
        int stance;
        int count;
        // Position 
        Vector2 position;
        int direction;

        // Textures 
        Texture2D texture;
        Rectangle currentTexture;
        Rectangle boundings;
        
        // Input
        KeyboardState keyboard;
        Keys key;
        
        public Player(Vector2 position, Keys key, int direction)
        {
            this.position = position;
            this.key = key;
            this.direction = direction;
            speed = 0.5f;
            stance = (int) SpriteID.Wait;
            currentTexture = new Rectangle(0, 0, 70, 114);
            boundings = new Rectangle((int) position.X, (int) position.Y, currentTexture.Width, currentTexture.Height);
            
        }


        public void LoadContent(ContentManager cm, String fileName)
        {            
            texture = cm.Load<Texture2D>(fileName);
        }

        public void Update(GameWindow window)
        {
            keyboard = Keyboard.GetState();

            switch (stance)
            {
                case ((int)SpriteID.Attack):
                    Attacking(); break;
                case ((int)SpriteID.Defense):
                    Defending(); break;
                case ((int)SpriteID.Wait):
                    Waiting(); break;
            }

            position.X = MathHelper.Clamp(position.X, 0, window.ClientBounds.Width - currentTexture.Width);

            boundings.X = (int) position.X;
            
        }

        private void Attacking()
        {
            position.X += 20;
            stance = (int)SpriteID.Wait;
        }

        private void Defending()
        {
            if (keyboard.IsKeyDown(key))
                position.X -= (direction * speed);
            else
                stance = (int)SpriteID.Wait;
        }

        private void Waiting()
        {
            if (keyboard.IsKeyDown(key))
            {
                count++;
                if (count > 5)
                {
                    stance = (int)SpriteID.Defense;
                    count = 0;
                }
            }
            else if (count > 1)
            {
                stance = (int) SpriteID.Attack;
                count = 0;
            }
            else
            {
                position.X += (direction * speed);
            }

        }


        private void updatePosition(float difference)
        {
            position.X += difference;

        
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            currentTexture.X = stance * currentTexture.Width;
            spriteBatch.Draw(texture, position, currentTexture, Color.White);
            
        }


        public Boolean IsDefending()
        {
            return true;
        }
        
    }
}
