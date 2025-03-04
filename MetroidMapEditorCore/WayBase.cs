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

            // 设置 LineRenderer 的基本属性
            lineRenderer.positionCount = 2; // 设置线的顶点数为 2
            lineRenderer.startWidth = .5f;   // 设置线宽
            lineRenderer.endWidth = .5f;
            lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // 设置材质
            lineRenderer.startColor = Color.red; // 设置线颜色
            lineRenderer.endColor = Color.red;
        }

        void UpdateLinePositions()
        {
            // 将 UI 坐标转换为世界坐标
            Vector3 startPos = RectTransformUtility.WorldToScreenPoint(Camera.main, doorPoss[0].position);
            startPos= Camera.main.ScreenToWorldPoint(new Vector3(startPos.x, startPos.y, 10));
            Vector3 endPos = RectTransformUtility.WorldToScreenPoint(Camera.main, doorPoss[1].position);
            endPos= Camera.main.ScreenToWorldPoint(new Vector3(endPos.x, endPos.y, 10));
            // 设置线的起点和终点
            lineRenderer.SetPosition(0, startPos);
            lineRenderer.SetPosition(1, endPos);
        }

        void initRawImage()
        {
            // 创建一个空的 GameObject 并添加 RawImage 组件
            GameObject lineObject = new GameObject("Line");
            lineObject.transform.SetParent(this.transform); // 设置为当前对象的子对象
            rawImage = lineObject.AddComponent<RawImage>();
        }


        void UpdateLine()
        {
            // 获取起点和终点的位置
            Vector2 startPos = doorPoss[0].anchoredPosition;
            Vector2 endPos = doorPoss[1].anchoredPosition;

            // 计算线条的长度和方向
            Vector2 direction = endPos - startPos;
            int width = Mathf.CeilToInt(direction.magnitude);
            int height = 5; // 线条宽度

            // 创建纹理
            Texture2D texture = new Texture2D(width, height);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    texture.SetPixel(x, y, Color.red); // 设置线条颜色
                }
            }
            texture.Apply();

            // 将纹理赋值给 RawImage
            rawImage.texture = texture;

            // 设置 RawImage 的位置和旋转
            RectTransform rawImageRect = rawImage.GetComponent<RectTransform>();
            rawImageRect.sizeDelta = new Vector2(width, height);
            rawImageRect.anchoredPosition = startPos + direction / 2;
            rawImageRect.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        }

    }

}
