﻿using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;

namespace MultiConverter.WPF.OpenGL
{
    public class Camera
    {
        public int Width, Height;
        public Matrix4 projectionMatrix;

        public bool flyMode = false;

        public Vector3 Pos;
        private Vector3 Target;
        private Vector3 Up;

        public float rotationAngle = 0.0f;

        private float stepSize = 0.2f;

        public Camera(int viewportWidth, int viewportHeight, Vector3 pos, Vector3 target)
        {
            viewportSize(viewportWidth, viewportHeight);

            Pos = pos;

            Target = target;
            Target.Normalize();

            Up = Vector3.UnitZ;
            Up.Normalize();
        }

        public void processKeyboardInput(KeyboardState state)
        {
            if (state.IsKeyDown(Key.W))
                Pos += (Target * stepSize);

            if (state.IsKeyDown(Key.S))
                Pos -= (Target * stepSize);

            if (flyMode)
            {
                if (state.IsKeyDown(Key.A))
                    Pos.Y -= 0.1f;

                if (state.IsKeyDown(Key.D))
                    Pos.Y += 0.1f;
            }
            else
            {
                if (state.IsKeyDown(Key.A))
                    rotationAngle += 0.1f;

                if (state.IsKeyDown(Key.D))
                    rotationAngle -= 0.1f;
            }


            if (state.IsKeyDown(Key.Up))
                Pos.Z += 0.1f;

            if (state.IsKeyDown(Key.Down))
                Pos.Z -= 0.1f;

            if (state.IsKeyDown(Key.Right))
                Pos.Y -= 0.1f;

            if (state.IsKeyDown(Key.Left))
                Pos.Y += 0.1f;

            if (state.IsKeyDown(Key.R))
                ResetCamera();

            if (state.IsKeyDown(Key.H))
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            if (state.IsKeyUp(Key.H))
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

            if (state.IsKeyDown(Key.I))
            {
                Console.WriteLine(Pos.ToString());
                Console.WriteLine(rotationAngle.ToString());
            }
        }

        public void ResetCamera()
        {
            Pos = new Vector3(11.0f, 0, 4.0f);
            rotationAngle = 0.0f;
        }

        public void viewportSize(int viewportWidth, int viewportHeight)
        {
            Width = viewportWidth;
            Height = viewportHeight;
            float aspectRatio = Width / (float)Height;
            projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 1.0f, 4096.0f);
        }

        public void setupGLRenderMatrix(int shaderProgram)
        {
            var projectionMatrixLocation = GL.GetUniformLocation(shaderProgram, "projection_matrix");
            GL.Viewport(0, 0, Width, Height);
            GL.UniformMatrix4(projectionMatrixLocation, false, ref projectionMatrix);

            var modelviewMatrixLocation = GL.GetUniformLocation(shaderProgram, "modelview_matrix");
            Matrix4 modelViewMatrix = Matrix4.LookAt(Pos, Pos + Target, Up);
            GL.UniformMatrix4(modelviewMatrixLocation, false, ref modelViewMatrix);

            var rotationMatrixLocation = GL.GetUniformLocation(shaderProgram, "rotation_matrix");
            Matrix4 rotationMatrix = Matrix4.CreateRotationZ(rotationAngle);
            GL.UniformMatrix4(rotationMatrixLocation, false, ref rotationMatrix);
        }

        private float DegreeToRadian(float angleInDegrees)
        {
            return (float)Math.PI * angleInDegrees / 180.0f;
        }

        public double RadiansToDegrees(double radians)
        {
            const double radToDeg = 180.0 / Math.PI;
            return radians * radToDeg;
        }
    }
}
