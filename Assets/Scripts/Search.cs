using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Search : MonoBehaviour
{
    [SerializeField]
    private InputField InputText;

    public void SearchItems()
    {
        Node temp = BinaryTree.SearchTree(InputText.text);
        if (temp != null)
        {

        }
    }
}
