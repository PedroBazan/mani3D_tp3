
using UnityEngine;

public class CameraConfigurator : MonoBehaviour
{
    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void OnGUI()
    {
        if (cam == null)
            return;

        GUILayout.BeginArea(new Rect(10, 10, 300, 320), GUI.skin.box);

        GUILayout.Label("Camera Configuration");

        string projection = cam.orthographic ? "Orthographic" : "Perspective";

        if (GUILayout.Button("Projection: " + projection))
        {
            cam.orthographic = !cam.orthographic;
        }

        if (cam.orthographic)
        {
            GUILayout.Label("Size: " + cam.orthographicSize.ToString("F1"));
            cam.orthographicSize = GUILayout.HorizontalSlider(
                cam.orthographicSize, 0.1f, 100f);
        }
        else
        {
            GUILayout.Label("Field of View: " + cam.fieldOfView.ToString("F1"));
            cam.fieldOfView = GUILayout.HorizontalSlider(
                cam.fieldOfView, 1f, 180f);
        }

        GUILayout.Space(8);
        GUILayout.Label("Clipping Planes");

        GUILayout.Label("Near: " + cam.nearClipPlane.ToString("F2"));
        float near = GUILayout.HorizontalSlider(
            cam.nearClipPlane, 0.01f, 100f);

        GUILayout.Label("Far: " + cam.farClipPlane.ToString("F0"));
        float far = GUILayout.HorizontalSlider(
            cam.farClipPlane, 1f, 1000f);

        // Mantener Far por delante de Near.
        cam.nearClipPlane = Mathf.Min(near, far - 0.01f);
        cam.farClipPlane = Mathf.Max(far, cam.nearClipPlane + 0.01f);

        GUILayout.EndArea();
    }

    private void OnDrawGizmos()
    {
        Camera cameraComponent = GetComponent<Camera>();

        if (cameraComponent == null)
            return;

        Gizmos.color = Color.yellow;

        Matrix4x4 previousMatrix = Gizmos.matrix;
        Gizmos.matrix = cameraComponent.transform.localToWorldMatrix;

        if (cameraComponent.orthographic)
        {
            float height = cameraComponent.orthographicSize * 2f;
            float width = height * cameraComponent.aspect;

            Vector3 center = new Vector3(
                0f, 0f,
                (cameraComponent.nearClipPlane + cameraComponent.farClipPlane) / 2f
            );

            Vector3 size = new Vector3(
                width,
                height,
                cameraComponent.farClipPlane - cameraComponent.nearClipPlane
            );

            Gizmos.DrawWireCube(center, size);
        }
        else
        {
            Gizmos.DrawFrustum(
                Vector3.zero,
                cameraComponent.fieldOfView,
                cameraComponent.farClipPlane,
                cameraComponent.nearClipPlane,
                cameraComponent.aspect
            );
        }

        Gizmos.matrix = previousMatrix;
    }
}
