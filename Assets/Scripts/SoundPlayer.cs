using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] List<AudioClip> audioClips;
    [SerializeField] List<string> audioClipNames;
    [SerializeField] List<float> audioClipVolumes;

    static Dictionary<string, AudioSource> audioDictionary;

    private void Awake()
    {
        audioDictionary = new Dictionary<string, AudioSource>();

        if (audioClips.Count != audioClipNames.Count)
        {
            throw new System.Exception("SoundPlayer does not have a matching number of AudioClips and string names for those AudioClips");
        }

        for (int i = 0; i < audioClips.Count; i++) {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = audioClips[i];
            audioSource.volume = (i < audioClipVolumes.Count) ? audioClipVolumes[i] : 1f;

            audioDictionary.Add(audioClipNames[i], audioSource);
        }
    }

    public static void Play(string audioClipName)
    {
        audioDictionary[audioClipName].Play();
    }
}
