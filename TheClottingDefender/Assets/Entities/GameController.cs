using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Entities;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{

	public int MaxHealth = 100;
	public int Money = 500;
	public Level LevelPrefab;
	public float WaveInterval = 4f;
	public float EnemyInterval = 0.1f;
	public int MinWaveStrength = 20;
	public int MaxWaveStrength = 50;
	public int MinEnemiesPerSingleSpawn = 1;
	public int MaxEnemiesPerSingleSpawn = 15;

	private Level level;
	private RegularTimer waveTimer;
	private RegularTimer enemyTimer;
	private int EnemiesToSpawn = 0;
	private UnityAction enemyDieAction; 

	// Use this for initialization
	void Start ()
	{
		level = Instantiate(LevelPrefab);
		waveTimer = new RegularTimer(WaveInterval);
		enemyTimer= new RegularTimer(EnemyInterval);
		StartWave(Random.Range(MinWaveStrength, MaxWaveStrength));
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (waveTimer.CheckElapsedAndAutoRewind()) StartWave(Random.Range(MinWaveStrength, MaxWaveStrength));
		if (EnemiesToSpawn > 0 && enemyTimer.CheckElapsedAndAutoRewind())
		{
			Enemy enemy = level.SpawnEnemy();
			enemy.dieEvent.AddListener(EnemyDied);
			EnemiesToSpawn--;
		}
		
		
	}

	private void StartWave(int waveStrength)
	{
		EnemiesToSpawn += waveStrength;
	}

	private void SpawnEnemies(int enemyCount)
	{
		enemyCount = EnemiesToSpawn < enemyCount ? EnemiesToSpawn : enemyCount;
		for (int i = 0; i < enemyCount; i++)
		{
			level.SpawnEnemy();
			EnemiesToSpawn--;
		}
	}

	private void EnemyDied(Enemy diedEnemy)
	{
		Money += diedEnemy.Bounty;
	}
}
