using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Intelmatix.Tools
{
    public class CubeController : MonoBehaviour
    {

        [Header("Rotation")]
        [SerializeField] GameObject pivot;
        public float speed = 15f;

        float rotation;
        bool shouldRotate = true;

        [Header("Cubes")]
        public LeanTweenType cubeEase = LeanTweenType.notUsed;
        public float cubeTime;
        [Space]
        [SerializeField] List<Cube> cubes = new();
        readonly List<Cube> candidates = new();

        private bool _isOnAnimation = false;

        private void Update() => Rotate();

        void Rotate()
        {
            if (shouldRotate)
            {
                rotation = speed * Time.deltaTime;
                pivot.transform.Rotate(new Vector3(0f, rotation, 0f), Space.World);
            }
            else
            {
                rotation -= Time.deltaTime;

                if (rotation <= 0) rotation = 0;

                pivot.transform.Rotate(new Vector3(0f, rotation, 0f), Space.World);
            }
        }
        public void EvaluateCandidates()
        {
            if (_isOnAnimation) return;

            // Filter the cubes that are in-front
            foreach (Cube cube in cubes)
            {
                cube.activeEase = cubeEase;
                cube.time = cubeTime;

                if (!cube.isCandidate) continue;

                else
                {
                    candidates.Add(cube);
                }
            }

            // Select a random cube
            int random = Random.Range(0, candidates.Count);

            var selectedCube = candidates[random];

            selectedCube.Activate();
            LeanTween.delayedCall(4f, () =>
            {
                selectedCube.Deactivate();
                _isOnAnimation = false;
            });
        }
    }
}
