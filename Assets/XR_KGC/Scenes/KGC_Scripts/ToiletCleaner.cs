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
            // Ŭ�� ��ġ�� ���� PointerEventData ����
            PointerEventData pointerEventData = new PointerEventData(eventSystem)
            {
                position = Input.mousePosition
            };

            // Raycast ����� ������ ����Ʈ ����
            List<RaycastResult> results = new List<RaycastResult>();

            // UI ����ĳ��Ʈ ����
            graphicRaycaster.Raycast(pointerEventData, results);

            // ����ĳ��Ʈ ��� ó��
            if (results.Count > 0)
            {
                foreach (var result in results)
                {
                    // �±׳� �̸����� ���� Ȯ��
                    if (result.gameObject.CompareTag("Dirt")) // "YourTag"�� ���ϴ� �±׷� ����
                    {
                        Debug.Log("������ �±��� UI ��� Ŭ����!");

                        result.gameObject.transform.parent.gameObject.SetActive(false);
                        // ���ϴ� �۾� ����
                    }
                    else
                    {
                        Debug.Log("�ٸ� �±��� UI ��� Ŭ����.");
                    }
                }
            }
            else
            {
                Debug.Log("UI ��Ұ� Ŭ������ ����.");
            }
        }
    }
}
