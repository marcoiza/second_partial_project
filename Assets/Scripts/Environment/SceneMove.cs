using System.Collections.Generic;
using UnityEngine;

    public class SceneMove : MonoBehaviour
    {
        private float speed = -5f; //Velocidad con la que se mueve el escenario
        private static bool isPaused = false; //Atributo que verifica si el movimiento del escenario y la generación de objetos debe estar en pausa
        private static List<SceneMove> allSceneMoves = new List<SceneMove>(); // Lista estática para todas las instancias de SceneMove
        
        void Update()
        {
            if (!isPaused) {
                transform.position += new Vector3(0, 0, speed) * Time.deltaTime;
            }

            // Pausar el movimiento al presionar la tecla "P"
            if (Input.GetKeyDown(KeyCode.P))
            {
                isPaused = !isPaused;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Destroy"))
            {
                Destroy(gameObject);
            }
        }

        public void SetPauseState(bool newState)
        {
            isPaused = newState;
        }

        public static void AddSceneMoveInstance(SceneMove instance)
        {
            if (!allSceneMoves.Contains(instance))
            {
                allSceneMoves.Add(instance);
            }
        }

        public static void RemoveSceneMoveInstance(SceneMove instance)
        {
            allSceneMoves.Remove(instance);
        }

        public static List<SceneMove> GetAllSceneMoves()
        {
            return allSceneMoves;
        }

    }