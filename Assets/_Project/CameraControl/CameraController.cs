using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace CameraControl
{
    /// <summary>
    /// A big ol' fancy camera controller
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        // Serialized fields
        [SerializeField, Tooltip("The player object - keep this object in view at all times")] 
        private Transform player;
        [SerializeField, Tooltip("The main lookat target for the system")] 
        private Transform lookTarget;
        [SerializeField, Tooltip("The minimum orthographic size of the camera")] 
        private float minZoom;
        [SerializeField, Tooltip("The maximum orthographic size of the camera")] 
        private float maxZoom;
        [SerializeField, Tooltip("The maximum speed to change the orthographic size per second")] 
        private float zoomSpeed;
        [SerializeField, Tooltip("The minimum speed to change the orthographic size per second")] 
        private float zoomReturnSpeed = 1f;
        [SerializeField, Tooltip("The maximum distance an enemy can be to affect the camera's zoom level")] 
        private float zoomLimiter;
        [SerializeField, Tooltip("The minimum enemy distance to use for the zoom level")] 
        private float minEnemyDistance = 15f;
        
        [SerializeField, Tooltip("The list of objects to try keep in view")] 
        private List<Transform> enemies;
        [SerializeField, Tooltip("The weights of the objects to keep in view")] 
        private List<float> enemyWeights;
        
        [SerializeField, Tooltip("The amount of padding around the player")] 
        private float edgeDistance = 1f;
        [SerializeField, Tooltip("The speed at which the camera moves to the target position")] 
        private float smoothTime = 0.3f;
        [SerializeField, Tooltip("The maximum amount of camera shake")]
        private float camShakeMax = 3f;
        [SerializeField, Tooltip("The maximum amount of camera shake rotation")]
        private float camShakeMaxRotation = 10f;

        // Private fields
        private Vector3 _velocity;
        private Vector3 _offset;
        private Vector3 _targetAveragePosition;
        private UnityEngine.Camera _cam;

        private Vector3 _playerPosition;
        private Vector3 _lookTargetPosition;

        private Vector3 _targetPosition;
        private Vector3 _camShakeOffset;

        private float _trauma;
        private float _seed;
        private float _elapsedTime;

        private Tweener _knockbackTween;

        /// <summary>
        /// Called when the object is created
        /// </summary>
        private void Start()
        {
            // get the starting offset
            _offset = transform.position - player.position;
            // and the camera
            _cam = GetComponent<UnityEngine.Camera>();
            // and seed the camera shake
            _seed = Random.Range(0, 99999);

            Screen.fullScreen = true;
        }

        /// <summary>
        /// Called every physics step
        /// Using FixedUpdate for this because all the objects are rigidbodies and updated with physics
        /// </summary>
        private void FixedUpdate()
        {
            // cache the relevant positions
            _lookTargetPosition = lookTarget.position;
            _playerPosition = player.position;
            
            // get a weighted average of the positions of the targets
            Vector3 newTargetAveragePosition = GetAveragePosition();
            
            // smoothly move the camera to the new position
            _targetAveragePosition = Vector3.SmoothDamp(_targetAveragePosition, newTargetAveragePosition, ref _velocity,
                smoothTime);
            
            // get the new position of the camera
            Vector3 newPosition = _targetAveragePosition + _offset;

            // Clamp the camera position
            float camHalfHeight = _cam.orthographicSize;
            float camHalfWidth = _cam.aspect * camHalfHeight;

            float minX = _playerPosition.x - camHalfWidth + edgeDistance;
            float maxX = _playerPosition.x + camHalfWidth - edgeDistance;
            float minY = _playerPosition.y - camHalfHeight + edgeDistance;
            float maxY = _playerPosition.y + camHalfHeight - edgeDistance;

            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
            newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

            _targetPosition = newPosition;

            // get the greatest distance between the player and the targets
            var greatestDistance = GetGreatestDistance();

            // calculate the new zoom level
            float newZoom;
            float normalizedDistance = Mathf.InverseLerp(minEnemyDistance, zoomLimiter, greatestDistance);
            var smoothZoomSpeed = Mathf.Lerp(zoomSpeed, zoomReturnSpeed, normalizedDistance);

            // if the greatest distance is less than the zoom limiter, lerp between the max and min zoom based on the distance
            if (greatestDistance <= zoomLimiter)
                newZoom = Mathf.Lerp(maxZoom, minZoom, 1 - Mathf.Clamp01(greatestDistance / zoomLimiter));
            else // otherwise just zoom out as far as possible
                newZoom = minZoom;

            // smoothly set the zoom level
            _cam.orthographicSize = SmoothZoom(_cam.orthographicSize, newZoom, smoothZoomSpeed);

            // apply the camera shake if there's no knockback
            if (_knockbackTween == null)
                GetCamShakeFromTrauma();

            // finally set the camera position
            transform.position = _targetPosition + _camShakeOffset;
        }

        /// <summary>
        /// Calculate camera shake from a trauma value
        /// </summary>
        private void GetCamShakeFromTrauma()
        {
            // reduce the trauma over time
            _trauma -= Time.deltaTime;
            _trauma = Mathf.Clamp(_trauma, 0, 1);

            // Perlin noise makes the shake smoother and more "natural" vs completely random
            // it will also slow down with Time.timeScale
            var xPos = GetPerlin(_seed, 100, camShakeMax);
            var yPos = GetPerlin(_seed + 1, 100, camShakeMax);
            var rot = GetPerlin(_seed + 2, 100, camShakeMaxRotation);
            // calculate the new camera shake offset
            _camShakeOffset = new Vector3(xPos, yPos);
            transform.eulerAngles = new Vector3(0, 0, rot);
            // increment the elapsed time so we can get new values next frame
            _elapsedTime += Time.deltaTime;
        }

        /// <summary>
        /// Calculate a perlin noise value
        /// </summary>
        /// <param name="newSeed">The seed to use</param>
        /// <param name="frequency">How quickly does the shake change</param>
        /// <param name="strength">How strong is the shake</param>
        /// <returns>A random float between 0 and 1</returns>
        private float GetPerlin(float newSeed, float frequency, float strength)
        {
            var noise = Mathf.PerlinNoise(newSeed + _elapsedTime * frequency, newSeed + _elapsedTime * frequency);
            noise = noise * 2 - 1;
            return noise * Mathf.Pow(_trauma, 2) * strength;
        }

        /// <summary>
        /// Shake the camera in a specific direction over a duration
        /// </summary>
        /// <param name="direction">The direction of the shake</param>
        /// <param name="distance">How far should the camera move</param>
        /// <param name="duration">How long should it take to move back</param>
        /// <param name="ease">An ease to describe the return motion</param>
        public void ShakeDirectional(Vector3 direction, float distance, float duration, Ease ease)
        {
            // if there's already a knockback, don't do anything
            if (_knockbackTween != null)
                return;
            
            // otherwise tween the camera shake offset
            _camShakeOffset = direction * distance;
            _knockbackTween = DOVirtual
                .Vector3(_camShakeOffset, Vector3.zero, duration, val => _camShakeOffset = val)
                .SetEase(ease)
                .OnComplete(() => _knockbackTween = null);
        }

        /// <summary>
        /// Add an explosive shake to the camera with a force
        /// </summary>
        /// <param name="force">How strong should the shake be, clamped to 0-1 range</param>
        public void ShakeExplosive(float force)
        {
            _trauma = force;
        }

        /// <summary>
        /// Calculate the zoom value based on the current zoom, target zoom and speed
        /// </summary>
        /// <param name="currentZoom"></param>
        /// <param name="targetZoom"></param>
        /// <param name="speed"></param>
        /// <returns></returns>
        private float SmoothZoom(float currentZoom, float targetZoom, float speed)
        {
            float smoothZoom = currentZoom + (targetZoom - currentZoom) * Time.deltaTime * speed;
            return smoothZoom;
        }

        /// <summary>
        /// Get the weighted average position of the targets & player
        /// </summary>
        /// <returns>The new position</returns>
        private Vector3 GetAveragePosition()
        {
            // set the player weight to a constant value
            var playerWeight = 5f;
            // initialise a position vector and weight
            var totalPosition = _lookTargetPosition * playerWeight;
            var totalWeight = playerWeight;

            for (int i = 0; i < enemies.Count; i++)
            {
                // if the enemy is null, skip it
                if (enemies[i] == null) continue;
                
                // get the distance to the player
                float distanceToPlayerSq = (_lookTargetPosition - enemies[i].position).sqrMagnitude;
                // calculate the weight based on the distance
                float normalizedDistance = Mathf.InverseLerp(zoomLimiter * zoomLimiter - 25,
                    zoomLimiter * zoomLimiter, distanceToPlayerSq);
                float weight = enemyWeights[i] * Mathf.Clamp01(1 - normalizedDistance);

                // add the position and weight to the total
                totalPosition += enemies[i].position * weight;
                totalWeight += weight;
            }

            // return the weighted average
            return totalPosition / totalWeight;
        }
        
        /// <summary>
        /// Get the greatest distance between the look position and the targets
        /// </summary>
        /// <returns></returns>
        private float GetGreatestDistance()
        {
            float greatestDistanceSquared = 0f;

            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i] != null)
                {
                    float distanceSquared = (_lookTargetPosition - enemies[i].position).sqrMagnitude;
                    greatestDistanceSquared = Mathf.Max(greatestDistanceSquared, distanceSquared);
                }
            }

            return Mathf.Sqrt(greatestDistanceSquared);
        }


        /// <summary>
        /// Add an enemy to the list of enemies to track
        /// </summary>
        /// <param name="enemy"></param>
        /// <param name="weight"></param>
        public void AddEnemy(Transform enemy, float weight)
        {
            enemies.Add(enemy);
            enemyWeights.Add(weight);
        }

        /// <summary>
        /// Remove an enemy from the list of enemies to track
        /// </summary>
        /// <param name="enemy"></param>
        public void RemoveEnemy(Transform enemy)
        {
            int index = enemies.IndexOf(enemy);
            if (index >= 0)
            {
                enemies.RemoveAt(index);
                enemyWeights.RemoveAt(index);
            }
        }
    }
}