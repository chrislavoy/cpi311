using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace CPI311.GameEngine
{
    public class GUIGroup : GUIElement
    {
        public List<GUIElement> Children { get; set; }
        public int SelectedIndex { get; set; }

        public GUIGroup()
        {
            Children = new List<GUIElement>();
            SelectedIndex = 0;
        }

        public override void Update()
        {
            if (Children.Count > 0)
            {
                int newIndex = SelectedIndex;
                if (InputManager.IsKeyPressed(Keys.Up))
                    newIndex = (SelectedIndex + Children.Count - 1) % Children.Count;
                if (InputManager.IsKeyPressed(Keys.Down))
                    newIndex = (SelectedIndex + 1) % Children.Count;
                if (newIndex != SelectedIndex)
                {
                    Children[SelectedIndex].Selected = false;
                    Children[SelectedIndex = newIndex].Selected = true;
                }
            }
            foreach (GUIElement child in Children)
                child.Update();
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            foreach(GUIElement child in Children)
                child.Draw(spriteBatch, font);
        }

    }
}
