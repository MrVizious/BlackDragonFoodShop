using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;
using System.Linq;

[CreateAssetMenu(fileName = "ClientSpritesCollection", menuName = "Black Dragon Food Shop/ClientSpritesCollection", order = 0)]
public class ClientSpritesCollection : ScriptableObject
{
    public int currentIndex = 0;
    public int dontRepeatLastNIndices = 10;
    private List<int> lastChosenIndices = new List<int>();
    public List<SpriteLibraryAsset> libraries;

    public SpriteLibraryAsset getNext()
    {
        return libraries[(++currentIndex) % libraries.Count];
    }

    public SpriteLibraryAsset getRandom()
    {
        List<int> allIndices = new List<int>();
        for (int i = 0; i < libraries.Count; i++)
        {
            allIndices.Add(i);
        }
        List<int> possibleIndices = allIndices.Except(lastChosenIndices).ToList();
        int newIndex = possibleIndices[Random.Range(0, possibleIndices.Count)];
        if (lastChosenIndices.Count >= dontRepeatLastNIndices)
        {
            lastChosenIndices.RemoveAt(0);
        }
        lastChosenIndices.Add(newIndex);
        return libraries[newIndex];
    }
}
