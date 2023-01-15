using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using ZnZUtil;
using ZnZUtil.Editor;

namespace ZnZUtiliy.Editor
{
    public abstract class FileManagerBase : EditorWindow
    {
        protected readonly struct ImportPathGroup
        {
            public readonly string remotePath, localPath;

            public ImportPathGroup(string remotePath, string localPath)
            {
                this.remotePath = remotePath;
                this.localPath = localPath;
            }
        }

        protected readonly struct AssetPathGroup
        {
            public readonly string name, textAssetPath, objectAssetPath;
            public readonly Action<string, string> importFunction;

            public AssetPathGroup(string name, string textAssetPath, string objectAssetPath,
                Action<string, string> importFunction)
            {
                this.name = name;
                this.textAssetPath = textAssetPath;
                this.objectAssetPath = objectAssetPath;
                this.importFunction = importFunction;
            }
        }

        private readonly HashSet<ImportPathGroup> importerPaths = new();
        private readonly HashSet<AssetPathGroup> assetPaths = new();

        protected void AddRemoteImportPath(ImportPathGroup pathGroup) => importerPaths.Add(pathGroup);
        protected void AddAssetReimportPath(AssetPathGroup pathGroup) => assetPaths.Add(pathGroup);

        private void OnEnable() => Initialize();

        protected abstract void Initialize();

        protected virtual void OnGUI()
        {
            GUILayout.Space(10);

            if (GUILayout.Button("Import all files"))
            {
                foreach (var p in importerPaths)
                    CopyFiles(p.remotePath, p.localPath);
                AssetDatabase.Refresh();
            }

            if (GUILayout.Button("Export all files"))
                foreach (var p in importerPaths)
                    CopyFiles(p.localPath, p.remotePath);

            GUILayout.Space(20);

            foreach (var asset in assetPaths)
                if (GUILayout.Button($"Reimport {asset.name}"))
                    asset.importFunction(asset.textAssetPath, asset.objectAssetPath);

            GUILayout.Space(10);
        }

        private static void CopyFiles(string sourceDir, string targetDir, bool overwrite = true)
        {
            var files = Directory.GetFiles(sourceDir);
            foreach (var file in files.Where(f => Path.GetExtension(f) != ".meta"))
                File.Copy(file, targetDir + Path.GetFileName(file), overwrite);
            Debug.Log($"Copied {files.Length} files from {sourceDir} to {targetDir}");
        }

        protected static void ReimportObjects<T>(string textAssetPath, string assetPath) where T : TextImportObject
            => ReimportObjects<T>(textAssetPath, assetPath, "");

        protected static void ReimportObjects<T>(string textAssetPath, string assetPath, string nameAppendix)
            where T : TextImportObject
        {
            int updated = 0, created = 0;
            var textFiles = AssetUtil.LoadAssetsInFolder<TextAsset>(textAssetPath);
            var existingUnits = AssetUtil.LoadAssetsInFolder<T>(assetPath);
            foreach (var text in textFiles)
            {
                var unit = existingUnits.Find(u => u.name == $"{text.name}{nameAppendix}");
                if (unit != null)
                {
                    updated++;
                    unit.ImportTextFile(text);
                }
                else
                {
                    created++;
                    CreateAsset<T>(text.name + nameAppendix, assetPath)
                        .ImportTextFile(text);
                }
            }

            Debug.Log($"Created {created} and updated {updated} {typeof(T)}-Assets at path {assetPath}");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static T CreateAsset<T>(string name, string path)
            where T : TextImportObject
        {
            var asset = CreateInstance<T>();
            asset.name = name;
            AssetDatabase.CreateAsset(asset, $"{path}{asset.name}.asset");
            return asset;
        }
    }
}