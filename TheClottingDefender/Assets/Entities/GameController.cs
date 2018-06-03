using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Entities;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
	public int Health = 100;
	public int Money = 500;
	public Level LevelPrefab;
	public float WaveInterval = 4f;
	public float EnemyInterval = 0.1f;
	public int MinWaveStrength = 20;
	public int MaxWaveStrength = 50;

	public Canvas GameOverCanvas;

	private Level level;
	private RegularTimer waveTimer;
	private RegularTimer enemyTimer;
	private int EnemiesToSpawn;
	private UnityAction enemyDieAction;

	// Use this for initialization
	void Start()
	{
		level = Instantiate(LevelPrefab);
		level.GameController = this;
		waveTimer = new RegularTimer(WaveInterval);
		enemyTimer = new RegularTimer(EnemyInterval);
		StartWave(Random.Range(MinWaveStrength, MaxWaveStrength));
	}

	// Update is called once per frame
	void Update()
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

	private void EnemyDied(Enemy diedEnemy)
	{
		Money += diedEnemy.Bounty;
	}

	public void OnEnemyPasses(Enemy enemy)
	{
		Health = Math.Max(0, Health - enemy.DamageOnPassing);
		Destroy(enemy.gameObject);
		if (Health == 0) GameOverCanvas.gameObject.SetActive(true);
	}
}