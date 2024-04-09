using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEditor;
using UnityEngine;

public class EnemyEditorWindow : EditorWindow
{
    private static EnemyEditorWindow m_Instance;

    private Vector2 m_ScrollViewPosition;
    private List<Enemy> m_Enemies;

    #region Window init
    private void OnEnable()
    {
        m_Enemies = new List<Enemy>();
        for (int i = 0; i < 10; i++) 
        {
            Enemy newEnemy = new Enemy();
            newEnemy.Name = $"Enemey #{i}";
            m_Enemies.Add(newEnemy);
        }
    }

    [MenuItem("Tharle/Enemy Editor")]
    public static void ShowEnemyEditor()
    {
        if (m_Instance != null) return;
        m_Instance = (EnemyEditorWindow) GetWindow(typeof(EnemyEditorWindow));
        m_Instance.titleContent = new GUIContent("Enemy Editor");
    }
    #endregion

    private void OnGUI()
    {
        m_ScrollViewPosition = EditorGUILayout.BeginScrollView(m_ScrollViewPosition);
        {
            EditorGUILayout.LabelField("Ici il faut ajouter du contenu"); 
            EditorGUILayout.BeginHorizontal();
            {
                for (int i = 0; i < m_Enemies.Count; i++)
                {
                    EditorGUILayout.BeginVertical();
                    {
                        EditorGUILayout.LabelField("Name: ");
                        EditorGUILayout.LabelField("HP: ");
                        EditorGUILayout.LabelField("Type: ");
                    }
                    EditorGUILayout.EndVertical();

                    Enemy enemy = m_Enemies[i];
                    // Name
                    EditorGUILayout.BeginVertical();
                    {
                        enemy.Name = EditorGUILayout.TextField(enemy.Name);
                        
                        string hitPointsText =  EditorGUILayout.TextField(enemy.HitPoints.ToString());
                        int.TryParse(hitPointsText, out enemy.HitPoints);

                        enemy.TypeId = (EEnemyType) EditorGUILayout.EnumPopup(enemy.TypeId);

                    }
                    EditorGUILayout.EndVertical();
                    m_Enemies[i] = enemy;

                }
            }
            EditorGUILayout.EndHorizontal();

            // Contenu

        }
        EditorGUILayout.EndScrollView();
    }
}
