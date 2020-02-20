using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    /// <summary>
    /// The layer of object this projectile is looking to hit, defaults to 30 which is the "Enemies" layer
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

    /// <summary>
    /// The attached rigidbody component
    /// </summary>
    private Rigidbody rb;

    /// <summary>
    /// Task cancellation token for the projectile kill
    /// </summary>
    readonly CancellationTokenSource killCancel = new CancellationTokenSource();

    /// <summary>
    /// Variable that represents the projectile kill task, allows collision call to cancel the async expiration 
    /// </summary>
    Task t;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();       
    }

    private void OnEnable()
    {
        KillProjectile(); //ignore this underline
    }


    /// <summary>
    /// Asynchronous function that is called on Awake, this method kills the projectile after its lifetime expires
    /// </summary>
    private async Task KillProjectile()
    {
        //Delay by the lifetime, and give the cancellation token needed to stop the task if the projectile hit its enemy
        t = Task.Delay((int)(projectileLifetime * 1000), killCancel.Token);

        //Call the delay task to start the expiration countdown
        await t;

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
            //Damage the enemy the projectile just hit
            other.GetComponent<EnemyBehaviour>().LoseHP(damageValue);

            //Tell the task cancellation token to cancel the delay task in KillProjectile because we're going to kill the projectile early
            killCancel.Cancel();

            //Instead of destroy send projectile back into Object Pool                   
            this.gameObject.SetActive(false);
        }

    }
}
