using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBagController : MonoBehaviour
{
    private PlayerInputActions _actions;
    private GameObject         _player;

    public GameObject[] bag;
    public int          bagIndex = 0;
    public int          bagSize  = 8;

    private void Start()
    {
        if (!CompareTag("Player"))
        {
            throw new Exception("PlayerBagController 必须绑定到 tag 'Player' 对象上");
        }

        _player = this.gameObject;
        bag     = new GameObject[8];

        _actions = new PlayerInputActions();

        _actions.Player.DropItem.performed += DropItem; // 丢弃物品
        _actions.Player.UseItem.performed  += UseItem;  // 使用物品
        _actions.Player.NextItem.performed += context =>
        {
            Debug.Log(context.ReadValueAsButton());
            bagIndex = (bagIndex + 1) % bagSize;
        }; // 下一个物品
        _actions.Player.PostItem.performed += context =>
        {
            Debug.Log(context.ReadValueAsButton());
            bagIndex = (bagIndex - 1 + bagSize) % bagSize;
        }; // 上一个物品

        _actions.Player.Enable();
    }

    public void DropItem(InputAction.CallbackContext ctx)
    {
        Debug.Log(ctx.ReadValueAsButton());
    }

    public void UseItem(InputAction.CallbackContext ctx)
    {
        Debug.Log(ctx.ReadValueAsButton());
    }
}