using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    [SerializeField] private float mouseSensitivity = 700f;
    public Transform cameraTransform;

    private CharacterController controller;
    private float yaw;   // ángulo Y
    private float pitch; // ángulo X

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        cameraTransform = GetComponentInChildren<Camera>().transform;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {


        // 4) Movimiento en XZ
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 dir = transform.forward * z + transform.right * x;
        dir.y = 0f; 
        if (dir.sqrMagnitude > 1f) dir.Normalize();

        controller.Move(dir * moveSpeed * Time.deltaTime);
    }
    
    private void UpdateCamera(){
        //Capturamos los movimientos del ratón
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //Calculamos la rotación de la cámara
        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -80f, 80f);

        //Aplicamos la rotación de la cámara
        transform.localRotation = Quaternion.Euler(0f, yaw, 0f);
        cameraTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }
}
