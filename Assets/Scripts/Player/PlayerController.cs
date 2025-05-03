using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    [SerializeField] private float mouseSensitivity = 700f;
    public Transform cameraTransform;

    private CharacterController controller;
    private float yaw;
    private float pitch;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        cameraTransform = GetComponentInChildren<Camera>().transform;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {

        this.UpdateCamera();

        this.UpdateMovement();
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

    private void UpdateMovement()
    {
        //Capturamos los movimientos del teclado
        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveZ = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        //Aplicamos más velocidad si el shift está pulsado
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            moveX *= 2f;
            moveZ *= 2f;
        }

        //Calculamos la dirección de movimiento
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        //Aplicamos la gravedad
        move.y -= 9.81f * Time.deltaTime;

        //Movemos al jugador
        controller.Move(move);
    }


}
