using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentScript : MonoBehaviour
{
    [SerializeField] public float Health = 1000;
    [SerializeField] private Material flashMaterial;
    [SerializeField] private float duration;
    private Coroutine flashRoutine;
    public SpriteRenderer spriteRenderer;
    public Material originalMaterial;
    public GameObject damageText;
    
    private float CurrentHealth;
    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = Health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Flash()
    {
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }
        flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        spriteRenderer.material = flashMaterial;
        yield return new WaitForSeconds(duration);
        spriteRenderer.material = originalMaterial;
        flashRoutine = null;
    }

    public void TakeDamage(float damage) {
        CurrentHealth -= damage;
        Flash();
        if (damageText && CurrentHealth > 0) {
            ShowDamageText();
        }
        if (CurrentHealth <= 0) {
            Destroy(gameObject);
        }
    }

    public void ShowDamageText() {
        var go = Instantiate(damageText, transform.position, Quaternion.identity, transform);
        go.GetComponent<TextMesh>().text = CurrentHealth.ToString();
    }
}
