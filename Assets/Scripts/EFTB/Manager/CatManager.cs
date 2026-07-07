using JumboJumps.EFTB.Constant.Gameplay;
using JumboJumps.EFTB.GI;
using JumboJumps.EFTB.Interface;
using JumboJumps.EFTB.State;
using JumboJumps.EFTB.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace JumboJumps.EFTB.Manager
{
    public class CatManager
    {
        public List<ICatStateController> Cats {  get; private set; }
        private Dictionary<GICat, ICatStateController> catControllers;

        public CatManager()
        {
            Cats = new List<ICatStateController>();
        }

        public void Intialize(IEnumerable<GICat> sceneCats, Transform playerTarget)
        {
            catControllers = new Dictionary<GICat, ICatStateController>();

            if (sceneCats != null)
            {
                foreach(var giCat in sceneCats)
                {
                    if (giCat == null) continue;

                    float currentX = giCat.transform.position.x;
                    if (currentX > -ConstGameplay.Cat.CatSpawnThreshold && currentX < ConstGameplay.Cat.CatSpawnThreshold)
                    {
                        float targetX = (currentX <= 0f) 
                            ? ConstGameplay.Cat.CatLeftLaneSpawnPosition 
                            : ConstGameplay.Cat.CatRightLaneSpawnPosition;
                        
                        giCat.transform.position = new Vector3(targetX, giCat.transform.position.y, giCat.transform.position.z);
                        
                        var lookDir = (targetX < 0f) ? CatSightDirection.Right : CatSightDirection.Left;
                        giCat.SetDirection(lookDir);
                    }

                    var controller = giCat.BuildStateController(playerTarget);
                    BaseStateController baseController = controller as BaseStateController;
                    if (baseController != null)
                    {
                        baseController.Initialize();
                        baseController.StartStateController();
                        Cats.Add(controller);
                        catControllers.Add(giCat, controller);
                    }
                }
            }
            GameContext.Instance.Add(this);
        }

        public void RegisterDynamicCat(GICat giCat, Transform playerTarget)
        {
            if (giCat == null || playerTarget == null) return;
            if (catControllers.ContainsKey(giCat)) return;

            var controller = giCat.BuildStateController(playerTarget);
            BaseStateController baseController = controller as BaseStateController;
            if (baseController != null)
            {
                baseController.Initialize();
                baseController.StartStateController();
                Cats.Add(controller);
                catControllers.Add(giCat, controller);
                DebugLogHelper.Log($"[CatManager] Dynamically registered cat: {giCat.name}");
            }
        }

        public void DeregisterCat(GICat giCat)
        {
            if (giCat == null || catControllers == null) return;
            if (catControllers.TryGetValue(giCat, out var controller))
            {
                controller.Dispose();
                Cats.Remove(controller);
                catControllers.Remove(giCat);
                DebugLogHelper.Log($"[CatManager] Dynamically deregistered and disposed cat: {giCat.name}");
            }
        }

        public void ReturnCat(GICat giCat)
        {
            if (giCat == null) return;

            GameObject giCatGo = giCat.gameObject;

            GISegment giSegment = giCatGo.transform.parent?.GetComponent<GISegment>();
            giSegment?.DeregisterSpawnedObject(giCatGo);

            SceneObjectContext.Instance?.Deregister(giCat);
            DeregisterCat(giCat);

            ObjectPoolManager poolManager = GameContext.Instance.Get<ObjectPoolManager>();
            poolManager?.Recycle(giCatGo);
        }

        public void Dispose()
        {
            if (Cats != null)
            {
                foreach (var cat in Cats)
                {
                    cat?.Dispose();
                }
                Cats.Clear();
            }
            catControllers?.Clear();
            GameContext.Instance.Remove(this);
        }

        public void UpdateLogic(float deltaTime)
        {
            if (Cats == null) return;

            for (int i = Cats.Count - 1; i >= 0; i--)
            {
                if (i < Cats.Count && Cats[i] != null)
                {
                    Cats[i].UpdateLogic(deltaTime);
                }
            }
        }
    }
}
