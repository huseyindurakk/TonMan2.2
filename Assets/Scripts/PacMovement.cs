using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacMovement : MonoBehaviour
{
    public float moveSpeed = 9f; // Varsay�lan hareket h�z�
    public float boostSpeed = 15f; // Boost hareket h�z�
    public float rotationSpeed = 720f; // D�nd�rme h�z�
    public float gravity = -9.8f; // Yer�ekimi kuvveti
    public float groundCheckDistance = 0.2f; // Yere yak�nl�k kontrol mesafesi
    public LayerMask groundMask; // Zemin katman�
    public DynamicJoystick joystick; // Dinamik joystick referans�

    private CharacterController controller;
    private Vector3 velocity;
    private Vector3 moveDirection;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (joystick == null)
        {
            Debug.LogError("Joystick referans� atanmad�. L�tfen Inspector'dan atanmas�n� sa�lay�n.");
        }
    }

    void Update()
    {
        CheckGrounded();
        HandleMovement();
        ApplyGravity();
    }

    void CheckGrounded()
    {
        // Karakterin zeminde olup olmad���n� kontrol eder
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Zemine yap��may� sa�lamak i�in k���k bir negatif de�er
        }
    }

    void HandleMovement()
    {
        // Joystick girdilerini al�r
        float inputX = joystick.Horizontal; // Yatay joystick girdisi
        float inputZ = joystick.Vertical; // Dikey joystick girdisi

        // Hareket y�n�n� hesapla
        Vector3 inputDirection = new Vector3(inputX, 0, inputZ).normalized;

        if (inputDirection.magnitude >= 0.1f)
        {
            // Karakterin bak�� y�n�n� ayarla
            Quaternion targetRotation = Quaternion.LookRotation(inputDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Hareket y�n�n� belirle
            moveDirection = inputDirection * (Input.GetKey(KeyCode.LeftShift) ? boostSpeed : moveSpeed);
        }
        else
        {
            moveDirection = Vector3.zero; // E�er joystick s�f�rsa karakter durur
        }

        // Karakteri hareket ettir
        controller.Move(moveDirection * Time.deltaTime);
    }

    void ApplyGravity()
    {
        // Yer�ekimi uygula
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
