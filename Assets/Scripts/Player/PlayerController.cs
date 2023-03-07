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
    public bool damageFlag;
    public bool canMove;
    public GameObject _currentPlateform; 

    [SerializeField] private float _gravity = 1f;
    public float _yVel;    
    private void Awake()
    {
        if(Instance != null) Destroy(Instance.gameObject);
        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        portalFlag = false;
        respawnFlag = false;
        damageFlag = false;
        canMove = true;
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
        if(damageFlag){
            characterController.Move((transform.forward + transform.right).normalized * (-5*Time.deltaTime));
        }
        else{
            if (!portalFlag && !respawnFlag && canMove)
            {
                // Déplacement du joueur
                Vector3 direction = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
                if (direction.magnitude > 1)
                {
                    direction.Normalize();
                }
                direction *= speed*Time.deltaTime;

                // Gravité + plateformes
                RaycastHit hit;
                
                var layerInt = LayerMask.GetMask("Ground");

                //launch n raycast around the player to check if he is grounded
                _isGrounded = false;
                bool isOnPlateform = false;
                float rayon = 0.5f;
                int n = 10;
                for(int i=0; i<n; i++){
                    float angle = i * 2 * Mathf.PI / n;
                    Vector3 dir = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
                    dir *= rayon;
                    
                    if (Physics.Raycast(highControl.position + dir, highControl.up * -1, out hit, _highCheck+ 0.0001f))
                    {
                    
                        if(hit.transform.gameObject.layer == 10)
                        {
                            isOnPlateform = true;
                            _currentPlateform = hit.collider.gameObject;
                        }
                        Debug.DrawRay(highControl.position + dir, highControl.up * (-1 * hit.distance), Color.green);
                        _isGrounded = true;
                    }
                    else
                    {
                        Debug.DrawRay(highControl.position + dir, highControl.up * -2, Color.red);
                    }
                }
                if(_currentPlateform == null){
                    isOnPlateform = false;
                }
                if(isOnPlateform)
                {
                    PlateformeBoss plateform = _currentPlateform.GetComponentInParent<PlateformeBoss>();
                    if(plateform.isUp){
                        rotationSpeed = plateform.rotationSpeed;
                        Vector3 rotationCenter = plateform.transform.position;
                        Vector3 playerPos = transform.position;
                        Vector3 playerPosRelative = playerPos - rotationCenter;
                        Vector3 playerPosRelativeRotated = Quaternion.AngleAxis(plateform.rotationSpeed * Time.deltaTime, Vector3.up) * playerPosRelative;
                        Vector3 playerPosRotated = playerPosRelativeRotated + rotationCenter;
                        direction+=(playerPosRotated - playerPos);
                    }
                    else{
                        direction.y+=((_currentPlateform.transform.position - transform.position).y)*2;
                    }
                }
                else
                {
                    _currentPlateform = null;
                }
                
                if (_isGrounded)
                {
                    _yVel = 0;
                }
                else
                {
                    _yVel -= _gravity * Time.deltaTime*Time.deltaTime*speed;
                }

                direction.y = _yVel;
                characterController.Move(direction);

                // Rotation du joueur
                direction.y = 0;
                if (direction == Vector3.zero)
                {
                    direction = playerPivot.transform.forward;
                }
                Quaternion tr = Quaternion.LookRotation(direction);
                playerPivot.transform.rotation = Quaternion.Slerp(playerPivot.transform.rotation, tr, rotationSpeed * Time.deltaTime);
            }
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
