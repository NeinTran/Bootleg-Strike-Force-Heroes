using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellExtractor : MonoBehaviour
{
    public Transform shellExtractor;
    public Rigidbody2D shell;
    float randomExtractionForce, randomTorque;

    [SerializeField] public float fireRate;
    [SerializeField] public float ReadyForNextShot;

    void Update()
    {
        Fire();
    }


    void Fire()
    {
        if (Input.GetMouseButtonDown(0)) {
            randomExtractionForce = Random.Range(150f,200f);
            randomTorque = Random.Range(300f,700f);
            if (Time.time > ReadyForNextShot)
            {
                ReadyForNextShot = Time.time + 1/fireRate;
                ExtractSell();
            }
        }
    }

    void ExtractSell() {
        var extractedShell = Instantiate(shell, shellExtractor.position, shellExtractor.rotation);
        extractedShell.AddForce(shellExtractor.up * randomExtractionForce, ForceMode2D.Force);
        extractedShell.AddTorque(randomTorque);
    }
}
