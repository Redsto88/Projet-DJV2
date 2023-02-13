using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    [SerializeField] private float speed = 2;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Transform highControl;
    
    public  GameObject playerPivot;
    public CharacterController characterController;
    public bool portalFlag;

    private float _highCheck;
    private bool _isGrounded;
    private float _gravity = 9.8f;
    private float _yVel;    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else 
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        portalFlag = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        _highCheck = highControl.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (!portalFlag)
        {
            // Déplacement du joueur
            Vector3 direction = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
            if (direction.magnitude > 1)
            {
                direction.Normalize();
            }

            // Gravité
            RaycastHit hit;
            
            var layerInt = LayerMask.GetMask("Ground");
            
            if (Physics.Raycast(highControl.position, highControl.up * -1, out hit, _highCheck+ 0.0001f, layerInt))
            {
                Debug.DrawRay(highControl.position, highControl.up * (-1 * hit.distance), Color.green);
                _isGrounded = true;
            }
            else
            {
                Debug.DrawRay(highControl.position, highControl.up * -2, Color.red);
                _isGrounded = false;
            }
            
            if (_isGrounded)
            {
                _yVel = 0;
            }
            else
            {
                _yVel -= _gravity * Time.deltaTime;
            }

            direction.y = _yVel;
            characterController.Move(direction * (speed * Time.deltaTime));

            // Rotation du joueur
            if (direction == Vector3.zero)
            {
                direction = playerPivot.transform.forward;
            }
            direction.y = 0;
            Quaternion tr = Quaternion.LookRotation(direction);
            playerPivot.transform.rotation = Quaternion.Slerp(playerPivot.transform.rotation, tr, rotationSpeed * Time.deltaTime);
        }
        
        //Hauteur du joueur
        //transform.position = new Vector3(transform.position.x,0,transform.position.z);
    }
}
