using UnityEngine;

namespace WeaponSystem {
    public class ObjectPenetration : MonoBehaviour {
        [Header("Penetration Settings")]
        public PenetrationLevel penetrationLevel;
        public bool useAudio;
        [HideInInspector] public AudioSource audioSource;

        private void Start() {
            if (useAudio == true && audioSource == null) {
                Debug.LogWarning("ObjectPenetration did not have an assigned audioSource. Attempting to find suitable replacement.");
                audioSource = gameObject.AddComponent<AudioSource>();

            }

            GetComponent<SpriteRenderer>().sortingOrder = 10;
        }

        public enum PenetrationLevel {
            VeryLow,
            Low,
            Medium,
            High,
            VeryHigh
        };

        //Adds new audio source
        public void AddAudioSource() {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }

        //Removes audio source
        public void RemoveAudioSource() {
            DestroyImmediate(audioSource);
        }
    }
}