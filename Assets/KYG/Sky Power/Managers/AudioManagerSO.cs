using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYG_skyPower
{
    [CreateAssetMenu(fileName = "AudioManagerSO", menuName = "Manager/AudioManager")]
    public class AudioManagerSO : SOSingleton<AudioManagerSO>
    {


        [Header("���� �����ͺ��̽�")]
        public AudioDataBase audioDB; // ������Ʈ ���ݿ� ��ģ ����� �����ͺ��̽�

        [Header("����Ʈ BGM / SFX")] // ���� ���۽� �⺻ ����
        public AudioData defaultBGM;

        [Header("Ǯ ũ��")]
        public int sfxPoolSize = 10; // SFX Ǯ ũ�� (�̻��, ���� ���� ����)


        private Dictionary<string, AudioData> audioDict; // �̸����� ������ ã�� ��ųʸ�
        private AudioSource bgmSource; // BGM ����� ����� �ҽ�
        private List<AudioSource> sfxPool = new List<AudioSource>(); // SFX ����� ����� �ҽ� Ǯ (�̻��, ���� ���� ����)

        public override void Init() // ����� DB ��ŷ����� �ʱ�ȭ
        {
            if (audioDict == null)
            {
                audioDict = new Dictionary<string, AudioData>();
                foreach (var data in audioDB.audioList)
                {
                    if (!audioDict.ContainsKey(data.clipName))
                        audioDict.Add(data.clipName, data);
                }
            }

            if (bgmSource == null)
            {
                var bgmGo = new GameObject("BGMSource");
                GameObject.DontDestroyOnLoad(bgmGo);
                bgmSource = bgmGo.AddComponent<AudioSource>();
                bgmSource.loop = true;
            }

            if (sfxPool.Count == 0)
            {
                var sfxGo = new GameObject("SFXPool");
                GameObject.DontDestroyOnLoad(sfxGo);
                for (int i = 0; i < sfxPoolSize; i++)
                {
                    var src = sfxGo.AddComponent<AudioSource>();
                    sfxPool.Add(src);
                }
            }
        }

        public AudioData GetAudioData(string name)
        {
            if (audioDict == null) Init();
            audioDict.TryGetValue(name, out AudioData data);
            return data;
        }

        // �� ���� SFX ���� ��� ����
        private AudioSource GetAvailableSFXSource()
        {
            foreach (var src in sfxPool)
                if (!src.isPlaying) return src;

            // ��� �ҽ� ��� ���̸� ���� �߰� (���� �δ��� ���� ��츸 ����)
            var sfxGo = sfxPool[0].gameObject;
            var extra = sfxGo.AddComponent<AudioSource>();
            sfxPool.Add(extra);
            return extra;
        }

        public void PlaySFX(string name)
        {
            var data = GetAudioData(name);
            if (data == null || data.clipSource == null) return;

            var src = GetAvailableSFXSource();
            src.clip = data.clipSource;
            src.volume = data.volume;
            src.loop = data.loop;
            src.Play();
        }

        // �� PlayOneShot ��� (Ǯ�� ���� �����ϰ� ���� ȿ���� ����, ������ ���� ����)
        public void PlaySFX_OneShot(string name)
        {
            var data = GetAudioData(name);
            if (data == null || data.clipSource == null) return;
            GetAvailableSFXSource().PlayOneShot(data.clipSource, data.volume);
        }

        // �� BGM ���̵� ��/�ƿ� ����
        public void FadeBGM(string name, float fadeTime = 1f)
        {
            if (bgmSource == null) Init();
            AudioManagerSO_CoroutineRunner.Instance.StartCoroutine(FadeBGM_Coroutine(name, fadeTime));
        }

        private IEnumerator FadeBGM_Coroutine(string name, float fadeTime)
        {
            float startVol = bgmSource.volume;

            // ���̵� �ƿ�
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

                // ���̵� ��
                for (float t = 0; t < fadeTime; t += Time.unscaledDeltaTime)
                {
                    bgmSource.volume = Mathf.Lerp(0, data.volume, t / fadeTime);
                    yield return null;
                }
                bgmSource.volume = data.volume;
            }
        }

        // ��� BGM ���
        public void PlayBGM(string name)
        {
            var data = GetAudioData(name);
            if (data == null || data.clipSource == null) return;
            bgmSource.clip = data.clipSource;
            bgmSource.volume = data.volume;
            bgmSource.loop = data.loop;
            bgmSource.Play();
        }

        // ��� BGM ����
        public void StopBGM() => bgmSource.Stop();
    }
}

