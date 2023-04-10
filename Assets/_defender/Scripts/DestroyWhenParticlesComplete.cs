using System.Collections;
 using UnityEngine;
 
 [RequireComponent(typeof(ParticleSystem))]
 public class DestroyWhenParticlesComplete : MonoBehaviour
 {
     ParticleSystem _particleSystem;
 
     void Awake()
     {
         _particleSystem = GetComponent<ParticleSystem>();
     }
 
     IEnumerator Start()
     {
         while (_particleSystem.isPlaying) yield return null;
         Destroy(gameObject);
     }
 }
