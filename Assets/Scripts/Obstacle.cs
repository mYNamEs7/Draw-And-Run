using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private enum ObstacleType { Other, Mine }

    [SerializeField] private ObstacleType type;

    private bool isExplosion;

    void Start()
    {
        GameController.Instance.OnGameStarted += GameController_OnGameStarted;
    }

    private void GameController_OnGameStarted()
    {
        if (TryGetComponent(out Animator animator))
            animator.enabled = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player) && !player.IsStatic)
        {
            if (type == ObstacleType.Mine)
            {
                isExplosion = true;
                GetComponent<BoxCollider>().enabled = true;
            }
            else
            {
                UnitFactory.Instance.RemoveUnit(other.transform);
                player.DestroyPlayer();
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Player player) && !player.IsStatic && isExplosion)
        {
            StartCoroutine(S(other));
        }
    }

    IEnumerator S(Collider other)
    {
        while (other.TryGetComponent(out Player player) && !player.IsStatic && isExplosion)
        {
            UnitFactory.Instance.RemoveUnit(other.transform);
            player.ExplosionPlayer();

            yield return null;
        }

        isExplosion = false;
    }
}
