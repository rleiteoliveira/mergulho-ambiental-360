using UnityEngine;

namespace MergulhoAmbiental360.Video360
{
    public class Video360SphereSetup : MonoBehaviour
    {
        public float radius = 24f;
        public Material sphereMaterial;

        public Renderer SphereRenderer { get; private set; }

        private void Awake()
        {
            ConfigureSphere();
        }

        public void ConfigureSphere()
        {
            transform.localScale = new Vector3(-radius, radius, radius);

            SphereRenderer = GetComponent<Renderer>();
            if (SphereRenderer == null)
            {
                SphereRenderer = gameObject.AddComponent<MeshRenderer>();
            }

            MeshFilter meshFilter = GetComponent<MeshFilter>();
            if (meshFilter == null)
            {
                meshFilter = gameObject.AddComponent<MeshFilter>();
            }

            if (meshFilter.sharedMesh == null)
            {
                GameObject primitive = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                meshFilter.sharedMesh = primitive.GetComponent<MeshFilter>().sharedMesh;
                Destroy(primitive);
            }

            if (sphereMaterial == null)
            {
                Shader shader = Shader.Find("Unlit/Texture");
                sphereMaterial = new Material(shader);
            }

            SphereRenderer.sharedMaterial = sphereMaterial;
        }
    }
}
