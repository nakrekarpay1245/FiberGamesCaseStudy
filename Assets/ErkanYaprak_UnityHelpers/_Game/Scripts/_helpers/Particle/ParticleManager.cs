using System.Collections.Generic;
using UnityEngine;

namespace _Game._helpers.Particles
{
    /// <summary>
    /// Manages the generation, activation, and deactivation of particle effects in the game.
    /// </summary>
    public class ParticleManager : MonoBehaviour
    {
        [Header("Particle Configuration")]
        [Tooltip("List of particle data configurations.")]
        public List<ParticleData> ParticleDataList = new List<ParticleData>();

        public List<ParticleSystem> ParticleList = new List<ParticleSystem>();

        private void Awake()
        {
            GenerateParticles();
            StopAndDeactivateAllParticles();
        }

        /// <summary>
        /// Generates the particles based on the configurations in ParticleDataList.
        /// </summary>
        private void GenerateParticles()
        {
            foreach (var data in ParticleDataList)
            {
                for (int i = 0; i < data.ParticleCount; i++)
                {
                    ParticleSystem particleInstance = Instantiate(data.ParticleSystem);
                    particleInstance.Stop();
                    particleInstance.name = data.ParticleName;
                    particleInstance.transform.SetParent(transform);
                    ParticleList.Add(particleInstance);
                }
            }
        }

        /// <summary>
        /// Stops and deactivates all particles, resetting them to the manager's transform.
        /// </summary>
        private void StopAndDeactivateAllParticles()
        {
            foreach (var particle in ParticleList)
            {
                particle.Stop();
                particle.gameObject.SetActive(false);
                particle.transform.SetParent(transform);
            }
        }

        /// <summary>
        /// Plays a particle effect at the specified position with the given rotation and parent transform.
        /// </summary>
        /// <param name="particleName">The name of the particle effect to play.</param>
        /// <param name="position">The position to play the particle at.</param>
        /// <param name="rotation">The rotation to apply to the particle.</param>
        /// <param name="parent">The parent transform to attach the particle to (optional).</param>
        public void PlayParticleAtPoint(string particleName, Vector3 position, Quaternion rotation,
            Transform parent = null)
        {
            var particle = GetAvailableParticle(particleName);
            if (particle != null)
            {
                ActivateParticle(particle, position, rotation, parent);
            }
            else
            {
                Debug.LogWarning("Partcile is null!");
            }
        }

        /// <summary>
        /// Plays a particle effect at the specified position with an optional parent transform.
        /// </summary>
        /// <param name="particleName">The name of the particle effect to play.</param>
        /// <param name="position">The position to play the particle at.</param>
        /// <param name="parent">The parent transform to attach the particle to (optional).</param>
        public void PlayParticleAtPoint(string particleName, Vector3 position, Transform parent = null)
        {
            var particle = GetAvailableParticle(particleName);
            if (particle != null)
            {
                ActivateParticle(particle, position, Quaternion.identity, parent);
            }
            else
            {
                Debug.LogWarning("Partcile is null! --> " + particleName);
            }
        }

        /// <summary>
        /// Retrieves an available particle system that matches the given name and is not currently playing.
        /// </summary>
        /// <param name="particleName">The name of the particle effect to retrieve.</param>
        /// <returns>An available ParticleSystem, or null if none are found.</returns>
        private ParticleSystem GetAvailableParticle(string particleName)
        {
            foreach (var particle in ParticleList)
            {
                if (particle == null) // Check if the particle has been destroyed
                {
                    //Debug.LogWarning($"Particle {particleName} is null. It might have been destroyed.");
                    continue;
                }

                if (particle.name == particleName && !particle.isPlaying)
                {
                    return particle;
                }
            }

            Debug.LogWarning("There is no available particle with the specified name --> " + particleName);
            return null;
        }

        /// <summary>
        /// Activates and plays a particle system at the specified position, rotation, and parent.
        /// </summary>
        /// <param name="particle">The particle system to activate and play.</param>
        /// <param name="position">The position to set for the particle.</param>
        /// <param name="rotation">The rotation to set for the particle.</param>
        /// <param name="parent">The parent transform to attach the particle to (optional).</param>
        private void ActivateParticle(ParticleSystem particle, Vector3 position, Quaternion rotation,
            Transform parent)
        {
            parent = parent == null ? transform : parent;

            particle.transform.SetParent(parent);
            particle.transform.position = position;
            particle.transform.rotation = rotation;
            particle.gameObject.SetActive(true);
            particle.Play();

            //if (!particle.main.loop)
            //{
            //    Debug.Log($"{particle.name} is not looping");
            //}
            //else
            //{
            //    Debug.Log($"{particle.name} is looping");
            //}
        }
    }
}