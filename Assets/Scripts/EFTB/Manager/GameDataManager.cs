using JumboJumps.EFTB.Utilities;
using UnityEngine;

namespace JumboJumps.EFTB.Manager
{
    public class GameDataManager : MonoBehaviour
    {
        [SerializeField]
        private TextAsset localLevelSegmentData;

        public void Initialize()
        {
            LoadGameData();

            GameContext.Instance.Add(this);
        }

        public void Dispose()
        {
            GameContext.Instance.Remove(this);
        }

        public void LoadGameData()
        {
            OnGameDataLoaded(localLevelSegmentData.text);
        }

        public void OnGameDataLoaded(string jsonText)
        {
            ParseAndApplyGameDataFromJson(jsonText);
        }

        public void ParseAndApplyGameDataFromJson(string jsonText)
        {

        }
    }
}
