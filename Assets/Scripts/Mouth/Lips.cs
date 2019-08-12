using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FaceGiants
{
    public class Lips : MonoBehaviour
    {
        public Lip UpperLip;
        public Lip BottomLip;

        private bool AreLipsMoving => !(UpperLip.IsStopped && BottomLip.IsStopped);

        public IEnumerator OpenLipsAndWaitForFinish()
        {
            UpperLip.OpenLip();
            BottomLip.OpenLip();

            while (AreLipsMoving)
            {
                yield return null;
            }
        }

        public IEnumerator CloseLipsAndWaitForFinish()
        {
            UpperLip.CloseLip();
            BottomLip.CloseLip();

            while (AreLipsMoving)
            {
                yield return null;
            }
        }


        public void Reset()
        {
            UpperLip.Reset();
            BottomLip.Reset();
        }
    }
}
