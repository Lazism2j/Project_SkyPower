using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYG_skyPower
{
    [CreateAssetMenu(fileName = "AudioManagerSO", menuName = "Manager/AudioManager")]
    public class AudioManagerSO : SOSingleton<AudioManagerSO>
    {
        [Header("���� �����ͺ��̽�")]
        public AudioDataBase audioDB;

        [Header("����Ʈ BGM / SFX")]
        public AudioData defaultBGM;

        [Header("Ǯ ũ��")]
        public int sfxPoolSize = 35;

        private Dictionary<string, AudioData> audioDict;

        [System.NonSerialized] private AudioSource bgmSource;
        [System.NonSerialized] private List<AudioSource> sfxPool;

        public static class Sound
        {
            public static void PlayBGM(string name) => Instance.PlayBGM(name);
            public static void PlaySFXOneShot(string name) => Instance.PlaySFX(name, true); // forceOneShot �⺻�� true
            public static void FadeBGM(string name, float time = 1f) => Instance.FadeBGM(name, time);
            public static void StopBGM() => Instance.StopBGM();
        }

        public override void Init()
        {
            // ����� ������ ��ųʸ� �ʱ�ȭ
            if (audioDict == null)
            {
                audioDict = new Dictionary<string, AudioData>();
                if (audioDB == null)
                {
                    Debug.LogError("[AudioManagerSO] AudioDB ���� �ʿ�!");
                    return;
                }
                foreach (var data in audioDB.audioList)
                {
                    if (!audioDict.ContainsKey(data.clipName) && data.clipSource != null)
                        audioDict.Add(data.clipName, data);
                }
            }

            // BGMSource ������Ʈ/������Ʈ �غ�
            if (bgmSource == null || bgmSource.gameObject == null)
            {
                var go = GameObject.Find("BGMSource");
                if (go == null)
                {
                    go = new GameObject("BGMSource");
                    GameObject.DontDestroyOnLoad(go);
                }
                bgmSource = go.GetComponent<AudioSource>();
                if (bgmSource == null)
                    bgmSource = go.AddComponent<AudioSource>();
                bgmSource.loop = true;
            }

            // SFX Ǯ ���ʱ�ȭ
            if (sfxPool == null)
                sfxPool = new List<AudioSource>();
            else
                sfxPool.Clear();

            // SFXPool ������Ʈ �غ�
            GameObject sfxGo = GameObject.Find("SFXPool");
            if (sfxGo == null)
            {
                sfxGo = new GameObject("SFXPool");
                GameObject.DontDestroyOnLoad(sfxGo);
            }

            var audioSources = sfxGo.GetComponents<AudioSource>();
            for (int i = 0; i < sfxPoolSize; i++)
            {
                AudioSource src = (i < audioSources.Length) ? audioSources[i] : sfxGo.AddComponent<AudioSource>();
                sfxPool.Add(src);
            }
        }

        public AudioData GetAudioData(string name)
        {
            if (audioDict == null) Init();
            if (!audioDict.TryGetValue(name, out var data))
            {
                Debug.LogWarning($"[AudioManagerSO] ����� ������ ������: {name}");
                return null;
            }
            return data;
        }

        /// <summary>
        /// �׻� PlayOneShot���� ȿ���� ��� (��ø, �ݺ� ��� �Ϻ� ����)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="forceOneShot"></param>
        public void PlaySFX(string name, bool forceOneShot = true)
        {
            var data = GetAudioData(name);
            if (data == null || data.clipSource == null) return;
            var src = GetAvailableSFXSource();
            if (forceOneShot)
            {
                src.PlayOneShot(data.clipSource, data.volume);
            }
            else
            {
                src.clip = data.clipSource;
                src.volume = data.volume;
                src.loop = data.loop;
                src.Play();
            }
        }

        /// <summary>
        /// PlayOneShot�� ������ ��� (UI/Ŭ��/��ȸ�� ȿ������)
        /// </summary>
        public void PlaySFXOneShot(string name)
        {
            Instance.PlaySFX(name, true);
        }

        private AudioSource GetAvailableSFXSource()
        {
            if (sfxPool == null || sfxPool.Count == 0) Init();
            foreach (var src in sfxPool)
                if (src != null && !src.isPlaying)
                    return src;

            // ��� ������̸� �������� �߰� (Ǯ �����÷�)
            var sfxGo = sfxPool[0].gameObject;
            var extra = sfxGo.AddComponent<AudioSource>();
            sfxPool.Add(extra);
            return extra;
        }

        public void FadeBGM(string name, float fadeTime = 1f)
        {
            if (bgmSource == null) Init();
            AudioManagerSO_CoroutineRunner.Instance.StartCoroutine(FadeBGM_Coroutine(name, fadeTime));
        }

        private IEnumerator FadeBGM_Coroutine(string name, float fadeTime)
        {
            float startVol = bgmSource.volume;
            for (float t = 0; t < fadeTime; t += Time.unscaledDeltaTime)
            {
                bgmSource.volume = Mathf.Lerp(startVol, 0, t / fadeTime);
                yield return null;
            }
            bgmSource.Stop();

            var data = GetAudioData(name);
            if (data != null && data.clipSource != null)
            {
                bgmSource.clip = data.clipSource;
                bgmSource.volume = 0;
                bgmSource.loop = data.loop;
                bgmSource.Play();

                for (float t = 0; t < fadeTime; t += Time.unscaledDeltaTime)
                {
                    bgmSource.volume = Mathf.Lerp(0, data.volume, t / fadeTime);
                    yield return null;
                }
                bgmSource.volume = data.volume;
            }
        }

        public void PlayBGM(string name)
        {
            var data = GetAudioData(name);
            if (data == null || data.clipSource == null) return;
            bgmSource.clip = data.clipSource;
            bgmSource.volume = data.volume;
            bgmSource.loop = data.loop;
            bgmSource.Play();
        }

        public void StopBGM() => bgmSource?.Stop();
    }
}