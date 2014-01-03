
using System;
using System.Collections.Generic;
using UnityEngine;

// Scores of the players
public class PlayerScore
{
	
	private PlayerScore() {
	}
	
	private static PlayerScore instance;
	
	public static PlayerScore Instance {
		get {
			if (instance == null) {
				instance = new PlayerScore();
			}
			return instance;
		}
	}
	
	private Dictionary<string, int> scores = new Dictionary<string, int>();
	public Dictionary<string, int> Scores {
		get {
			return scores;
		}
	}
	
		
	public void SetScore(string playerName, int score) {
		scores[playerName] = score;
	}
	
	
}
