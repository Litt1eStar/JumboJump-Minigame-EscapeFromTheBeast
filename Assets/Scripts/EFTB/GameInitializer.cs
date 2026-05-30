using JumboJumps.EFTB.GI;
using JumboJumps.EFTB.Manager;
using JumboJumps.EFTB.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace JumboJumps.EFTB
{
    public class GameInitializer : MonoBehaviour
    {
        [SerializeField]
        private Input2DManager input2DManager;

        private PlayerManager playerManager;
        private CatManager catManager;

        [SerializeField]
        private Transform playerTransform;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Initialize();
        }

        private void Update()
        {
            playerManager.UpdateLogic(Time.deltaTime);
            input2DManager.UpdateLogic(Time.deltaTime);
            catManager.UpdateLogic(Time.deltaTime);
        }

        private void OnDestroy()
        {
            Dispose();
        }
        private void Initialize()
        {
            input2DManager.Initialize();

            playerManager = new PlayerManager();
            playerManager.Initialize();

            catManager = new CatManager();
            IEnumerable<GICat> sceneCats = SceneObjectContext.Instance.GetAll<GICat>();
            catManager.Intialize(sceneCats, playerTransform);
        }

        private void Dispose()
        {
            playerManager.Dispose();
            playerManager = null;
            
            catManager.Dispose();
            catManager = null;
        }
    }
}
