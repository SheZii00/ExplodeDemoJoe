using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Explode : MonoBehaviour
{
    public List<GOs> GO;
    public Material BlikerMat;
    public Transform LastFocusedGO;
    public CameraOrbit cameraOrbit;
    public Transform LastCameraPosition;
    public bool isLastFocused;

    public void Update()
    {
        if (isLastFocused)
        {
            //if (Vector3.Distance(Camera.main.transform.position, LastCameraPosition.position) > 0.05f)
            //    Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, LastCameraPosition.position, Time.deltaTime * 0.75f);

            if (cameraOrbit.distance > 3.5f)
                cameraOrbit.distance = Mathf.Lerp(cameraOrbit.distance, 3.5f, Time.deltaTime * 1.5f);
        }
    }

    public void StartExplode()
    {
        StartCoroutine(StartExplodeGOs());
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public IEnumerator StartExplodeGOs()
    {
        foreach (GOs go in GO)
        {
            foreach (GameObject g in go.gos)
            {
                var rb = g.AddComponent<Rigidbody>();
                rb.useGravity = false;

                foreach(MeshRenderer mr in g.GetComponentsInChildren<MeshRenderer>())
                    mr.material = BlikerMat;
            }

            yield return new WaitForSeconds(go.time / 2);

            foreach (GameObject g in go.gos)
            {
                g.AddComponent<ConstantForce>();
                g.GetComponent<ConstantForce>().force = go.angle;
                Destroy(g, 5);
                yield return new WaitForSeconds(go.timeIn);
            }

            yield return new WaitForSeconds(go.time);
        }

        InvokeRepeating("LookAt", 0.01f, 0.01f);

        isLastFocused = true;
    }

    private void LookAt ()
    {
        if(!isLastFocused)
            Camera.main.transform.LookAt(LastFocusedGO);
    }

    [Serializable]
    public struct GOs
    {
        public List<GameObject> gos;
        public Vector3 angle;
        public float time;
        public float timeIn;
    }
}

