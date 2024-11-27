using UnityEngine;
using Spine.Unity;

[RequireComponent(typeof(SkeletonAnimation))]
public class GradientColorOverlay : MonoBehaviour
{
    private SkeletonAnimation skeletonAnimation;

    [SerializeField]
    private float cycleDuration = 2.0f; // 한 사이클(빨주노초파남보)을 도는 시간 (초)
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

        // 슬롯별로 색상 적용
        ApplyGradientOverlay();
    }

    private void ApplyGradientOverlay()
    {
        if (skeletonAnimation != null)
        {
            var skeleton = skeletonAnimation.skeleton;
            var slots = skeleton.Slots; // 모든 슬롯 가져오기

            int slotCount = slots.Count;
            for (int i = 0; i < slotCount; i++)
            {
                var slot = slots.Items[i];

                // 슬롯의 위치에 따라 색상 계산
                float t = (timeElapsed / cycleDuration + (float)i / slotCount) % 1.0f; // 슬롯별 시간 오프셋
                Color color = GetRainbowColor(t);

                // 슬롯에 색상 적용
                slot.SetColor(color);
            }
        }
    }

    private Color GetRainbowColor(float t)
    {
        // t를 0~1 범위로 변환 (순환)
        t = t % 1.0f;

        // 빨주노초파남보 색상을 생성하며 Saturation을 적용
        return Color.HSVToRGB(t, saturation, 1.0f);
    }
}
