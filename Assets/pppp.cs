
using Spine.Unity;
using UnityEngine;

public class pppp : MonoBehaviour
{
    public SkeletonAnimation[] data;
    // ½ºÄÌ·¹Å» ¿ÜÇü º¯°æ ¹æ¹ý

    public void ChangeToDataAsset1()
    {
        SkeletonAnimation sa = GetComponent<SkeletonAnimation>();

        sa.initialSkinName = "2";
        sa.Initialize(true);
        
    }
    public void ChangeToDataAsset2()
    {
        SkeletonAnimation sa = GetComponent<SkeletonAnimation>();

        sa.initialSkinName = "1";
        sa.Initialize(true);
    }
}
