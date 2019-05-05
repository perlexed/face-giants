using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FaceGiants
{
    public class JawsController : MonoBehaviour
    {
        public GameObject upperJaw;
        public GameObject bottomJaw;

        private MouthController.OnJawsStatusChange localCallback;

        public delegate void OnJawStatusChange();

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void FireTeeth(MouthController.OnJawsStatusChange callback)
        {
            localCallback = callback;

            OnJawStatusChange jawCallback = OnJawStatusChangeCallback; 
            upperJaw.GetComponent<JawController>().FireRandomTeeth(jawCallback);
        }

        public void OnJawStatusChangeCallback()
        {
            localCallback();
        }
    }
}
