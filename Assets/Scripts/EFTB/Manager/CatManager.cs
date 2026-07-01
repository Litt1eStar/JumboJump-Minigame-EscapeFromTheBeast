using JumboJumps.EFTB.GI;
using JumboJumps.EFTB.Interface;
using JumboJumps.EFTB.State;
using JumboJumps.EFTB.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace JumboJumps.EFTB.Manager
{
    public class CatManager
    {
        public List<ICatStateController> Cats {  get; private set; }
        private Dictionary<GICat, ICatStateController> catControllers = new();

        public void Intialize(IEnumerable<GICat> sceneCats, Transform playerTarget)
        {
            Cats = new List<ICatStateController>();
            catControllers = new Dictionary<GICat, ICatStateController>();
            
            foreach(var giCat in sceneCats)
            {
                if (giCat == null) continue;
                var controller = giCat.BuildStateController(playerTarget);
                BaseStateController baseController = controller as BaseStateController;
                baseController.Initialize();
                baseController.StartStateController();
                Cats.Add(controller);
                catControllers.Add(giCat, controller);
            }
            GameContext.Instance.Add(this);
        }

        public void RegisterDynamicCat(GICat giCat, Transform playerTarget)
        {
            if (giCat == null || playerTarget == null) return;
            if (catControllers.ContainsKey(giCat)) return;

            var controller = giCat.BuildStateController(playerTarget);
            BaseStateController baseController = controller as BaseStateController;
            baseController.Initialize();
            baseController.StartStateController();
            
            Cats.Add(controller);
            catControllers.Add(giCat, controller);
            DebugLogHelper.Log($"[CatManager] Dynamically registered cat: {giCat.name}");
        }

        public void DeregisterCat(GICat giCat)
        {
            if (giCat == null) return;
            if (catControllers.TryGetValue(giCat, out var controller))
            {
                controller.Dispose();
                Cats.Remove(controller);
                catControllers.Remove(giCat);
                DebugLogHelper.Log($"[CatManager] Dynamically deregistered and disposed cat: {giCat.name}");
            }
        }

        public void Dispose()
        {
            foreach (var cat in Cats)
            {
                cat.Dispose();
            }
            Cats.Clear();
            catControllers.Clear();
            GameContext.Instance.Remove(this);
        }

        public void UpdateLogic(float deltaTime)
        {
            foreach (var cat in Cats)
            {
                cat.UpdateLogic(deltaTime);
            }
        }
    }
}
