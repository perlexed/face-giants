using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FaceGiants
{
    public class Mouth : MonoBehaviour
    {
        public float pauseDuration = 2f;
        public Lips Lips;
        public Jaws Jaws;

        public void Reset()
        {
            Lips.Reset();
            Jaws.Reset();
        }

        public IEnumerator RunLifecycle()
        {
            yield return Lips.OpenLipsAndWaitForFinish();
            yield return new WaitForSeconds(pauseDuration);
            yield return Jaws.FireTeethAndWaitForFinish();
            yield return new WaitForSeconds(pauseDuration);
            yield return Lips.CloseLipsAndWaitForFinish();
        }
    }
}
