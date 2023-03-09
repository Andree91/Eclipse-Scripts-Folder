using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class RopeLenghtChanger : MonoBehaviour
    {
        Transform ropeTransform;
        float positionY;
        float scaleY;

        public GameObject platformTop;
        public Vector3 platformOriginalPos;

        void Awake() 
        {
            ropeTransform = GetComponent<Transform>();
            positionY = ropeTransform.position.y;
            scaleY = ropeTransform.localScale.y;
            platformOriginalPos = platformTop.transform.position;
        }

        void Update() 
        {
            if (platformTop.transform.position.y != platformOriginalPos.y)
            {
                ChangeRopeLenghtBasedOnPlatform();
            }
        }

        public void ChangeRopeLenghtBasedOnPlatform()
        {
            if (platformTop.transform.position.y < platformOriginalPos.y)
            {
                // Change rope position to lower
                ropeTransform.position = new Vector3(ropeTransform.position.x, platformTop.transform.position.y, ropeTransform.position.z);
                
                // Extend the rope lenght
                ropeTransform.localScale = new Vector3 (ropeTransform.localScale.x, (-platformTop.transform.position.y + (platformOriginalPos.y + 0.35f)) / 2f, ropeTransform.localScale.z);
            }
            else if (platformTop.transform.position.y > platformOriginalPos.y)
            {
                // Change rope position to higer
                ropeTransform.position = new Vector3(ropeTransform.position.x, platformTop.transform.position.y, ropeTransform.position.z);

                // Shrink the rope lenght
                ropeTransform.localScale = new Vector3(ropeTransform.localScale.x, (platformTop.transform.position.y - (platformOriginalPos.y + 0.35f)) / 2f, ropeTransform.localScale.z);
            }
            else
            {
                Debug.Log("The platform top and originalPos are the same");
            }
        }
    }
}
