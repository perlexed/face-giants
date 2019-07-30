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

        void Start()
        {
            upperLipController = upperLip.GetComponent<LipController>();
            bottomLipController = bottomLip.GetComponent<LipController>();
        }

        public IEnumerator OpenLipsAndWaitForFinish()
        {
            upperLipController.OpenLip();
            bottomLipController.OpenLip();

            while (!upperLipController.isStopped || !bottomLipController.isStopped)
            {
                yield return null;
            }
        }

        public IEnumerator CloseLipsAndWaitForFinish()
        {
            upperLipController.CloseLip();
            bottomLipController.CloseLip();

            while (!upperLipController.isStopped || !bottomLipController.isStopped)
            {
                yield return null;
            }
        }

        public void Reset()
        {
            upperLip.transform.localPosition = new Vector2(0, 0.6f);
            bottomLip.transform.localPosition = new Vector2(0, -0.6f);

            // @todo refactor this
            StopAllCoroutines();
        }
    }
}
