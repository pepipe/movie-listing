﻿using System.IO;
using System.Linq;
using UnityEngine;

namespace Parser {
    public class CsvParser : IParser {
        // private void Start() {
        //     Stopwatch s = new Stopwatch();
        //     s.Start();
        //     for (int i = 0; i < 100; ++i) {
        //         Parse();
        //     }
        //     s.Stop();
        //     Debug.Log("TIME: " + (s.ElapsedMilliseconds / 1000f).ToString() + " secs.");
        // }
        
        public IFileData Parse(string pathToFile, string csvFileName) {
            var lines = File.ReadLines(Path.Combine(csvFileName));
            return new CsvData(lines
                .Select(l => l.Split(',')
                    .ToList())
                .ToList());
        }
        
        public IFileData ParseFromStreamingAssets(string csvFileName) {
            return Parse(Application.streamingAssetsPath, csvFileName);
        }

        public IFileData ParseFromResources(string csvFileName) {
            var file = (TextAsset) Resources.Load(csvFileName);
            return new CsvData(file.text.Split('\n')
                                        .Select(l => l.Split(',')
                                        .ToList())
                                        .ToList());
        }
        
    }
}