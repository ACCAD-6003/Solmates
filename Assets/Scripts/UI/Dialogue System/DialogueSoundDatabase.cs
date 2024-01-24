using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using static DialogueHelperClass;

[CreateAssetMenu(menuName = "SO/Sound Database", fileName = "New Sound File")]
public class DialogueSoundDatabase : SerializedScriptableObject
{
    [SerializeField] Dictionary<string, AudioClip> converter = new Dictionary<string, AudioClip>();

    public AudioClip GetClip(string fileName)
    {
        if(converter.TryGetValue(fileName, out var clip))
        {
            return clip;
        }

        if (fileName != EMPTY_MARKER)
        {
            Debug.LogWarning("WAS UNABLE TO FIND " + fileName + " IN AUDIO DATABASE");
        }

        return null;
    }

    public bool GetClip(string fileName, out AudioClip clip)
    {
        clip = GetClip(fileName);
        return clip != null;
    }
}
