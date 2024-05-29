using System;
using UnityEngine;
using Unity.Entities;

namespace DefaultNamespace
{
    public class FollowEntity : MonoBehaviour
    {
        public GameObject player;

        private void LateUpdate()
        {
            transform.position = player.transform.position;
        }
    }
}