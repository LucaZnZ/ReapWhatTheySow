using UnityEngine;

namespace ZnZUtil
{
    public abstract class TextImportObject : ScriptableObject
    {
        [SerializeField] private TextAsset textFile;

        protected virtual void OnValidate()
        {
            if (textFile != null)
                ImportTextFile(textFile);
        }

        public void ImportTextFile(TextAsset asset)
        {
            textFile = asset;
            OnImport(asset);
        }

        protected abstract void OnImport(TextAsset asset);
    }

    public abstract class TextImportBehaviour : MonoBehaviour
    {
        [SerializeField] private TextAsset textFile;

        protected virtual void OnValidate()
        {
            if (textFile != null)
                ImportTextFile(textFile);
        }

        public void ImportTextFile(TextAsset asset)
        {
            textFile = asset;
            OnImport(asset);
        }

        protected abstract void OnImport(TextAsset asset);
    }
}