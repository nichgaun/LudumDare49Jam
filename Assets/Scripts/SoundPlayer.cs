using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] List<AudioClip> audioClips;
    [SerializeField] List<string> audioClipNames;
    [SerializeField] List<float> audioClipVolumes;

    static Dictionary<string, AudioSource> audioSourceDictionary;
    static Dictionary<string, AudioClip> audioClipDictionary;

    private void Awake()
    {
        audioSourceDictionary = new Dictionary<string, AudioSource>();
        audioClipDictionary = new Dictionary<string, AudioClip>();

        if (audioClips.Count != audioClipNames.Count)
        {
            throw new System.Exception("SoundPlayer does not have a matching number of AudioClips and string names for those AudioClips");
        }

        for (int i = 0; i < audioClips.Count; i++) {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = audioClips[i];
            audioSource.volume = (i < audioClipVolumes.Count) ? audioClipVolumes[i] : 1f;

            audioClipDictionary.Add(audioClipNames[i], audioClips[i]);
            audioSourceDictionary.Add(audioClipNames[i], audioSource);
        }
    }

    public static void Play(string audioClipName, bool loop = false, bool asOneShot = true)
    {
        if (asOneShot)
        {
            audioSourceDictionary[audioClipName].PlayOneShot(audioClipDictionary[audioClipName]);
        }
        else
        {
            audioSourceDictionary[audioClipName].loop = loop;
            audioSourceDictionary[audioClipName].Play();
        }
    }

    public static void Stop(string audioClipName)
    {
        audioSourceDictionary[audioClipName].Stop();
    }
}
