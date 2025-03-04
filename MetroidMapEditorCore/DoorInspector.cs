using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidMapEditorCore
{
    public class DoorInspector : MonoBehaviour
    {
        public static DoorInspector current;
        public DoorBase _NowSelectDoor;
        // Start is called before the first frame update
        void Start()
        {
            if (!current)
                current = this;
            gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
