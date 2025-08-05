using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class HealthZombie : MonoBehaviour
{
    private GameObject target;
    private NavMeshAgent agente;

    private Animator animator;
    private Collider collider1;
    private Vida vidaJugador;
    private LogicaJugador logicaRegular_Character;
    public bool Vida0 = false;
    public bool estaAtacando = false;
    public float speed = 1.0f;
    public float angularSpeed = 120;
    public float daño = 25;
    public bool mirando;
    public Vida vida;
    public bool canDie = true;                  // Whether or not this health can die

    public float startingHealth = 100.0f;       // The amount of health to start with
    public float maxHealth = 100.0f;            // The maximum amount of health
    private float currentHealth;                // The current ammount of health

    public bool replaceWhenDead = false;        // Whether or not a dead replacement should be instantiated.  (Useful for breaking/shattering/exploding effects)
    public GameObject deadReplacement;          // The prefab to instantiate when this GameObject dies
    public bool makeExplosion = false;          // Whether or not an explosion prefab should be instantiated
    public GameObject explosion;                // The explosion prefab to be instantiated

    public bool isPlayer = false;               // Whether or not this health is the player
    public GameObject deathCam;                 // The camera to activate when the player dies

    private bool dead = false;                  // Used to make sure the Die() function isn't called twice

    // Use this for initialization
    void Start()
    {
        // Initialize the currentHealth variable to the value specified by the user in startingHealth
        currentHealth = startingHealth;
    }
    void Update()
    {
        //RevisarVida();
        Perseguir();
        RevisarAtaque();
        EstaDefrenteAlJugador();
     
        
    }
    void EstaDefrenteAlJugador()
    {
        Vector3 adelante = transform.forward;
        Vector3 targetJugador = (GameObject.Find("Regular_Character").transform.position - transform.position).normalized;

        if (Vector3.Dot(adelante, targetJugador) < 0.6f)
        {
            mirando = false;
        }
        else
        {
            mirando = true;
        }
    }

    public void ChangeHealth(float amount)
    {
        // Change the health by the amount specified in the amount variable
        currentHealth += amount;

        // If the health runs out, then Die.
        if (currentHealth <= 0 && !dead && canDie)
            Die();

        // Make sure that the health never exceeds the maximum health
        else if (currentHealth > maxHealth)
            currentHealth = maxHealth;
    }

    public void Die()
    {
        // This GameObject is officially dead.  This is used to make sure the Die() function isn't called again
        dead = true;

        // Make death effects
        if (replaceWhenDead)
            Instantiate(deadReplacement, transform.position, transform.rotation);
        animator.CrossFadeInFixedTime("Vida0", 0.1f);
        if (makeExplosion)
            Instantiate(explosion, transform.position, transform.rotation);

        if (isPlayer && deathCam != null)
            deathCam.SetActive(true);

        // Remove this GameObject from the scene
        Destroy(gameObject);
    }
    void Perseguir()
    {
        if (Vida0) return;
        if (logicaRegular_Character.Vida0) return;
        agente.destination = target.transform.position;
    }
    void RevisarAtaque()
    {
        if (Vida0) return;
        if (estaAtacando) return;
        if (logicaRegular_Character.Vida0) return;
        float distanciaDelBlanco = Vector3.Distance(target.transform.position, transform.position);

        if (distanciaDelBlanco <= 2.0 && mirando)
        {
            Atacar();
        }
    }
    void Atacar()
    {
        vidaRegular_Character.RecibirDaño(daño);
        agente.speed = 0;
        agente.angularSpeed = 0;
        estaAtacando = true;
        animator.SetTrigger("DebeAtacar");
        Invoke("ReiniciarAtaque", 1.5f);
    }
    void ReiniciarAtaque()
    {
        estaAtacando = false;
        agente.speed = speed;
        agente.angularSpeed = angularSpeed;
    }
}
