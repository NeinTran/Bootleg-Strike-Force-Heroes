using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M16Shoot : MonoBehaviour
{
    public float fireRate;
    public float ReadyForNextShot;


    [SerializeField] private float _speed;
    [SerializeField] private Transform _gunPoint;
    [SerializeField] private GameObject _bulletTrail;
    [SerializeField] private float _weaponRange = 10f;
    [SerializeField] private Animator _muzzleFlashAnimator;
    [SerializeField] private Animator _recoilAnimator;

    public ParticleSystem bulletImpact;

    public int Max_Ammo = 30;
    public int Current_Ammo = 30;
    M16BulletTrail trailScript;

    public int damage = 3;
    // Start is called before the first frame update
    void Start()
    {
        if (Current_Ammo <= -1) {
            Current_Ammo = Max_Ammo;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Current_Ammo <= 0) {
            Reload();
        }

        if (Input.GetMouseButton(0))
        {
            if (Time.time > ReadyForNextShot)
            {
                ReadyForNextShot = Time.time + 1/fireRate;
                Shoot();
            }
        }

        if (Input.GetButtonDown("Reload"))
        {
            Reload();
        }
        
        
    }

    
    private void Reload()
    {
        //Run reloading animation while running a reload timer
        _recoilAnimator.SetTrigger("Reload");
        Current_Ammo = Max_Ammo;
    }

    private void Shoot()
    {
        Current_Ammo--;
        Quaternion shotAngle = Quaternion.identity;
        _muzzleFlashAnimator.SetTrigger("Shoot");
        _recoilAnimator.SetTrigger("Shoot");
        
        shotAngle.eulerAngles = new Vector3(0, 0, Random.Range(-2.0f, 2.0f));

        RaycastHit2D hit = Physics2D.Raycast(
            _gunPoint.position,
            shotAngle * transform.up,
            _weaponRange
        );

        var trail = Instantiate(
            _bulletTrail,
            _gunPoint.position,
            Quaternion.LookRotation(shotAngle * transform.up)
        );
        
        trailScript = trail.GetComponent<M16BulletTrail>();
        if (hit.collider == null)
        {
            var endPosition = _gunPoint.position + (shotAngle * transform.up) * _weaponRange;
            trailScript.SetTargetPosition(endPosition);
        }
        if (hit.collider != null) {
            trailScript.SetTargetPosition(hit.point);
            Instantiate(bulletImpact, hit.point, Quaternion.identity);
            if (hit.collider.gameObject.TryGetComponent<StudentScript>(out StudentScript studentComponent)) {
                studentComponent.TakeDamage(this.trailScript.damage);
                Rigidbody2D rgbd = hit.collider.gameObject.GetComponent<Rigidbody2D>();
                if (rgbd != null) {
                    Vector2 knockbackDirection = (hit.collider.gameObject.transform.position - transform.position).normalized;
                    rgbd.AddForce(knockbackDirection * this.trailScript.knockbackForce, ForceMode2D.Impulse);
                }
            }

        }
            
    }
}

