using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    [SerializeField] private float speed = 2;
    [SerializeField] private float rotationSpeed;
    
    public  GameObject playerPivot;
    public CharacterController characterController;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Déplacement du joueur
        Vector3 direction = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
        if (direction.magnitude > 1)
        {
            direction.Normalize();
        }
        characterController.Move(direction * (speed * Time.deltaTime));

        // Rotation du joueur
        if (direction == Vector3.zero)
        {
            direction = playerPivot.transform.forward;
        }
        Quaternion tr = Quaternion.LookRotation(direction);
        playerPivot.transform.rotation = Quaternion.Slerp(playerPivot.transform.rotation, tr, rotationSpeed * Time.deltaTime);
        
    }
}
