using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Noesis
{
    public partial class Texture
    {
        static Texture()
        {
            Noesis.GUI.Init();
            Noesis_SetUnregisterTextureCallback(_unregisterTexture);
        }

        public static Texture WrapTexture(object texture, IntPtr nativePointer,
            int width, int height, int numLevels)
        {
            IntPtr texturePtr = Noesis_WrapTexture(nativePointer, width, height, numLevels);
            Noesis.Texture tex = new Noesis.Texture(texturePtr, true);

            // A texture with the same pointer could be enqueued for release, just remove it
            // now to avoid an exception when adding the new texture to the dictionary
            if (Textures.ContainsKey(texturePtr.ToInt64()))
            {
                Noesis_RemoveEnqueuedTexture(texturePtr);
                Textures.Remove(texturePtr.ToInt64());
            }

            Textures.Add(texturePtr.ToInt64(), texture);

            return tex;
        }

        private static Dictionary<long, object> Textures = new Dictionary<long, object>();

        private delegate void UnregisterTextureCallback(IntPtr texturePtr);
        private static UnregisterTextureCallback _unregisterTexture = UnregisterTexture;
        [MonoPInvokeCallback(typeof(UnregisterTextureCallback))]
        private static void UnregisterTexture(IntPtr texturePtr)
        {
            try
            {
                Textures.Remove(texturePtr.ToInt64());
            }
            catch (Exception exception)
            {
                Error.UnhandledException(exception);
            }
        }

        #region Imports
        [DllImport(Library.Name)]
        static extern void Noesis_RemoveEnqueuedTexture(IntPtr texturePtr);

        [DllImport(Library.Name)]
        static extern IntPtr Noesis_WrapTexture(IntPtr texture, int width, int height,int numLevels);

        [DllImport(Library.Name)]
        static extern void Noesis_SetUnregisterTextureCallback(UnregisterTextureCallback callback);
        #endregion
    }
}
