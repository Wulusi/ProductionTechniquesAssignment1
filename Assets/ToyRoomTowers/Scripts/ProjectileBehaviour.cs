using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour, PooledObjInterface
{
    /// <summary>
    /// The layer of object this projectile is looking to hit
    /// </summary>
    [SerializeField]
    [Range(0, 31)]
    private int enemyLayer = 30;

    /// <summary>
    /// The amount of damage this projectile deals (defaults to 2)
    /// </summary>
    public float damageValue = 2;

    /// <summary>
    /// The speed at which this projectile travels
    /// </summary>
    public float projectileSpeed = 10;

    /// <summary>
    /// The amount of time in second that the projectile will be alive for (defaults to 1)
    /// </summary>
    public float projectileLifetime = 1;

    /// <summary>
    /// If this is the first time that the projectile is spawned in the world, if it isn't do not activate the interface
    /// again
    /// </summary>
    public bool isFirstSpawned;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        KillProjectile(); //ignore this underline
    }

    /// <summary>
    /// Asynchronous function that is called on Awake, this method kills the projectile after its lifetime expires
    /// </summary>
    private async Task KillProjectile()
    {
        await Task.Delay((int)(projectileLifetime * 1000));
        //Destroy(gameObject);
        //Instead of destroy send projectile back into Object Pool
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// Moves the projectile by its speed towards its forward direction
    /// </summary>
    private void MoveProjectile()
    {
        rb.velocity = transform.forward * (projectileSpeed * Time.fixedDeltaTime);
    }

    private void FixedUpdate()
    {
        MoveProjectile();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == enemyLayer)
        {
            other.GetComponent<EnemyBehaviour>().LoseHP(damageValue);
            //Destroy(gameObject);
            //Instead of destroy send projectile back into Object Pool
            this.gameObject.SetActive(false);
        }

    }

    public void OnPooledObjSpawn()
    {
        if (isFirstSpawned)
        {
            KillProjectile();
        }
    }
}
