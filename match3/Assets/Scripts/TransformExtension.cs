using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtension {

    public static IEnumerator Move(this Transform transform, Vector3 target, float duration)
    {
        Vector3 diffVector = (target - transform.position);
        float diffLength = diffVector.magnitude;
        diffVector.Normalize();
        float counter = 0;
        while(counter < duration)
        {
            float movemount = (Time.deltaTime * diffLength / duration);

            transform.position += diffVector * movemount;
            counter += Time.deltaTime;
            yield return null;
        }
        transform.position = target;
    }

}
