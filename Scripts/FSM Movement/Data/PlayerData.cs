using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    public Rigidbody2D rgbd;
    [Header("Move State")] public float movementVelocity = 10f;
    [Header("Jump State")] public float JumpVelocity = 8f;
    [Range(0, .3f)] [SerializeField] public float MovementSmoothing = .05f;
    [Header("Check Variables")] public float groundCheckRadius = 0.3f;
    public LayerMask WhatIsGround;
    [SerializeField] public float knockbackForce = 10f;
    [Header("Crouch States")] public float crouchMoveSpeed = 5f;
    
}
    
