using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject candy_drop, health_drop, energy_drop ;

    public void DropItems()
    {
        int randomNum = Random.Range(1, 3);

        if (randomNum == 1)
        {
            Instantiate(health_drop, transform.position, Quaternion.identity);
            int randomAmountOfCandy = Random.Range(1, 2);

            for (int i = 0; i < randomAmountOfCandy; i++)
            {
                Instantiate(candy_drop, transform.position, Quaternion.identity);
            }

        }

        if (randomNum == 2)
        {
            Instantiate(energy_drop, transform.position, Quaternion.identity);
            int randomAmountOfCandy = Random.Range(1, 2);

            for (int i = 0; i < randomAmountOfCandy; i++)
            {
                Instantiate(candy_drop, transform.position, Quaternion.identity);
            }
        }

        if (randomNum == 3)
        {
            int randomAmountOfCandy = Random.Range(2, 4);

            for (int i = 0; i < randomAmountOfCandy; i++)
            {
                Instantiate(candy_drop, transform.position, Quaternion.identity);
            }
        }
    }
}
