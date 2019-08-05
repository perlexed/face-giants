using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FaceGiants
{
    public class Jaws : MonoBehaviour
    {
        public Jaw UpperJaw;
        public Jaw BottomJaw;

        private Jaw _contextJaw = null;

        public IEnumerator FireTeethAndWaitForFinish()
        {
            _contextJaw = Random.Range(0f, 1f) >= 0.5f ? UpperJaw : BottomJaw;
            yield return _contextJaw.FireRandomTeethAndWaitForFinish();
            _contextJaw = null;
        }

        public void Reset()
        {
            if (_contextJaw != null)
            {
                _contextJaw.Reset();
            }
        }
    }
}
