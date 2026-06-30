using System;

namespace JumboJumps.EFTB.GameData
{
    [AttributeUsage(AttributeTargets.Field)]
    public class GameDataAttribute : Attribute
    {
        public string JsonElement { get; }

        public GameDataAttribute(string jsonElement)
        {
            JsonElement = jsonElement;
        }
    }
}
