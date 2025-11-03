using UnityEngine;

namespace Assets.Source.Scripts.Utility
{
    public class Incinirator : MonoBehaviour
    {
        [SerializeField] private Flashlight _flashlight;
        [SerializeField] private LayerMask _enemyLayer;
        [SerializeField] private LayerMask _ignoringLayer;
        [SerializeField] private float _frequency = 0.25f;
        [SerializeField] private float _radius = 0.5f;
        [SerializeField] private float _distance = 4;

        private float _aggregator = 0;

        private void Update()
        {
            _aggregator += Time.deltaTime;

            if (_aggregator > _frequency)
            {
                _aggregator = 0;
                Incinirate();
            }
        }

        private void Incinirate()
        {
            if (_flashlight.IsWorking == false)
                return;

            Vector3 origin = _flashlight.transform.position;
            Vector3 direction = _flashlight.transform.forward;

            RaycastHit[] hits = Physics.SphereCastAll(
                origin, _radius, direction, _distance, _enemyLayer
            );

            foreach (RaycastHit hit in hits)
            {
                // Более точная проверка: луч к центру объекта
                Vector3 targetDirection = (hit.collider.bounds.center - origin).normalized;

                if (Physics.Raycast(origin, targetDirection, out RaycastHit sightHit, _distance, ~_ignoringLayer))
                {
                    if (sightHit.collider == hit.collider)
                    {
                        hit.collider.gameObject.GetComponent<PlayerDetector>().Scare();

                    }
                }
            }

        }
    }
}