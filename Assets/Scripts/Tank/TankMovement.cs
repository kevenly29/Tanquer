using UnityEngine;

public class TankMovement : MonoBehaviour
{
    public int m_PlayerNumber = 1;         
    public float m_Speed = 12f;            
    public float m_TurnSpeed = 180f;       
    public AudioSource m_MovementAudio;    
    public AudioClip m_EngineIdling;       
    public AudioClip m_EngineDriving;      
    public float m_PitchRange = 0.2f;

    
    private string m_MovementAxisName;     
    private string m_TurnAxisName;         
    private Rigidbody m_Rigidbody;         
    private float m_MovementInputValue;    
    private float m_TurnInputValue;        
    private float m_OriginalPitch;         


    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }


    private void OnEnable ()
    {
        // Al arrancar / habilitar el tanque, deshabilitamos la kinemática del tanque para que se pueda mover.
        m_Rigidbody.isKinematic = false;
        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f;
    }


    private void OnDisable ()
    {
        m_Rigidbody.isKinematic = true;
    }


    private void Start()
    {
        m_MovementAxisName = "Vertical" + m_PlayerNumber;
        m_TurnAxisName = "Horizontal" + m_PlayerNumber;

        m_OriginalPitch = m_MovementAudio.pitch;
    }
    

    private void Update()
    {
        // Store the player's input and make sure the audio for the engine is playing.
        // Almaceno los valores de entrada.
        m_MovementInputValue = Input.GetAxis(m_MovementAxisName);
        m_TurnInputValue = Input.GetAxis(m_TurnAxisName);
        
 //Llamo a la función que gestiona el audio del motor
            EngineAudio();

    }


    private void EngineAudio()
    {
        // Play the correct audio clip based on whether or not the tank is moving and what audio is currently playing.
        // Si no hay entrada, es que está quieto...
        if (Mathf.Abs(m_MovementInputValue) < 0.1f && Mathf.Abs(m_TurnInputValue) < 0.1f)
        {
            // ... y si estaba reproduciendo el audio de moverse...
            if (m_MovementAudio.clip == m_EngineDriving)
            {
                // ... cambio el audio al de estar parado y lo reproduzco.
                m_MovementAudio.clip = m_EngineIdling;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }

        }
        else
        {
            //Si hay entrada es que se está moviendo. Si estaba reproduciendo eld e estar parado...
            if (m_MovementAudio.clip == m_EngineIdling)
            {
                // ... cambio el audio al de moverse y lo reproduzco.
                m_MovementAudio.clip = m_EngineDriving;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch -
                m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
    }

        private void FixedUpdate()
    {
        // Move and turn the tank.
        Move();
        Turn();
    }


    private void Move()
    {
        // Adjust the position of the tank based on the player's input.
        // Creo un vector en la dirección en la que apunta el tanque, con una magnitud basada en la entrada, la velocidad y el tiempo entre frames.
           Vector3 movement = transform.forward * m_MovementInputValue* m_Speed *Time.deltaTime;
        
 // Aplico ese vector de movimiento al rigidbody del tanque.
             m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
        
    }


    private void Turn()
    {
        // Adjust the rotation of the tank based on the player's input.
        // Calculo el número de grados de rotación basandome en la entrada, la velocidad y el tiempo entre frames.
 float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;
        
 // Convierto ese número en una rotación en el eje Y.
Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        
 // AAplico esa rotación al rigidbody del tanque
            m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);

    }
}