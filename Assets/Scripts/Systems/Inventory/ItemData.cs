namespace Game.Systems.Inventory
{
    using UnityEngine;

    /// <summary>
    /// Base class for all ScriptableObject Items.
    /// Uses abstract Use method for decoupled action implementation.
    /// </summary>
    public abstract class ItemData : ScriptableObject
    {
        public string ItemName;
        [TextArea(2, 4)]
        public string Description;
        public Sprite Icon;

        /// <summary>
        /// Defines what happens when the item is consumed.
        /// </summary>
        public abstract void Use();
    }
}
