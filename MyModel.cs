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
    
    //Model types
    public enum ModelType
    {
        Colored, Textured, Loaded
    }

    //Model class for all renderable objects
    public class MyModel
    {
		public bool wasLoaded = false;          //Descriptor for whether the model was externally loaded or not
        public Buffer vertices;                 //Vertex buffer for the model
        public VertexInputLayout inputLayout;   //Vertex input layout for the model
		public Model model;                     //Model for loaded models
        public int vertexStride;                //Which direction to step around vertices
        public ModelType modelType;             //What type of model
        public Texture2D Texture;               //Texture information for textured models
        public float collisionRadius;           //Bounding sphere collision detection
		public Vector3[][] modelMap;            //Model map for terrain

		/// <summary>
		/// Create a new model.
		/// </summary>
		/// <param name="game"></param>
		/// <param name="shapeArray"></param>
		/// <param name="textureName"></param>
		/// <param name="collisionRadius"></param>
        
        //Create a new coloured model with no normals... because why not
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
        
        //Create a coloured model with normals
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
        /// 

        //Create a textured model with normals
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
    }
}
