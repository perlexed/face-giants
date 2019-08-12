using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FaceGiants
{
    public class GameBounds : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject == Player.Instance.gameObject)
            {
                GameTimeline.Instance.PlayerDead();
            }
        }
    }
}
