using UnityEngine;
using Spine.Unity;

[RequireComponent(typeof(SkeletonAnimation))]
public class GradientColorOverlay : MonoBehaviour
{
    private SkeletonAnimation skeletonAnimation;

    [SerializeField]
    private float cycleDuration = 2.0f; // �� ����Ŭ(���ֳ����ĳ���)�� ���� �ð� (��)
    [SerializeField, Range(0, 1)]
    private float saturation = 0.8f; // ä�� (1: ���� ��, 0: ���)

    private float timeElapsed; // ��� �ð�

    void Awake()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;

        // ���Ժ��� ���� ����
        ApplyGradientOverlay();
    }

    private void ApplyGradientOverlay()
    {
        if (skeletonAnimation != null)
        {
            var skeleton = skeletonAnimation.skeleton;
            var slots = skeleton.Slots; // ��� ���� ��������

            int slotCount = slots.Count;
            for (int i = 0; i < slotCount; i++)
            {
                var slot = slots.Items[i];

                // ������ ��ġ�� ���� ���� ���
                float t = (timeElapsed / cycleDuration + (float)i / slotCount) % 1.0f; // ���Ժ� �ð� ������
                Color color = GetRainbowColor(t);

                // ���Կ� ���� ����
                slot.SetColor(color);
            }
        }
    }

    private Color GetRainbowColor(float t)
    {
        // t�� 0~1 ������ ��ȯ (��ȯ)
        t = t % 1.0f;

        // ���ֳ����ĳ��� ������ �����ϸ� Saturation�� ����
        return Color.HSVToRGB(t, saturation, 1.0f);
    }
}
