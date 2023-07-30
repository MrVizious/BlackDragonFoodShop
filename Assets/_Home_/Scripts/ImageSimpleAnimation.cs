using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

[RequireComponent(typeof(Image))]
public class ImageSimpleAnimation : MonoBehaviour
{
    public int millisecondsBetweenFrames = 800;
    public List<Sprite> sprites = new List<Sprite>();

    private int currentSpriteIndex = 0;

    private Image _image;
    private Image image
    {
        get
        {
            if (_image == null) _image = GetComponent<Image>();
            return _image;
        }
    }

    private async void Start()
    {

        image.sprite = sprites[currentSpriteIndex];
        while (true)
        {
            await UniTask.Delay(millisecondsBetweenFrames);
            currentSpriteIndex++;
            currentSpriteIndex %= sprites.Count;
            image.sprite = sprites[currentSpriteIndex];
        }

    }



}
