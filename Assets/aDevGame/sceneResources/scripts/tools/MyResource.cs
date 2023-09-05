#if UNITY_EDITOR
#nullable enable

using System;
using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace aDevGame.sceneResources.scripts.tools
{
    /// <summary>
    /// 资源打包
    /// </summary>
    public class MyResource : MonoBehaviour
    {
        public static readonly string REMOTE_RES_URL = "http://localhost:8080/StreamingAssets";
        // 打包输出目录,通常为StreamingAssets（若不存在该目录需要新建一个）,这是Unity的一个特殊目录，打包时该目录下所有资源会被打进包体中
        //StreamingAssets与Resources的区别在于，StreamingAssets不会被压缩打进包体，而Resources会被压缩
        public static readonly string RES_OUTPUT_PATH         = "Assets/StreamingAssets";
        public static readonly string RES_OUTPUT_PATH_IOS     = RES_OUTPUT_PATH + "/IOS";
        public static readonly string RES_OUTPUT_PATH_ANDROID = RES_OUTPUT_PATH + "/ANDROID";
        public static readonly string RES_OUTPUT_PATH_WIN64   = RES_OUTPUT_PATH + "/WIN";
        public static readonly string RES_OUTPUT_PATH_OSX     = RES_OUTPUT_PATH + "/MAC";

        //MenuItem会在unity菜单栏添加自定义新项
        [MenuItem("CustomEditor/BuildWindows AssetBundle")]
        private static void BuildWindows()
        {
            if (!string.IsNullOrWhiteSpace(RES_OUTPUT_PATH_WIN64) && !Directory.Exists(RES_OUTPUT_PATH_WIN64))
            {
                Directory.CreateDirectory(RES_OUTPUT_PATH_WIN64);
            }

            //打包，第一个参数是AB的输出目录，第二个参数是打包选项,第三个参数是打包的平台,IOS,Android,Win要区分开，不然AB使用的时候会有问题。
            BuildPipeline.BuildAssetBundles(RES_OUTPUT_PATH_WIN64, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows64);
            AssetDatabase.Refresh();
        }
    }
}
#endif