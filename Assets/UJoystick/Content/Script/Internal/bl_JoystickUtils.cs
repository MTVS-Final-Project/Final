using UnityEngine;

public static class bl_JoystickUtils
{

    public static Vector3 TouchPosition(this Canvas _Canvas, int touchID)
    {
        Vector3 Return = Vector3.zero;

        // 터치 입력 확인
        if (Input.touchCount > touchID)
        {
            if (_Canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
#if UNITY_ANDROID
                Return = Input.GetTouch(touchID).position;
#else
                Return = Input.mousePosition;
#endif
            }
            else if (_Canvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                Vector2 tempVector = Vector2.zero;
                Vector3 pos;

#if UNITY_ANDROID
                pos = Input.GetTouch(touchID).position;
#else
                pos = Input.mousePosition;
#endif

                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    _Canvas.transform as RectTransform,
                    pos,
                    _Canvas.worldCamera,
                    out tempVector))
                {
                    Return = _Canvas.transform.TransformPoint(tempVector);
                }
            }
        }

        return Return;
    }
}