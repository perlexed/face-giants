using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FaceGiants
{
    public class JawsController : MonoBehaviour
    {
        public JawController upperJaw;
        public JawController bottomJaw;

        protected JawController contextJaw;

        public IEnumerator FireTeethAndWaitForFinish()
        {
            contextJaw = Random.Range(0, 2) >= 1 ? upperJaw : bottomJaw;
            yield return contextJaw.FireRandomTeethAndWaitForFinish();
        }

        public void Reset()
        {
            if (contextJaw)
            {
                contextJaw.Reset();

                // @todo refactor this
                StopAllCoroutines();
            }
        }
    }
}
