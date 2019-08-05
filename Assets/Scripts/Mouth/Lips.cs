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

        public IEnumerator OpenLipsAndWaitForFinish()
        {
            UpperLip.OpenLip();
            BottomLip.OpenLip();

            while (!UpperLip.IsStopped || !BottomLip.IsStopped)
            {
                yield return null;
            }
        }

        public IEnumerator CloseLipsAndWaitForFinish()
        {
            UpperLip.CloseLip();
            BottomLip.CloseLip();

            while (!UpperLip.IsStopped || !BottomLip.IsStopped)
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
