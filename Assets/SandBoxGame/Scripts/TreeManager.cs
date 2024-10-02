using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
    Animator animator;

    private int treeHP = 3;
    public GameObject dropItem;
    private int dropItemCount;

    private void Start()
    {
        animator = GetComponent<Animator>();
        dropItemCount = Random.Range(1, 4);
    }

    public void CutTree()
    {
        animator.SetTrigger("TreeAttack");
        treeHP -= 1;

        if (treeHP <= 0)
        {
            DropItem();
            Destroy(gameObject);
        }
    }

    void DropItem()
    {
        for (int i = 0; i < dropItemCount; i++)
        {
            Instantiate(dropItem, transform.position - new Vector3(0, 0.8f, 0), Quaternion.identity);
        }
    }
}
