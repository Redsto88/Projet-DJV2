using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cursor = UnityEngine.Cursor;

public class PortalCursor : MonoBehaviour
{
    private new Camera camera;
    public bool canSpawnPortal;
    public bool usingMouseInput;

    [SerializeField] private Material enableMaterial;
    [SerializeField] private Material disableMaterial;
    
    [Header("For Gamepad Controls Only")] 
    public float cursorSpeed = 10;
    
    private Vector2 _cursorPos;

    private Renderer _renderer;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        camera = Camera.main;
        _renderer = GetComponent<Renderer>();
        _renderer.material.color = Color.cyan;
        _cursorPos = new Vector2(Screen.width / 2, Screen.height / 2);
    }

    // Update is called once per frame
    void Update()
    {
        if (MapManager.Instance.paused || CinematicManager.cinematicPause) return;
        if(usingMouseInput)
        {
            _cursorPos = Input.mousePosition;
        }
        else
        {
            _cursorPos += new Vector2(Input.GetAxis("GamepadCursorHorizontal"), Input.GetAxis("GamepadCursorVertical")) * (cursorSpeed * Time.deltaTime);
            _cursorPos.x = Mathf.Clamp(_cursorPos.x, 0, Screen.width - 1);
            _cursorPos.y = Mathf.Clamp(_cursorPos.y, 0, Screen.height - 1);
        }
        
        if(camera == null) return;
        var ray = camera.ScreenPointToRay(_cursorPos);

        var layerInt = LayerMask.GetMask("Ground","Plateform");
        //var layerInt2 = ~(layerInt<<2);
        //print(layerInt + " // " + layerInt2);
        
        if (Physics.Raycast(ray, out var x, Mathf.Infinity, layerInt, QueryTriggerInteraction.UseGlobal))
        {
            canSpawnPortal = true;
            transform.position = x.point; 
            if (_renderer.material != enableMaterial)
            {
                _renderer.material = enableMaterial;
            }

        }
        else if (Physics.Raycast(ray, out var y, Mathf.Infinity, ~layerInt, QueryTriggerInteraction.UseGlobal))
        {
            canSpawnPortal = false;
            transform.position = y.point;
            if(_renderer.material != disableMaterial)
            {
                _renderer.material = disableMaterial;
            }
        }
    }
}
