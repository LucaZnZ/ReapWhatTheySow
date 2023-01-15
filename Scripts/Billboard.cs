using System;
using UnityEngine;

namespace ZnZUtil
{
    public class Billboard : MonoBehaviour
    {
        private new Camera camera;
        private Vector3 oldOrientation;

        private enum Method
        {
            CamForward,
            CamLookAt
        }

        [SerializeField] private Method method;
        [SerializeField] private bool invert;

        private void DoBillboard()
        {
            switch (method)
            {
                case Method.CamForward:
                    transform.forward = camera.transform.forward;
                    break;
                case Method.CamLookAt:
                    transform.LookAt(camera.transform);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (invert)
                transform.forward *= -1;
        }

        private void Start()
        {
            camera = Camera.main;
            oldOrientation = transform.forward;
        }

        private void Update()
        {
            if (enabled) DoBillboard();
        }

        private void OnDisable()
        {
            transform.forward = oldOrientation;
        }
    }
}