using UnityEngine;
using Assets.Scripts;
using System;
using UnityEngine.Audio;
using System.Collections;
//using UnityEngine.iOS; jeśli chcesz tego używać to należy dodać ifa(ale tego nie używasz narazie)

public class AudioManager : MonoBehaviour
{
    public const String VFX_VOLUME = "VFX_VOLUME";
    public const String MUSIC_VOLUME = "MUSIC_VOLUME";

    public const String COIN_SOUND_NAME = "poptest";
    public const Int32 MAX_SIMULTANEOUS_COIN_SOUNDS = 2;


    public Sound[] Sounds;
    public static AudioManager Instance;
    public AudioMixerGroup MixerGroupVFX;
    public AudioMixerGroup MixerGroupMusic;
    public AudioMixer MASTERMixer;

    private Single _vfxVolume;
    private Single _musicVolume;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in Sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.Clip;
            s.source.spatialBlend = s.SpatialBlend;
        }

        Play("theme", SoundCategory.Music, isTheme: true);

    }

    private void Start()
    {
        //UpdateVolumePreferences(PlayerPrefs.GetFloat(VFX_VOLUME, 1f), PlayerPrefs.GetFloat(MUSIC_VOLUME, 1f)); // TA METODA MUSI BYC TU BO SETFLOAT OD MIXERA NIE DZIALA W AWAKE


    }

    public AudioSource Play(String name, SoundCategory category, RandomPitchSoundParameters randomPitch = null, Single? pitch = null, Single? volume = null, Boolean playOneShot = false,
        Boolean isTheme = false)
    {
        if (randomPitch == null)
            randomPitch = new RandomPitchSoundParameters();

        Sound sound = Array.Find(Sounds, s => s.Name == name);
        if (sound == null)
        {
            Debug.LogWarning($"Probowales odtworzyc nieisniejacy dzwiek o nazwie {name}");
            return null;
        }


        //sound.source.volume = category == SoundCategory.Music ? _musicVolume : _vfxVolume;
        sound.source.outputAudioMixerGroup = category == SoundCategory.Music ? MixerGroupMusic : MixerGroupVFX;
        sound.source.pitch = UnityEngine.Random.Range(randomPitch.GetMinPitch(), randomPitch.GetMaxPitch());

        if (volume.HasValue)
            sound.source.volume = volume.Value;

        if (pitch.HasValue)
            sound.source.pitch = pitch.Value;

        if(isTheme)
        {
            sound.source.loop = true;
        }

        if (playOneShot)
            sound.source.PlayOneShot(sound.Clip);
        else
            sound.source.Play();

        return sound.source;
    }

    public void UpdateVolumePreferences(Single vfx, Single music)
    {
        MASTERMixer.SetFloat("VFXVolume", Mathf.Log10(vfx) * 20);
        MASTERMixer.SetFloat("MusicVolume", Mathf.Log10(music) * 20);

        _musicVolume = music;
        _vfxVolume = vfx;
    }

    Boolean _isMuted = false;
    public Boolean IsMuted() => _isMuted;
    public void ToggleSound()
    {
        if(_isMuted)
        {
            UpdateVolumePreferences(0f, 0f);
        
        }
        else
        {
            UpdateVolumePreferences(Single.MinValue, Single.MinValue);

        }
        _isMuted = !_isMuted;
    }

}

public class RandomPitchSoundParameters
{
    private Single _pitchMin;
    private Single _pitchMax;

    public RandomPitchSoundParameters()
    {
        _pitchMin = 1f;
        _pitchMax = 1f;
    }

    public RandomPitchSoundParameters(Single pmin, Single pmax)
    {
        _pitchMin = pmin;
        _pitchMax = pmax;
    }

    public Single GetMinPitch() => _pitchMin;
    public Single GetMaxPitch() => _pitchMax;
}