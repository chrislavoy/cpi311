#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using CPI311.GameEngine;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.IO;
#endregion

namespace CPI311.Labs
{
    public class Lab08 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;
        Effect effect;
        Texture2D texture;
        SoundEffect gunSound;

        Model cube;
        List<Transform> transforms;
        List<Collider> colliders;
        Camera camera;

        Vector3 mousePositionWorld;

        public Lab08()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            transforms = new List<Transform>();
            colliders = new List<Collider>();
            
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Time.Initialize();
            InputManager.Initialize();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            effect = Content.Load<Effect>("Effects/SimpleShading");
            texture = Content.Load<Texture2D>("Textures/Square");
            font = Content.Load<SpriteFont>("Fonts/Arial");
            //gunSound = Content.Load<SoundEffect>("Sounds/Gun");
            gunSound = SoundEffect.FromStream(new FileStream("../Content/Sounds/Gun.wav",FileMode.Open));
            cube = Content.Load<Model>("Models/Sphere");
            (cube.Meshes[0].Effects[0] as BasicEffect).EnableDefaultLighting();

            Transform transform = new Transform();
            SphereCollider collider = new SphereCollider();
            collider.Radius = 1f;
            collider.Transform = transform;
            transforms.Add(transform);
            colliders.Add(collider);

            camera = new Camera();
            camera.Transform = new Transform();
            camera.Transform.LocalPosition = Vector3.Backward * 5;
        }

        protected override void Update(GameTime gameTime)
        {
            Time.Update(gameTime);
            InputManager.Update();
            if (InputManager.IsKeyDown(Keys.Escape))
                Exit();
            Vector3 mousePositionScreen = new Vector3(InputManager.GetMousePosition(),0);
            mousePositionWorld = GraphicsDevice.Viewport.Unproject(
                mousePositionScreen, // mouse position
                camera.Projection, // Projection
                camera.View, // View
                Matrix.Identity); // don't give world matrix
            foreach(Collider collider in colliders)
            {
                //collider.Transform.Rotate(Vector3.Up, Time.ElapsedGameTime);
                collider.Transform.Rotate(Vector3.Right, Time.ElapsedGameTime);
                //collider.Transform.Rotate(Vector3.Forward, Time.ElapsedGameTime);

                Vector3 cameraPositionObject =
                    Vector3.Transform(camera.Transform.Position,
                    Matrix.Invert(collider.Transform.World));
                Vector3 mousePositionObject =
                    Vector3.Transform(mousePositionWorld,
                    Matrix.Invert(collider.Transform.World));
                Ray ray = new Ray(cameraPositionObject, 
                    Vector3.Normalize(mousePositionObject-cameraPositionObject));
                if(collider.Intersects(ray) != null)
                {
                    effect.Parameters["DiffuseColor"].SetValue(
                        Color.Red.ToVector3());
                    (cube.Meshes[0].Effects[0] as BasicEffect).DiffuseColor = Color.Blue.ToVector3();
                    SoundEffectInstance soundInstance = gunSound.CreateInstance();
                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        soundInstance.IsLooped = false;
                        soundInstance.Play();
                    }
                        
                }
                else
                {
                    effect.Parameters["DiffuseColor"].SetValue(
                        Color.Blue.ToVector3());
                    (cube.Meshes[0].Effects[0] as BasicEffect).DiffuseColor = Color.Red.ToVector3();
                }
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.DepthStencilState = new DepthStencilState();

            Matrix view = camera.View;
            Matrix projection = camera.Projection;
            //cube.Draw(transforms[0].World, view, projection);
            
            effect.CurrentTechnique = effect.Techniques[1];
            effect.Parameters["View"].SetValue(view);
            effect.Parameters["Projection"].SetValue(projection);
            effect.Parameters["LightPosition"].SetValue(Vector3.Backward * 10 + Vector3.Right * 5);
            effect.Parameters["CameraPosition"].SetValue(camera.Transform.Position);
            effect.Parameters["Shininess"].SetValue(20f);
            effect.Parameters["AmbientColor"].SetValue(new Vector3(0.2f, 0.2f, 0.2f));
            effect.Parameters["SpecularColor"].SetValue(new Vector3(0, 0, 0.5f));
            effect.Parameters["DiffuseTexture"].SetValue(texture);
            foreach (Transform transform in transforms)
            {
                effect.Parameters["World"].SetValue(transform.World);
                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    foreach (ModelMesh mesh in cube.Meshes)
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
           
            spriteBatch.Begin();
            spriteBatch.DrawString(font, mousePositionWorld.ToString(),
                                        Vector2.Zero,Color.Black);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
