using System;
using UnityEngine;
using static WeaponSystem.GunObject;

namespace WeaponSystem {
    [CreateAssetMenu(menuName = "Simple Weapon System/New Gun", order = 1, fileName = "New Gun")]
    public class GunObject : ScriptableObject {
        [Header("Gun Name")]
        public string gunName;

        [Header("Settings")]
        public bool useDelayBetweenShots = true;
        public bool useAmmoLimit = false;
        public bool useAudio = false;

        public WeaponType weaponType;

        [HideInInspector] public int burstCount = 3;
        [HideInInspector] public float timeBetweenShots = 0.5f;
        [HideInInspector] public int maxClipSize = 15;
        [HideInInspector] public float reloadTime = 1.5f;

        [HideInInspector] public AudioClip fireSound;
        [HideInInspector] public AudioClip dryFireSound;
        [HideInInspector] public AudioClip reloadSound;
        [HideInInspector] public AudioClip explosionSound;
        [HideInInspector] public bool useSetReloadTime = false;
        [HideInInspector] public bool automaticReload = true;

        public enum WeaponType {
            Single,
            Automatic,
            Burst
        }

        [Header("Bullet Settings")]
        public BulletSettings bulletSettings;

        [Serializable]
        public class BulletSettings
        {
            public float bulletSpeed = 20f;
            public float bulletDamage = 10;
            public float bulletLife = 2.5f;
            public bool explosiveBullets;
            public bool bulletPenetration;

            [HideInInspector] public GameObject impactParticlePrefab;
            [HideInInspector] public GameObject explosionParticlePrefab;
            [HideInInspector] public BulletPenetrationLevel bulletPenetrationLevel;

            public enum BulletPenetrationLevel
            {
                VeryLow,
                Low,
                Medium,
                High,
                VeryHigh
            }
        }
    }
}