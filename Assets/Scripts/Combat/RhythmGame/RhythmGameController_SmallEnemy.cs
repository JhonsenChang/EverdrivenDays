// This is a duplicate of RhythmGameController, to be adapted for small fry enemies.
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

namespace EverdrivenDays
{
    [Serializable]
    public class RhythmNote
    {
        public int lane; // 0, 1, 2, or 3 (4-lane gameplay)
        public float time; // Time in seconds when the note should be hit
        public float duration = 0f; // For hold notes - 0 for tap notes
    }

    [Serializable]
    public class SongData
    {
        public string songName;
        public AudioClip songClip;
        public float bpm;
        public float offset = 0f; // Start offset in seconds
        public List<RhythmNote> notes = new List<RhythmNote>(); // Will be auto-generated if empty
        public int difficulty = 1; // 1-10 scale
        public bool generateNotes = true; // Whether to procedurally generate notes
    }

    [Serializable]
    public class NoteGenerationSettings
    {
        [Header("Basic Settings")]
        public bool enabled = true;
        [Range(1, 10)]
        public int density = 5; // 1-10 scale of how many notes to generate
        [Range(0f, 1f)]
        public float randomness = 0.2f; // How much randomness to add to note timings
        
        [Header("Pattern Settings")]
        public bool usePatterns = true;
        [Range(0, 100)]
        public int chanceOfDoubleNote = 15; // % chance of having 2 notes at once
        [Range(0, 100)]
        public int chanceOfTripleNote = 5; // % chance of having 3 notes at once
        [Range(0, 100)]
        public int chanceOfHoldNote = 10; // % chance of generating a hold note
        [Range(0.1f, 2f)]
        public float holdNoteDuration = 0.5f; // Duration of hold notes in beats
        
        [Header("Rhythm Settings")]
        public bool useQuarterNotes = true; // Notes on quarter beats (1, 2, 3, 4)
        public bool useEighthNotes = true; // Notes on eighth beats (1, 1.5, 2, 2.5, etc)
        public bool useSixteenthNotes = false; // Notes on sixteenth beats
        public bool useTriplets = false; // Notes on triplet beats
        
        // Advanced settings (not shown in inspector)
        [HideInInspector]
        public bool useHighDensityPatterns = false; // For extremely difficult patterns
    }

    public class RhythmGameController_SmallEnemy : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private GameObject notePrefab;
        [SerializeField] private RectTransform[] laneTargets; // The hit positions
        [SerializeField] private RectTransform[] laneSpawnPoints; // Where notes spawn from
        [SerializeField] private Slider progressBar;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI comboText;
        [SerializeField] private TextMeshProUGUI accuracyText;
        [SerializeField] private TextMeshProUGUI gradeText;
        [SerializeField] private PlayerStats playerStats; // Reference to the player's stats
        [SerializeField] private int enemyAttack = 10; // Enemy's attack stat for rhythm game damage

        [Header("Game Settings")]
        [SerializeField] private List<SongData> availableSongs = new List<SongData>();
        [SerializeField] private float noteSpeed = 500f; // Speed in units per second
        [SerializeField] private float perfectWindow = 30f; // Time in milliseconds (tighter than before)
        [SerializeField] private float goodWindow = 60f; // Time in milliseconds (tighter than before)
        [SerializeField] private float okayWindow = 90f; // Time in milliseconds (tighter than before)

        [Header("Procedural Generation")]
        [SerializeField] private NoteGenerationSettings noteGenSettings = new NoteGenerationSettings();

        [Header("Key Bindings")]
        [SerializeField] private KeyCode[] laneKeys = new KeyCode[4] { KeyCode.D, KeyCode.F, KeyCode.J, KeyCode.K };

        // Game state
        private bool isPlaying = false;
        private float gameStartTime;
        private float currentPlayTime;
        private SongData currentSong;
        private List<GameObject> activeNotes = new List<GameObject>();
        private List<RhythmNote> remainingNotes = new List<RhythmNote>();
        private int currentScore = 0;
        private int maxCombo = 0;
        private int currentCombo = 0;
        private int perfectHits = 0;
        private int goodHits = 0;
        private int okayHits = 0;
        private int missedHits = 0;
        private bool[] lanePressed = new bool[4];

        // Properties for external access
        public bool IsGameActive => isPlaying;
        public bool PlayerWon { get; private set; }

        // The rest of the logic will be adapted in the next steps
    }
} 