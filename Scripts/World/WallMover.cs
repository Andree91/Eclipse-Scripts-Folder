using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class WallMover : MonoBehaviour
    {
        public Vector3 moveTo;
        public LeverMoveWalls leverMoveWalls;
        public GameObject dustParticle;
        public Transform particlePosition;

        Vector3 originalPosition;
        Vector3 translationPosition;
        //Animator wallsAnimator;
        public List<Collider> wallColliders = new List<Collider>();

        void OnEnable()
        {
            //wallsAnimator = GetComponent<Animator>();
            originalPosition = transform.position;
        }

        public void MoveWalls()
        {
            foreach (Collider collider in wallColliders)
            {
                collider.enabled = false;
            }

            Instantiate(dustParticle, particlePosition.position, Quaternion.identity, transform);
            Instantiate(dustParticle, particlePosition.position, Quaternion.identity, transform);

            StartCoroutine(MoveWallsOverTime());
            Destroy(gameObject, 11f);
        }

        IEnumerator MoveWallsOverTime()
        {
            float tempTime = Time.deltaTime;
            float timer = 0;

            while (timer <= tempTime + 10f)
            {
                if (timer == 0.3f)
                {
                    Instantiate(dustParticle, particlePosition.position, Quaternion.identity, transform);
                }
                timer += Time.deltaTime;
                transform.position = Vector3.Lerp(transform.position, moveTo, Time.deltaTime * 0.3f);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
