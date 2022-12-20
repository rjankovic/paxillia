using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GertrudaScript : MonoBehaviour
{
    private const int MIN_POS = -14;
    private const int MAX_POS = 13; //14;
    private const float MOVEMENT_SPEED = 0.1f;

    private int movementDirection = 0;
    private bool shootingEnabled = false;
    private Transform transform;
    [SerializeField]
    private GameObject gun;
    [SerializeField]
    private GameObject _speckPrefab = null;

    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
        EventHub.Instance.OnInputEnabled += OnInputEnabled;
        EventHub.Instance.OnBallBumOffPlayer += OnBallBumOffPlayer;
        EventHub.Instance.OnGrandmaHealthUpdated += OnGrandmaHealthUpdated;
        EventHub.Instance.OnBallLost += OnBallLost;
        EventHub.Instance.OnLevelWon += OnLevelWon;

        movementDirection = 0;
        gun.active = false;
    }

    private void OnLevelWon()
    {
        shootingEnabled = false;
        movementDirection = 0;
    }

    private void OnBallLost(GameObject obj)
    {
        if (GameManager.Instance.BallCount == 0)
        {
            shootingEnabled = false;
        }
    }

    private void OnGrandmaHealthUpdated()
    {
        if (GameManager.Instance.GrandmaHealth <= 950 && !shootingEnabled)
        {
            DialogManager.Instance.StartIngameDialog(new Dialog()
            {
                Messages = new List<Message>()
                {
                    new Message() { Character = Constants.CHAR_GRANDMA, Text = "Au! Time das Speckgewehr 89 zu deploy!", Duration = 5 }
                }
            });

            shootingEnabled = true;
            gun.SetActive(true);
        }
    }

    private void OnBallBumOffPlayer()
    {
        // 50% chance switch direction
        movementDirection *= (Random.value >= 0.5 ? 1 : -1);
    }

    private void OnInputEnabled(bool obj)
    {
        StartMovement();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartMovement()
    {
        movementDirection = Random.value >= 0.5 ? 1 : -1;
    }

    private void FixedUpdate()
    {
        Movement();
        ShootSpeck();
    }

    private void Movement()
    {
        if (movementDirection == 0)
        {
            return;
        }

        var targetPosition = transform.position.x + movementDirection * MOVEMENT_SPEED;

        if (targetPosition >= MAX_POS)
        {
            targetPosition = MAX_POS;
            movementDirection = -1;
        }
        if (targetPosition <= MIN_POS)
        {
            targetPosition = MIN_POS;
            movementDirection = 1;
        }

        transform.Translate(new Vector3(targetPosition - transform.position.x, 0, 0));

        var r = Random.value;
        var changeDirection = r < Time.fixedDeltaTime / 4f;

        if (changeDirection)
        {
            movementDirection *= -1;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != Constants.TAG_BALL)
        {
            return;
        }

        GameManager.Instance.GrandmaHealth -= 50;
    }

    private void ShootSpeck()
    {
        if (!shootingEnabled)
        {
            return;
        }

        var r = Random.value;
        var shoot = r < Time.fixedDeltaTime / 3f;
        if (!shoot)
        {
            return;
        }

        var speckPosition = new Vector3(transform.position.x + 2f, transform.position.y - 1f, 0);
        var speckObject = Instantiate(_speckPrefab, speckPosition, Quaternion.identity);

    }
}
