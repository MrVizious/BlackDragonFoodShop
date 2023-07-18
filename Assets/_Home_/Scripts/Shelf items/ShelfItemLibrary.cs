using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShelfItemLibrary", menuName = "Black Dragon Food Shop/ShelfItemLibrary", order = 0)]
public class ShelfItemLibrary : ScriptableObject
{

    public List<ShelfItemSprite> itemSprites;

    public int GetRandomIndex()
    {
        return Random.Range(0, itemSprites.Count);
    }

    public Sprite GetFullSprite(int index)
    {
        return itemSprites[index].full;
    }
    public Sprite GetHalfSprite(int index)
    {
        return itemSprites[index].half;
    }

    [System.Serializable]
    public struct ShelfItemSprite
    {
        public Sprite full;
        public Sprite half;
    }
}