using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField]
    AudioClip[] clips;
    [SerializeField]
    GeneratedClip[] partialClips;
    [SerializeField]
    AudioSource globalPlayer;
    [SerializeField]
    GameObject positionPlayer;
    public Dictionary<string, AudioClip> AudioClips = new Dictionary<string, AudioClip>();
    protected void Awake()
    {
        foreach (var c in clips)
            AudioClips[c.name] = c;
        foreach (var c in partialClips)
        {
            if (c.startSamples == -1)
                c.startSamples = (int)(c.source.frequency * c.startTime);
            if (c.endSamples == -1)
                c.endSamples = (int)(c.source.frequency * c.endTime);
            var clip = AudioClip.Create(c.name,c.endSamples - c.startSamples,c.source.channels,c.source.frequency,false);
            var data = new float[clip.samples * clip.channels];
            c.source.GetData(data, c.startSamples);
            if (c.volume != 1)
                for (int i = 0; i < data.Length; i++)
                    data[i] = data[i] * c.volume;
            clip.SetData(data, 0);
            AudioClips[c.name] = clip;
        }
    }
    void Update()
    {
        if (MainListener.Instance)
            globalPlayer.transform.position = MainListener.Instance.listenPos;
    }
    void FixedUpdate() => Update();
    public AudioSource PlayOneShot(string name, Vector3 position)
    {
        if (AudioClips.TryGetValue(name, out var clip))
        {
            var g = Instantiate(positionPlayer, position, default);
            var s = g.GetComponent<AudioSource>();
            s.clip = clip;
            s.Play();
            return s;
        }
        return null;
    }
    public void PlayOneShot(string name) => PlayOneShot(name, globalPlayer);
    public void PlayOneShot(string name, AudioSource source)
    {
        if (source && source.enabled && AudioClips.TryGetValue(name, out var clip))
            source.PlayOneShot(clip);
    }
}

[System.Serializable]
class GeneratedClip
{
    public string name;
    public AudioClip source;
    public int startSamples = -1;
    public int endSamples = -1;
    public float startTime;
    public float endTime;
    public float volume = 1;
}
