using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class AIWanderArea : MonoBehaviour
    {
        public enum AreaType { Circle, Box };


        [Tooltip("Type of Area to wander")]
        public AreaType m_AreaType = AreaType.Circle;

        [Min(0)] public float radius = 5;

        public Vector3 BoxArea = new Vector3(10, 1, 10);

        [Range(0, 1), Tooltip("Probability of keep wandering on this WayPoint Area")]
        public float WanderWeight = 1f;

        public Vector3 destination;

        private Transform currentNextTarget;
        public float stoppingDistance;
        public float slowingDistance;
    }
}
