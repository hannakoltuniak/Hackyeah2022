using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Slot : MonoBehaviour
{
    const Boolean SHOW_SLOT_POSITION = false;
    const Single SELECTED_INDICATOR_SCALE = 1.1f;

    public Int32 X { get; set; }
    public Int32 Y { get; set; }

    public Tile Tile;

    public GameObject objSelectedFrameIndicator;
    public TextMeshPro txt;

    public void Init(Int32 x, Int32 y)
    {
        X = x;
        Y = y;

        if (SHOW_SLOT_POSITION)
        {
            txt.text = $"{x} {y}";
        }
        else
        {
            Destroy(txt.gameObject);
        }
    }

    public void SwapWithTile(Tile newTile)
    {
        Tile = newTile;
        newTile.transform.SetParent(transform);
    }

    Boolean _beingInteracted = false;
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _beingInteracted = true;
            //print(X + " " + Y);

            BoardController.Instance.OnSlotClicked(this);

        }
        else if(!_beingInteracted && Input.GetMouseButton(0))
        {

            BoardController.Instance.OnSlotClicked(this);
        }
    }

    public void ShowSelectedIndicator()
    {

        LeanTween.scale(objSelectedFrameIndicator, new Vector3(SELECTED_INDICATOR_SCALE, SELECTED_INDICATOR_SCALE, SELECTED_INDICATOR_SCALE), 0.15f)
            .setEaseOutSine();

    }

    public void CancelSelectedIndicator()
    {
        LeanTween.scale(objSelectedFrameIndicator, Vector3.zero, 0.1f);
        _beingInteracted = false;
    }

    private Vector2 _lastDirection;
    private const Single INDICATOR_ANIMATION_TIME = .35f;
    public void ExpandSelectionTo(Vector2 direction)
    {
        _lastDirection = direction;

        LeanTween.value(1.3f, 2.5f, INDICATOR_ANIMATION_TIME)
            .setOnUpdate((Single val) =>
            {
                if (!objSelectedFrameIndicator)
                    return; 

                var rect = objSelectedFrameIndicator.GetComponent<RectTransform>().rect;
                if (direction == Vector2.up || direction == Vector2.down)
                    rect.height = val;
                else rect.width = val;

                objSelectedFrameIndicator.GetComponent<RectTransform>().rect.Set(rect.x, rect.y, rect.width, rect.height);
                objSelectedFrameIndicator.GetComponent<SpriteRenderer>().size = new Vector2(rect.width, rect.height);
            });

        LeanTween.value(0f, (direction == Vector2.up || direction == Vector2.down ? direction.y : direction.x) * 0.6f, INDICATOR_ANIMATION_TIME)
            .setOnUpdate((Single val) =>
            {
                if (!objSelectedFrameIndicator)
                    return;

                if (direction == Vector2.up || direction == Vector2.down)
                    objSelectedFrameIndicator.transform.localPosition = new Vector3(0, val);
                else
                    objSelectedFrameIndicator.transform.localPosition = new Vector3(val, 0);
            });

    }

    public void RemoveSelectedIndicator()
    {
        _beingInteracted = false;
        LeanTween.value(2.5f, 1.3f, INDICATOR_ANIMATION_TIME)
            .setOnUpdate((Single val) =>
            {
                if (!objSelectedFrameIndicator)
                    return;

                var rect = objSelectedFrameIndicator.GetComponent<RectTransform>().rect;
                if (_lastDirection == Vector2.up || _lastDirection == Vector2.down)
                    rect.height = val;
                else rect.width = val;

                objSelectedFrameIndicator.GetComponent<RectTransform>().rect.Set(rect.x, rect.y, rect.width, rect.height);
                objSelectedFrameIndicator.GetComponent<SpriteRenderer>().size = new Vector2(rect.width, rect.height);
            });

        LeanTween.value((_lastDirection == Vector2.up || _lastDirection == Vector2.down ? _lastDirection.y : _lastDirection.x) * 0.6f, 0f, INDICATOR_ANIMATION_TIME)
            .setOnUpdate((Single val) =>
            {
                if (!objSelectedFrameIndicator)
                    return;

                if (_lastDirection == Vector2.up || _lastDirection == Vector2.down)
                    objSelectedFrameIndicator.transform.localPosition = new Vector3(0, val);
                else
                    objSelectedFrameIndicator.transform.localPosition = new Vector3(val, 0);
            });

        LeanTween.delayedCall(INDICATOR_ANIMATION_TIME, () => LeanTween.scale(objSelectedFrameIndicator, Vector3.zero, INDICATOR_ANIMATION_TIME));
    }

    public override bool Equals(object other)
    {
        if (other is Slot secondSlot)
        {
            return this.X == secondSlot.X && this.Y == secondSlot.Y;
        }

        return base.Equals(other);
    }

    internal void ShakeIndicator()
    {
        StartCoroutine(ShakeIndicatorInternal());
    }

    private IEnumerator ShakeIndicatorInternal()
    {
        var colorBefore = objSelectedFrameIndicator.GetComponent<SpriteRenderer>().color;
        objSelectedFrameIndicator.GetComponent<SpriteRenderer>().color = Color.yellow;
        yield return new WaitForSeconds(0.25f);
        objSelectedFrameIndicator.GetComponent<SpriteRenderer>().color = colorBefore;

    }
}
