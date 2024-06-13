using UnityEngine;
using UnityEngine.UI;

public class Boss1 : MonoBehaviour
{
    public Enemy en;
    public Image healthBar;
    public float StartingHealth;
    private static int numberOfSplits = 2;
    public static int numberOfEntities = (int)Mathf.Pow(2, numberOfSplits+1) - 1;

    void Start()
    {
        en = GetComponent<Enemy>();
    }
    
    void Update()
    {
        if (en.CurrentHealth / en.MaxHealth < 0.5f && en.MaxHealth / 2 >= StartingHealth / Mathf.Pow(2, numberOfSplits))
        {
            for (int i = 0; i < numberOfSplits; i++)
            {
                GameObject split = Instantiate(gameObject, gameObject.transform.position, gameObject.transform.rotation);
                split.transform.localScale = gameObject.transform.localScale / 2;
                Enemy enemy = split.GetComponent<Enemy>();
                Boss1 boss = split.GetComponent<Boss1>();
                boss.en = enemy;
                enemy.MaxHealth = en.MaxHealth / 2;
                enemy.CurrentHealth = en.CurrentHealth / 2;
            }
            Destroy(gameObject);
            numberOfEntities--;
        }


        healthBar.fillAmount = en.CurrentHealth / en.MaxHealth;
    }
}
