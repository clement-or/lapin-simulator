using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitMouseCamera : MonoBehaviour
{
     [Header("Cible")]
     public Transform target;
     
     [Header("Paramètres de la caméra")]
     public float distance = 2.0f;
     public Vector2 speed = new Vector2(20, 20);
     public Range yLimit = new Range(-90, 90);
     public float smoothTime = 2f;
     
     // Variables de travail
     private Vector2 _camRotation = Vector2.zero;
     private Vector2 _velocity = Vector2.zero;
     
     // Components
     private Rigidbody _rb;

     void Start()
     {
         Debug.Assert(_rb); // La caméra doit avoir un Rigidbody

         // Récupérer les components
         _rb = GetComponent<Rigidbody>();
         
         _camRotation = transform.eulerAngles;  // Initialiser la rotation
         _rb.freezeRotation = true;             // Bloquer la rotation du Rigidbody
     }
     
     void Update()
     {
         if (target.Equals(null)) return;
         
         // Calculer la vélocité selon l'input du joueur
        _velocity.x += speed.x * Input.GetAxis("Mouse X") * distance * Time.deltaTime;
        _velocity.y += speed.y * Input.GetAxis("Mouse Y") * Time.deltaTime;

        _camRotation = _velocity;
        _camRotation.x = Helpers.ClampAngle(_camRotation.x, yLimit.min, yLimit.max);    // Bloquer la caméra pour ne pas qu'elle fasse un 360 no scope

        Quaternion rot = transform.rotation;
        Quaternion startRotation = Quaternion.Euler(rot.eulerAngles.x, rot.eulerAngles.y, 0);    // Retirer la composante Z
        Quaternion targetRotation = Quaternion.Euler(_camRotation.x, _camRotation.y, 0);
        
        // Gérer les murs (la caméra s'approche quand un obstacle se trouve entre elle et la cible)
        RaycastHit hit;
        if (Physics.Linecast(target.position, transform.position, out hit))
            distance -= hit.distance;
        
        Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
        Vector3 targetPosition = targetRotation * negDistance + target.position;

        // Appliquer les transformations
        transform.rotation = targetRotation;
        transform.position = targetPosition;

        // Réduire la vélocité petit à petit
        _velocity = Vector3.Lerp(_velocity, Vector3.zero, Time.deltaTime * smoothTime);
     }

}


