using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(Sound))]
public class AudioManager : MonoBehaviour
{
    // static fields

    public static AudioManager Instance { get; private set; }
    // fields

    [SerializeField]
    protected Sound[] soundSamples = default;

    protected Dictionary<string, Sound> sounds;
    protected Dictionary<string, AudioSource> audioSources;

    protected AudioSource audioSource;

    // methods

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        InitSoundMap();
        audioSource = GetComponent<AudioSource>();
    }

    private void InitSoundMap()
    {
        sounds = new Dictionary<string, Sound>();
        audioSources = new Dictionary<string, AudioSource>();

        foreach (Sound sound in soundSamples)
        {
            sounds[sound.id] = sound;
            AudioSource source = gameObject.AddComponent<AudioSource>();
            audioSources[sound.id] = source;

            source.loop = sound.looping;
            source.volume = sound.volume;
            source.clip = sound.audioClip;
            source.outputAudioMixerGroup = sound.audioMixerGroup;
            source.playOnAwake = false;
        }

        audioSources["alarm"].pitch = 0.7f;
    }

    public void Register(string id, AudioSource source)
    {
        Sound sound = sounds[id];
        source.loop = sound.looping;
        source.volume = sound.volume;
        source.clip = sound.audioClip;
        source.outputAudioMixerGroup = sound.audioMixerGroup;
        source.playOnAwake = false;
        source.spatialBlend = 1.0f;
    }

    public void Play(string id)
    {
        if (!sounds.ContainsKey(id))
        {
            return;
        }

        if (sounds[id].looping)
        {
            audioSources[id].loop = true;
        }

        audioSources[id].Play();
    }

    public void Stop(string id)
    {
        if (!sounds.ContainsKey(id))
        {
            return;
        }

        if (sounds[id].looping)
        {
            audioSources[id].loop = false;
        }
        else
        {
            audioSources[id].Stop();
        }
    }

    // internal classes
    [System.Serializable]
    public struct Sound
    {
        public string id;
        public AudioClip audioClip;
        public AudioMixerGroup audioMixerGroup;
        [Range(0f, 1f)]
        public float volume;

        public bool looping;
    }
}

