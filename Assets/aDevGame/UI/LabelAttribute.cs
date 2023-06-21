using UnityEngine;

namespace aDevGame.UI
{
    public class LabelAttribute:PropertyAttribute
    {
        private string name;   
    
        public string Name => name;

        public LabelAttribute(string name)
        {
            this.name = name;
        }
    }
}