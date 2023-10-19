// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.SceneManagement;

// public class SceneLikeCamera : MonoBehaviour
// {
// //     [Header("Movement")]
// //     private float moveSpeed = 15.0f;
// //     private float rotateSpeed = 5.0f;
// //     private float zoomSpeed = 100.0f;


// //     private KeyCode forwardKey = KeyCode.O;
// //     private KeyCode backKey = KeyCode.P;
// //     private KeyCode leftKey = KeyCode.LeftArrow;
// //     private KeyCode rightKey = KeyCode.RightArrow;

// //     private KeyCode upKey = KeyCode.UpArrow;
// //     private KeyCode downKey = KeyCode.DownArrow;

// //     // public float distance = 10.0f;
// //     // public float height = 10.0f;
// //     // public float rotationDamping = 3.0f;
// //     private GameObject target = NavMeshLine.origin;
// //     private float height = 50.0f; // the height of the camera above the target
// //     private float distance = 50.0f; // the distance between the camera and the target
// //     private float rotationDamping = 3.0f;

    

//     void Start()
//     {
//         SceneManager.LoadScene("Scenes/N2");
//         Debug.Log("move along line switched scene");
// //         //  if (target != null)
// //         // {
// //         //     // calculate the desired position and rotation of the camera
// //         //     Vector3 targetPosition = target.transform.position + Vector3.up * height;
// //         //     Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position, Vector3.up);

// //         //     // move the camera towards the target position and rotation
// //         //     transform.position = Vector3.Lerp(transform.position, targetPosition - target.transform.forward * distance, Time.deltaTime * 5.0f);
// //         //     transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationDamping);
// //         // }
//     }
    
// //     void Update()
// //     {
        
// //     }

// //     // void LateUpdate()
// //     // {
// //     //     
// //     // }

// //     private void FixedUpdate()
// //     {
// //         Vector3 move = Vector3.zero;

// //         if (Input.GetKey(forwardKey))
// //             move += Vector3.forward * moveSpeed;
// //         if (Input.GetKey(backKey))
// //             move += Vector3.back * moveSpeed;
// //         if (Input.GetKey(rightKey))
// //             move += Vector3.right * moveSpeed;
// //         if (Input.GetKey(leftKey))
// //             move += Vector3.left * moveSpeed;

// //         if (Input.GetKey(upKey))
// //             move += Vector3.up * moveSpeed;
// //         if (Input.GetKey(downKey))
// //             move += Vector3.down  * moveSpeed;
        
        
        

// //         // if (Input.GetKey(anchoredRotateKey))
// //         // {
// //         //     transform.RotateAround(transform.position, transform.right, mouseMoveY * rotateSpeed);
// //         //     transform.RotateAround(transform.position, Vector3.up, mouseMoveX * rotateSpeed);
// //         // }


// //         transform.Translate(move);
// //     }

// //     // private void LateUpdate()
// //     // {
// //     //     float mouseScroll = Input.GetAxis("Mouse ScrollWheel");
// //     //     transform.Translate(Vector3.forward * mouseScroll);

// //     // }
// }
