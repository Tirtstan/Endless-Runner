public interface IDamagable
{
    public int MaxHealth { get; set; }
    public int CurrentHealth { get; set; }
    public void TakeDamage(int damage);
    public void Heal(int healAmount);
    public void Death();
}
