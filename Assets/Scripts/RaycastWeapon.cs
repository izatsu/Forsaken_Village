using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class RaycastWeapon : MonoBehaviour
{
    class Bullet
    {
        public float time;
        public Vector3 initialPostion;
        public Vector3 initialVelocity;
        public TrailRenderer tracer;
        public int bounce; 
    }

    public ActiveWeapon.WeaponSlots weaponSlot;

    [Header("Bullet attack")]
    public bool isFiring = false;
    public int fireRate = 25;
    [SerializeField] float bulletSpeed = 1000;
    [SerializeField] float bulletDrop = 0;
    [SerializeField] int maxBounce = 0;

    public bool debug = false;

    [Header("Effect")]
    [SerializeField] ParticleSystem[] muzzleFalsh;
    [SerializeField] ParticleSystem hitEffect;
    [SerializeField] TrailRenderer tracerEffect;

    [Header("Animation player with weapon")]
    public string weaponName;


    [Header("Aim")]
    // Fix aim gun
    public Transform raycastOrigin;
    public Transform raycastDestination;
    
    Ray ray;
    RaycastHit hitInfo;
    float accumulatedTime;
    List<Bullet> bullets = new List<Bullet>();
    float maxLifeTime = 3;

    Vector3 GetPosition(Bullet bullet)
    {
        // p + v*t + 0.5 * g * t * t
        // Công thúc vật lý - Công thức này mô tả vị trí của vật thể tại thời điểm t trong chuyển động tự do.
        Vector3 gravity = Vector3.down * bulletDrop;
        return (bullet.initialPostion) + (bullet.initialVelocity * bullet.time) + (0.5f * gravity * bullet.time * bullet.time);
    }


    // Tạo đạn với vị trí ở nòng súng và bay theo hướng đã tính
    Bullet CreateBullet(Vector3 position, Vector3 velocity)
    {
        Bullet bullet = new Bullet();
        bullet.initialPostion = position;
        bullet.initialVelocity = velocity;
        bullet.time = 0f;
        bullet.tracer= Instantiate(tracerEffect, position, Quaternion.identity);
        bullet.tracer.AddPosition(position);
        bullet.bounce = maxBounce;
        return bullet;
    }


    public void StartFiring()
    {
        isFiring = true;
        accumulatedTime = 0f;
        FireBullet();
    }

    public void StopFiring()
    {
        isFiring = false;
    }


    //Khi bắn gọi tạo đạn 
    private void FireBullet()
    {
        foreach (var p in muzzleFalsh)
        {
            p.Emit(1);
        }

        Vector3 velocity = (raycastDestination.position - raycastOrigin.position).normalized * bulletSpeed;
        var bullet = CreateBullet(raycastOrigin.position, velocity);
        bullets.Add(bullet);
    }

    // Tính toán trong 1s sẽ bắn ra bao nhiêu viên đạn
    public void UpdateFiring(float deltaTime)
    {
        //Thời gian đếm
        accumulatedTime += Time.deltaTime;

        //1 viên sẽ tốn bao nhiêu thời gian
        float fireInterval = 1.0f / fireRate;

        // Nếu tời gian đếm > 0 thì có thể bắn ra đạn
        while (accumulatedTime >= 0f)
        {
            FireBullet();

            //thời gian đếm trừ cho time cho ra 1 viên đạn để trong 1s không ra quá số đạn đã đề ra
            accumulatedTime -= fireInterval;
        }
    }

    public void UpdateBullets(float detalTime)
    {
        SimulateBullets(detalTime);
        DestroyBullets();
    }

    void SimulateBullets(float detalTime)
    {
        bullets.ForEach(bullet =>
        {
            Vector3 p0 = GetPosition(bullet);
            bullet.time += Time.deltaTime;
            Vector3 p1 = GetPosition(bullet);
            RaycastSegment(p0, p1, bullet);
        });
    }

    // Tính toán đến điểm đạn va chạm
    void RaycastSegment(Vector3 start, Vector3 end, Bullet bullet)
    {
        Vector3 direction = end - start;
        float distance = (end - start).magnitude;
        ray.origin = start;
        ray.direction = direction;

        if (Physics.Raycast(ray, out hitInfo, distance))
        {
            //Debug.DrawLine(ray.origin, hitInfo.point, Color.red, 1.0f);
            hitEffect.transform.position = hitInfo.point;
            hitEffect.transform.forward = hitInfo.normal;
            hitEffect.Emit(1);

            bullet.tracer.transform.position = hitInfo.point;
            bullet.time = maxLifeTime;
            end = hitInfo.point;

            if(bullet.bounce > 0)
            {
                bullet.time = 0;
                bullet.initialPostion = hitInfo.point;
                bullet.initialVelocity = Vector3.Reflect(bullet.initialVelocity, hitInfo.normal);
                bullet.bounce--;
            }

            var rb2d = hitInfo.collider.GetComponent<Rigidbody>();
            if (rb2d)
            {
                rb2d.AddForceAtPosition(ray.direction * 20, hitInfo.point, ForceMode.Impulse);
            }
        }
        else
        {
            bullet.tracer.transform.position = end;
        }
    }


    void DestroyBullets()
    {
        bullets.RemoveAll(bullet => bullet.time >= maxLifeTime );
    }
}
