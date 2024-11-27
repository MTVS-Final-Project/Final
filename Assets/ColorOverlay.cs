using UnityEngine;
using Spine.Unity;

[RequireComponent(typeof(SkeletonAnimation))]
public class ColorOverlay : MonoBehaviour
{
    private SkeletonAnimation skeletonAnimation;

    [SerializeField]
    private float cycleDuration = 2.0f; // 한 사이클(빨주노초파남보)을 도는 시간 (초)
    [SerializeField, Range(0, 1)]
    private float intensity = 1.0f; // 색상의 강도
    [SerializeField, Range(0, 1)]
    private float saturation = 0.8f; // 채도 (1: 원래 색, 0: 흑백)

    private float timeElapsed; // 경과 시간

    void Awake()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;

        // 색상 순환
        Color overlayColor = GetRainbowColor(timeElapsed / cycleDuration);
        ApplyColorOverlay(overlayColor);
    }

    private void ApplyColorOverlay(Color color)
    {
        if (skeletonAnimation != null)
        {
            var skeleton = skeletonAnimation.skeleton;
            skeleton.SetColor(color * intensity);
        }
    }

    private Color GetRainbowColor(float t)
    {
        // t를 0~1 범위로 변환 (순환)
        t = t % 1.0f;

        // 빨주노초파남보 색상을 생성하며, Saturation을 적용
        return Color.HSVToRGB(t, saturation, 1.0f);
    }

    // Saturation 설정 메서드
    public void SetSaturation(float newSaturation)
    {
        saturation = Mathf.Clamp01(newSaturation);
    }

    // Intensity 설정 메서드
    public void SetIntensity(float newIntensity)
    {
        intensity = Mathf.Clamp01(newIntensity);
    }
}
