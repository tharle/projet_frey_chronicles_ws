using Codice.CM.Client.Differences.Graphic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Unity.Plastic.Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemyEditorWindow : EditorWindow
{
    private static EnemyEditorWindow m_Instance;

    private Vector2 m_ScrollViewPosition;
    private List<EnemyData> m_Enemies;
    private GUIContent[] m_EnemyContents;
    private string m_Path = GameParametres.BundlePath.BUNDLE_ASSETS + GameParametres.BundlePath.DATA;

    private int m_SelectedIndexOld;
    private int m_SelectedIndex;
    private bool m_IsEnemyChanged;

    private Enemy m_SelectedEnemy;

    #region Window init
    private void OnEnable()
    {
        m_SelectedIndex = 0;
        CreateContent();

        m_SelectedEnemy = m_Enemies[m_SelectedIndex].Value;
        m_IsEnemyChanged = false;
    }

    private void CreateContent()
    {
        string[] paths = { m_Path };
        string[] guids = AssetDatabase.FindAssets("t:EnemyData", paths);
        m_Enemies = new List<EnemyData>();
        List<GUIContent> enemyContentList = new List<GUIContent>();
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            EnemyData enemyData = AssetDatabase.LoadAssetAtPath<EnemyData>(path);

            if (enemyData == null) continue;

            enemyContentList.Add(AddEnemy(enemyData));
        }

        m_EnemyContents = enemyContentList.ToArray();
    }

    private GUIContent AddEnemy(EnemyData enemyData)
    {
        //GameObject go = BundleLoader.Instance.Load<GameObject>(GameParametres.BundleNames.PREFAB_ENEMY, nameof(enemy.TypeId));
        GUIContent guiContent = new();
        guiContent.text = enemyData.Value.Name;
        m_Enemies.Add(enemyData);
        return guiContent;
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
        m_SelectedIndexOld = m_SelectedIndex;
        m_SelectedIndex = GUILayout.SelectionGrid(m_SelectedIndex, m_EnemyContents, 4);

        if (m_SelectedIndex != m_SelectedIndexOld) 
        { 
            CheckAndSaveSelectEnemy();
            m_SelectedEnemy = m_Enemies[m_SelectedIndex].Value;
            m_IsEnemyChanged = false;
            GUI.FocusControl(null);
        }

        DrawUILine(Color.black);
        ShowOperationButtons();
        DrawUILine(Color.black);
        ShowEnemy();
        EditorGUILayout.EndScrollView();
        DrawUILine(Color.black);
        ShowEndButtons();
    }

    private void CheckAndSaveSelectEnemy()
    {
        if(m_IsEnemyChanged && EditorUtility.DisplayDialog("Change enemy?",
                $"Do you want to save all your modification in \"{m_SelectedEnemy.Name}\" before?", "Yes", "No"))
        {
            m_Enemies[m_SelectedIndexOld].Value = m_SelectedEnemy;
            GUIContent enemyContent = m_EnemyContents[m_SelectedIndexOld];
            enemyContent.text = m_SelectedEnemy.Name;
            m_IsEnemyChanged = false;

            // TODO vérifier s'il n'existe pas
            // TODO add rename
            //AssetDatabase.RenameAsset($"{m_Path}{oldName}.asset", $"{enemy.Name}.asset");
        }
    }

    private void ShowOperationButtons()
    {
        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("+")) OnClickAddEnemy();
            if (GUILayout.Button("-")) OnClickRemoveSelectedEnemy();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void OnClickAddEnemy()
    {
        EnemyData enemy = new EnemyData();
        enemy.Value.Name = m_SelectedEnemy.Name + " 1";
        enemy.Value.TypeId = m_SelectedEnemy.TypeId;
        enemy.Value.ElementalId = m_SelectedEnemy.ElementalId;
        enemy.Value.HitPoints = m_SelectedEnemy.HitPoints;
        enemy.Value.HitPointsMax = m_SelectedEnemy.HitPointsMax;
        enemy.Value.TensionPoints = m_SelectedEnemy.TensionPoints;
        enemy.Value.SpeedInitiative = m_SelectedEnemy.SpeedInitiative;
        enemy.Value.SpeedMovement = m_SelectedEnemy.SpeedMovement;
        enemy.Value.DistanceAttack = m_SelectedEnemy.DistanceAttack;

        AssetDatabase.CreateAsset(enemy, $"{m_Path}/{enemy.Value.Name}.asset");

        AssetDatabase.SaveAssets();

        GUIContent gui = AddEnemy(enemy);
        m_EnemyContents = m_EnemyContents.Concat<GUIContent>(new GUIContent[] { gui }).ToArray();
    }
    private void OnClickRemoveSelectedEnemy()
    {
        if (m_EnemyContents.Length <= 1) 
        {
            EditorUtility.DisplayDialog("Delete enemy", "You cannot remove the last enemy.", "Ok =(");
            return;
        } 

        if (EditorUtility.DisplayDialog("Delete enemy",
                $"Do you want to REMOVE AND DELETE from disc the enemy \"{m_SelectedEnemy.Name}\"?", "Yes", "No"))
        {
            EnemyData enemyData = m_Enemies[m_SelectedIndex];
            m_Enemies.RemoveAt(m_SelectedIndex);
            List<GUIContent> enemyContetList = new List<GUIContent>(m_EnemyContents);
            enemyContetList.RemoveAt(m_SelectedIndex);
            m_SelectedIndex--;
            m_SelectedIndex = m_SelectedIndex < 0 ? 0 : m_SelectedIndex;
            m_EnemyContents = enemyContetList.ToArray();

            AssetBundle.DestroyImmediate(enemyData, true);
        }

    }

    private void ShowEnemy()
    {
        if (m_Enemies.Count < 0) return;

        EditorGUILayout.BeginHorizontal();
        { 
            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.LabelField("Name: ");
                EditorGUILayout.LabelField("Type: ");
                EditorGUILayout.LabelField("Elemental: ");
                EditorGUILayout.LabelField("HP Max: ");
                EditorGUILayout.LabelField("TP: ");
                EditorGUILayout.LabelField("Iniciative: ");
                EditorGUILayout.LabelField("Speed: ");
                EditorGUILayout.LabelField("Distance: ");

            }
            EditorGUILayout.EndVertical();

            Enemy enemy = m_SelectedEnemy;
            EditorGUILayout.BeginVertical();
            {
                string oldName = enemy.Name;
                enemy.Name = EditorGUILayout.TextField(enemy.Name);

                EEnemyType oldTypeId = enemy.TypeId;
                enemy.TypeId = (EEnemyType) EditorGUILayout.EnumPopup(enemy.TypeId);

                EElemental oldElementalId = enemy.ElementalId;
                enemy.ElementalId = (EElemental)EditorGUILayout.EnumPopup(enemy.ElementalId);

                int oldHitPointsMax = enemy.HitPointsMax;
                string hitPointsText = EditorGUILayout.TextField(enemy.HitPointsMax.ToString());
                int.TryParse(hitPointsText, out enemy.HitPointsMax);
                enemy.HitPoints = enemy.HitPointsMax;

                int oldTp = enemy.TensionPoints;
                enemy.TensionPoints = EditorGUILayout.IntSlider(
                    enemy.TensionPoints, GameParametres.EnemyLimits.TP_MIN, GameParametres.EnemyLimits.TP_MAX);

                int oldInitiative = enemy.SpeedInitiative;
                enemy.SpeedInitiative = EditorGUILayout.IntSlider(
                    enemy.SpeedInitiative, GameParametres.EnemyLimits.INICIATIVE_MIN, GameParametres.EnemyLimits.INICIATIVE_MAX);

                float oldSpeed = enemy.SpeedMovement;
                enemy.SpeedMovement = EditorGUILayout.Slider(
                    enemy.SpeedMovement, GameParametres.EnemyLimits.SPEED_MIN, GameParametres.EnemyLimits.SPEED_MAX);

                float oldDistance = enemy.DistanceAttack;
                enemy.DistanceAttack = EditorGUILayout.Slider(
                    enemy.DistanceAttack, GameParametres.EnemyLimits.DISTANCE_MIN, GameParametres.EnemyLimits.DISTANCE_MAX);


                if(oldName != enemy.Name || oldTypeId != enemy.TypeId || oldElementalId != enemy.ElementalId 
                    || oldHitPointsMax != enemy.HitPointsMax || oldTp != enemy.TensionPoints || oldInitiative != enemy.SpeedInitiative
                    || oldSpeed != enemy.SpeedMovement || oldDistance !=  enemy.DistanceAttack)
                {
                    m_IsEnemyChanged = true;
                }

                m_SelectedEnemy = enemy;

            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void ShowEndButtons()
    {
        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Save")) OnClickSave();
            if (GUILayout.Button("Close")) OnClickCloseWithoutSave();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void OnClickCloseWithoutSave()
    {
        if (!m_IsEnemyChanged || EditorUtility.DisplayDialog("Closing",
                $"Do you want to exit without save yours modifications in \"{m_SelectedEnemy.Name}\"?", "Yes, I want exit without save", "No"))
        {
            Close();
        }
    }

    private void OnClickSave()
    {
        CheckAndSaveSelectEnemy();
        AssetDatabase.SaveAssets();
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
