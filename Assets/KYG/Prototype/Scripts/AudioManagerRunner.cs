using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYG
{
    public class AudioManagerRunner : MonoBehaviour // // AudioSource�� ScriptableObject�� �����ϴ� ����
    {
        public AudioManagerSO audioManager;
        public AudioSource bgmSource;
        public AudioSource sfxSource;

        void Awake()
        {
            audioManager.Init(bgmSource, sfxSource); // ��Ÿ�� AudioSource ����
        }
    }
}