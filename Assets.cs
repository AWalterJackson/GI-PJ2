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
    public class Assets
    {
        LabGame game;

		// Private variables used by functions
		public static int HEIGHT_MIN = 5;
		public static int HEIGHT_MAX = -5;
		public static int CORNER = -1;
		public static int HEIGHT_INIT = 3;
		public static float WORLD_WIDTH = 20.0f;
		
		// List of specific model types to load
		private Model shipModel, projectileModel;
		private BoundingSphere modelBounds;

        public Assets(LabGame game)
        {
            this.game = game;
        }

        // Dictionary of currently loaded models.
        // New/existing models are loaded by calling GetModel(modelName, modelMaker).
        public Dictionary<String, MyModel> modelDict = new Dictionary<String, MyModel>();

        // Load a model from the model dictionary.
        // If the model name hasn't been loaded before then modelMaker will be called to generate the model.
        public delegate MyModel ModelMaker();

		/// <summary>
		/// Get a model by name.
		/// </summary>
		/// <param name="modelName">Name of model.</param>
		/// <param name="modelMaker">Maker class used to make model.</param>
		/// <returns>A model.</returns>
        public MyModel GetModel(String modelName, ModelMaker modelMaker)
        {
            if (!modelDict.ContainsKey(modelName))
            {
                modelDict[modelName] = modelMaker();
            }
            return modelDict[modelName];
        }

		/// <summary>
		/// Create a cube with one texture for all faces.
		/// </summary>
		/// <param name="textureName">Texture file to use.</param>
		/// <param name="size">Size of cube.</param>
		/// <returns>A textured cube.</returns>
        public MyModel CreateTexturedCube(String textureName, float size)
        {
            return CreateTexturedCube(textureName, new Vector3(size, size, size));
        }

		/// <summary>
		/// Create a cube with one texture for all faces.
		/// </summary>
		/// <param name="texturePath">Texture file to use.</param>
		/// <param name="size">Size of cube.</param>
		/// <returns>A textured cube.</returns>
        public MyModel CreateTexturedCube(String texturePath, Vector3 size)
        {
            VertexPositionNormalTexture[] shapeArray = new VertexPositionNormalTexture[]{
            new VertexPositionNormalTexture(new Vector3(-1.0f, -1.0f, -1.0f), -Vector3.UnitZ, new Vector2(0.0f, 1.0f)), // Front
            new VertexPositionNormalTexture(new Vector3(-1.0f, 1.0f, -1.0f), -Vector3.UnitZ, new Vector2(0.0f, 0.0f)),
            new VertexPositionNormalTexture(new Vector3(1.0f, 1.0f, -1.0f), -Vector3.UnitZ, new Vector2(1.0f, 0.0f)),
            new VertexPositionNormalTexture(new Vector3(-1.0f, -1.0f, -1.0f), -Vector3.UnitZ, new Vector2(0.0f, 1.0f)),
            new VertexPositionNormalTexture(new Vector3(1.0f, 1.0f, -1.0f), -Vector3.UnitZ, new Vector2(1.0f, 0.0f)),
            new VertexPositionNormalTexture(new Vector3(1.0f, -1.0f, -1.0f), -Vector3.UnitZ, new Vector2(1.0f, 1.0f)),

            new VertexPositionNormalTexture(new Vector3(-1.0f, -1.0f, 1.0f), Vector3.UnitZ, new Vector2(1.0f, 1.0f)), // BACK
            new VertexPositionNormalTexture(new Vector3(1.0f, 1.0f, 1.0f), Vector3.UnitZ, new Vector2(0.0f, 0.0f)),
            new VertexPositionNormalTexture(new Vector3(-1.0f, 1.0f, 1.0f), Vector3.UnitZ, new Vector2(1.0f, 0.0f)),
            new VertexPositionNormalTexture(new Vector3(-1.0f, -1.0f, 1.0f), Vector3.UnitZ, new Vector2(1.0f, 1.0f)),
            new VertexPositionNormalTexture(new Vector3(1.0f, -1.0f, 1.0f), Vector3.UnitZ, new Vector2(0.0f, 1.0f)),
            new VertexPositionNormalTexture(new Vector3(1.0f, 1.0f, 1.0f), Vector3.UnitZ, new Vector2(0.0f, 0.0f)),

            new VertexPositionNormalTexture(new Vector3(-1.0f, 1.0f, -1.0f), Vector3.UnitY, new Vector2(0.0f, 1.0f)), // Top
            new VertexPositionNormalTexture(new Vector3(-1.0f, 1.0f, 1.0f), Vector3.UnitY, new Vector2(0.0f, 0.0f)),
            new VertexPositionNormalTexture(new Vector3(1.0f, 1.0f, 1.0f), Vector3.UnitY, new Vector2(1.0f, 0.0f)),
            new VertexPositionNormalTexture(new Vector3(-1.0f, 1.0f, -1.0f), Vector3.UnitY, new Vector2(0.0f, 1.0f)),
            new VertexPositionNormalTexture(new Vector3(1.0f, 1.0f, 1.0f), Vector3.UnitY, new Vector2(1.0f, 0.0f)),
            new VertexPositionNormalTexture(new Vector3(1.0f, 1.0f, -1.0f), Vector3.UnitY, new Vector2(1.0f, 1.0f)),

            new VertexPositionNormalTexture(new Vector3(-1.0f, -1.0f, -1.0f), -Vector3.UnitY, new Vector2(0.0f, 0.0f)), // Bottom
            new VertexPositionNormalTexture(new Vector3(1.0f, -1.0f, 1.0f), -Vector3.UnitY, new Vector2(1.0f, 1.0f)),
            new VertexPositionNormalTexture(new Vector3(-1.0f, -1.0f, 1.0f), -Vector3.UnitY, new Vector2(0.0f, 1.0f)),
            new VertexPositionNormalTexture(new Vector3(-1.0f, -1.0f, -1.0f), -Vector3.UnitY, new Vector2(0.0f, 0.0f)),
            new VertexPositionNormalTexture(new Vector3(1.0f, -1.0f, -1.0f), -Vector3.UnitY, new Vector2(1.0f, 0.0f)),
            new VertexPositionNormalTexture(new Vector3(1.0f, -1.0f, 1.0f), -Vector3.UnitY, new Vector2(1.0f, 1.0f)),

            new VertexPositionNormalTexture(new Vector3(-1.0f, -1.0f, -1.0f), -Vector3.UnitX, new Vector2(1.0f, 1.0f)), // Left
            new VertexPositionNormalTexture(new Vector3(-1.0f, -1.0f, 1.0f), -Vector3.UnitX, new Vector2(0.0f, 1.0f)),
            new VertexPositionNormalTexture(new Vector3(-1.0f, 1.0f, 1.0f), -Vector3.UnitX, new Vector2(0.0f, 0.0f)),
            new VertexPositionNormalTexture(new Vector3(-1.0f, -1.0f, -1.0f), -Vector3.UnitX, new Vector2(1.0f, 1.0f)),
            new VertexPositionNormalTexture(new Vector3(-1.0f, 1.0f, 1.0f), -Vector3.UnitX, new Vector2(0.0f, 0.0f)),
            new VertexPositionNormalTexture(new Vector3(-1.0f, 1.0f, -1.0f), -Vector3.UnitX, new Vector2(1.0f, 0.0f)),

            new VertexPositionNormalTexture(new Vector3(1.0f, -1.0f, -1.0f), Vector3.UnitX, new Vector2(0.0f, 1.0f)), // Right
            new VertexPositionNormalTexture(new Vector3(1.0f, 1.0f, 1.0f), Vector3.UnitX, new Vector2(1.0f, 0.0f)),
            new VertexPositionNormalTexture(new Vector3(1.0f, -1.0f, 1.0f), Vector3.UnitX, new Vector2(1.0f, 1.0f)),
            new VertexPositionNormalTexture(new Vector3(1.0f, -1.0f, -1.0f), Vector3.UnitX, new Vector2(0.0f, 1.0f)),
            new VertexPositionNormalTexture(new Vector3(1.0f, 1.0f, -1.0f), Vector3.UnitX, new Vector2(0.0f, 0.0f)),
            new VertexPositionNormalTexture(new Vector3(1.0f, 1.0f, 1.0f), Vector3.UnitX, new Vector2(1.0f, 0.0f)),
            };

            for (int i = 0; i < shapeArray.Length; i++)
            {
                shapeArray[i].Position.X *= size.X / 2;
                shapeArray[i].Position.Y *= size.Y / 2;
                shapeArray[i].Position.Z *= size.Z / 2;
            }

            float collisionRadius = (size.X + size.Y + size.Z) / 6 ;
            return new MyModel(game, shapeArray, texturePath, collisionRadius);
        }

		/// <summary>
		/// Create a world base to use.
		/// </summary>
		/// <param name="size">Size of world.</param>
		/// <returns>A world model.</returns>
        public MyModel CreateWorldBase(int size)
        {
            
            float collisionRadius = 1;
			float magnitude = HEIGHT_INIT;
            int sidelength = (int)Math.Pow(2, size);
            int min = -sidelength / 2;
            int k = 0;
			// Data structures for generating vertice heightmap
			Vector3[][] points = new Vector3[sidelength][];
			Vector3[][] normals = new Vector3[sidelength][];
			Random rand = new Random();
			
			// Generate a map by calling the function
			points = genMap(sidelength, -WORLD_WIDTH, WORLD_WIDTH, -WORLD_WIDTH, WORLD_WIDTH, 0.0f);

			// Apply diamond square algorithm
			points = diamondSquare(points, rand, HEIGHT_INIT, 0, sidelength, 0, sidelength, CORNER);

			// Apply diamond square algorithm
			points = flattenSection(points, sidelength, 30,50,30,50);
			points = diamondSquare(points, rand, 1, 32, 49, 32, 49, -4);

			// Apply diamond square algorithm
			points = flattenSection(points, sidelength, 30,50,85,100);
			points = diamondSquare(points, rand, 1, 32, 49, 87, 96, -4);

			// Apply diamond square algorithm
			points = flattenSection(points, sidelength, 85,100,30,50);
			points = diamondSquare(points, rand, 1, 87, 96, 32, 49, -4);

			// Apply diamond square algorithm
			points = flattenSection(points, sidelength, 85,100,85,100);
			points = diamondSquare(points, rand, 1, 87, 96, 87, 96, -4);

			// Lower points
			points = lowerSeaFloor(points, sidelength, -1, 1.5f);

			// Calculate vertex normals
			normals = getNormals(points);

            VertexPositionNormalColor[] shapeArray = new VertexPositionNormalColor[(sidelength + 1) * (sidelength + 1) *6];

            for (int i = 0; i < sidelength; i++)
            {
                for (int j = 0; j < sidelength; j++)
                {
                    //Each step creates a square in the map mesh
                    //Bottom triangle
                    shapeArray[k] = new VertexPositionNormalColor(points[i][j], -Vector3.UnitZ,
						getColor(points[i][j]));
                    shapeArray[k + 1] = new VertexPositionNormalColor(points[i+1][j+1], -Vector3.UnitZ,
						getColor(points[i+1][j+1]));
                    shapeArray[k + 2] = new VertexPositionNormalColor(points[i+1][j], -Vector3.UnitZ,
						getColor(points[i+1][j]));

                    //Top Triangle
                    shapeArray[k + 3] = new VertexPositionNormalColor(points[i][j], -Vector3.UnitZ,
						getColor(points[i][j]));
                    shapeArray[k + 4] = new VertexPositionNormalColor(points[i][j+1], -Vector3.UnitZ, 
						getColor(points[i][j+1]));
                    shapeArray[k + 5] = new VertexPositionNormalColor(points[i+1][j+1], -Vector3.UnitZ, 
						getColor(points[i+1][j+1]));

                    k += 6;
                }
            }

			MyModel m = new MyModel(game, shapeArray, collisionRadius);

			// Make reference to points
			m.modelMap = points;

            return m;
        }

		/// <summary>
		/// Create a new ocean.
		/// </summary>
		/// <param name="size">Size of ocean</param>
		/// <param name="seaLevel">Height of ocean.</param>
		/// <returns>An ocean model.</returns>
        public MyModel CreateOcean(int size, int seaLevel)
        {

			float collisionRadius = 1;
            int sidelength = (int)Math.Pow(2, size);
            int min = -sidelength / 2;
            int k = 0;
			// Data structures for generating vertice heightmap
			Vector3[][] points = new Vector3[sidelength][];
			Vector3[][] normals = new Vector3[sidelength][];
			Random rand = new Random();

			// Generate an ocean by calling the function
			points = genMap(sidelength, -WORLD_WIDTH, WORLD_WIDTH, -WORLD_WIDTH, WORLD_WIDTH, -seaLevel);

			// Calculate vertex normals
			normals = getNormals(points);

            VertexPositionNormalColor[] shapeArray = new VertexPositionNormalColor[sidelength * sidelength * 6];

			for (int i = 0; i < sidelength; i++)
            {
                for (int j = 0; j < sidelength; j++)
                {
                    //Each step creates a square in the map mesh
                    //Bottom triangle
                    shapeArray[k] = new VertexPositionNormalColor(points[i][j], -Vector3.UnitZ,
						Color.SkyBlue);
                    shapeArray[k + 1] = new VertexPositionNormalColor(points[i+1][j+1], -Vector3.UnitZ,
						Color.SkyBlue);
                    shapeArray[k + 2] = new VertexPositionNormalColor(points[i+1][j], -Vector3.UnitZ,
						Color.SkyBlue);

                    //Top Triangle
                    shapeArray[k + 3] = new VertexPositionNormalColor(points[i][j], -Vector3.UnitZ,
						Color.SkyBlue);
                    shapeArray[k + 4] = new VertexPositionNormalColor(points[i][j+1], -Vector3.UnitZ, 
						Color.SkyBlue);
                    shapeArray[k + 5] = new VertexPositionNormalColor(points[i+1][j+1], -Vector3.UnitZ, 
						Color.SkyBlue);

                    k += 6;
                }
            }

            MyModel m = new MyModel(game, shapeArray, collisionRadius);

			// Make reference to points
			m.modelMap = points;

            return m;
        }

		/// <summary>
		/// Create a new ship model.
		/// </summary>
		/// <param name="texturePath">Texture file to use.</param>
		/// <returns>A ship model.</returns>
        public MyModel CreateShip(String texturePath)
        {
            return CreateTexturedCube(texturePath, 0.7f);
        }

		/// <summary>
		/// Create a new cannonball model.
		/// </summary>
		/// <returns>A cannonball model.</returns>
        public MyModel CreateCannonBall()
        {
            return CreateTexturedCube("balltexture.jpg", 0.1f);
        }

		/// <summary>
		/// Create a power-up model.
		/// </summary>
		/// <param name="texturePath">Texture file to use.</param>
		/// <returns>A power-up model.</returns>
        public MyModel CreatePowerup(String texturePath)
        {
            return CreateTexturedCube(texturePath, 1);
        }


		/// <summary>
		/// Generate vertex normals for a terrian map.
		/// </summary>
		/// <param name="map">The vertex map</param>
		/// <returns>A two dimensional normal array corresponding to map</returns>
		private Vector3[][] getNormals(Vector3[][] map) {
			// Get the normals for landscape vertices
			Vector3[] s = new Vector3[(2*map.Length*map[0].Length)];
			Vector3[][] n = new Vector3[map.Length][];
			Vector3 avg, v1, v2, v3, p1, p2;
			int i1, i2, i3;
			int avgItms = 0;
			for (int k = 0; k < n.Length; k++) {
				n[k] = new Vector3[map[0].Length];
			}
			Vector3 point;
			int c = 0;
			// get surface normals
			for (int j = 0; j < map[0].Length - 1; j++) {
				for (int i = 0; i < map.Length - 1; i++) {
					// calculate surface normals for two current triangles
					i1 = (j*map[0].Length)*i;
					i2 = (j*map[0].Length)*i+1;
					i3 = ((j+1)*map[0].Length)*i;
					// used for calculating surface normals and vertex normals
					v1 = new Vector3(map[i][j].X, map[i][j].Y, map[i][j].Z);
					v2 = new Vector3(map[i+1][j].X, map[i+1][j].Y, map[i+1][j].Z);
					v3 = new Vector3(map[i][j+1].X, map[i+1][j+1].Y, map[i][j+1].Z);
					p1 = new Vector3(v1.X - v3.X, v1.Y-v3.Y, v1.Z-v3.Z);
					p2 = new Vector3(v3.X - v2.X, v3.Y-v2.Y, v3.Z-v2.Z);
					point = Vector3.Cross(p1, p2);
					s[c++] = point;
				}
			}
			c = 0;
			// get vertice normals by averaging over adjacent surface normals
			for (int j = 0; j < map[0].Length - 1; j++) {
				for (int i = 0; i < map.Length - 1; i++) {
					avg = Vector3.Zero;
					avgItms = 0;
					// check if top left triangle intersects vertice;
					if ((i-1)>= 0 && (j-1)>=0) {
						avg += n[i-1][j-1];
						avgItms++;
					}
					// check if top right triangle intersects vertice;
					if (i < map[0].Length-1  && j-1>=0) {
						avg += n[i][j-1];
						avgItms++;
					}
					// check if bottom left triangle intersects vertice;
					if ((i-1)>= 0 && j <= map[0].Length-1) {
						avg += n[i-1][j];
						avgItms++;
					}
					// check if bottom right triangle intersects vertice;
					if (i < map[0].Length-1 && j <= map[0].Length-1) {
						avg += n[i][j];
						avgItms++;
					}
					avgItms = 0;
					avg /= avgItms;
					n[i][j] = -Vector3.Normalize(avg);
				}
			}
			return n;
		}

		/// <summary>
		/// Get the color at a vertex.
		/// </summary>
		/// <param name="vertice">Vertex to color.</param>
		/// <returns></returns>
		private Color getColor(Vector3 vertice) {
			// Get the color for a vertice
			Color c = new Color();
			// TO-DO: Implement height specific colouring
			if (vertice.Z <= -1) {
				c = Color.ForestGreen;
			} else {
				c = Color.SandyBrown;
			}
			return c;
		}
		
		/// <summary>
		/// Apply the daimond square algorithm to map.
		/// </summary>
		/// <param name="map"></param>
		/// <param name="rand"></param>
		/// <param name="magnitude"></param>
		/// <param name="x1"></param>
		/// <param name="x2"></param>
		/// <param name="y1"></param>
		/// <param name="y2"></param>
		/// <returns></returns>
		private Vector3[][] diamondSquare(Vector3[][] map, Random rand,
			float magnitude, int x1, int x2, int y1, int y2, float corner) {
			if (x1 >= x2 || y1 >= y2) {
				return map;
			}
			float avg, min, max;
			int i, gap, mainGap, avgItms, c;
			Vector2[][] squares, newSquares;
			Vector2 topLeft, topRight, bottomLeft, bottomRight,
				topMiddle, midLeft, midPoint, midRight, bottomMiddle;
			squares = new Vector2[1][];
			squares[0] = new Vector2[4];
			// set corner points
			min = HEIGHT_MIN;
			max = HEIGHT_MAX;
			squares[0][0] = new Vector2((float)x1,(float)y1);
			squares[0][1] = new Vector2((float)x2,(float)y1);
			squares[0][2] = new Vector2((float)x1,(float)y2);
			squares[0][3] = new Vector2((float)x2,(float)y2);
			map[(int)squares[0][0].X][(int)squares[0][0].Y].Z = corner;
			map[(int)squares[0][1].X][(int)squares[0][1].Y].Z = corner;
			map[(int)squares[0][2].X][(int)squares[0][2].Y].Z = corner;
			map[(int)squares[0][3].X][(int)squares[0][3].Y].Z = corner;
			/*
			map[(int)squares[0][0].X][(int)squares[0][0].Y].Z = rand.NextFloat(min, max);
			map[(int)squares[0][1].X][(int)squares[0][1].Y].Z = rand.NextFloat(min, max);
			map[(int)squares[0][2].X][(int)squares[0][2].Y].Z = rand.NextFloat(min, max);
			map[(int)squares[0][3].X][(int)squares[0][3].Y].Z = rand.NextFloat(min, max);
			*/
			bool done = false;
			mainGap = (x2-x1);
			while (!done) {
				min = 0;
				max = magnitude;
				gap = (int)mainGap/2;
				// perform diamond step for every square
				for (i = 0; i < squares.Length; i++) {
					// create reference vectors for creating more squares
					topLeft = squares[i][0];
					topRight = squares[i][1];
					bottomLeft = squares[i][2];
					bottomRight = squares[i][3];
					// take the average of the four corner points of square
					map[(int)(topLeft.X+gap)][(int)(topLeft.Y + gap)].Z = (
						map[(int)topLeft.X][(int)topLeft.Y].Z + 
						map[(int)topRight.X][(int)topRight.Y].Z + 
						map[(int)bottomLeft.X][(int)bottomLeft.Y].Z + 
						map[(int)bottomRight.X][(int)bottomRight.Y].Z
						)/4 + rand.NextFloat(min, max);
				}
				// perform square step for every diamond
				/* for each horizontally and vertically neighbouring 
				vertex lining up with the midpoint, take the average of the 
				four surronding square vertices that are the same distance away as 
				the diamond midpoint */
				for (i = 0; i < squares.Length; i++) {
					topLeft = squares[i][0];
					topRight = squares[i][1];
					bottomLeft = squares[i][2];
					bottomRight = squares[i][3];
					topMiddle = new Vector2(topLeft.X + gap,
						topLeft.Y);
					midLeft = new Vector2(topLeft.X,
						topLeft.Y + gap);
					midPoint = new Vector2(topLeft.X + gap,
						topLeft.Y + gap);
					midRight = new Vector2(topRight.X,
						topLeft.Y + gap);
					bottomMiddle = new Vector2(topLeft.X + gap,
						bottomLeft.Y);
					// average out topMiddle
					avg = 0;
					avgItms = 0;
					if (topMiddle.Y - gap >= y1) {
						// if a top diamond corner exists, include it in average
						avg += map[(int)(topMiddle.X)][(int)(topMiddle.Y-gap)].Z;
						avgItms += 1;
					}
					avg += map[(int)(topMiddle.X)][(int)(topMiddle.Y+gap)].Z;
					avg += map[(int)(topMiddle.X-gap)][(int)(topMiddle.Y)].Z;
					avg += map[(int)(topMiddle.X+gap)][(int)(topMiddle.Y)].Z;
					avgItms += 3;
					map[(int)(topMiddle.X)][(int)(topMiddle.Y)].Z = (
						(avg/avgItms) + rand.NextFloat(min, max));
					// average out middleLeft
					avg = 0;
					avgItms = 0;
					if (midLeft.X - gap >= x1) {
						// if a top diamond corner exists, include it in average
						avg += map[(int)(midLeft.X-gap)][(int)(midLeft.Y)].Z;
						avgItms += 1;
					}
					avg += map[(int)(midLeft.X)][(int)(midLeft.Y+gap)].Z;
					avg += map[(int)(midLeft.X)][(int)(midLeft.Y-gap)].Z;
					avg += map[(int)(midLeft.X+gap)][(int)(midLeft.Y)].Z;
					avgItms += 3;
					map[(int)(midLeft.X)][(int)(midLeft.Y)].Z = (
						(avg/avgItms) + rand.NextFloat(min, max));
					// average out middleRight
					avg = 0;
					avgItms = 0;
					if (midRight.X + gap <= x2) {
						// if a top diamond corner exists, include it in average
						avg += map[(int)(midRight.X+gap)][(int)(midRight.Y)].Z;
						avgItms += 1;
					}
					avg += map[(int)(midRight.X)][(int)(midRight.Y+gap)].Z;
					avg += map[(int)(midRight.X-gap)][(int)(midRight.Y)].Z;
					avg += map[(int)(midRight.X)][(int)(midRight.Y-gap)].Z;
					avgItms += 3;
					map[(int)(midRight.X)][(int)(midRight.Y)].Z = (
						(avg/avgItms) + rand.NextFloat(min, max));
					// average out bottomMiddle
					avg = 0;
					avgItms = 0;
					if (bottomMiddle.Y + gap <= y2) {
						// if a bottom diamond corner exists, include it in average
						avg += map[(int)(bottomMiddle.X)][(int)(bottomMiddle.Y+gap)].Z;
						avgItms += 1;
					}
					avg += map[(int)(bottomMiddle.X)][(int)(bottomMiddle.Y-gap)].Z;
					avg += map[(int)(bottomMiddle.X-gap)][(int)(bottomMiddle.Y)].Z;
					avg += map[(int)(bottomMiddle.X+gap)][(int)(bottomMiddle.Y)].Z;
					avgItms += 3;
					map[(int)(bottomMiddle.X)][(int)(bottomMiddle.Y)].Z = (
						(avg/avgItms) + rand.NextFloat(min, max));
				}
				// stop upon making size 1 squares and return
				if (mainGap/2 <= 1) {
					break;
				}
				c = 0;
				newSquares = new Vector2[squares.Length*4][];
				// break squares into 4 smaller squares of half size
				for (i = 0; i < squares.Length; i++) {
					topLeft = squares[i][0]; topRight = squares[i][1];
					bottomLeft = squares[i][2]; bottomRight = squares[i][3];
					topMiddle = new Vector2(topLeft.X + gap,
						topLeft.Y);
					midLeft = new Vector2(topLeft.X,
						topLeft.Y + gap);
					midPoint = new Vector2(topLeft.X + gap,
						topLeft.Y + gap);
					midRight = new Vector2(topRight.X,
						topLeft.Y + gap);
					bottomMiddle = new Vector2(topLeft.X + gap,
						bottomLeft.Y);
					// TO-DO: split current square into 4 smaller squares of size gap;
					// TOPLEFT SQUARE
					newSquares[c] = new Vector2[4];
					newSquares[c][0] = topLeft; newSquares[c][1] = topMiddle;
					newSquares[c][2] = midLeft; newSquares[c][3] = midPoint;
					c++;
					// TOPRIGHT SQUARE
					newSquares[c] = new Vector2[4];
					newSquares[c][0] = topMiddle; newSquares[c][1] = topRight;
					newSquares[c][2] = midPoint; newSquares[c][3] = midRight;
					c++;
					// BOTTOMLEFT SQUARE
					newSquares[c] = new Vector2[4];
					newSquares[c][0] = midLeft; newSquares[c][1] = midPoint;
					newSquares[c][2] = bottomLeft; newSquares[c][3] = bottomMiddle;
					c++;
					// BOTTOMRIGHT SQUARE
					newSquares[c] = new Vector2[4];
					newSquares[c][0] = midPoint; newSquares[c][1] = midRight;
					newSquares[c][2] = bottomMiddle; newSquares[c][3] = bottomRight;
					c++;
				}
				// reduce the random magnitude
				squares = newSquares;
				magnitude = Math.Max(magnitude/2,0.01f);
				mainGap = gap;
			}
			return map;
		}
		
		/// <summary>
		/// Lower parts of the sea floor that would cause grounding if a boat were in the sea.
		/// </summary>
		/// <param name="map"></param>
		/// <param name="n"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		private Vector3[][] lowerSeaFloor(Vector3[][] map, int n, float seaLevel, float minDepth){
			// Add vertices to array
			for (int i = 0; i < n; i++) {
				for (int j = 0; j < n; j++){
					// Check the point height, lower if necessary
					if (map[i][j].Z > seaLevel && map[i][j].Z > seaLevel + minDepth){
						map[i][j].Z = seaLevel + minDepth;
					}
				}
			}
			return map;
		}

		/// <summary>
		/// Lower parts of the sea floor that would cause grounding if a boat were in the sea.
		/// </summary>
		/// <param name="map"></param>
		/// <param name="n"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		private Vector3[][] flattenSection(Vector3[][] map, int n, int i1, int i2, int j1, int j2){
			// Add vertices to array
			for (int i = 0; i < n; i++) {
				for (int j = 0; j < n; j++){
					// Check the point height, lower if necessary
					if (i >= i1 && i <= i2 && j >= j1 && j <= j2){
						map[i][j].Z = 0;
					}
				}
			}
			return map;
		}
		
		/// <summary>
		/// Generate a flat (n x n) vertice map from (xLeft,0,zFront) to (xRight, 0, zBack)
		/// </summary>
		/// <param name="n"></param>
		/// <param name="xLeft"></param>
		/// <param name="xRight"></param>
		/// <param name="zFront"></param>
		/// <param name="zBack"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		private Vector3[][] genMap(int n, float xLeft, float xRight, float zFront, float zBack, float height) {
			// Return a new flat plane of size n*n + 1 covering (xleft, 0, zBack) to (xRight, 0, zFront)
			Vector3[][] vertices = new Vector3[n+1][];
			// Determine spacing between x and z boundaries
			float xGap = Math.Abs(xRight - xLeft);
			float zGap = Math.Abs(zFront - zBack);
			// Add vertices to array
			for (int i = 0; i <= n; i++) {
				vertices[i] = new Vector3[n+1];
				for (int j = 0; j <= n; j++){
					// Generate a vertice in the array
					vertices[i][j] = new Vector3(xLeft + (xGap*((float)i/n)), 
						zFront + (zGap*((float)j/n)), height);
				}
			}
			return vertices;
		}

    }
}
