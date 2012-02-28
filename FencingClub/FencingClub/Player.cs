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

   public enum SpriteID { Attack, Defense, Wait }

    public class Player
    {
        
        float speed;
        SpriteID stance;
        int count;
        // Position 
        Vector2 position;
        int direction;

        // Textures 
        Texture2D texture;
        Rectangle currentTexture;
        public Rectangle boundings;
        private Dictionary<SpriteID, Color[]> colorArrayMap;
        
        // Input
        KeyboardState keyboard;
        Keys key;
        
        public Player(Vector2 position, Keys key, int direction)
        {
            this.position = position;
            this.key = key;
            this.direction = direction;
            speed = 0.5f;
            stance = SpriteID.Wait;
            currentTexture = new Rectangle(0, 0, 70, 114);
            boundings = new Rectangle((int) position.X, (int) position.Y, currentTexture.Width, currentTexture.Height);
            colorArrayMap = new Dictionary<SpriteID, Color[]>();
            
        }


        public void LoadContent(ContentManager cm, String fileName)
        {            
            texture = cm.Load<Texture2D>(fileName);
            
            Color[] textureData = new Color[currentTexture.Width * currentTexture.Height];
            
            texture.GetData(0, currentTexture, textureData, currentTexture.X* (int)SpriteID.Attack,textureData.Length);
            colorArrayMap.Add(SpriteID.Attack, textureData);
            
            texture.GetData(0, currentTexture, textureData, currentTexture.X * (int)SpriteID.Defense, textureData.Length);
            colorArrayMap.Add(SpriteID.Defense, textureData);
            
            texture.GetData(0, currentTexture, textureData, currentTexture.X * (int)SpriteID.Wait, textureData.Length);
            colorArrayMap.Add(SpriteID.Wait, textureData);
        }

        public void Update(GameWindow window)
        {
            keyboard = Keyboard.GetState();

            switch (stance)
            {
                case (SpriteID.Attack):
                    AttackStance(); break;
                case (SpriteID.Defense):
                    DefenseStance(); break;
                case (SpriteID.Wait):
                    WaitStance(); break;
            }

            position.X = MathHelper.Clamp(position.X, 0, window.ClientBounds.Width - currentTexture.Width);

            boundings.X = (int) position.X;
            
        }

        
        private void AttackStance()
        {
            position.X += direction * 20;
            stance = SpriteID.Wait;
        }

        private void DefenseStance()
        {
            if (keyboard.IsKeyDown(key))
                position.X -= (direction * speed);
            else
                stance = SpriteID.Wait;
        }

        private void WaitStance()
        {
            if (keyboard.IsKeyDown(key))
            {
                count++;
                if (count > 5)
                {
                    stance = SpriteID.Defense;
                    count = 0;
                }
            }
            else if (count > 1)
            {
                stance = SpriteID.Attack;
                count = 0;
            }
            else
            {
                position.X += (direction * speed);
            }

        }


        public void updatePosition(float newX)
        {
            position.X += direction * newX;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            currentTexture.X = (int)stance * currentTexture.Width;
            spriteBatch.Draw(texture, position, currentTexture, Color.White);     
        }


        public Rectangle GetBoundings()
        {
            return boundings;
        }

        public Color[] GetCurrentSpriteData()
        {
            Color[] colorArray;
            colorArrayMap.TryGetValue(stance,out colorArray);
            return colorArray;
            
        }
        
        public SpriteID GetCurrentStance()
        {
            return stance;
        }
    }
}
