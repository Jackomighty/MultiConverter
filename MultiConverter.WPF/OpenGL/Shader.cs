﻿using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiConverter.WPF.OpenGL
{
    public static class Shader
    {
        public static int CompileShader(string type)
        {
            Console.WriteLine($"OpenGL Version: {GL.GetString(StringName.Version)}");
            Console.WriteLine($"OpenGL Vendor: {GL.GetString(StringName.Vendor)}");

            var vertexShader = GL.CreateShader(ShaderType.VertexShader);

            var vertexSource = File.ReadAllText($"Shaders/{type}.vertex.shader");
            GL.ShaderSource(vertexShader, vertexSource);

            GL.CompileShader(vertexShader);

            GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out int vertexShaderStatus);
            Console.WriteLine($"[{type}][VERTEX] Shader compile status: {vertexShaderStatus}");

            GL.GetShaderInfoLog(vertexShader, out string vertexShaderLog);
            Console.Write(vertexShaderLog);

            // Fragment shader
            var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);

            var fragmentSource = File.ReadAllText($"Shaders/{type}.fragment.shader");
            GL.ShaderSource(fragmentShader, fragmentSource);

            GL.CompileShader(fragmentShader);

            GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out int fragmentShaderStatus);
            Console.WriteLine($"[{type}][FRAGMENT] Shader compile status: {fragmentShaderStatus}");

            GL.GetShaderInfoLog(fragmentShader, out string fragmentShaderLog);
            Console.Write(fragmentShaderLog);

            // Shader program
            var shaderProgram = GL.CreateProgram();
            GL.AttachShader(shaderProgram, vertexShader);
            GL.AttachShader(shaderProgram, fragmentShader);

            GL.BindFragDataLocation(shaderProgram, 0, "outColor");

            GL.LinkProgram(shaderProgram);
            var programInfoLog = GL.GetProgramInfoLog(shaderProgram);
            Console.Write(programInfoLog);

            GL.GetProgram(shaderProgram, GetProgramParameterName.LinkStatus, out int programStatus);
            Console.WriteLine($"[{type}][PROGRAM] Program link status: {programStatus}");
            GL.UseProgram(shaderProgram);

            GL.ValidateProgram(shaderProgram);

            GL.DetachShader(shaderProgram, vertexShader);
            GL.DeleteShader(vertexShader);

            GL.DetachShader(shaderProgram, fragmentShader);
            GL.DeleteShader(fragmentShader);

            return shaderProgram;
        }
    }
}
