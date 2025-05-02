using UnityEngine;

public class Player : CharacterBase, ILives
{
    private Rigidbody rb;

    public int MaxLives { get; private set; } = 3;
    public int CurrentLives { get; private set; } = 3;
    
    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
    }

    public void LoseLife()
    {
        CurrentLives--;
        if (CurrentLives <= 0)
        {
            Die();
        }
    }

    public override void Die()
    {
        base.Die();
        SceneController.Instance.LoadGameOverScene();
    }   
}
