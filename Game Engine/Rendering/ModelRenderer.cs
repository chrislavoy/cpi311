using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CPI311.GameEngine
{
    public class ModelRenderer : Renderer
    {
        private Matrix[] BoneTransforms { get; set; }
        private Model model;
        public Model Model
        {
            get { return model; }
            set{
              if((model = value) != null)
              {
                BoneTransforms = new Matrix[Model.Bones.Count];
                model.CopyAbsoluteBoneTransformsTo(BoneTransforms);
              }
        }
        }
        public ModelRenderer(Model model)
        {
            Model = model;
        }

        public ModelRenderer() { }

        public override void Draw()
        {
            if (Model == null)
                return;
            if (Material == null)
                Model.Draw(Transform.World, Camera.Current.View, Camera.Current.Projection);
            else
            {
                foreach (ModelMesh mesh in Model.Meshes)
                {
                    Material.Apply(BoneTransforms[mesh.ParentBone.Index] * Transform.World);  
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
}
