using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FaceGiants
{
    public class JawController : MonoBehaviour
    {
        private GameObject contextTeeth;
        private Color originalTeethColor;

        private JawsController.OnJawStatusChange localCallback;

        void Start()
        {
            SpriteRenderer[] spriteComponents = gameObject.GetComponentsInChildren<SpriteRenderer>();

            foreach (SpriteRenderer spriteComponent in spriteComponents)
            {
                //spriteComponent.color = Color.green;
            }
            //gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        }

        public void FireRandomTeeth(JawsController.OnJawStatusChange callback)
        {
            localCallback = callback;

            int randomTeethIndex = Random.Range(1, transform.childCount);
            contextTeeth = transform.GetChild(randomTeethIndex).gameObject;
            originalTeethColor = contextTeeth.GetComponent<SpriteRenderer>().color;

            StartCoroutine(TeethFireCycle());
        }

        IEnumerator TeethFireCycle()
        {
            contextTeeth.GetComponent<SpriteRenderer>().color = Color.green;

            yield return new WaitForSeconds(5);

            contextTeeth.GetComponent<SpriteRenderer>().color = originalTeethColor;

            localCallback();
        }
    }

}
