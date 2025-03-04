using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MetroidMapEditorCore
{
    public class WayBase : MonoBehaviour
    {
        public LineRenderer lineRenderer;
        public DoorBase[] attachDoors;
        public RawImage rawImage;
        public RectTransform[] doorPoss;
        // Start is called before the first frame update
        void Start()
        {
            doorPoss = new RectTransform[2];
            for(int i = 0; i < 2; i++)
            {
                doorPoss[i] = attachDoors[i].doorTransform;
            }
  //          initRawImage();
            initLR();
 //           UpdateLine();
           UpdateLinePositions();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateLinePositions();
        }

        void initLR()
        {
            lineRenderer = gameObject.GetComponent<LineRenderer>();

            // ���� LineRenderer �Ļ�������
            lineRenderer.positionCount = 2; // �����ߵĶ�����Ϊ 2
            lineRenderer.startWidth = .5f;   // �����߿�
            lineRenderer.endWidth = .5f;
            lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // ���ò���
            lineRenderer.startColor = Color.red; // ��������ɫ
            lineRenderer.endColor = Color.red;
        }

        void UpdateLinePositions()
        {
            // �� UI ����ת��Ϊ��������
            Vector3 startPos = RectTransformUtility.WorldToScreenPoint(Camera.main, doorPoss[0].position);
            startPos= Camera.main.ScreenToWorldPoint(new Vector3(startPos.x, startPos.y, 10));
            Vector3 endPos = RectTransformUtility.WorldToScreenPoint(Camera.main, doorPoss[1].position);
            endPos= Camera.main.ScreenToWorldPoint(new Vector3(endPos.x, endPos.y, 10));
            // �����ߵ������յ�
            lineRenderer.SetPosition(0, startPos);
            lineRenderer.SetPosition(1, endPos);
        }

        void initRawImage()
        {
            // ����һ���յ� GameObject ����� RawImage ���
            GameObject lineObject = new GameObject("Line");
            lineObject.transform.SetParent(this.transform); // ����Ϊ��ǰ������Ӷ���
            rawImage = lineObject.AddComponent<RawImage>();
        }


        void UpdateLine()
        {
            // ��ȡ�����յ��λ��
            Vector2 startPos = doorPoss[0].anchoredPosition;
            Vector2 endPos = doorPoss[1].anchoredPosition;

            // ���������ĳ��Ⱥͷ���
            Vector2 direction = endPos - startPos;
            int width = Mathf.CeilToInt(direction.magnitude);
            int height = 5; // �������

            // ��������
            Texture2D texture = new Texture2D(width, height);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    texture.SetPixel(x, y, Color.red); // ����������ɫ
                }
            }
            texture.Apply();

            // ������ֵ�� RawImage
            rawImage.texture = texture;

            // ���� RawImage ��λ�ú���ת
            RectTransform rawImageRect = rawImage.GetComponent<RectTransform>();
            rawImageRect.sizeDelta = new Vector2(width, height);
            rawImageRect.anchoredPosition = startPos + direction / 2;
            rawImageRect.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        }

    }

}
