using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidMapEditorCore
{
    public class SampleUIObjs : MonoBehaviour
    {
        public static SampleUIObjs main;
        public RoomBase sampleRoom;
        public DoorBase sampleDoor;
        // Start is called before the first frame update
        void Start()
        {
            if (!main)
                main = this;

            if (!sampleRoom)
                sampleRoom = GetComponentInChildren<RoomBase>();
            if (!sampleDoor)
                sampleDoor = GetComponentInChildren<DoorBase>();
            if (sampleRoom)
                sampleRoom.gameObject.SetActive(false);
            if (sampleDoor)
                sampleDoor.gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

