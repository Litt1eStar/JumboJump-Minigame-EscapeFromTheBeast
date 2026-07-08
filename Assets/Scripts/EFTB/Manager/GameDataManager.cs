using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using JumboJumps.EFTB.Model;
using JumboJumps.EFTB.Utilities;
using JumboJumps.EFTB.GameData;
using Newtonsoft.Json;

namespace JumboJumps.EFTB.Manager
{
    public class GameDataManager : MonoBehaviour
    {
        [Header("Assets")]
        [SerializeField] private PrefabRegistrySO prefabRegistry;
        [SerializeField] private TextAsset localGameData;

        [GameData("level_segments")]
        private List<LevelGeneratorData.LevelSegmentData> levelSegmentDataList = new();

        private Dictionary<int, LevelGeneratorData.LevelSegmentData> levelSegmentData = new();
        public Dictionary<int, LevelGeneratorData.LevelSegmentData> LevelSegmentData => levelSegmentData;

        public bool IsDataLoaded { get; private set; }

        public void Initialize()
        {
            IsDataLoaded = false;
            LoadGameData();
            GameContext.Instance.Add(this);
        }

        public void Dispose()
        {
            GameContext.Instance.Remove(this);
            IsDataLoaded = false;
        }

        private void LoadGameData()
        {
            if (localGameData != null)
            {
                ParseAndApplyGameDataFromJson(localGameData.text);
                IsDataLoaded = true;
                DebugLogHelper.Log($"[{GetType().Name}] Game data loaded successfully.");
            }
            else
            {
                DebugLogHelper.LogError($"[{GetType().Name}] LocalGameData TextAsset is not assigned!");
            }
        }

        private void ParseAndApplyGameDataFromJson(string jsonText)
        {
            try
            {
                var gameDataDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonText);
                if (gameDataDictionary == null) return;

                var gameDataFields = typeof(GameDataManager).GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                                                            .Where(x => Attribute.IsDefined(x, typeof(GameDataAttribute)))
                                                            .ToList();

                foreach (var item in gameDataFields)
                {
                    var attribute = (GameDataAttribute)Attribute.GetCustomAttribute(item, typeof(GameDataAttribute));
                    if (attribute == null) continue;

                    if (!gameDataDictionary.TryGetValue(attribute.JsonElement, out var fieldGameDataObj))
                    {
                        DebugLogHelper.LogWarning($"[{GetType().Name}] GameData element '{attribute.JsonElement}' not found in JSON.");
                        continue;
                    }

                    var fieldType = item.FieldType;
                    var itemValue = JsonConvert.DeserializeObject(fieldGameDataObj.ToString(), fieldType);
                    item.SetValue(this, itemValue);
                }

                ProcessGameDataLookup();
            }
            catch (Exception ex)
            {
                DebugLogHelper.LogError($"[{GetType().Name}] Failed to parse master JSON: {ex.Message}");
            }
        }

        private void ProcessGameDataLookup()
        {
            levelSegmentData.Clear();
            if (levelSegmentDataList != null)
            {
                foreach (var segment in levelSegmentDataList)
                {
                    if (segment != null)
                    {
                        levelSegmentData[segment.Id] = segment;
                    }
                }
            }
        }

        public GameObject GetPrefab(string key)
        {
            return prefabRegistry != null ? prefabRegistry.GetPrefab(key) : null;
        }

        public bool HasPrefab(string key)
        {
            return prefabRegistry != null && prefabRegistry.HasPrefab(key);
        }
    }
}
