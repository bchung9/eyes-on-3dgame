using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    public float interactDistance = 10f;

    private PipePiece targetedPipe;
    private NavKeypad.KeypadButton targetedNavButton;
    private InteractableDrawing targetedDrawing;

    private InteractableDrawing heldDrawing;

    private Outline currentOutline;
    private PlayerControls controls;

    void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Interact.performed += ctx => OnInteract();
    }

    void OnEnable() => controls.Enable();
    void OnDisable() => controls.Disable();

    void Update()
    {
        DetectInteractable();
    }

    void DetectInteractable()
    {
        if (heldDrawing != null)
        {
            ClearOutline();
            targetedPipe = null;
            targetedNavButton = null;
            targetedDrawing = null;
            return;
        }

        ClearOutline();

        targetedPipe = null;
        targetedNavButton = null;
        targetedDrawing = null;

        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            InteractableDrawing drawing = hit.collider.GetComponent<InteractableDrawing>();
            if (drawing != null)
            {
                targetedDrawing = drawing;
                ApplyOutline(drawing);
                return;
            }

            PipePiece pipe = hit.collider.GetComponent<PipePiece>();
            if (pipe != null)
            {
                targetedPipe = pipe;
                ApplyOutline(pipe);
                return;
            }

            NavKeypad.KeypadButton navButton = hit.collider.GetComponent<NavKeypad.KeypadButton>();
            if (navButton != null)
            {
                targetedNavButton = navButton;

                Outline outline = navButton.GetComponent<Outline>();
                if (outline != null)
                {
                    outline.enabled = true;
                    currentOutline = outline;
                }
                return;
            }
        }
    }

    void ApplyOutline(Component obj)
    {
        Outline outline = obj.GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = true;
            currentOutline = outline;
        }
    }

    void ClearOutline()
    {
        if (currentOutline != null)
            currentOutline.enabled = false;

        currentOutline = null;
    }

    void OnInteract()
    {
        if (heldDrawing != null)
        {
            heldDrawing.Drop(this);
            return;
        }

        if (targetedDrawing != null)
        {
            targetedDrawing.Pickup(this);
            return;
        }

        if (targetedPipe != null)
        {
            targetedPipe.RotatePiece();
            return;
        }

        if (targetedNavButton != null)
        {
            targetedNavButton.PressButton();
            return;
        }
    }

    public void HoldDrawing(InteractableDrawing drawing)
    {
        heldDrawing = drawing;

        drawing.transform.SetParent(Camera.main.transform);
    }

    public void ReleaseDrawing()
    {
        heldDrawing = null;
    }

    void OnGUI()
    {
        if (heldDrawing != null)
        {
            GUI.Label(
                new Rect(Screen.width / 2 - 70, Screen.height - 100, 300, 30),
                "Press E to put back"
            );
        }
        else if (targetedDrawing != null)
        {
            GUI.Label(
                new Rect(Screen.width / 2 - 70, Screen.height - 100, 300, 30),
                "Press E to pick up"
            );
        }
        else if (targetedPipe != null)
        {
            GUI.Label(
                new Rect(Screen.width / 2 - 50, Screen.height - 100, 200, 30),
                "Press E to rotate"
            );
        }
        else if (targetedNavButton != null)
        {
            GUI.Label(
                new Rect(Screen.width / 2 - 50, Screen.height - 100, 200, 30),
                "Press E to input digit"
            );
        }
    }
}
