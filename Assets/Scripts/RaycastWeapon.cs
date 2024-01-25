using System.Collections.Generic;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

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
    [SerializeField] private int fireRate = 25;
    [SerializeField] float bulletSpeed = 1000;
    [SerializeField] float bulletDrop = 0;
    [SerializeField] int maxBounce = 0;

    [Header("Effect")]
    [SerializeField] ParticleSystem[] muzzleFalsh;
    [SerializeField] ParticleSystem hitEffect;
    [SerializeField] TrailRenderer tracerEffect;

    [Header("Animation player with weapon")]
    public string weaponName;


    [Header("Aim")]
    // Fix aim gun
    public Transform raycastOrigin;
    [HideInInspector]public Transform raycastDestination;

    [Header("Recoil")] 
    [HideInInspector]public WeaponRecoil recoil;
    

    private Ray _ray;
    private RaycastHit _hitInfo;
    private float _accumulatedTime;
    private List<Bullet> _bullets = new List<Bullet>();
    private float _maxLifeTime = 3;

    [Header("Magazine")]
    public GameObject magazine;

    [Header("Bullet")] 
    public int ammoCount;
    public int clipSize;
    [HideInInspector]public bool reloading= false;
    
    [Header("Sound")] 
    private AudioSource _audioSource;
    [SerializeField] private AudioClip soundFire;

    private PhotonView _view;
    private void Awake()
    {
        recoil = GetComponent<WeaponRecoil>();
        _audioSource = GetComponent<AudioSource>();
        _view = GetComponent<PhotonView>();
    }

    Vector3 GetPosition(Bullet bullet)
    {
        // p + v*t + 0.5 * g * t * t
        // Công thúc vật lý - Công thức này mô tả vị trí của vật thể tại thời điểm t trong chuyển động tự do.
        Vector3 gravity = Vector3.down * bulletDrop;
        return (bullet.initialPostion) + (bullet.initialVelocity * bullet.time) + (0.5f * gravity * bullet.time * bullet.time);
    }


    // Tạo đạn với vị trí ở nòng súng và bay theo hướng đã tính
    //[PunRPC]
    Bullet CreateBullet(Vector3 position, Vector3 velocity)
    {
        Bullet bullet = new Bullet();
        bullet.initialPostion = position;
        bullet.initialVelocity = velocity;
        bullet.time = 0f;
        GameObject trail = PhotonNetwork.Instantiate(tracerEffect.name, position, quaternion.identity,0);
        bullet.tracer = trail.GetComponent<TrailRenderer>();
        bullet.tracer.AddPosition(position);
        bullet.bounce = maxBounce;
        return bullet;
    }


    public void StartFiring()
    {
        isFiring = true;
        _accumulatedTime = 0f;
    }

    public void StopFiring()
    {
        isFiring = false;
    }


    //Khi bắn gọi tạo đạn 
    private void FireBullet()
    {
        if (ammoCount <= 0 || reloading == true)
        {
            return;
        }

        ammoCount--; 
        _view.RPC(nameof(MuzzelFlashGun), RpcTarget.All);

        Vector3 velocity = (raycastDestination.position - raycastOrigin.position).normalized * bulletSpeed;
        PlaySound(soundFire);
        var bullet = CreateBullet(raycastOrigin.position, velocity);
        _bullets.Add(bullet);

        recoil.GenerateRecoil(weaponName);
    }
    
    [PunRPC]
    private void MuzzelFlashGun()
    {
        foreach (var p in muzzleFalsh)
        {
            p.Emit(1);
        }
    }

    // Tính toán trong 1s sẽ bắn ra bao nhiêu viên đạn
    public void UpdateFiring(float deltaTime)
    {
        //Thời gian đếm
        _accumulatedTime += Time.deltaTime;

        //1 viên sẽ tốn bao nhiêu thời gian
        float fireInterval = 1.0f / fireRate;

        // Nếu tời gian đếm > 0 thì có thể bắn ra đạn
        while (_accumulatedTime >= 0f)
        {
            FireBullet();
            //Recoil();
            //thời gian đếm trừ cho time cho ra 1 viên đạn để trong 1s không ra quá số đạn đã đề ra
            _accumulatedTime -= fireInterval;
        }
    }

    public void UpdateBullets(float detalTime)
    {
        SimulateBullets(detalTime);
        DestroyBullets();
    }

    void SimulateBullets(float detalTime)
    {
        _bullets.ForEach(bullet =>
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
        _ray.origin = start;
        _ray.direction = direction;

        if (Physics.Raycast(_ray, out _hitInfo, distance))
        {
            //Debug.DrawLine(ray.origin, hitInfo.point, Color.red, 1.0f);
            _view.RPC(nameof(HitEffect), RpcTarget.All);

            bullet.tracer.transform.position = _hitInfo.point;
            bullet.time = _maxLifeTime;
            end = _hitInfo.point;

            if(bullet.bounce > 0)
            {
                bullet.time = 0;
                bullet.initialPostion = _hitInfo.point;
                bullet.initialVelocity = Vector3.Reflect(bullet.initialVelocity, _hitInfo.normal);
                bullet.bounce--;
            }

            var rb2d = _hitInfo.collider.GetComponent<Rigidbody>();
            if (rb2d)
            {
                rb2d.AddForceAtPosition(_ray.direction * 20, _hitInfo.point, ForceMode.Impulse);
            }
        }
        else
        {
            bullet.tracer.transform.position = end;
        }
    }

    [PunRPC]
    private void HitEffect()
    {
        hitEffect.transform.position = _hitInfo.point;
        hitEffect.transform.forward = _hitInfo.normal;
        hitEffect.Emit(1);
    }

    [PunRPC]
    void DestroyBullets()
    {
        _bullets.RemoveAll(bullet => bullet.time >= _maxLifeTime );
    }

    public void PlaySound(AudioClip audioClip)
    {
        _audioSource.clip = audioClip; 
        _audioSource.Play();
    }
}
