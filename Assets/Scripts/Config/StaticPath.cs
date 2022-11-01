using UnityEngine;

namespace Config
{
    public static class StaticPath 
    {
        
        public static string AssetsPath => Resources.Load("/TestText").ToString();
    }
}