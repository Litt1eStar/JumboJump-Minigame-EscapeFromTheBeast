using JumboJumps.EFTB.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;


namespace JumboJumps.EFTB.GameData
{
    public class CSVParserToJSON : EditorWindow
    {
        private TextAsset csvFile;

        [MenuItem("Tools/EFTB/CSVtoJSON")]
        public static void ShowWindow()
        {
            GetWindow<CSVParserToJSON>("CSV Parser Window");
        }

        private void OnGUI()
        {
            GUILayout.Label("CSV to JSON Parser", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            csvFile = (TextAsset)EditorGUILayout.ObjectField("CSV TextAsset", csvFile, typeof(TextAsset), false);

            EditorGUILayout.Space();

            if (GUILayout.Button("Parse CSV to JSON"))
            {
                if (csvFile == null)
                {
                    EditorUtility.DisplayDialog("Error", "Please drag a valid CSV TextAsset file!", "OK");
                    return;
                }

                ParseCSV(csvFile.text);
            }
        }

        private void ParseCSV(string rawText)
        {
            try
            {
                var lines = rawText.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                List<string> activeLines = new List<string>();
                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line.Replace(",", "").Trim()))
                    {
                        continue;
                    }
                    activeLines.Add(line);
                }

                if (activeLines.Count < 2)
                {
                    Debug.LogError("[CSVParserToJSON] CSV file has insufficient data.");
                    EditorUtility.DisplayDialog("Error", "CSV file has insufficient data.", "OK");
                    return;
                }

                string headerLine = activeLines[0];
                string[] headers = headerLine.Split(',').Select(h => h.Trim()).ToArray();
                int idIdx = Array.IndexOf(headers, "ID");
                int nameIdx = Array.IndexOf(headers, "Name");
                int segmentPrefabIdx = Array.IndexOf(headers, "Segment-Prefab");
                int segmentHeightIdx = Array.IndexOf(headers, "Segment-Height");
                int difficultyIdx = Array.IndexOf(headers, "Difficulty");
                int segmentTypeIdx = Array.IndexOf(headers, "Segment-Type");
                int laneIdx = Array.IndexOf(headers, "LaneIndex");
                int yOffsetIdx = Array.IndexOf(headers, "YOffset");
                int prefabIdx = Array.IndexOf(headers, "Prefab");
                int speedIdx = Array.IndexOf(headers, "Speed");

                if (idIdx == -1 || segmentTypeIdx == -1 || prefabIdx == -1)
                {
                    Debug.LogError("[CSVParserToJSON] CSV headers are missing required columns (ID, Segment-Type, Prefab).");
                    EditorUtility.DisplayDialog("Error", "CSV headers are missing required columns (ID, Segment-Type, Prefab).", "OK");
                    return;
                }

                var segmentsDict = new Dictionary<int, LevelGeneratorData.LevelSegmentData>();

                for (int i = 1; i < activeLines.Count; i++)
                {
                    string[] cols = activeLines[i].Split(',').Select(c => c.Trim()).ToArray();
                    if (cols.Length <= Math.Max(idIdx, prefabIdx)) continue;

                    if (!int.TryParse(cols[idIdx], out int id))
                    {
                        continue;
                    }

                    if (!segmentsDict.TryGetValue(id, out var segment))
                    {
                        segment = new LevelGeneratorData.LevelSegmentData
                        {
                            id = id,
                            segmentPrefabName = segmentPrefabIdx != -1 && cols.Length > segmentPrefabIdx ? cols[segmentPrefabIdx] : "",
                            segmentHeight = segmentHeightIdx != -1 && cols.Length > segmentHeightIdx && float.TryParse(cols[segmentHeightIdx], out float h) ? h : 20f,
                            difficulty = difficultyIdx != -1 && cols.Length > difficultyIdx ? cols[difficultyIdx] : "Easy",
                            prePlacedObjectDatas = new List<LevelGeneratorData.LaneObjectData>(),
                            laneEventDatas = new List<LevelGeneratorData.LaneEventData>()
                        };
                        segmentsDict.Add(id, segment);
                    }

                    string type = cols[segmentTypeIdx];
                    if (type.Equals("Object", StringComparison.OrdinalIgnoreCase))
                    {
                        segment.prePlacedObjectDatas.Add(new LevelGeneratorData.LaneObjectData
                        {
                            laneIndex = laneIdx != -1 && cols.Length > laneIdx && int.TryParse(cols[laneIdx], out int l) ? l : 0,
                            yOffset = yOffsetIdx != -1 && cols.Length > yOffsetIdx && float.TryParse(cols[yOffsetIdx], out float y) ? y : 0f,
                            prefabName = cols[prefabIdx]
                        });
                    }
                    else if (type.Equals("Event", StringComparison.OrdinalIgnoreCase))
                    {
                        segment.laneEventDatas.Add(new LevelGeneratorData.LaneEventData
                        {
                            targetLaneIndex = laneIdx != -1 && cols.Length > laneIdx && int.TryParse(cols[laneIdx], out int l) ? l : 0,
                            triggerYOffset = yOffsetIdx != -1 && cols.Length > yOffsetIdx && float.TryParse(cols[yOffsetIdx], out float y) ? y : 0f,
                            prefabName = cols[prefabIdx],
                            speed = speedIdx != -1 && cols.Length > speedIdx && float.TryParse(cols[speedIdx], out float s) ? s : 0f
                        });
                    }
                }

                var wrapper = new LevelSegmentListWrapper
                {
                    level_segments = segmentsDict.Values.ToList()
                };

                string jsonText = JsonUtility.ToJson(wrapper, true);

                string outFolder = Path.Combine(Application.dataPath, "Resources", "LevelData");
                if (!Directory.Exists(outFolder))
                {
                    Directory.CreateDirectory(outFolder);
                }

                string outPath = Path.Combine(outFolder, "LocalGameData.json");
                File.WriteAllText(outPath, jsonText, Encoding.UTF8);

                AssetDatabase.Refresh();
                EditorUtility.DisplayDialog("Success", $"Successfully parsed CSV and wrote JSON to:\n{outPath}", "OK");
            }
            catch (Exception ex)
            {
                EditorUtility.DisplayDialog("Exception", $"Exception occurred during parsing: {ex.Message}", "OK");
            }
        }

        [Serializable]
        private class LevelSegmentListWrapper
        {
            public List<LevelGeneratorData.LevelSegmentData> level_segments;
        }
    }
}
