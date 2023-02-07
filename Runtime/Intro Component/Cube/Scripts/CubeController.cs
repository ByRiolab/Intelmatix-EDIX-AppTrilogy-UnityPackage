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
        [SerializeField] List<Cube> cubes = new List<Cube>();
        List<Cube> candidates = new List<Cube>();

        private bool _isOnAnimation = false;

        void Update()
        {




            //Debug
            // if (Input.GetKeyDown(KeyCode.Space)) InteractionDetected();
            // if (Input.GetKeyDown(KeyCode.R)) UnityEngine.SceneManagement.SceneManager.LoadScene("Cube");

            Rotate();
        }

        void Rotate()
        {
            // Para rotar
            if (shouldRotate)
            {

                rotation = speed * Time.deltaTime;
                pivot.transform.Rotate(new Vector3(0f, rotation, 0f), Space.World);
            }
            // Para frenar
            else
            {

                rotation = rotation - Time.deltaTime;

                if (rotation <= 0) rotation = 0;

                pivot.transform.Rotate(new Vector3(0f, rotation, 0f), Space.World);
            }
        }

        // // Metodo para subscribirse
        // public void InteractionDetected()
        // {

        //     // Si se quiere dejar de girar
        //     //shouldRotate = false;

        //     LeanTween.delayedCall(0.5f, EvaluateCandidates);
        // }

        public void EvaluateCandidates()
        {
            if (_isOnAnimation) return;
            // LeanTween.delayedCall(0.5f, () => _isOnAnimation = false);


            // Para filtrar los cubos que estan en frente
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

            // Para eleccionar uno al azar
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
