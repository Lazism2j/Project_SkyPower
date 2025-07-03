using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYG_skyPower
{
    [CreateAssetMenu(fileName = "AudioManagerSO", menuName = "Manager/AudioManager")]
    public class AudioManagerSO : SOSingleton<AudioManagerSO>
    {


        [Header("사운드 데이터베이스")]
        public AudioDataBase audioDB; // 프로젝트 전반에 걸친 오디오 데이터베이스

        [Header("디폴트 BGM / SFX")] // 게임 시작시 기본 사운드
        public AudioData defaultBGM;

        [Header("풀 크기")]
        public int sfxPoolSize = 10; // SFX 풀 크기 (미사용, 추후 구현 예정)


        private Dictionary<string, AudioData> audioDict; // 이름으로 데이터 찾는 딕셔너리
        private AudioSource bgmSource; // BGM 재생용 오디오 소스
        private List<AudioSource> sfxPool = new List<AudioSource>(); // SFX 재생용 오디오 소스 풀 (미사용, 추후 구현 예정)

        public override void Init() // 오디오 DB 딕셔러리로 초기화
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

        // ★ 여러 SFX 동시 재생 지원
        private AudioSource GetAvailableSFXSource()
        {
            foreach (var src in sfxPool)
                if (!src.isPlaying) return src;

            // 모든 소스 사용 중이면 동적 추가 (성능 부담이 적은 경우만 권장)
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

        // ★ PlayOneShot 방식 (풀링 없이 간단하게 여러 효과음 가능, 볼륨만 조정 가능)
        public void PlaySFX_OneShot(string name)
        {
            var data = GetAudioData(name);
            if (data == null || data.clipSource == null) return;
            GetAvailableSFXSource().PlayOneShot(data.clipSource, data.volume);
        }

        // ★ BGM 페이드 인/아웃 지원
        public void FadeBGM(string name, float fadeTime = 1f)
        {
            if (bgmSource == null) Init();
            AudioManagerSO_CoroutineRunner.Instance.StartCoroutine(FadeBGM_Coroutine(name, fadeTime));
        }

        private IEnumerator FadeBGM_Coroutine(string name, float fadeTime)
        {
            float startVol = bgmSource.volume;

            // 페이드 아웃
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

                // 페이드 인
                for (float t = 0; t < fadeTime; t += Time.unscaledDeltaTime)
                {
                    bgmSource.volume = Mathf.Lerp(0, data.volume, t / fadeTime);
                    yield return null;
                }
                bgmSource.volume = data.volume;
            }
        }

        // 즉시 BGM 재생
        public void PlayBGM(string name)
        {
            var data = GetAudioData(name);
            if (data == null || data.clipSource == null) return;
            bgmSource.clip = data.clipSource;
            bgmSource.volume = data.volume;
            bgmSource.loop = data.loop;
            bgmSource.Play();
        }

        // 즉시 BGM 정지
        public void StopBGM() => bgmSource.Stop();
    }
}

