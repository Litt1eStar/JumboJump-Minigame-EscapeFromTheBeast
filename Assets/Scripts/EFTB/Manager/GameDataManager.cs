using System;
using System.Collections.Generic;
using UnityEngine;
using JumboJumps.EFTB.Model;
using JumboJumps.EFTB.Utilities;
using JumboJumps.EFTB.GameData;

namespace JumboJumps.EFTB.Manager
{
    public class GameDataManager : MonoBehaviour
    {
        [Header("Assets")]
        [SerializeField] private PrefabRegistrySO prefabRegistry;
        [SerializeField] private TextAsset localGameData;

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

        [Serializable]
        private class LevelSegmentListWrapper
        {
            public List<LevelGeneratorData.LevelSegmentData> level_segments;
        }

        private void ParseAndApplyGameDataFromJson(string jsonText)
        {
            try
            {
                levelSegmentData.Clear();
                var wrapper = JsonUtility.FromJson<LevelSegmentListWrapper>(jsonText);
                if (wrapper != null && wrapper.level_segments != null)
                {
                    foreach (var segment in wrapper.level_segments)
                    {
                        if (segment != null)
                        {
                            levelSegmentData[segment.id] = segment;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DebugLogHelper.LogError($"[{GetType().Name}] Failed to parse master JSON using JsonUtility: {ex.Message}");
            }
        }

        public GameObject GetPrefab(string key)
        {
            return prefabRegistry != null ? prefabRegistry.GetPrefab(key) : null;
        }
    }
}
