using Assets.Scripts.EFTB.GI;
using Assets.Scripts.EFTB.Interface;
using Assets.Scripts.EFTB.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.EFTB.Manager
{
    public class CatManager
    {
        public List<ICatStateController> Cats {  get; private set; }
        public void Intialize(IEnumerable<GICat> sceneCats, Transform playerTarget)
        {
            Cats = new List<ICatStateController>();
            foreach(var giSight in sceneCats)
            {
                var controller = giSight.BuildStateController(playerTarget);
                controller.Initialize();
                Cats.Add(controller);
            }
        }

        public void Dispose()
        {
            foreach (var cat in Cats)
            {
                cat.Dispose();
            }
            Cats.Clear();
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
