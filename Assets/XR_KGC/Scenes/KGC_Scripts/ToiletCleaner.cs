using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class ToiletCleaner : MonoBehaviour
{
    public GraphicRaycaster graphicRaycaster;
    public EventSystem eventSystem;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 클릭 위치에 대한 PointerEventData 생성
            PointerEventData pointerEventData = new PointerEventData(eventSystem)
            {
                position = Input.mousePosition
            };

            // Raycast 결과를 저장할 리스트 생성
            List<RaycastResult> results = new List<RaycastResult>();

            // UI 레이캐스트 수행
            graphicRaycaster.Raycast(pointerEventData, results);

            // 레이캐스트 결과 처리
            if (results.Count > 0)
            {
                foreach (var result in results)
                {
                    // 태그나 이름으로 조건 확인
                    if (result.gameObject.CompareTag("Dirt")) // "YourTag"를 원하는 태그로 변경
                    {
                        Debug.Log("지정된 태그의 UI 요소 클릭됨!");

                        result.gameObject.transform.parent.gameObject.SetActive(false);
                        // 원하는 작업 수행
                    }
                    else
                    {
                        Debug.Log("다른 태그의 UI 요소 클릭됨.");
                    }
                }
            }
            else
            {
                Debug.Log("UI 요소가 클릭되지 않음.");
            }
        }
    }
}
