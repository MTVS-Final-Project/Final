using UnityEngine;
using Spine.Unity;

[RequireComponent(typeof(SkeletonAnimation))]
public class ColorOverlay : MonoBehaviour
{
    private SkeletonAnimation skeletonAnimation;

    [SerializeField]
    private float cycleDuration = 2.0f; // �� ����Ŭ(���ֳ����ĳ���)�� ���� �ð� (��)
    [SerializeField, Range(0, 1)]
    private float intensity = 1.0f; // ������ ����
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

        // ���� ��ȯ
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
        // t�� 0~1 ������ ��ȯ (��ȯ)
        t = t % 1.0f;

        // ���ֳ����ĳ��� ������ �����ϸ�, Saturation�� ����
        return Color.HSVToRGB(t, saturation, 1.0f);
    }

    // Saturation ���� �޼���
    public void SetSaturation(float newSaturation)
    {
        saturation = Mathf.Clamp01(newSaturation);
    }

    // Intensity ���� �޼���
    public void SetIntensity(float newIntensity)
    {
        intensity = Mathf.Clamp01(newIntensity);
    }
}
