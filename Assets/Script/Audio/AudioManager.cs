using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<Sound> sounds;

    public List<Sound> purchaseSound;
    [SerializeField] private bool isMute;
    private void Start()
    {
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
        foreach(Sound sound in purchaseSound)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }
    public void Play(string soundName)
    {
        if (isMute)
        {
            return;
        }
        Sound sound = sounds.Find(s => s.soundName == soundName);
        if (sound != null)
        {
            sound.source.Play();
        }
        else
        {
            Debug.LogError("Sound Not Found!");
        }
    }
    public void Stop(string soundName)
    {
        if (isMute)
        {
            return;
        }
        Sound sound = sounds.Find(s => s.soundName == soundName);
        if (sound != null)
        {
            sound.source.Stop();
        }
        else
        {
            Debug.LogError("Sound Not Found!");
        }
    }
    public void Pause(string soundName)
    {
        if(isMute)
        {
            return;
        }
        Sound sound = sounds.Find(s => s.soundName == soundName);
        if (sound != null)
        {
            sound.source.Pause();
        }
        else
        {
            Debug.LogError("Sound Not Found!");
        }
    }

    public void Resume(string soundName)
    {
        if(isMute)
        {
            return;
        }
        Sound sound = sounds.Find(s => s.soundName == soundName);
        if (sound != null)
        {
            sound.source.UnPause();
        }
        else
        {
            Debug.LogWarning("Sound Not Found");
        }
    }
    public void playOnePurchaseSound(string soundName)
    {
        if (isMute)
        {
            return;
        }
        Sound sound = purchaseSound.Find(s => s.soundName == soundName);
        if (sound != null)
        {
            Debug.Log("sound: " + sound.source);
            sound.source.Play();
        }
        else
        {
            Debug.LogError("Sound Not Found!");
        }
    }
    public void playPurchaseSound()
    {
        int len = sounds.Count;
        int idx = Random.Range(0, len);
        playOnePurchaseSound(purchaseSound[idx].soundName);
    }

}
