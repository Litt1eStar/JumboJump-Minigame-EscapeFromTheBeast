using Assets.Scripts.EFTB.GI;
using Assets.Scripts.EFTB.Interface;
using Assets.Scripts.EFTB.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.EFTB.Manager
{
    public class CatManager
    {
        public List<ICatStateController> cats {  get; private set; }
        public void Intialize(IEnumerable<GICatSight> sceneCats, Transform playerTarget)
        {
            cats = new List<ICatStateController>();
            foreach(var giSight in sceneCats)
            {
                var controller = giSight.BuildStateController(playerTarget);
                controller.Initialize();
                cats.Add(controller);
            }

            GameContext.Instance.Add(this);
        }

        public void Dispose()
        {
            foreach (var cat in cats)
            {
                cat.Dispose();
            }
            cats.Clear();
        }

        public void UpdateLogic(float deltaTime)
        {
            foreach (var cat in cats)
            {
                cat.UpdateLogic(deltaTime);
            }
        }
    }
}
