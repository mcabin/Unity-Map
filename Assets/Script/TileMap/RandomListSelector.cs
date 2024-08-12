using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomListSelector
{
    
        private System.Random random ;

    public RandomListSelector(int seed)
    {
        random = new System.Random(seed);
    }

        public T SelectWeighted<T>(IEnumerable<T> items, Func<T, int> weightSelector)
        {
            var totalWeight = items.Sum(weightSelector);
            var randomWeight = random.Next(totalWeight);
            var currentWeight = 0;

            foreach (var item in items)
            {
                currentWeight += weightSelector(item);
                if (randomWeight < currentWeight)
                    return item;
            }

            // Cela ne devrait jamais arriver, mais c'est une bonne pratique de le gérer
            throw new InvalidOperationException("Aucun élément sélectionné");
        }
    
}
