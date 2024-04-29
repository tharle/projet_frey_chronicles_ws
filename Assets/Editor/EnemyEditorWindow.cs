using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyEditorWindow : EditorWindow
{
    private static EnemyEditorWindow m_Instance;

    private Vector2 m_ScrollViewPosition;
    private List<Enemy> m_Enemies;
    private GUIContent[] m_EnemyContents;

    private int m_SelectedIndex;

    #region Window init
    private void OnEnable()
    {
        m_SelectedIndex = 0;
        CreateContent();
    }

    private void CreateContent()
    {
        string[] paths = { "Assets/BundleAssets/Data" };
        string[] guids = AssetDatabase.FindAssets("t:EnemyData", paths);
        m_Enemies = new List<Enemy>();
        List<GUIContent> enemyContents = new List<GUIContent>();
        foreach (string guid in guids) 
        { 
            string path = AssetDatabase.GUIDToAssetPath(guid);
            EnemyData enemyData = AssetDatabase.LoadAssetAtPath<EnemyData>(path);
            
            if (enemyData == null) continue;

            Enemy enemy = enemyData.Value;

            GUIContent guiContent = new();
            guiContent.text = enemy.Name;
            enemyContents.Add(guiContent);
            m_Enemies.Add(enemy);

        }

        m_EnemyContents = enemyContents.ToArray();
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
        m_SelectedIndex = GUILayout.SelectionGrid(m_SelectedIndex, m_EnemyContents, 4);

        DrawUILine(Color.black);

        ShowEnemy();
        EditorGUILayout.EndScrollView();
    }

    private void ShowEnemy()
    {
        if (m_Enemies.Count < 0) return;

        EditorGUILayout.BeginHorizontal();
        { 
            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.LabelField("Name: ");
                EditorGUILayout.LabelField("HP: ");
                EditorGUILayout.LabelField("Type: ");
            }
            EditorGUILayout.EndVertical();

            Enemy enemy = m_Enemies[m_SelectedIndex];
            GUIContent enemyContent = m_EnemyContents[m_SelectedIndex];
            EditorGUILayout.BeginVertical();
            {
                enemy.Name = EditorGUILayout.TextField(enemy.Name);
                enemyContent.text = enemy.Name;

                string hitPointsText = EditorGUILayout.TextField(enemy.HitPoints.ToString());
                int.TryParse(hitPointsText, out enemy.HitPoints);

                enemy.TypeId = (EEnemyType) EditorGUILayout.EnumPopup(enemy.TypeId);
                m_Enemies[m_SelectedIndex] = enemy;
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();
    }

    public static void DrawUILine(Color color, int thickness = 1, int padding = 10)
    {
        Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
        r.height = thickness;
        r.y += padding / 2;
        r.x -= 2;
        r.width += 6;
        EditorGUI.DrawRect(r, color);
    }
}
