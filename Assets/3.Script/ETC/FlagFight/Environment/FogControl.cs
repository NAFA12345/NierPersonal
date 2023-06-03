using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogControl : MonoBehaviour
{
    [Header("속도")]
    [SerializeField] float fadeOutDuration = 2.0f;

    [Space(0.5f)]
    [Header("확인용")]
    [SerializeField] FlagFightSubTitleManager flagFightSubTitleManager;
    [SerializeField] new ParticleSystem particleSystem;

    private void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
        flagFightSubTitleManager = FindObjectOfType<FlagFightSubTitleManager>();
        flagFightSubTitleManager.phase1_13.AddListener(phase01_13);
    }

    void phase01_13()
    {
        StartCoroutine(Co_fadeOut());
    }

    private IEnumerator Co_fadeOut()
    {
        // 파티클 시스템의 메인 모듈을 가져옵니다.
        ParticleSystem.MainModule mainModule = particleSystem.main;

        // 페이드 아웃 효과를 적용하기 위한 시작 알파 값과 타이머를 초기화합니다.
        float startAlpha = mainModule.startColor.color.a;
        float fadeOutTimer = 0f;

        while (fadeOutTimer < fadeOutDuration)
        {
            // 페이드 아웃 타이머를 업데이트합니다.
            fadeOutTimer += Time.deltaTime;

            // 페이드 아웃 효과를 적용하기 위해 알파 값을 조절합니다.
            float alpha = Mathf.Lerp(startAlpha, 0f, fadeOutTimer / fadeOutDuration);
            mainModule.startColor = new Color(mainModule.startColor.color.r, mainModule.startColor.color.g, mainModule.startColor.color.b, alpha);

            yield return null;
        }

        // 페이드 아웃이 완료되면 파티클 시스템을 정지합니다.
        particleSystem.Stop();
    }
}