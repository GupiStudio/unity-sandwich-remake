using UnityEngine;

public class SwipeGesture : MonoBehaviour
{
    [SerializeField]
    private FloatReference _minimumSwipeDistance;

    private Vector3 _startPosition;
    private Vector3 _endPosition;

    private RaycastHit _hit;

    private bool _isSwiping;

    private void Awake() 
    {
        _hit = new RaycastHit();
    }

    private void Update()
    {
        HandleSwipe();
    }

    private void HandleSwipe()
    {
        if (!IsScreenTouched()) { return; }

        Touch touch = Input.GetTouch(0);

        if (!IsTouchHit(touch.position, ref _hit)) { return; }

        DetermineStartPosition(touch);

        if (touch.phase == TouchPhase.Moved)
        {
            _endPosition = touch.position;

            if (_endPosition == _startPosition) { return; }

            if (_isSwiping) { return; }

            var differencePosition = _startPosition - _endPosition;
            differencePosition.Normalize();

            var swipeDirection = new SwipeDirection(differencePosition, _minimumSwipeDistance.Value);

            var ingredient = _hit.transform.GetComponent<IngredientController>();

            if (ingredient != null)
            {
                ingredient.Fold(swipeDirection);
            }
            else
            {
                Debug.LogError("SwipeGesture: ingredient is null");
            }

            _isSwiping = true;
        }

        if (touch.phase == TouchPhase.Ended)
        {
            _isSwiping = false;
        }
    }

    private bool IsScreenTouched()
    {
        return Input.touchCount > 0;
    }

    private bool IsTouchHit(Vector2 touchPosition, ref RaycastHit hit)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        return Physics.Raycast(ray, out hit);
    }

    private void DetermineStartPosition(Touch touch)
    {
        if (touch.phase == TouchPhase.Began)
        {
            _startPosition = touch.position;
        }
    }
}
