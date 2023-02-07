using UnityEngine;

namespace Intelmatix.Tools
{

    public class Cube : MonoBehaviour
    {

        MeshRenderer mesh;
        Collider col;

        public bool isCandidate = false;
        [Space]
        public Vector3 activePosition;
        Vector3 standbyPos;

        [HideInInspector] public LeanTweenType activeEase = LeanTweenType.notUsed;
        [HideInInspector] public float time;

        void Start()
        {

            mesh = GetComponent<MeshRenderer>();
            col = GetComponent<BoxCollider>();
            standbyPos = transform.localPosition;
        }

        public void Activate()
        {

            ToActivePosition();
            LeanTween.delayedCall(time / 2, ToActiveColor);
        }

        public void Deactivate()
        {

            ToStandbyPosition();
            ToStandbyColor();
        }

        public void OnTriggerEnter(Collider zone)
        {

            if (zone.gameObject.tag == "Front")
            {
                isCandidate = true;
                //ToActiveColor();
            }
            else if (zone.gameObject.tag == "Back")
            {
                isCandidate = false;
            }
        }

        public void OnTriggerExit(Collider zone)
        {

            if (zone.gameObject.tag == "Front")
            {
                isCandidate = false;
                //ToStandbyColor();
            }
            else if (zone.gameObject.tag == "Back")
            {
                isCandidate = true;
            }
        }

        void ToActiveColor()
        {

            LeanTween.value(gameObject, 0f, 1f, 0.6f).setOnUpdate((float o) =>
            {
                mesh.material.SetFloat("_TexLerp", o);
            }).setEase(activeEase);
        }

        void ToStandbyColor()
        {
            LeanTween.value(gameObject, 1f, 0f, 0.6f).setOnUpdate((float o) =>
            {
                mesh.material.SetFloat("_TexLerp", o);
            }).setEase(activeEase);

        }

        void ToActivePosition()
        {

            LeanTween.moveLocal(gameObject, activePosition, time).setEase(activeEase);
        }

        void ToStandbyPosition()
        {

            LeanTween.moveLocal(gameObject, standbyPos, time).setEase(activeEase);
        }
    }
}