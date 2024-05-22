using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;
using System;

namespace CommonScripts.TestedModules
{
    public class SaveManager
    {
        private static readonly string StrSaveFilePath = Application.persistentDataPath + CommonKeys.SavedResultsFile;
        
        /// <summary>
        /// Reads all saved data from a save file
        /// </summary>
        /// <returns>List of all results data or null</returns>
        public static List<SavedResults> ReadData()
        {
            if (!File.Exists(StrSaveFilePath))
                return null;

            var binaryFormatter = new BinaryFormatter();
            var fileStream = new FileStream(StrSaveFilePath, FileMode.Open);
            var listSavedResults = binaryFormatter.Deserialize(fileStream) as List<SavedResults>;
            fileStream.Close();

            return listSavedResults;
        }
        
        /// <summary>
        /// Reads all stored data from a file and returns data at the specified level
        /// </summary>
        /// <param name="lvlNumber">Level number</param>
        /// <returns>Data about lvlNumber level or null</returns>
        public static SavedResults ReadData(int lvlNumber)
        {
            if (!File.Exists(StrSaveFilePath))
                return null;

            var savedResultOfLevel = new SavedResults(0, 0, 0);

            var binaryFormatter = new BinaryFormatter();
            var fileStream = new FileStream(StrSaveFilePath, FileMode.Open);

            if (binaryFormatter.Deserialize(fileStream) is List<SavedResults> listSavedResults)
                savedResultOfLevel = listSavedResults.First(savedResult => savedResult.LvlNumber == lvlNumber);
            fileStream.Close();

            return savedResultOfLevel.LvlNumber == 0 ? null : savedResultOfLevel;
        }

        /// <summary>
        /// Saves data to a file in binary form
        /// </summary>
        /// <param name="listSavedResults">Game results data about level</param>
        /// <returns>true - If the data was successfully saved, otherwise - false</returns>
        public static bool WriteData(List<SavedResults> listSavedResults)
        {
            var binaryFormatter = new BinaryFormatter();
            try
            {
                var fileStream = new FileStream(StrSaveFilePath, FileMode.Create);
                binaryFormatter.Serialize(fileStream, listSavedResults);
                fileStream.Close();
                return true;
            }
            catch (IOException e)
            {
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
                var listSavedResults = ReadData();
                listSavedResults ??= new List<SavedResults>();

                // Replacing the previous save
                var lvlNumber = savedResult.LvlNumber;
                listSavedResults.RemoveAll(sr => sr.LvlNumber == lvlNumber);
                listSavedResults.Add(savedResult);

                var bRes = WriteData(listSavedResults);
                return bRes;
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }

    [Serializable]
    public class SavedResults
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
                if (TimeBest < _timeRecent)
                    TimeBest = _timeRecent;
            }
        }

        public SavedResults(int lvlNumber, float timeBest, float timeRecent)
        {
            LvlNumber = lvlNumber;
            TimeBest = timeBest;
            TimeRecent = timeRecent;
        }
    }
}