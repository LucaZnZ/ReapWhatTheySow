using System;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveToXML<T>(T data, string filepath)
    {
        if(filepath == string.Empty || data == null)
        {
            Debug.LogError("SaveSystem::No data or filepath received for saving xml");
            return;
        }

        Debug.Log("SaveSystem::Saving xml file: " + filepath);

        var xmlSerializer = new XmlSerializer(typeof(T));   // create the serializer

        using (FileStream stream = new FileStream(filepath, FileMode.Create, FileAccess.Write))     // open filestream
        {
            try
            {
                xmlSerializer.Serialize(stream, data);
            }
            catch (Exception e)
            {
                Debug.LogError($"SaveSystem::Failed saving xml file {filepath} /n{e.Message}");
            }
        }
    }
    public static T LoadFromXML<T>(string filepath) where T : class
    {
        if (filepath == string.Empty)
        {
            Debug.LogError("SaveSystem::No filepath received for loading xml");
            return default(T);
        }

        Debug.Log("SaveSystem::loading xml file: " + filepath);

        var xmlSerializer = new XmlSerializer(typeof(T));   // create the serializer
        T data;

        using (FileStream stream = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Read))     // open filestream
        {
            try
            {
                data = xmlSerializer.Deserialize(stream) as T;
                return data;
            }
            catch (Exception e)
            {
                Debug.LogError($"SaveSystem::Failed loading xml file {filepath} /n{e.Message}");
                return default(T);
            }
        }
    }

    public static void SaveBinary<T>(T data, string filepath)
	{
        if (filepath == string.Empty || data == null)
        {
            Debug.LogError("SaveSystem::No data or filepath received for saving binary");
            return;
        }

        Debug.Log("SaveSystem::Saving binary file: " + filepath);

        var xmlSerializer = new BinaryFormatter();   // create the serializer

        using (FileStream stream = new FileStream(filepath, FileMode.Create, FileAccess.Write))     // open filestream
        {
            try
            {
                xmlSerializer.Serialize(stream, data);
            }
            catch (Exception e)
            {
                Debug.LogError($"SaveSystem::Failed saving binary file of type {typeof(T)} at {filepath} /n{e.Message}");
            }
        }
    }

    public static T LoadBinary<T>(string filepath)
	{
        if (filepath == string.Empty)
        {
            Debug.LogError("SaveSystem::No filepath received for loading binary");
            return default(T);
        }

        Debug.Log("SaveSystem::Loading binary file from " + filepath);

        T data;
        BinaryFormatter formatter = new BinaryFormatter();

        using (FileStream stream = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Read))
        {
            try
            {
                data = (T)formatter.Deserialize(stream);
                return data;
            }
            catch (Exception e)
            {
                Debug.LogError($"SaveSystem::Failed loading binary file of type {typeof(T)} at {filepath} /n{e.Message}");
                return default(T);
            }
        }
    }
}
