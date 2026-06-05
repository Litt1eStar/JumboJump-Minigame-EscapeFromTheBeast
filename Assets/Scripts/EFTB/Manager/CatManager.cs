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

        public void Intialize(IEnumerable<GICat> sceneCats, Transform playerTarget)
        {
            Cats = new List<ICatStateController>();
            foreach(var giCat in sceneCats)
            {
                var controller = giCat.BuildStateController(playerTarget);
                BaseStateController baseController = controller as BaseStateController;
                baseController.Initialize();
                baseController.StartStateController();
                Cats.Add((ICatStateController)baseController);
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
