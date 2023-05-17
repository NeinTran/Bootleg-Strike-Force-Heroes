using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunShoot : MonoBehaviour
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

    ShotgunBulletTrail trailScript;
    public int pellets;
    public float spread;
    public int Max_Ammo = 8;
    public int Current_Ammo = 8;
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
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time > ReadyForNextShot)
            {
                ReadyForNextShot = Time.time + 1/fireRate;
                Shoot();
            }
        }
        
        
    }

    private void Reload()
    {
        //Run reloading animation while running a reload timer
        for (int i = 0; i < Max_Ammo; i++) {
            Current_Ammo++;
            //Run reloading animation
        }
    }

    private void Shoot()
    {
        Current_Ammo--;
        Quaternion shotAngle = Quaternion.identity;
        _muzzleFlashAnimator.SetTrigger("Shoot");
        _recoilAnimator.SetTrigger("Shoot");

        for (int i = 0; i < pellets; i++) {
            shotAngle.eulerAngles = new Vector3(0, 0, Random.Range(-8.0f, 8.0f));

            var hit = Physics2D.Raycast(_gunPoint.position,shotAngle * transform.up,_weaponRange);
            var trail = Instantiate(_bulletTrail,_gunPoint.position,Quaternion.LookRotation(shotAngle * transform.up));

            trailScript = trail.GetComponent<ShotgunBulletTrail>();

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

}
