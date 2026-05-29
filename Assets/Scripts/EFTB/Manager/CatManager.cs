using JumboJumps.EFTB.GI;
using JumboJumps.EFTB.Interface;
using JumboJumps.EFTB.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace JumboJumps.EFTB.Manager
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
            GameContext.Instance.Add(this);
        }

        public void Dispose()
        {
            foreach (var cat in Cats)
            {
                cat.Dispose();
            }
            Cats.Clear();
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
