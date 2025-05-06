using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assests.Script
{
    public class CameraController : MonoBehaviour
    {
        public float normalSpeed;
        public float fastSpeed;
        private float movementSpeed;
        public float movementTime;
        public float zoomSpeed;

        public Vector3 newPosition;
        public Quaternion newRotation;

        void Start()
        {
            newRotation = transform.rotation;
            newPosition = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            HandleMovementInput();
        }

        void HandleMovementInput()
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                movementSpeed = fastSpeed;
            }
            else
            {
                movementSpeed = normalSpeed;
            }
            if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.UpArrow))
            {
                newPosition += (transform.forward * movementSpeed);
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                newPosition += (transform.forward * -movementSpeed);
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                newPosition += (transform.right * movementSpeed);
            }
            if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.LeftArrow))
            {
                newPosition += (transform.right * -movementSpeed);
            }
            if (Input.GetKey(KeyCode.A))
            {
                newRotation *= Quaternion.Euler(Vector3.up * -movementSpeed);
            }
            if (Input.GetKey(KeyCode.E))
            {
                newRotation *= Quaternion.Euler(Vector3.up * movementSpeed);
            }
            float scroll = Input.mouseScrollDelta.y;
            if (scroll != 0f)
            {
                newPosition += transform.up * scroll * zoomSpeed;
            }
            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
        }
    }
}