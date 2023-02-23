using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum Emotion //TODO maybe more
{
    Neutral,
    Annoyed,
    Angry,
    Puzzled,
    Laughing,
    Sad,
    Joy,
    Proud
}

public enum DialogEventType
{
    Fade,
    Appear,
    Move,
    Swap,
    Talk
}

public enum TransitionType
{
    None,
    Linear,
    Hyperbolic
}

[System.Serializable]
public class DialogEvent
{
    public DialogEventType type;
    public bool IsMove => type == DialogEventType.Move;
    public bool IsSwap => type == DialogEventType.Swap;
    public bool IsTalk => type == DialogEventType.Talk;
    public bool IsntTalk => type != DialogEventType.Talk;
    [Tooltip("The index of the character performing the action.")]
    public int characterIndex;
    [Tooltip("The wanted position relative to the screen. This value is clamped between 0 and 1.")]
    [NaughtyAttributes.MinValue(0f)]
    [NaughtyAttributes.MaxValue(1f)]
    [NaughtyAttributes.ShowIf("type", DialogEventType.Move)]
    [NaughtyAttributes.AllowNesting]
    public float position;
    [Tooltip("The index of the character with whom he will swap places.")]
    [NaughtyAttributes.ShowIf("type", DialogEventType.Swap)]
    [NaughtyAttributes.AllowNesting]
    public int otherCharacterIndex;
    [Tooltip("Defines the movement type.\nNone is instantaneous.")]
    [NaughtyAttributes.ShowIf(NaughtyAttributes.EConditionOperator.Or, "IsMove", "IsSwap")]
    [NaughtyAttributes.AllowNesting]
    public TransitionType transitionType;
    [Tooltip("The time the action will take to finish.")]
    [NaughtyAttributes.ShowIf("IsntTalk")]
    [NaughtyAttributes.AllowNesting]
    public float transitionTime;
    [Tooltip("The text that will be displayed in the box.")]
    [NaughtyAttributes.ShowIf("type", DialogEventType.Talk)]
    [NaughtyAttributes.AllowNesting]
    public string text;
    [Tooltip("Should this dialog produce sound ?")]
    [NaughtyAttributes.ShowIf("type", DialogEventType.Talk)]
    [NaughtyAttributes.AllowNesting]
    public bool makesSound;
    [Tooltip("The base sound each letter will produce.")]
    [NaughtyAttributes.ShowIf("makesSound")]
    [NaughtyAttributes.AllowNesting]
    public AudioClip sound;
    [Tooltip("The ")]
    [NaughtyAttributes.ShowIf("makesSound")]
    [NaughtyAttributes.MinMaxSlider(-2f,2f)]
    [NaughtyAttributes.AllowNesting]
    public Vector2 toneVariation;
    [Tooltip("The speed at which a new letter appears in milliseconds.")]
    [NaughtyAttributes.ShowIf("type", DialogEventType.Talk)]
    [NaughtyAttributes.AllowNesting]
    public float textSpeed;
    [Tooltip("The emotion displayed by the character.")]
    [NaughtyAttributes.ShowIf("type", DialogEventType.Talk)]
    [NaughtyAttributes.AllowNesting]
    public Emotion emotion;
}


[CreateAssetMenu(fileName = "New Dialog", menuName = "Game/Dialogs/Dialog")]
public class DialogData : ScriptableObject
{
    [Tooltip("The characters interacting in the dialog.")]
    public List<InitCharacters> characters;
    [Tooltip("The list of events occuring in the dialog.")]
    public List<DialogEvent> events;
}

[System.Serializable]
public class InitCharacters
{
    public DialogCharacter character;
    [Tooltip("The position where the character will appear. This value is clamped between 0 and 1.")]
    [NaughtyAttributes.MinValue(0f)]
    [NaughtyAttributes.MaxValue(1f)]
    public float basePosition;
    [Tooltip("The emotion displayed at first by the character.")]
    public Emotion baseEmotion;
}