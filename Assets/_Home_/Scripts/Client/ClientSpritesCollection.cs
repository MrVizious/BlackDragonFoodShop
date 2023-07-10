using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

[CreateAssetMenu(fileName = "ClientSpritesCollection", menuName = "Black Dragon Food Shop/ClientSpritesCollection", order = 0)]
public class ClientSpritesCollection : ScriptableObject
{
    public int currentIndex = 0;
    public List<SpriteLibraryAsset> libraries;

    public SpriteLibraryAsset getNext()
    {
        return libraries[(++currentIndex) % libraries.Count];
    }
}
