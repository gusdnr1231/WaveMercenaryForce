using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRenderer : MonoBehaviour, IPlayerComponent
{
    //private readonly int _disappearPropertyId = Shader.PropertyToID("_Disappear");

    private PlayerCharacter _plc;
    private PlayerMovement _movement;

    private SpriteRenderer _spriteRenderer;
    private Material characterMat;

    public void Initilize(PlayerCharacter plc)
    {
        _plc = plc;
        _movement = plc.GetCompo<PlayerMovement>();

        _spriteRenderer = GetComponent<SpriteRenderer>();
        characterMat = _spriteRenderer.material;
    }

    public void AfterInitilize()
    {
        _movement.MoveToDirection += HandleMoveDirection;
    }

    private void HandleMoveDirection(Vector3 dir)
    {
        _spriteRenderer.flipX = dir.x < _plc.transform.position.x;
    }
}
