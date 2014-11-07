using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPI311.GameEngine
{
    public class TerrainRenderer : Component
    {
        private VertexPositionTexture[] Vertices { get; set; }
        private int[] Indices { get; set; }
        private float[] Heights { get; set; }
        public Texture2D HeightMap { get; set; }
        public Texture2D NormalMap { get; set; }
        Vector2 size;

        public TerrainRenderer(Texture2D heightMap, 
            Vector2 size, Vector2 resolution)
        {
            this.size = resolution;
            HeightMap = heightMap;
            CreateHeights();
            int rows = (int)(resolution.Y) + 1;
            int cols = (int)(resolution.X) + 1;

            Vertices = new VertexPositionTexture[rows * cols];
            for(int r = 0; r < rows; r++)
                for(int c = 0; c < cols; c++)
                {
                    Vertices[r * cols + c] = new VertexPositionTexture
                    (new Vector3(c, 
                        GetHeight(new Vector2(r/(float)rows, c/(float)cols)), 
                        r), 
                    Vector2.Zero);
                }

            Indices = new int[(rows - 1) * (cols - 1) * 6];
            int index = 0;
            for(int r = 0; r < rows-1; r++)
                for (int c = 0; c < cols - 1; c++)
                {
                    Indices[index++] = r * cols + c;
                    Indices[index++] = r * cols + c + 1;
                    Indices[index++] = (r + 1) * cols + c;

                    Indices[index++] = (r + 1) * cols + (c + 1);
                    Indices[index++] = (r + 1) * cols + c;
                    Indices[index++] = r * cols + c + 1;
                }
        }

        private void CreateHeights()
        {
            Color[] data = new Color[HeightMap.Width * HeightMap.Height];
            HeightMap.GetData<Color>(data);
            Heights = new float[HeightMap.Width * HeightMap.Height];
            for (int i = 0; i < Heights.Length; i++)
                Heights[i] = data[i].G / 255f;
        }

        public float GetHeight(Vector2 tex)
        {
            tex *= //Vector2.Clamp(tex Vector2.Zero, Vector2.One) *
                new Vector2(HeightMap.Width, HeightMap.Height);
            return Heights[(int)tex.Y * HeightMap.Width + (int)tex.X];
        }

        public float GetAltitude(Vector3 position)
        {
            if (position.X > 0 && position.X < size.Y &&
                position.Z > 0 && position.Z < size.X)
                return GetHeight(new Vector2(position.X / size.X, position.Z / size.Y)) *
                    Transform.LocalScale.Y ;
            return -1;

        }

        public void Draw()
        {
            ScreenManager.GraphicsDevice.
                DrawUserIndexedPrimitives<VertexPositionTexture>(
                PrimitiveType.TriangleList,
                Vertices, 0, Vertices.Length,
                Indices, 0, Indices.Length / 3);
        }
    }
}
