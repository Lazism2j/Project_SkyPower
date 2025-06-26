using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYG_skyPower
{
    public class AudioManagerRunner : MonoBehaviour
    {
        public AudioManagerSO audioManagerSO; // ������ ���� �ʼ�

        [Header("AudioSource")]
        public AudioSource bgmSource;
        public AudioSource sfxSource;

        void Awake() // �ʱ�ȭ
        {
            if (audioManagerSO != null)
                audioManagerSO.Init();
        }

        void Start() // �⺻ ���� ���
        {
            PlaySFX(audioManagerSO.defaultSFX);
            PlayBGM(audioManagerSO.defaultBGM);
        }

        public void PlayClip(string name, Vector3 pos) // 3D�� ��ġ��� ����
        {
            var data = audioManagerSO.GetAudioData(name);
            if (data == null || data.clipSource == null)
            {
                Debug.LogWarning($"����� ������ �� ã��: {name}");
                return;
            }

            GameObject go = new GameObject($"AudioClip_{name}"); // 1ȸ��
            go.transform.position = pos;
            var source = go.AddComponent<AudioSource>();
            source.clip = data.clipSource;
            source.volume = data.volume;
            source.loop = data.loop;
            source.spatialBlend = 1;
            source.Play();

            if (!data.loop) // ������ �ƴ� ���, ��� ���� �� ������Ʈ �ڵ� ����
                Destroy(go, data.clipSource.length);
        }

        private void PlayBGM(AudioData bgm) // BGM ���
        {
            if (bgm == null || bgm.clipSource == null) return;
            if (bgmSource.isPlaying && bgmSource.clip == bgm.clipSource) return;

            bgmSource.clip = bgm.clipSource;
            bgmSource.volume = bgm.volume;
            bgmSource.loop = bgm.loop;
            bgmSource.Play();
        }

        private void PlaySFX(AudioData SFX) // SFX ���
        {
            if (SFX == null || SFX.clipSource == null) return;
            if (sfxSource.isPlaying && sfxSource.clip == SFX.clipSource) return;

            sfxSource.clip = SFX.clipSource;
            sfxSource.volume = SFX.volume;
            sfxSource.loop = SFX.loop;
            sfxSource.Play();
        }
    }
}
