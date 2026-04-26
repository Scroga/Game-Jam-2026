using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private bool shoot = true;
    [SerializeField] private float shootFrequency;
    [SerializeField] private float shootDist = 2.0f;
    [SerializeField] private float shootPointDist = 1.0f;
    [SerializeField] private string soundName;

    private float timeSinceLastShoot;
    private Rigidbody2D rb;

    void Start()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }
        rb = GetComponent<Rigidbody2D>();
    }

    public void StartShoot()
    {
        shoot = true;
    }
    public void StopShoot()
    {
        shoot = false;
    }

    public void Shoot()
    {
        if (target && shoot && timeSinceLastShoot >= shootFrequency)
        {
            SoundManager.Instance.PlaySound2D(soundName);
            Vector2 dir = (Vector2)(target.transform.position - gameObject.transform.position);
            float dist = dir.magnitude;

            if (dist > shootDist) return;

            Vector2 spawnPos = (dir.normalized * shootPointDist) + (Vector2)gameObject.transform.position;

            GameObject bullet = Instantiate(
                bulletPrefab,
                spawnPos,
                Quaternion.identity
            );

            if (bullet.TryGetComponent<EnemyBullet>(out var enemyBullet))
            {
                enemyBullet.Setup(target.transform);
            }
            timeSinceLastShoot = 0;
        }

    }

    void Update()
    {
        timeSinceLastShoot += Time.deltaTime;
        Shoot();
    }
}
