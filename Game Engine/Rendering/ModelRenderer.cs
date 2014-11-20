using Microsoft.Xna.Framework.Graphics;

namespace CPI311.GameEngine
{
    public class ModelRenderer : Renderer
    {
        public Model Model { get; set; }

        public ModelRenderer(Model model)
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
                Material.Apply(Transform.World);
                foreach (ModelMesh mesh in Model.Meshes)
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        ScreenManager.GraphicsDevice.SetVertexBuffer(part.VertexBuffer);
                        ScreenManager.GraphicsDevice.Indices = part.IndexBuffer;
                        ScreenManager.GraphicsDevice.DrawIndexedPrimitives(
                            PrimitiveType.TriangleList, part.VertexOffset, 0,
                            part.NumVertices, part.StartIndex, part.PrimitiveCount);
                    }
            }
        }
    }
}
