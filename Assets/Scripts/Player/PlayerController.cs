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
    public bool respawnFlag;
    
    private float _highCheck;
    private bool _isGrounded;
    [SerializeField] private float _gravity = 1f;
    private float _yVel;    
    private void Awake()
    {
        if(Instance != null) Destroy(Instance.gameObject);
        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        portalFlag = false;
        respawnFlag = false;
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
        if (!portalFlag && !respawnFlag)
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

            //launch n raycast around the player to check if he is grounded
            _isGrounded = false;
            float rayon = 0.5f;
            int n = 10;
            for(int i=0; i<n; i++){
                float angle = i * 2 * Mathf.PI / n;
                Vector3 dir = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
                dir *= rayon;
                if (Physics.Raycast(highControl.position + dir, highControl.up * -1, out hit, _highCheck+ 0.0001f))
                {
                    Debug.DrawRay(highControl.position + dir, highControl.up * (-1 * hit.distance), Color.green);
                    _isGrounded = true;
                }
                else
                {
                    Debug.DrawRay(highControl.position + dir, highControl.up * -2, Color.red);
                }
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
            direction.y = 0;
            if (direction == Vector3.zero)
            {
                direction = playerPivot.transform.forward;
            }
            Quaternion tr = Quaternion.LookRotation(direction);
            playerPivot.transform.rotation = Quaternion.Slerp(playerPivot.transform.rotation, tr, rotationSpeed * Time.deltaTime);
        }
        
        //Hauteur du joueur
        //transform.position = new Vector3(transform.position.x,0,transform.position.z);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
