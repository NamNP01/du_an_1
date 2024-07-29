using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public int coin;
    public Animator ani; 
    private Transform target;
    public int damage;
    [Header("Audio")]
    public AudioSource src;
    public AudioClip _Die;

    public void Seek(Transform target, int damage, ArcherTower.TowerType towerType, float slowAmount, float slowTime)
    {
        this.target = target;
        this.damage = damage;
    }
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0)
            return; // Bỏ qua nếu đã chết

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Dynamite")&&collision.gameObject!= target)
    //    {
    //        TakeDamage(damage);
    //    }
    //}
    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
    private void Die()
    {
        src.clip = _Die;
        src.Play();
        // Thực hiện các hành động khi enemy chết như phát hiện animation, thêm tiền, v.v.
        CoinManager.instance.AddCoins(coin);
        Destroy(gameObject, 0.3f);
        ani.SetTrigger("Die");
    }
}
