using System.Collections.Generic;
using JumboJumps.EFTB.Constant.Sound;
using JumboJumps.EFTB.Utilities;
using UnityEngine;

namespace JumboJumps.EFTB.Manager
{
    public class SoundManager
    {
        private GameObject hostObject;
        private AudioSource bgmSource;
        private AudioSource sfxSource;

        private readonly Dictionary<string, AudioClip> audioClipCache = new Dictionary<string, AudioClip>();

        public string CurrentBGMKey { get; private set; }

        public void Initialize()
        {
            hostObject = new GameObject("SoundManagerHost");
            GameObject.DontDestroyOnLoad(hostObject);

            var bgmHost = new GameObject("BGMAudioSource");
            bgmHost.transform.SetParent(hostObject.transform);
            bgmSource = bgmHost.AddComponent<AudioSource>();
            bgmSource.loop = true;
            bgmSource.playOnAwake = false;

            var sfxHost = new GameObject("SFXAudioSource");
            sfxHost.transform.SetParent(hostObject.transform);
            sfxSource = sfxHost.AddComponent<AudioSource>();
            sfxSource.loop = false;
            sfxSource.playOnAwake = false;

            PreloadAudioClips();

            GameContext.Instance.Add(this);
            DebugLogHelper.Log($"[{GetType().Name}] Initialized successfully.");
        }

        public void Dispose()
        {
            GameContext.Instance.Remove(this);
            CurrentBGMKey = null;

            audioClipCache.Clear();

            if (hostObject != null)
            {
                GameObject.Destroy(hostObject);
                hostObject = null;
            }

            DebugLogHelper.Log($"[{GetType().Name}] Disposed.");
        }

        private void PreloadAudioClips()
        {
            audioClipCache.Clear();

            string[] soundKeys = new string[]
            {
                ConstSound.Keys.Cat.MEOW_SAD,
                ConstSound.Keys.Collectible.TREAT_COLLECT,
                ConstSound.Keys.UI.BUTTON_CLICK,
                ConstSound.Keys.UI.GAME_OVER,
                ConstSound.Keys.UI.READY,
                ConstSound.Keys.UI.GO,
                ConstSound.Keys.BGM.GAMEPLAY
            };

            foreach (var key in soundKeys)
            {
                if (string.IsNullOrEmpty(key)) continue;

                AudioClip clip = Resources.Load<AudioClip>($"Sounds/{key}");

                if (clip != null)
                {
                    audioClipCache[key] = clip;
                }
            }
        }

        /// <summary>
        /// Play a Sound Effect by sound key using the preloaded audio clip cache.
        /// </summary>
        public void PlaySFX(string soundKey)
        {
            if (string.IsNullOrEmpty(soundKey)) return;

            if (audioClipCache.TryGetValue(soundKey, out AudioClip clip) && clip != null)
            {
                sfxSource?.PlayOneShot(clip);
            }
            else
            {
                DebugLogHelper.LogWarning($"[{GetType().Name}] Audio clip asset for SFX key '{soundKey}' is missing or not cached.");
            }
        }

        /// <summary>
        /// Play or change Background Music by BGM key using the preloaded audio clip cache.
        /// </summary>
        public void PlayBGM(string bgmKey)
        {
            if (string.IsNullOrEmpty(bgmKey)) return;
            if (CurrentBGMKey == bgmKey && bgmSource != null && bgmSource.isPlaying) return;

            CurrentBGMKey = bgmKey;
            if (audioClipCache.TryGetValue(bgmKey, out AudioClip clip) && clip != null)
            {
                if (bgmSource != null)
                {
                    bgmSource.clip = clip;
                    bgmSource.Play();
                    DebugLogHelper.Log($"[{GetType().Name}] Playing BGM: '{bgmKey}'");
                }
            }
            else
            {
                DebugLogHelper.LogWarning($"[{GetType().Name}] Audio clip asset for BGM key '{bgmKey}' is missing or not cached.");
            }
        }

        public void StopBGM()
        {
            CurrentBGMKey = null;
            if (bgmSource != null)
            {
                bgmSource.Stop();
                bgmSource.clip = null;
            }
            DebugLogHelper.Log($"[{GetType().Name}] Stopped BGM.");
        }
    }
}
