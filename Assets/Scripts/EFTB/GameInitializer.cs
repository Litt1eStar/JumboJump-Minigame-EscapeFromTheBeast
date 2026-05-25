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
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Initialize();
        }

        private void Update()
        {
            playerManager.UpdateLogic(Time.deltaTime);
            input2DManager.UpdateLogic(Time.deltaTime);
        }
        private void Initialize()
        {
            input2DManager.Initialize();

            playerManager = new PlayerManager();
            playerManager.Initialize();

            catManager = new CatManager();
            catManager.Intialize();
        }
    }
}
