using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPI311.GameEngine
{
    public class ModelRenderer : Renderer
    {
        public Model Model { get; set; }

        public ModelRenderer(GraphicsDevice graphics, Model model)
            : base(graphics)
        {
            Model = model;
        }

        public override void Draw()
        {
            if (Model == null)
                return;
            if (Material == null)
                Model.Draw(Transform.World, Camera.Current.View, Camera.Current.Projection);
            else
            {
                foreach (ModelMesh mesh in Model.Meshes)
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        GraphicsDevice.SetVertexBuffer(part.VertexBuffer);
                        GraphicsDevice.Indices = part.IndexBuffer;
                        GraphicsDevice.DrawIndexedPrimitives(
                            PrimitiveType.TriangleList, part.VertexOffset, 0,
                            part.NumVertices, part.StartIndex, part.PrimitiveCount);
                    }
            }
        }
    }
}
