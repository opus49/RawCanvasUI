using System;
using System.Collections.Generic;
using System.IO;
using Rage;

namespace RawCanvasUI.Util
{
    /// <summary>
    /// Class for loading and accessing textures.
    /// </summary>
    internal static class TextureHandler
    {
        private static readonly Dictionary<string, Dictionary<string, Texture>> Textures = new Dictionary<string, Dictionary<string, Texture>>();
        private static readonly Dictionary<string, List<string>> Blacklist = new Dictionary<string, List<string>>();

        /// <summary>
        /// Loads textures from the specified directory path.
        /// </summary>
        /// <param name="uuid">The unique identifer of the canvas.</param>
        /// <param name="path">The path to the directory containing the textures.</param>
        public static void Load(string uuid, string path)
        {
            var textures = new Dictionary<string, Texture>();
            try
            {
                foreach (string file in Directory.GetFiles(path, "*.png", SearchOption.AllDirectories))
                {
                    var texture = LoadTexture(file);
                    if (texture != null)
                    {
                        var name = GetRelativePath(path, file);
                        textures[name] = texture;
                        Logging.Debug($"loaded texture: {name}");
                    }
                    else
                    {
                        Logging.Warning($"could not load texture: {file}");
                    }
                }

                Textures[uuid] = textures;
                Logging.Info($"loaded {textures.Count} textures for {uuid}");
            }
            catch (IOException ex)
            {
                Logging.Error($"cannot access texture path: {path}", ex);
            }
            catch (System.Security.SecurityException ex)
            {
                Logging.Error($"LoadTextures cannot access calling assembly!", ex);
            }
        }

        /// <summary>
        /// Gets a texture by its name for the specified uuid.
        /// </summary>
        /// <param name="uuid">The unique identifer of the canvas.</param>
        /// <param name="name">The name of the texture.</param>
        /// <returns>The texture if it exists, otherwise null.</returns>
        public static Texture Get(string uuid, string name)
        {
            if (Textures.TryGetValue(uuid, out Dictionary<string, Texture> textures))
            {
                if (textures.TryGetValue(name, out Texture texture))
                {
                    return texture;
                }
            }

            if (!Blacklist.ContainsKey(uuid))
            {
                Blacklist[uuid] = new List<string>();
            }

            if (!Blacklist[uuid].Contains(name))
            {
                Blacklist[uuid].Add(name);
                Logging.Warning($"could not load texture {name} for canvas {uuid}");
            }

            return null;
        }

        /// <summary>
        /// Loads a texture from disk.
        /// </summary>
        /// <param name="filename">The filename of the texture.</param>
        /// <returns>A Rage texture object.</returns>
        public static Texture LoadTexture(string filename)
        {
            try
            {
                return Game.CreateTextureFromFile(filename);
            }
            catch (Exception ex)
            {
                Logging.Error("could not load texture", ex);
            }

            return null;
        }

        private static string GetRelativePath(string rootPath, string fullPath)
        {
            Uri rootUri = new Uri($"{rootPath}/");
            Uri fullUri = new Uri(fullPath);
            return rootUri.MakeRelativeUri(fullUri).ToString();
        }
    }
}
