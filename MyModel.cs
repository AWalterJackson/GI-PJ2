using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Toolkit;
namespace Project
{
    using SharpDX.Toolkit.Graphics;
    
    public enum ModelType
    {
        Colored, Textured, Loaded
    }
    public class MyModel
    {
		public bool wasLoaded = false;
        public Buffer vertices;
        public VertexInputLayout inputLayout;
		public Model model;
        public int vertexStride;
        public ModelType modelType;
        public Texture2D Texture;
        public float collisionRadius;
		public Vector3[][] modelMap;

		/// <summary>
		/// Create a new model.
		/// </summary>
		/// <param name="game"></param>
		/// <param name="shapeArray"></param>
		/// <param name="textureName"></param>
		/// <param name="collisionRadius"></param>
        public MyModel(LabGame game, VertexPositionColor[] shapeArray, String textureName, float collisionRadius)
        {
            this.vertices = Buffer.Vertex.New(game.GraphicsDevice, shapeArray);
            this.inputLayout = VertexInputLayout.New<VertexPositionColor>(0);
            vertexStride = Utilities.SizeOf<VertexPositionColor>();
            modelType = ModelType.Colored;
            this.collisionRadius = collisionRadius;
			wasLoaded = false;
        }

		/// <summary>
		/// Create a new model.
		/// </summary>
		/// <param name="game"></param>
		/// <param name="shapeArray"></param>
		/// <param name="collisionRadius"></param>
        public MyModel(LabGame game, VertexPositionNormalColor[] shapeArray, float collisionRadius)
        {
            this.vertices = Buffer.Vertex.New(game.GraphicsDevice, shapeArray);
            this.inputLayout = VertexInputLayout.New<VertexPositionNormalColor>(0);
            vertexStride = Utilities.SizeOf<VertexPositionNormalColor>();
            modelType = ModelType.Colored;
            this.collisionRadius = collisionRadius;
			wasLoaded = false;
        }

		/// <summary>
		/// Create a new model.
		/// </summary>
		/// <param name="game"></param>
		/// <param name="shapeArray"></param>
		/// <param name="textureName"></param>
		/// <param name="collisionRadius"></param>
        public MyModel(LabGame game, VertexPositionNormalTexture[] shapeArray, String textureName, float collisionRadius)
        {
            this.vertices = Buffer.Vertex.New(game.GraphicsDevice, shapeArray);
            this.inputLayout = VertexInputLayout.New<VertexPositionNormalTexture>(0);
            vertexStride = Utilities.SizeOf<VertexPositionNormalTexture>();
            modelType = ModelType.Textured;
            Texture = game.Content.Load<Texture2D>(textureName);
            this.collisionRadius = collisionRadius;
			wasLoaded = false;
        }

		/// <summary>
		/// Create a new model.
		/// </summary>
		/// <param name="game"></param>
		/// <param name="modelName"></param>
        public MyModel(LabGame game, string modelName)
        {
			model = game.Content.Load<Model>(modelName);
			wasLoaded = true;
            modelType = ModelType.Colored;
        }
    }
}
