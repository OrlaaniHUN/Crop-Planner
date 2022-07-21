using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley.Menus;
using StardewValley;

namespace Crop_Planner
{
    
    public class ModEntry : Mod
    {
        
        public override void Entry(IModHelper helper)
        {
            helper.Events.Input.ButtonPressed += this.OnButtonPressed;
            helper.Events.Display.RenderedWorld += this.RenderedWorld;
        }
        List<Vector2> GreenTiles = new List<Vector2>();
        List<Vector2> RedTiles = new List<Vector2>();
        bool SelectionState = false;
        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            
            if (!Context.IsWorldReady)
                return;
            
            //this.Monitor.Log($"{Game1.player.Name} pressed {e.Button}.", LogLevel.Debug);


            if (e.Button == SButton.U)
            {
                string message = "Select tiles: Numpad 2 & Numpad 3^or mouse side buttons^Delete all selected tiles: numpad 0";
                Game1.activeClickableMenu = new DialogueBox(message);
            }

            if (e.Button == SButton.NumPad3 || e.Button == SButton.MouseX2)
            {

                if (!RedTiles.Contains(e.Cursor.Tile))
                {
                    if (GreenTiles.Contains(e.Cursor.Tile))
                    {
                        GreenTiles.Remove(e.Cursor.Tile);
                    }
                    RedTiles.Add(e.Cursor.Tile);
                }else
                {
                    RedTiles.Remove(e.Cursor.Tile);
                }
                this.Monitor.Log($"Added tile to Red: {e.Cursor.Tile}. Total tiles:", LogLevel.Trace);
                Game1.hudMessages.Clear();
                Game1.addHUDMessage(new HUDMessage(message: $"Selected Tiles(Red): {RedTiles.Count}", leaveMeNull:null) {timeLeft = 900000});
                Game1.addHUDMessage(new HUDMessage(message: $"Selected Tiles(Green): {GreenTiles.Count}", leaveMeNull:null) {timeLeft = 900000});

            }

            if (e.Button == SButton.NumPad2 || e.Button == SButton.MouseX1)
            {

                if (!GreenTiles.Contains(e.Cursor.Tile))
                {
                    if (RedTiles.Contains(e.Cursor.Tile))
                    {
                        RedTiles.Remove(e.Cursor.Tile);
                    }
                    GreenTiles.Add(e.Cursor.Tile);
                } else
                {
                    GreenTiles.Remove(e.Cursor.Tile);
                }
                
                this.Monitor.Log($"Added tile to Green: {e.Cursor.Tile}. Total tiles:", LogLevel.Trace);
                Game1.hudMessages.Clear();
                Game1.addHUDMessage(new HUDMessage(message: $"Selected Tiles(Red): {RedTiles.Count}", leaveMeNull:null) {timeLeft = 900000});
                Game1.addHUDMessage(new HUDMessage(message: $"Selected Tiles(Green): {GreenTiles.Count}", leaveMeNull:null) {timeLeft = 900000});

            }

            if (e.Button == SButton.NumPad0)
            {
                RedTiles.Clear();
                GreenTiles.Clear();
                Game1.hudMessages.Clear();
            }
            

        }

        private void RenderedWorld(object sender, RenderedWorldEventArgs e) {
            SpriteBatch b = e.SpriteBatch;
            
            foreach (Vector2 tile in RedTiles)
            {
                var pos = Game1.GlobalToLocal(tile * Game1.tileSize);
                b.Draw(Game1.staminaRect, new Rectangle((int)pos.X, (int)pos.Y, 64, 64), null, Color.Red);
            }
            foreach (Vector2 tile in GreenTiles)
            {
                var pos = Game1.GlobalToLocal(tile * Game1.tileSize);
                b.Draw(Game1.staminaRect, new Rectangle((int)pos.X, (int)pos.Y, 64, 64), null, Color.Green);
            }
        }

        
    }
}
