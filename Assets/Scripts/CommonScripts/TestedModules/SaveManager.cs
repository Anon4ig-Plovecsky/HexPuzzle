using UnityEngine.ResourceManagement.AsyncOperations;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.AddressableAssets;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Xml;
using System.IO;
using System;

namespace CommonScripts.TestedModules
{
    public class SaveManager
    {
        private TextAsset _textAsset;
        
        private static readonly string StrSaveFilePath = Application.persistentDataPath;

        /// <summary>
        /// Generates the path to the save file
        /// </summary>
        /// <param name="fileName">File name without format</param>
        /// <returns>Absolute path to the save file</returns>
        private static string GetStrPath(string fileName) =>
            $"{StrSaveFilePath}/{fileName}{CommonKeys.SavedFormatFile}";
            
        /// <summary>
        /// Reads all saved data from a save file
        /// </summary>
        /// <returns>List of all results data or null</returns>
        /// <typeparam name="T">Serialized object type</typeparam>
        public static List<T> ReadData<T>() where T : ISerializable
        {
            var strPath = GetStrPath(typeof(T).Name);
            if (!File.Exists(strPath))
                return null;

            var binaryFormatter = new BinaryFormatter();
            var fileStream = new FileStream(strPath, FileMode.Open);
            try
            {
                var listSavedResults = binaryFormatter.Deserialize(fileStream) as List<T>;
                fileStream.Close();
                
                return listSavedResults;
            }
            catch (Exception e)
            {
                fileStream.Close();
                
                Console.WriteLine(e.Message);
                return null;
            }
        }
        
        /// <summary>
        /// Reads all stored data from a file and returns data at the specified level
        /// </summary>
        /// <param name="lvlNumber">Level number</param>
        /// <returns>Data about lvlNumber level or null</returns>
        public static SavedResults ReadData(int lvlNumber)
        {
            var strPath = GetStrPath(nameof(SavedResults));
            if (!File.Exists(strPath))
                return null;

            var savedResultOfLevel = new SavedResults(0, 0, 0);
            
            var binaryFormatter = new BinaryFormatter();
            var fileStream = new FileStream(strPath, FileMode.Open);
            try
            {
                if (binaryFormatter.Deserialize(fileStream) is List<SavedResults> listSavedResults)
                    savedResultOfLevel = listSavedResults.First(savedResult => savedResult.LvlNumber == lvlNumber);
                fileStream.Close();
            }
            catch (Exception e)
            {
                fileStream.Close();
                
                Console.WriteLine(e.Message);
                return null;
            }

            return savedResultOfLevel.LvlNumber == 0 ? null : savedResultOfLevel;
        }

        /// <summary>
        /// Saves data to a file in binary form
        /// </summary>
        /// <param name="listSavedResults">Game results data about level</param>
        /// <returns>true - If the data was successfully saved, otherwise - false</returns>
        /// <typeparam name="T">Serialized object type</typeparam>
        public static bool WriteData<T>(List<T> listSavedResults) where T : ISerializable
        {
            var strPath = GetStrPath(typeof(T).Name);
            var binaryFormatter = new BinaryFormatter();
            var fileStream = new FileStream(strPath, FileMode.Create);
            try
            {
                binaryFormatter.Serialize(fileStream, listSavedResults);
                fileStream.Close();
                return true;
            }
            catch (Exception e)
            {
                fileStream.Close();
                
                Console.WriteLine(e.Message);
                return false;
            }
        }

        /// <summary>
        /// Adds the specified data to all saves, replacing previous data of that level (if any)
        /// </summary>
        /// <param name="savedResult"></param>
        /// <returns>true - If the data was successfully saved, otherwise - false</returns>
        public static bool WriteData(SavedResults savedResult)
        {
            try
            {
                var listSavedResults = ReadData<SavedResults>();
                listSavedResults ??= new List<SavedResults>();

                // Replacing the previous save
                var lvlNumber = savedResult.LvlNumber;
                listSavedResults.RemoveAll(sr => sr.LvlNumber == lvlNumber);
                listSavedResults.Add(savedResult);

                var bRes = WriteData(listSavedResults);
                return bRes;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        
        /// <summary>
        /// Gets the name of the picture from the file name of their XML file
        /// </summary>
        /// <returns>true - if it was possible to get the name; false - if unsuccessful</returns>
        public string ReadNamePainting(string strPaintingName)
        {
            var xmlDocument = new XmlDocument();
            
            if(_textAsset is null)
            {
                var asyncOperationHandle = Addressables.LoadAssetAsync<TextAsset>(CommonKeys.Addressable.Paintings);
                asyncOperationHandle.Completed += delegate
                {
                    if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
                        _textAsset = asyncOperationHandle.Result;
                    else
                        Debug.Log("Failed to read xml file");
                };
                asyncOperationHandle.WaitForCompletion();
            }
            if (_textAsset is null)
                return "";
            xmlDocument.LoadXml(_textAsset.text);

            var strFoundName = "";
            var xmlPaintingList = xmlDocument.GetElementsByTagName(CommonKeys.Names.Painting);
            
            for (var i = 0; i < xmlPaintingList.Count; i++)
            {
                var xmlPainting = xmlPaintingList[i];
                var xmlName = xmlPainting[CommonKeys.Names.PaintingName];
                var xmlRu = xmlPainting[CommonKeys.Names.PaintingRu];

                if(xmlName is null || xmlRu is null)
                    continue;
                var strName = xmlName.InnerText;
                var strRu = xmlRu.InnerText;
                if (!strName.Equals(strPaintingName)) 
                    continue;
                strFoundName = strRu;
                break;
            }
            
            return strFoundName;
        }
    }

    /// <summary>
    /// Data for saving the results of completing levels
    /// </summary>
    [Serializable]
    public class SavedResults : ISerializable
    {
        public int LvlNumber { get; private init; }
        public float TimeBest { get; private set; }

        private float _timeRecent;
        public float TimeRecent
        {
            get => _timeRecent;
            set
            {
                _timeRecent = value;
                if (_timeRecent < TimeBest)
                    TimeBest = _timeRecent;
            }
        }

        public SavedResults(int lvlNumber, float timeBest, float timeRecent)
        {
            LvlNumber = lvlNumber;
            TimeBest = timeBest;
            TimeRecent = timeRecent;
        }
        public SavedResults(SerializationInfo info, StreamingContext context)
        {
            LvlNumber = info.GetInt32(nameof(LvlNumber));
            TimeBest = info.GetSingle(nameof(TimeBest));
            TimeRecent = info.GetSingle(nameof(TimeRecent));
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(LvlNumber), LvlNumber);
            info.AddValue(nameof(TimeBest), TimeBest);
            info.AddValue(nameof(TimeRecent), TimeRecent);
        }
    }

    /// <summary>
    /// Number of levels completed by the player
    /// </summary>
    [Serializable]
    public class CompletedLevels : ISerializable
    {
        public int NumLevels { get; private init; }

        public CompletedLevels(int numLevels) =>
            NumLevels = numLevels;
        public CompletedLevels(SerializationInfo info, StreamingContext context) =>
            NumLevels = info.GetInt32(nameof(NumLevels));
        public void GetObjectData(SerializationInfo info, StreamingContext context) =>
            info.AddValue(nameof(NumLevels), NumLevels);
    }
}