using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

namespace DataAnalysis
{
    public class Utilities
    {
        /// <summary>
        /// In this particular case, I would like to parse all data at once
        /// </summary>
        /// <param name="filePath"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> ReadIdfFileByLines<T>(string filePath) where T : class
        {
            if (!File.Exists(filePath))
            {
                Debug.LogError("JSON file not found!");
                return null;
            }

            var result = new List<T>();
            T extractDatum;
            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                extractDatum = JsonUtility.FromJson<T>(line);
                result.Add(extractDatum);
            }

            return result;
        }
    }
}