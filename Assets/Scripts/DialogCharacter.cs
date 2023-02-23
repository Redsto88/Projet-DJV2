using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialog Character", menuName = "Game/Dialogs/Character")]
public class DialogCharacter : ScriptableObject
{
    public List<EmotionSprite> sprites;
    public Sprite SpriteByEmotion(Emotion em)
    {
        foreach(EmotionSprite es in sprites)
        {
            if (em == es.emotion) return es.sprite;
        }
        return null;
    }
}

[System.Serializable]
public class EmotionSprite
{
    public Sprite sprite;
    public Emotion emotion;
}
