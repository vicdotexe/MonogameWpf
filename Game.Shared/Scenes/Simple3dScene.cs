using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Vez.CoreServices;
using Vez.CoreServices.Inputs;

namespace Game.Shared.Scenes
{
    public class Simple3dScene : IDrawService, IUpdateService, IInitializeService
    {
        private readonly Input _input;
        private readonly GraphicsDevice _graphicsDevice;

        private Vector3 camTarget;
        private Vector3 camPosition;
        private Matrix projectionMatrix;
        private Matrix viewMatrix;
        private Matrix worldMatrix;

        private BasicEffect basicEffect;
        private VertexPositionColor[] pyramidVertices;
        private VertexBuffer vertexBuffer;
        private IndexBuffer indexBuffer;
        short[] pyramidIndices;

        public Simple3dScene(Input input, GraphicsDevice graphicsDevice)
        {
            _input = input;
            _graphicsDevice = graphicsDevice;
        }
        public void Initialize()
        {
            camTarget = new Vector3(0, 0, 0);
            camPosition = new Vector3(0, 0, -100f);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f), _graphicsDevice.DisplayMode.AspectRatio, 1f, 1000f);
            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, Vector3.Up);
            worldMatrix = Matrix.CreateWorld(camTarget, Vector3.Forward, Vector3.Up);

            //BasicEffect
            basicEffect = new BasicEffect(_graphicsDevice);
            basicEffect.Alpha = 1f;

            // Want to see the colors of the vertices, this needs to be on
            basicEffect.VertexColorEnabled = true;

            //Lighting requires normal information which VertexPositionColor does not have
            //If you want to use lighting and VPC you need to create a custom def
            basicEffect.LightingEnabled = false;

            // Geometry - a simple pyramid (tetrahedron) about the origin
            pyramidVertices = new VertexPositionColor[4];
            pyramidVertices[0] = new VertexPositionColor(new Vector3(0, 20, 0), Color.Red);
            pyramidVertices[1] = new VertexPositionColor(new Vector3(-20, -20, 20), Color.Green);
            pyramidVertices[2] = new VertexPositionColor(new Vector3(20, -20, 20), Color.Blue);
            pyramidVertices[3] = new VertexPositionColor(new Vector3(0, -20, -20), Color.Yellow);

            // Index buffer
            pyramidIndices = new short[]
            {
                0, 1, 2,
                0, 2, 3,
                0, 3, 1,
                1, 3, 2
            };

            vertexBuffer = new VertexBuffer(_graphicsDevice, typeof(VertexPositionColor), pyramidVertices.Length, BufferUsage.WriteOnly);
            vertexBuffer.SetData<VertexPositionColor>(pyramidVertices);

            indexBuffer = new IndexBuffer(_graphicsDevice, typeof(short), pyramidIndices.Length, BufferUsage.WriteOnly);
            indexBuffer.SetData(pyramidIndices);
        }

        private Vector2 _mouseDown;
        private Matrix _mouseDownWorldMatrix;

        public void Update(float deltaTime)
        {
            if (_input.LeftMouseButtonPressed)
            {
                _mouseDown = _input.MousePosition;
                _mouseDownWorldMatrix = worldMatrix;
            }

            if (!_input.LeftMouseButtonDown)
            {
                return;
            }

            Vector2 mouseDelta = _input.MousePosition - _mouseDown;

            float sensitivity = 1f;

            // Creating rotation matrix
            float rotationX = mouseDelta.X * sensitivity;
            float rotationY = mouseDelta.Y * sensitivity;
            Matrix rotationMatrix = Matrix.CreateRotationX(MathHelper.ToRadians(-rotationY)) * Matrix.CreateRotationY(MathHelper.ToRadians(-rotationX));

            worldMatrix = _mouseDownWorldMatrix * rotationMatrix;

        }

        public void Draw()
        {
            basicEffect.Projection = projectionMatrix;
            basicEffect.View = viewMatrix;
            basicEffect.World = worldMatrix;

            _graphicsDevice.Clear(Color.CornflowerBlue);
            _graphicsDevice.SetVertexBuffer(vertexBuffer);
            _graphicsDevice.Indices = indexBuffer;

            //Turn off culling so we see both sides of our rendered triangle
            _graphicsDevice.RasterizerState = new () {CullMode = CullMode.None};

            foreach (EffectPass pass in basicEffect.CurrentTechnique.
                         Passes)
            {
                pass.Apply();
                _graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, pyramidVertices.Length, 0, pyramidIndices.Length / 3);
            }
        }


    }
}