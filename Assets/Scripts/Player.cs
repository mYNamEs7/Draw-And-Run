using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool IsStatic => isStatic;

    [SerializeField] private bool isStatic;
    [SerializeField] private string isRunning = "IsRunning";
    [SerializeField] private string win = "Win";
    [SerializeField] private Transform particlePrefab;
    [SerializeField] private Transform explosionParticlePrefab;

    private Animator animator;
    private int isRunningHash;
    private int winHash;

    void Awake()
    {
        animator = GetComponent<Animator>();
        isRunningHash = Animator.StringToHash(isRunning);
        winHash = Animator.StringToHash(win);
    }

    void Start()
    {
        GameController.Instance.OnGameStarted += GameController_OnGameStarted;
    }

    public void StartVictoryAnimation()
    {
        animator.SetTrigger(winHash);
    }

    private void GameController_OnGameStarted()
    {
        animator.SetBool(isRunningHash, !isStatic);
    }

    private void AttachToHeap(Vector3 targetPosition)
    {
        isStatic = false;
        animator.SetBool(isRunningHash, !isStatic);

        targetPosition.z += 0.2f;
        transform.position = targetPosition;
        UnitFactory.Instance.AddUnit(transform);

        SpawnParticle(false);
    }

    public void DestroyPlayer()
    {
        SpawnParticle(false);

        Destroy(gameObject);
    }

    public void ExplosionPlayer()
    {
        SpawnParticle(true);

        Destroy(gameObject);
    }

    private void SpawnParticle(bool isExplosion)
    {
        Transform particle;
        if (isExplosion)
            particle = Instantiate(explosionParticlePrefab, transform.position, particlePrefab.rotation);
        else
            particle = Instantiate(particlePrefab, transform.position, particlePrefab.rotation);

        Destroy(particle.gameObject, 0.5f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isStatic) return;

        if (other.TryGetComponent(out Player player))
        {
            AttachToHeap(other.ClosestPointOnBounds(transform.position));
        }
    }
}
