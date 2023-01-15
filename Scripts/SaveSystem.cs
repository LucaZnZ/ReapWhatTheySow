using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using UnityEngine;
// TODO fix this, it has issues on build time
// using Unity.VisualScripting.YamlDotNet.Serialization;
// using Unity.VisualScripting.YamlDotNet.Serialization.NamingConventions;

namespace ZnZUtil
{
    public static class SaveSystem
    {
        public static void SaveToXML<T>(T data, string filepath)
            => Save(new XmlSaveSerializer<T>(), filepath, data);

        public static T LoadFromXML<T>(string filepath) where T : class
            => Load(new XmlSaveSerializer<T>(), filepath);

        public static void SaveBinary<T>(T data, string filepath)
            => Save(new BinarySaveSerializer<T>(), filepath, data);

        public static T LoadBinary<T>(string filepath)
            => Load(new BinarySaveSerializer<T>(), filepath);

        // public static void SaveYaml<T>(T data, string filepath)
        //     => Save(new YamlSaveSerializer<T>(), filepath, data);
        //
        // public static T LoadYaml<T>(string filepath)
        //     => Load(new YamlSaveSerializer<T>(), filepath);


        private static void Save<T>(ISaveSerializer<T> serializer, string filepath, T data)
        {
            if (filepath == string.Empty || data == null)
            {
                Debug.LogError("SaveSystem::No data or filepath received for saving binary");
                return;
            }

            Debug.Log("SaveSystem::Saving binary file: " + filepath);

            using FileStream stream = new FileStream(filepath, FileMode.Create, FileAccess.Write);
            try
            {
                serializer.Save(stream, data);
            }
            catch (Exception e)
            {
                Debug.LogError(
                    $"SaveSystem::Failed saving binary file of type {typeof(T)} at {filepath} /n{e.Message}");
            }
        }

        private static T Load<T>(ISaveSerializer<T> serializer, string filepath)
        {
            if (filepath == string.Empty)
            {
                Debug.LogError("SaveSystem::No filepath received for loading binary");
                return default(T);
            }

            Debug.Log("SaveSystem::Loading binary file from " + filepath);

            using FileStream stream = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Read);
            try
            {
                return serializer.Load(stream);
            }
            catch (Exception e)
            {
                Debug.LogError(
                    $"SaveSystem::Failed loading file of type {typeof(T)} at {filepath} /n{e.Message}");
                return default(T);
            }
        }

        private interface ISaveSerializer<T>
        {
            public T Load(Stream stream);
            public void Save(Stream stream, T data);
        }

        private class BinarySaveSerializer<T> : ISaveSerializer<T>
        {
            private readonly BinaryFormatter formatter = new BinaryFormatter();

            public T Load(Stream stream)
                => (T) formatter.Deserialize(stream);

            public void Save(Stream stream, T data)
                => formatter.Serialize(stream, data);
        }

        private class XmlSaveSerializer<T> : ISaveSerializer<T>
        {
            private readonly XmlSerializer formatter = new XmlSerializer(typeof(T));

            public T Load(Stream stream)
                => (T) formatter.Deserialize(stream);

            public void Save(Stream stream, T data)
                => formatter.Serialize(stream, data);
        }

        // private class YamlSaveSerializer<T> : ISaveSerializer<T>
        // {
        //     public T Load(Stream stream)
        //     {
        //         var formatter = new DeserializerBuilder()
        //             .WithNamingConvention(new CamelCaseNamingConvention()).Build();
        //         var reader = new StreamReader(stream);
        //         return formatter.Deserialize<T>(reader);
        //     }
        //
        //     public void Save(Stream stream, T data)
        //     {
        //         var formatter = new SerializerBuilder()
        //             .WithNamingConvention(new CamelCaseNamingConvention()).Build();
        //         var writer = new StreamWriter(stream);
        //         formatter.Serialize(writer, data);
        //     }
        // }
    }
}