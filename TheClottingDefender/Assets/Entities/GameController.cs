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

	public AudioSource GameStartAudio;
	public AudioSource GameOverAudio;
	public AudioSource WaveStartAudio;
	public AudioSource WaveEndAudio;
	public AudioSource TowerShootAudio;
	public AudioSource TowerBuiltAudio;
	public AudioSource TowerDestroyedAudio;
	public AudioSource EnemyHitAudio;
	public AudioSource EnemyKilledAudio;

	public AudioSource Music;

	private Level level;
	private RegularTimer waveTimer;
	private RegularTimer enemyTimer;
	private int enemiesToSpawn;
	private UnityAction enemyDieAction;
	private bool isGameOver = false;

	// Use this for initialization
	void Start()
	{
		level = Instantiate(LevelPrefab);
		level.GameController = this;
		waveTimer = new RegularTimer(WaveInterval);
		enemyTimer = new RegularTimer(EnemyInterval);
		GameStartAudio.Play();
		Music.Play();
		isGameOver = false;
	}

	// Update is called once per frame
	void Update()
	{
		if (waveTimer.CheckElapsedAndAutoRewind()) StartWave(Random.Range(MinWaveStrength, MaxWaveStrength));
		if (enemiesToSpawn > 0 && enemyTimer.CheckElapsedAndAutoRewind())
		{
			Enemy enemy = level.SpawnEnemy();
			enemy.DeathEvent.AddListener(EnemyDied);
			enemy.HitEvent.AddListener(EnemyHit);
			enemiesToSpawn--;
		}
		if (!isGameOver && !Music.isPlaying) Music.Play();
	}

	private void StartWave(int waveStrength)
	{
		enemiesToSpawn += waveStrength;
		WaveStartAudio.Play();
	}

	private void EnemyDied(Enemy diedEnemy)
	{
		Money += diedEnemy.Bounty;
		EnemyKilledAudio.Play();
	}

	private void EnemyHit(Enemy diedEnemy)
	{
		EnemyHitAudio.Play();
	}

	public void OnEnemyPasses(Enemy enemy)
	{
		Health = Math.Max(0, Health - enemy.DamageOnPassing);
		Destroy(enemy.gameObject);
		if (Health <= 0 && !isGameOver)
		{
			GameOverCanvas.gameObject.SetActive(true);
			GameOverAudio.Play();
			Music.Stop();
			isGameOver = true;
		}
	}
}