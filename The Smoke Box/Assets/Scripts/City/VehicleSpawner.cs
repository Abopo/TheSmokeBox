using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSpawner : MonoBehaviour {

    [SerializeField]
    GameObject[] _vehicles;

    [SerializeField]
    float _spawnMin = 0;
    [SerializeField]
    float _spawnMax = 5;
    float _nextSpawnTime = 0f;

    float _spawnTimer = 0f;

    [SerializeField]
    float _speed;

    // Start is called before the first frame update
    void Start() {
        NextSpawnTime();
    }

    void NextSpawnTime() {
        _nextSpawnTime = Random.Range(_spawnMin, _spawnMax);
        _spawnTimer = 0f;
    }

    // Update is called once per frame
    void Update() {
        _spawnTimer += Time.deltaTime;
        if (_spawnTimer > _nextSpawnTime) {
            SpawnVehicle();
            NextSpawnTime();
        }
    }

    void SpawnVehicle() {
        int rand = Random.Range(0, _vehicles.Length);
        GameObject vehicle = Instantiate(_vehicles[rand], transform);
        vehicle.GetComponent<Vehicle>().moveSpeed = _speed;
    }
}
