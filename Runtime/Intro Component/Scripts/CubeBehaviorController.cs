using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Intelmatix
{
    /// <summary>
    /// Controla el comportamiento de un conjunto de cubos en una escena.
    /// </summary>
    public class CubeBehaviorController : MonoBehaviour
    {
        [SerializeField] private Transform cubesParent;
        private readonly List<(Vector3 position, Quaternion rotation, Vector3 scale)> initialTransforms = new();
        [SerializeField] private Camera cubesCamera;

        [SerializeField] private float wordTransitionSpeed = 3;
        [SerializeField] private float cubeTransitionSpeed = 2;
        [SerializeField] private float rotationSpeed = 3;
        [SerializeField] private LeanTweenType menuEaseType = LeanTweenType.easeInOutBack;
        [SerializeField] private LeanTweenType wordEaseType = LeanTweenType.easeInOutBack;
        [SerializeField] private LeanTweenType cubeEaseType = LeanTweenType.easeInOutBack;

        [SerializeField] private bool cubeWordLoop = false;
        [SerializeField] private float loopTimeout = 5;

        private int alphaID;
        private List<Transform> cubes;

        private void Awake()
        {
            cubes = cubesParent.GetComponentsInChildren<Transform>().ToList();
            cubes.Remove(cubesParent);

            alphaID = Shader.PropertyToID("_CubeAlpha");
            foreach (Transform cube in cubes)
            {
                initialTransforms.Add((cube.localPosition, cube.localRotation, cube.localScale));
            }
            SetupCube();
        }

        /// <summary>
        /// Configura la visualización de palabras.
        /// </summary>
        private void SetupWordDisplay()
        {
            ShuffleCubesList();

            LeanTween.rotate(cubesParent.gameObject, Vector3.zero, rotationSpeed * 0.5f);

            for (int i = 0; i < initialTransforms.Count; i++)
            {
                var (pos, rot, scale) = initialTransforms[i];
                var cube = cubes[i];

                var cubeGameObject = cube.gameObject;

                LeanTween.cancel(cubeGameObject);

                var cubeRenderer = cube.GetComponent<MeshRenderer>();
                var startMaterial = cubeRenderer.material.GetFloat(alphaID);

                LeanTween.value(cubeGameObject, startMaterial, 1, wordTransitionSpeed)
                    .setEase(menuEaseType)
                    .setOnUpdate(value => cubeRenderer.material.SetFloat(alphaID, value));

                LeanTween.moveLocal(cubeGameObject, pos, wordTransitionSpeed).setEase(wordEaseType);
                LeanTween.rotateLocal(cubeGameObject, rot.eulerAngles, UnityEngine.Random.Range(0.5f, 0.75f) * wordTransitionSpeed).setEase(wordEaseType);
                LeanTween.scale(cubeGameObject, scale, wordTransitionSpeed).setEase(wordEaseType);
            }

            LeanTween.cancel(gameObject);
            if (cubeWordLoop)
                LeanTween.delayedCall(gameObject, loopTimeout, SetupCube);
        }

        /// <summary>
        /// Configura los cubos para que se posicionen como un cubo compuesto.
        /// </summary>
        private void SetupCube()
        {
            if (cubes == null) return;

            ShuffleCubesList();
            ResetVisibility();

            LeanTween.cancel(gameObject);

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    for (int z = 0; z < 3; z++)
                    {
                        int index = x * 9 + y * 3 + z;

                        if (index >= cubes.Count) continue;

                        var cube = cubes[index].gameObject;
                        LeanTween.cancel(cube);

                        if (!cube.activeSelf)
                            cube.SetActive(true);

                        var cubeRenderer = cube.GetComponent<MeshRenderer>();
                        var startMaterial = cubeRenderer.material.GetFloat(alphaID);

                        LeanTween.value(cube, startMaterial, .25f, cubeTransitionSpeed)
                            .setEase(menuEaseType)
                            .setOnUpdate(value => cubeRenderer.material.SetFloat(alphaID, value))
                            .setDelay(cubeTransitionSpeed)
                            .setOnComplete(() =>
                            {
                                LeanTween.value(cube, .25f, 1, cubeTransitionSpeed)
                                    .setEase(menuEaseType)
                                    .setOnUpdate(value => cubeRenderer.material.SetFloat(alphaID, value));
                            });

                        LeanTween.moveLocal(cube, new Vector3(x, y, z) - Vector3.one, cubeTransitionSpeed).setEase(cubeEaseType);
                        LeanTween.rotateLocal(cube, Vector3.zero, cubeTransitionSpeed).setEase(cubeEaseType);
                        LeanTween.scale(cube, Vector3.one, cubeTransitionSpeed).setEase(cubeEaseType);
                    }
                }
            }

            for (int i = 27; i < cubes.Count; i++)
            {
                var cube = cubes[i].gameObject;
                LeanTween.cancel(cube);
                LeanTween.scale(cube, Vector3.zero, cubeTransitionSpeed).setEase(cubeEaseType);
            }

            LeanTween.cancel(cubesParent.gameObject);
            LeanTween.rotate(cubesParent.gameObject, new Vector3(45, 45, 0), rotationSpeed);

            if (cubeWordLoop)
                LeanTween.delayedCall(gameObject, loopTimeout, SetupWordDisplay);
        }

        /// <summary>
        /// Colapsa los cubos.
        /// </summary>
        public void Collapse() => SetupCube();

        /// <summary>
        /// Expande la visualización de palabras.
        /// </summary>
        public void Expand() => SetupWordDisplay();

        /// <summary>
        /// Ordena aleatoriamente la lista de cubos,
        /// </summary>
        private void ShuffleCubesList()
        {
            try
            {
                int index = cubes.Count;
                System.Random rng = new();

                while (index > 1)
                {
                    index--;
                    int randomIndex = rng.Next(index + 1);
                    (cubes[index], cubes[randomIndex]) = (cubes[randomIndex], cubes[index]);
                }
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine($"Error al barajar los cubos: {ex.Message}");
                // Manejar la excepción de acuerdo a los requisitos de tu aplicación.
            }
        }


        /// <summary>
        /// Restablece los CanvasGroup a su estado inicial.
        /// </summary>
        private void ResetVisibility()
        {
            cubesCamera.enabled = true;
        }
        public void Deactivate()
        {
            cubesCamera.enabled = false;
            LeanTween.cancel(gameObject);
        }
    }
}
