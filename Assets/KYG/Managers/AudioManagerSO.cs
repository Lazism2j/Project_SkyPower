using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioManager", menuName = "Managers/AudioManager")]
public class AudioManagerSO : ScriptableObject
{
    // ��Ÿ�ӿ��� ���Թ޴� AudioSource��
    [HideInInspector] public AudioSource bgmSource;
    [HideInInspector] public AudioSource sfxSource;

    public void Init(AudioSource bgm, AudioSource sfx) // AudioSource�� �ܺο��� ����
    {
        bgmSource = bgm;
        sfxSource = sfx;
    }

    // ������� ���
    public void PlayBGM(AudioClip clip)
    {
        if (bgmSource == null || clip == null) return;
        bgmSource.clip = clip;
        bgmSource.Play();
    }

    // ȿ���� ���
    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource == null || clip == null) return;
        sfxSource.PlayOneShot(clip);
    }

    // ������� ����
    public void StopBGM()
    {
        bgmSource?.Stop();
    }
}
