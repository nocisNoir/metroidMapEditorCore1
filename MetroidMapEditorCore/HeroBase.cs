using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidMapEditorCore
{
    //玩家，永远存在于一个房间内，可以通过输入移动到其它房间
    public class HeroBase : MonoBehaviour
    {
        public RoomBase _NowAtRoom;
        public GameObject _HeroObj;
        public static HeroBase _MainHero;
        public static List<HeroBase> _testHeros;
        bool selectDoorState;
        // Start is called before the first frame update
        void Start()
        {
            if (!_MainHero)
                _MainHero = this;
            if (_HeroObj)
                _HeroObj.SetActive(false);
                
        }

        // Update is called once per frame
        void Update()
        {
            if (_HeroObj.activeInHierarchy && _NowAtRoom != null)
            {
                HeroMoveInput();
            }
        }

        public void CreateHero(RoomBase room,Vector2 offset=default)
        {
            RemoveHero();
            _HeroObj.SetActive(true);
            MoveToRoom(room, offset);
        }

        public void MoveToRoom(RoomBase room,Vector2 offset = default)
        {
            _HeroObj.transform.SetParent(room._MainRoomRect);
            if (offset == default)
                offset = Vector2.zero;
            _HeroObj.transform.localPosition = offset;
            _NowAtRoom = room;
        }

        public void RemoveHero()
        {
            if (_NowAtRoom)
                _NowAtRoom = null;
            _HeroObj.SetActive(false);
        }
        public void MoveOutDoor(DoorBase door)
        {
            if (!door._DoorWay)
            {
                Debug.LogWarning($"此门{door}不存在路线");
                return;
            }
            DoorBase toDoor = door._DoorWay.getOtherDoor(door);
            if (toDoor != default)
            {
                //执行一次进门
                if (door.checkDoorAbilityRequire())
                {
                    Debug.Log("进入新房间成功，需补充进出门函数");
                    MoveToRoom(toDoor._AttachRoom);
                }
                else
                {
                    Debug.Log("进门检测不通过");
                }

            }
            else
            {
                
            }
        }


        public void HeroMoveInput()
        {
            if (selectDoorState)
                return;
            if (_HeroObj.activeInHierarchy&&_NowAtRoom!=null)
            {

            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (_NowAtRoom.getEdgeDoors(RectTransform.Edge.Top).Count <= 0)
                {
                    Debug.LogWarning($"此房间的{RectTransform.Edge.Top}边不存在门");
                    return;
                }
                KeyCode[] keys = new KeyCode[2]
                {
                    KeyCode.UpArrow,KeyCode.Return
                };
                StartCoroutine(ChooseDoorInput(KeyCode.RightArrow, KeyCode.LeftArrow, keys, _NowAtRoom.getEdgeDoors(RectTransform.Edge.Top)));
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (_NowAtRoom.getEdgeDoors(RectTransform.Edge.Bottom).Count <= 0)
                {
                    Debug.LogWarning($"此房间的{RectTransform.Edge.Bottom}边不存在门");
                    return;
                }
                KeyCode[] keys = new KeyCode[2]
                {
                    KeyCode.DownArrow,KeyCode.Return
                };
                StartCoroutine(ChooseDoorInput(KeyCode.LeftArrow, KeyCode.RightArrow, keys, _NowAtRoom.getEdgeDoors(RectTransform.Edge.Bottom)));

            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (_NowAtRoom.getEdgeDoors(RectTransform.Edge.Left).Count <= 0)
                {
                    Debug.LogWarning($"此房间的{RectTransform.Edge.Left}边不存在门");
                    return;
                }
                KeyCode[] keys = new KeyCode[2]
                {
                    KeyCode.LeftArrow,KeyCode.Return
                };
                StartCoroutine(ChooseDoorInput(KeyCode.UpArrow, KeyCode.DownArrow, keys, _NowAtRoom.getEdgeDoors(RectTransform.Edge.Left)));
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (_NowAtRoom.getEdgeDoors(RectTransform.Edge.Right).Count <= 0)
                {
                    Debug.LogWarning($"此房间的{RectTransform.Edge.Right}边不存在门");
                    return;
                }
                KeyCode[] keys = new KeyCode[2]
                {
                    KeyCode.RightArrow,KeyCode.Return
                };
                StartCoroutine(ChooseDoorInput(KeyCode.DownArrow, KeyCode.UpArrow, keys, _NowAtRoom.getEdgeDoors(RectTransform.Edge.Right)));
            }
        }

        IEnumerator ChooseDoorInput(KeyCode positiveKey,KeyCode negativeKey,KeyCode[] outDoorKey,List<DoorBase> doors)
        {
            selectDoorState = true;
            int nowDoor = 0; // 当前选择的门索引
            int lastDoor = 0;
            doors[0].refreshDoorAltOutLine(true);
            yield return new WaitUntil(() => !Input.anyKey);
            // 循环检测输入，直到按下 outDoorKey
            while (true)
            {
                // 检测 positiveKey 输入
                if (Input.GetKeyDown(positiveKey))
                {
                    nowDoor++;
                    nowDoor = Mathf.Clamp(nowDoor, 0, doors.Count - 1);
                }

                // 检测 negativeKey 输入
                if (Input.GetKeyDown(negativeKey))
                {
                    nowDoor--;
                    nowDoor = Mathf.Clamp(nowDoor, 0, doors.Count - 1);
                }

                // 检测 outDoorKey 输入
                foreach(KeyCode key in outDoorKey)
                {
                    if (Input.GetKeyDown(key))
                    {
                        Debug.Log("最终选择门索引: " + nowDoor);
                        selectDoorState = false;
                        yield return new WaitForSeconds(.5f);
                        doors[nowDoor].refreshDoorAltOutLine(false);
                        MoveOutDoor(doors[nowDoor]);
                        yield break; // 结束协程
                    }
                }
                if (lastDoor != nowDoor)
                {
                    doors[lastDoor].refreshDoorAltOutLine(false);
                    doors[nowDoor].refreshDoorAltOutLine(true);
                    lastDoor = nowDoor;
                }
                yield return null; // 等待下一帧
            }
        }
    }

}



