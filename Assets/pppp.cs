
using Spine.Unity;
using UnityEngine;

public class pppp : MonoBehaviour
{
    public SkeletonAnimation[] data;
    // ���̷�Ż ���� ���� ���

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
