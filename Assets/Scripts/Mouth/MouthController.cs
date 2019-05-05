using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FaceGiants
{
    public class MouthController : MonoBehaviour
    {
        public GameObject lipsContainer;
        public GameObject jawsContainer;

        private LipsController lipsController;
        private int currentStep;

        public delegate void OnLipsStatusChange();
        public delegate void OnJawsStatusChange();

        void Start()
        {
            lipsController = lipsContainer.GetComponent<LipsController>();
            currentStep = 0;
        }

        void RunNextStep()
        {
            switch (currentStep)
            {
                case 0:
                    OpenLips();
                    currentStep = 1;
                    break;
                case 1:
                    FireTeeth();
                    currentStep = 2;
                    break;
                case 2:
                    CloseLips();
                    currentStep = 0;
                    break;
            }
        }

        public LipsController LipsController => lipsContainer.GetComponent<LipsController>();

        void OpenLips()
        {
            OnLipsStatusChange callback = OnLipsStatusChangeCallback;
            lipsContainer.GetComponent<LipsController>().OpenLips(callback);
        }

        void FireTeeth()
        {
            OnJawsStatusChange callback = OnJawsStatusChangeCallback;
            jawsContainer.GetComponent<JawsController>().FireTeeth(callback);
        }

        void CloseLips()
        {

            lipsContainer.GetComponent<LipsController>().CloseLips(OnLipsStatusChangeCallback);
        }

        public IEnumerator LifecycleCoroutine()
        {
            for(; ; )
            {
                yield return LipsController.OpenLipsAndWaitForFinish();

                //Open mouth
                //wait while opening finish
                //wait 2 sec
                //Fire theeth
                //wait finish
                //close
                //wait finish
                //pause 2

                //yield return new WaitUntil()

            }
        }

        public void RunMouthEvent()
        {
            if (currentStep == 0)
            {
                RunNextStep();
            }

        }

        public void OnLipsStatusChangeCallback()
        {
            RunNextStep();
        }

        public void OnJawsStatusChangeCallback()
        {
            RunNextStep();
        }
    }
}
