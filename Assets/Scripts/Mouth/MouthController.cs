using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FaceGiants
{
    public class MouthController : MonoBehaviour
    {
        public GameObject lipsContainer;
        public GameObject jawsContainer;

        public LipsController LipsController => lipsContainer.GetComponent<LipsController>();
        public JawsController JawsController => jawsContainer.GetComponent<JawsController>();

        public void Reset()
        {
            LipsController.Reset();
            JawsController.Reset();
        }

        public IEnumerator LifecycleCoroutine()
        {
            yield return LipsController.OpenLipsAndWaitForFinish();
            yield return new WaitForSeconds(2);
            yield return JawsController.FireTeethAndWaitForFinish();
            yield return new WaitForSeconds(2);
            yield return LipsController.CloseLipsAndWaitForFinish();
        }
    }
}
