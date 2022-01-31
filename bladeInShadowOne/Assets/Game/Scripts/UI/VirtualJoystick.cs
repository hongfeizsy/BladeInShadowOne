using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


// From youtube: Unity 5 Virtual Joystick [Tutorial][C#] - Unity 3d
namespace RPG.Core
{
    public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
    {
        private Image bgImg;
        private Image joystickImg;
        private Vector3 inputVec;

        private void Start()
        {
            bgImg = GetComponent<Image>();
            joystickImg = transform.GetChild(0).GetComponent<Image>();
            inputVec = Vector3.zero;
        }

        public virtual void OnPointerDown(PointerEventData ped)
        {
            OnDrag(ped);
        }

        public virtual void OnPointerUp(PointerEventData ped)
        {
            inputVec = Vector3.zero;
            joystickImg.rectTransform.anchoredPosition = Vector3.zero;
        }

        public virtual void OnDrag(PointerEventData ped)
        {
            Vector2 pos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImg.rectTransform, ped.position, ped.pressEventCamera, out pos))
            {
                pos.x = pos.x / bgImg.rectTransform.sizeDelta.x;
                pos.y = pos.y / bgImg.rectTransform.sizeDelta.y;

                inputVec = pos;
                inputVec = (inputVec.magnitude > 1f) ? inputVec.normalized : inputVec;
                joystickImg.rectTransform.anchoredPosition = new Vector3(inputVec.x * bgImg.rectTransform.sizeDelta.x / 2f,
                    inputVec.y * bgImg.rectTransform.sizeDelta.y / 2f, 0);
            }
        }

        public Vector3 GetJoystickInputVector()
        {
            float mutiplier = 1f;  // This is only for Android phone. 
            return new Vector3(inputVec.x, 0, inputVec.y) * mutiplier;
        }
    }
}
