using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Toolkit;
using Windows.UI.Input;
using Windows.UI.Core;

namespace Project
{
    using SharpDX.Toolkit.Graphics;
    public enum GameObjectType
    {
        None, Player, Enemy, Ocean, Terrain, Powerup
    }

    // Super class for all game objects.
    abstract public class GameObject
    {
        public MyModel myModel;
        public LabGame game;
        public GameObjectType type = GameObjectType.None;
        public Vector3 pos;
        public Matrix World;
        public Matrix WorldInverseTranspose;
        public Effect effect;
        public BasicEffect basicEffect;

        public abstract void Update(GameTime gametime);
        public void Draw(GameTime gametime)
        {
            // Some objects such as the Enemy Controller have no model and thus will not be drawn
            if (myModel != null)
            {
                if (myModel.wasLoaded)
                {
                    if (myModel.modelType == ModelType.Colored)
                    {
                        this.effect.Parameters["View"].SetValue(game.camera.View);
                        this.effect.Parameters["Projection"].SetValue(game.camera.Projection);
                        this.effect.Parameters["World"].SetValue(Matrix.Identity);
                        this.effect.Parameters["cameraPos"].SetValue(game.camera.pos);
                        this.effect.Parameters["worldInvTrp"].SetValue(WorldInverseTranspose);
                        myModel.model.Draw(game.GraphicsDevice,
                            Matrix.Identity, game.camera.View, game.camera.Projection);
                    }
                    if (myModel.modelType == ModelType.Textured)
                    {
                        this.basicEffect.World = Matrix.Identity;
                        this.basicEffect.Projection = game.camera.Projection;
                        this.basicEffect.View = game.camera.View;
                        myModel.model.Draw(game.GraphicsDevice,
                            basicEffect.World, basicEffect.View, basicEffect.Projection);
                    }
                    
                }
                else
                {
                    if (myModel.modelType == ModelType.Colored)
                    {
                        game.lighting.SetLighting(this.effect);

                        this.effect.Parameters["View"].SetValue(game.camera.View);
                        this.effect.Parameters["Projection"].SetValue(game.camera.Projection);
                    }
                    else
                    {
                        this.basicEffect.LightingEnabled = true;

                        basicEffect.AmbientLightColor = new Vector3(0.2f, 0.2f, 0.2f);
                        basicEffect.DirectionalLight0.Enabled = true;
                        basicEffect.DirectionalLight0.DiffuseColor = new Vector3(0.6f, 0.6f, 0.6f);
                        basicEffect.DirectionalLight0.Direction = new Vector3(0, 0, 1f);
                        basicEffect.DirectionalLight0.SpecularColor = new Vector3(0.1f, 0.1f, 0.166f);

                        basicEffect.View = game.camera.View;
                        basicEffect.Projection = game.camera.Projection;
                    }

                    // Setup the vertices
                    game.GraphicsDevice.SetVertexBuffer(0, myModel.vertices, myModel.vertexStride);
                    game.GraphicsDevice.SetVertexInputLayout(myModel.inputLayout);

                    if (type == GameObjectType.Ocean)
                    {
                        game.GraphicsDevice.SetBlendState(game.GraphicsDevice.BlendStates.AlphaBlend);
                    }

                    // Apply the basic effect technique and draw the object
                    if (myModel.modelType == ModelType.Colored)
                    {
                        effect.CurrentTechnique.Passes[0].Apply();
                    }
                    else
                    {
                        basicEffect.CurrentTechnique.Passes[0].Apply();
                    }
                    //System.Diagnostics.Debug.WriteLine(myModel.vertices.ElementCount);
                    game.GraphicsDevice.Draw(PrimitiveType.TriangleList, myModel.vertices.ElementCount);
                }
            }
        }

        public void GetParamsFromModel()
        {
            if (myModel.modelType == ModelType.Colored) {
                effect = game.Content.Load<Effect>("MultiPoint");
                this.effect.Parameters["View"].SetValue(game.camera.View);
                this.effect.Parameters["Projection"].SetValue(game.camera.Projection);
                this.effect.Parameters["World"].SetValue(Matrix.Identity);

            }
            else if (myModel.modelType == ModelType.Textured) {
                basicEffect = new BasicEffect(game.GraphicsDevice)
                {
                    View = game.camera.View,
                    Projection = game.camera.Projection,
                    World = Matrix.Identity,
                    Texture = myModel.Texture,
                    TextureEnabled = true,
                    VertexColorEnabled = false
                };
            }
        }

        // These virtual voids allow any object that extends GameObject to respond to tapped and manipulation events
        public virtual void Tapped(GestureRecognizer sender, TappedEventArgs args)
        {

        }

        public virtual void OnManipulationStarted(GestureRecognizer sender, ManipulationStartedEventArgs args)
        {

        }

        public virtual void OnManipulationUpdated(GestureRecognizer sender, ManipulationUpdatedEventArgs args)
        {

        }

        public virtual void OnManipulationCompleted(GestureRecognizer sender, ManipulationCompletedEventArgs args)
        {

        }
    }
}
