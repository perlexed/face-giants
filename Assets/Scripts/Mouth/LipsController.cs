using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FaceGiants {
    public class LipsController : MonoBehaviour
    {
        public GameObject upperLip;
        public GameObject bottomLip;

        private LipController upperLipController;
        private LipController bottomLipController;

        public bool areLipsOpened = false;

        private int lipsSyncCounter;
        private MouthController.OnLipsStatusChange localCallback;

        public delegate void OnLipStatusChange(bool status);

        void Start()
        {
            upperLipController = upperLip.GetComponent<LipController>();
            bottomLipController = bottomLip.GetComponent<LipController>();
        }

        public void OpenLips(MouthController.OnLipsStatusChange parentCallback)
        {
            localCallback = parentCallback;
            OnLipStatusChange callback = OnLipStatusChangeCallback;

            lipsSyncCounter = 0;
            upperLipController.OpenLip(callback);
            bottomLipController.OpenLip(callback);
        }

        public IEnumerator OpenLipsAndWaitForFinish()
        {
            upperLipController.OpenLip();
            bottomLipController.OpenLip();
            while (upperLipController.IsRunning || bottomLipController.IsRunning)
                yield return null;
        }

        public void CloseLips(Func<int, int> parentCallback)
        {
            OnLipStatusChange callback = OnLipStatusChangeCallback;

            upperLipController.CloseLip(callback);
            bottomLipController.CloseLip(callback);
            parentCallback(4);

        }

        public void Calc<T>(T obj)
        {
            GetComponentsInChildren<LipsController>();
            GetComponentsInChildren();
        }

        void OnLipStatusChangeCallback(bool status)
        {
            if (status)
            {
                lipsSyncCounter++;
            } else
            {
                lipsSyncCounter--;
            }

            Debug.Log("lip status callback <b>" + (status ? "opened" : "closed") + "</b>, result lipsSyncCounter: " + lipsSyncCounter);

            if (lipsSyncCounter != 1)
            {
                localCallback();
            }
        }
    }
}
