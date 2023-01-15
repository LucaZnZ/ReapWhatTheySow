using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ZnZUtil.Editor
{
    public static class AssetUtil
    {
        public static List<T> LoadAssetsInFolder<T>(string path) where T : Object
        {
            var absPath = Application.dataPath + "\\" +
                          (path.StartsWith("Assets") ? path.Remove(0, "Assets".Length) : path).TrimStart('/', '\\');

            var files = Directory.GetFiles(absPath);
            var assets = files
                .Select(file => AssetDatabase.LoadAssetAtPath<T>(path + Path.GetFileName(file)))
                .Where(asset => asset != null).ToList();
            Debug.Log($"loaded {assets.Count} from path {path}");
            return assets;
        }
    }
}