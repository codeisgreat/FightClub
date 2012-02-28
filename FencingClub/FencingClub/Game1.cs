using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    ///
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player player1;
        Player player2;

        Color[] player1TextureData;
        Color[] player2TextureData;
        //Nu har vi två spelare, varför kan vi inte ha tre?
        //lite skit här
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            player1 = new Player(new Vector2(Window.ClientBounds.Width/4, Window.ClientBounds.Height - 114), Keys.D, 1);
            player2 = new Player(new Vector2(Window.ClientBounds.Width * 3 / 4, Window.ClientBounds.Height - 114), Keys.S, -1);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            player1.LoadContent(this.Content, "test");
            player2.LoadContent(this.Content, "test");

            player1TextureData = new Color[player1.GetBoundings().Width * player1.GetBoundings().Height];
           // player1TextureData = player1.GetTextureData();
            player2TextureData = new Color[player2.GetBoundings().Width * player2.GetBoundings().Height];
           // player2TextureData = player2.GetTextureData();


            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            player1.Update(Window);
            player2.Update(Window);
            CheckForCollision();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            spriteBatch.Begin();
            player1.Draw(spriteBatch);
            player2.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
            
        }

        private void CheckForCollision()
        {
            Rectangle box1 = player1.boundings;
            Rectangle box2 = player2.boundings;
            // Om boxarna intersectar så blir det KAOS
            if (box1.Intersects(box2))
            {
                 player1TextureData = player1.GetCurrentSpriteData();
                 player2TextureData = player2.GetCurrentSpriteData();
                //player1.GetTextureData(player1TextureData);
               // player2.GetTextureData(player2TextureData);
                
                // Create the intersecting rectangle.
                int top = Math.Max(box1.Top, box2.Top);
                int bottom = Math.Min(box1.Bottom, box2.Bottom);
                int left = Math.Max(box1.Left, box2.Left);
                int right = Math.Min(box1.Right, box2.Right);
                //Loop through the intersecting rectangle and check each player's intersecting pixel.
                //If there is a pixel where both players individual pixels are not entierly transparent, then a collision has occured.
                for (int y = top; y < bottom; y++)
                {
                    for (int x = left; x < right; x++)
                    {
                        // Retrieve individual pixel color data from each player.
                        Color p1Color = player1TextureData[(x - player1.boundings.Left) + (y - player1.boundings.Top) * player1.boundings.Width];
                        Color p2Color = player2TextureData[(x - player2.boundings.Left) + (y - player2.boundings.Top) * player2.boundings.Width];
                        
                        // Check if thouse pixels are transparent or not. Iff both pixels are not entierly transparent, a collision has occured.
                        if (p1Color.A != 0 && p2Color.A != 0)
                        { 
                            //Hit occured!
                                  SpriteID p1Stance = player1.GetCurrentStance();
                                  SpriteID p2Stance = player2.GetCurrentStance();

                                    if (p1Stance == SpriteID.Wait && p2Stance == SpriteID.Wait)
                                    {
                                        player1.updatePosition(left - right);
                                        player2.updatePosition(left - right);
                                    }
                                    if (p1Stance == SpriteID.Attack && p2Stance == SpriteID.Wait)
                                    {
                                        GraphicsDevice.Clear(Color.Red);
                                        player1.updatePosition(left - right);
                                        player2.updatePosition(left - right);
                                    }
                                  /*  if (p1Stance == SpriteID.Wait && p2Stance == SpriteID.Wait)
                                    {
                                        player1.updatePosition(left - right);
                                        player2.updatePosition(left - right);
                                    }*/
                                    return;
                        }
                    }

                }

      
            }      
        }
        /*
        private void DetermineCollision()
        {
            SpriteID p1Stance = player1.GetCurrentStance();
            SpriteID p2Stance = player2.GetCurrentStance();

            if (p1Stance == SpriteID.Wait && p2Stance == SpriteID.Wait)
            { 
                player1.updatePosition
            }
        }

        */
    }
}
