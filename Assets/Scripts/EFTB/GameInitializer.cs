using Assets.Scripts.EFTB.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.EFTB
{
    public class GameInitializer : MonoBehaviour
    {
        [SerializeField]
        private Input2DManager input2DManager;

        private PlayerManager playerManager;
        private CatManager catManager;

        [Header("Sleepy Cat Behaviour Config")]
        [SerializeField]
        private float TIME_TILL_AWAKE;
        [SerializeField]
        private float TIME_TO_ALERT;
        [SerializeField]
        private float TIME_TO_CATCH;
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
        private void Initialize()
        {
            input2DManager.Initialize();

            playerManager = new PlayerManager();
            playerManager.Initialize();

            SleepyCatBehaviourConfig sleepyCatConfig = new SleepyCatBehaviourConfig(
                TIME_TILL_AWAKE,
                TIME_TO_ALERT,
                TIME_TO_CATCH
                );

            catManager = new CatManager();
            catManager.Intialize(sleepyCatConfig);
        }
    }
}
