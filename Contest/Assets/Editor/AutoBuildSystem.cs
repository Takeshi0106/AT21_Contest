using System;
using System.IO;
using System.IO.Compression; // CompressionLevel �� ZipFile ���܂�
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

// ���O��Ԃ̏Փ˂�����邽�߂̃G�C���A�X (�����I�Ɏg�p����ꍇ�ɔ�����)
using IOCompressionLevel = System.IO.Compression.CompressionLevel;

/// <summary>
/// �r���h�̎������A���k�A�A�b�v���[�h�A�Â��r���h�̍폜���s���G�f�B�^�g���N���X
/// </summary>
public class AutoBuildSystem : EditorWindow
{
    // �ݒ�
    private string googleDriveFolder = "";
    private int buildRetentionDays = 30; // �f�t�H���g�l
    private string configFilePath = "Assets/Editor/BuildConfig.json";

    // �r���h�ݒ�
    private bool buildDebug = true;
    private bool buildRelease = true;

    [MenuItem("Tools/Auto Build System")]
    public static void ShowWindow()
    {
        GetWindow<AutoBuildSystem>("Auto Build System");
    }

    private void OnGUI()
    {
        GUILayout.Label("�r���h�������V�X�e��", EditorStyles.boldLabel);

        googleDriveFolder = EditorGUILayout.TextField("Google�h���C�u�t�H���_�p�X", googleDriveFolder);
        buildRetentionDays = EditorGUILayout.IntField("�r���h�ێ�����", buildRetentionDays);

        EditorGUILayout.Space();

        buildDebug = EditorGUILayout.Toggle("�f�o�b�O�r���h", buildDebug);
        buildRelease = EditorGUILayout.Toggle("�����[�X�r���h", buildRelease);

        EditorGUILayout.Space();

        if (GUILayout.Button("�ݒ��ۑ�"))
        {
            SaveConfig();
        }

        if (GUILayout.Button("�r���h���s"))
        {
            PerformBuild();
        }

        if (GUILayout.Button("�Â��r���h���폜"))
        {
            CleanupOldBuilds();
        }
    }

    private void OnEnable()
    {
        LoadConfig();
    }

    /// <summary>
    /// �ݒ��JSON�t�@�C������ǂݍ���
    /// </summary>
    private void LoadConfig()
    {
        try
        {
            if (File.Exists(configFilePath))
            {
                string json = File.ReadAllText(configFilePath);
                BuildConfig config = JsonUtility.FromJson<BuildConfig>(json);

                googleDriveFolder = config.googleDriveFolder;
                buildRetentionDays = config.buildRetentionDays;
                buildDebug = config.buildDebug;
                buildRelease = config.buildRelease;

                Debug.Log("�ݒ��ǂݍ��݂܂���");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"�ݒ�ǂݍ��݃G���[: {e.Message}");
        }
    }

    /// <summary>
    /// �ݒ��JSON�t�@�C���ɕۑ�����
    /// </summary>
    private void SaveConfig()
    {
        try
        {
            BuildConfig config = new BuildConfig
            {
                googleDriveFolder = googleDriveFolder,
                buildRetentionDays = buildRetentionDays,
                buildDebug = buildDebug,
                buildRelease = buildRelease
            };

            string json = JsonUtility.ToJson(config, true);

            // �f�B���N�g�������݂��Ȃ��ꍇ�͍쐬
            string directory = Path.GetDirectoryName(configFilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(configFilePath, json);
            Debug.Log("�ݒ��ۑ����܂���");
        }
        catch (Exception e)
        {
            Debug.LogError($"�ݒ�ۑ��G���[: {e.Message}");
        }
    }

    /// <summary>
    /// �r���h�����s����
    /// </summary>
    private void PerformBuild()
    {
        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

        if (buildDebug)
        {
            BuildPlayerWithSettings(timestamp, true);
        }

        if (buildRelease)
        {
            BuildPlayerWithSettings(timestamp, false);
        }

        CleanupOldBuilds();
    }

    /// <summary>
    /// �w�肳�ꂽ�ݒ�Ńr���h�����s����
    /// </summary>
    private void BuildPlayerWithSettings(string timestamp, bool isDebug)
    {
        string buildType = isDebug ? "Debug" : "Release";

        // ��΃p�X���g�p
        string buildsRoot = Path.Combine(Application.dataPath, "..", "Builds");
        Directory.CreateDirectory(buildsRoot); // Builds �t�H���_�����݂��邱�Ƃ��m�F

        string productName = PlayerSettings.productName;
        string buildFolderName = $"{productName}_{buildType}_{timestamp}";
        string buildFolderPath = Path.Combine(buildsRoot, buildFolderName);

        // .exe �t�@�C������ݒ�
        string exeName = $"{productName}.exe";
        string buildPathWithExe = Path.Combine(buildFolderPath, exeName);

        Debug.Log($"�r���h�o�͐�: {buildPathWithExe}");

        // �r���h�ݒ�
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = EditorBuildSettings.scenes.Select(s => s.path).ToArray(),
            locationPathName = buildPathWithExe,
            target = BuildTarget.StandaloneWindows64, // Win 64�̂�
            options = isDebug
                ? BuildOptions.Development | BuildOptions.AllowDebugging
                : BuildOptions.None
        };

        Debug.Log($"{buildType}�r���h���J�n���܂�...");

        // �r���h���s
        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log($"{buildType}�r���h����: {summary.totalSize / 1048576} MB");

            // �r���h�t�H���_���{���ɑ��݂��邩�m�F
            if (Directory.Exists(buildFolderPath))
            {
                // �t�H���_���̃t�@�C���ꗗ�����O�ɏo�́i�f�o�b�O�p�j
                string[] files = Directory.GetFiles(buildFolderPath, "*", SearchOption.AllDirectories);
                Debug.Log($"�r���h�t�H���_���̃t�@�C����: {files.Length}");
                foreach (string file in files.Take(10)) // �ŏ���10�������O�o��
                {
                    Debug.Log($"�t�@�C��: {file}");
                }

                // ZIP�Ɉ��k
                string zipPath = $"{buildFolderPath}.zip";
                CompressBuild(buildFolderPath, zipPath);

                // Google�h���C�u�ɃA�b�v���[�h
                UploadToGoogleDrive(zipPath);
            }
            else
            {
                Debug.LogError($"�r���h�t�H���_��������܂���: {buildFolderPath}");
            }
        }
        else
        {
            Debug.LogError($"{buildType}�r���h���s: {summary.result}");
        }
    }

    /// <summary>
    /// �r���h�����k����
    /// </summary>
    private void CompressBuild(string buildPath, string zipPath)
    {
        try
        {
            Debug.Log($"�r���h�����k���Ă��܂�: ���t�H���_={buildPath}, ZIP={zipPath}");

            // �t�H���_�����݂��邩�m�F
            if (!Directory.Exists(buildPath))
            {
                Debug.LogError($"���k�Ώۂ̃t�H���_�����݂��܂���: {buildPath}");
                return;
            }

            // �t�@�C�������ɑ��݂���ꍇ�͍폜
            if (File.Exists(zipPath))
            {
                File.Delete(zipPath);
                Debug.Log($"������ZIP�t�@�C�����폜���܂���: {zipPath}");
            }

            // ZIP�t�@�C���쐬�𒼐ڏ���
            Debug.Log("ZIP�t�@�C���쐬�J�n...");
            using (var zipArchive = System.IO.Compression.ZipFile.Open(zipPath, System.IO.Compression.ZipArchiveMode.Create))
            {
                AddDirectoryToZip(zipArchive, buildPath, "");
            }

            // ZIP�t�@�C�����쐬���ꂽ���m�F
            if (File.Exists(zipPath))
            {
                FileInfo zipInfo = new FileInfo(zipPath);
                Debug.Log($"���k����: ZIP�t�@�C���T�C�Y={zipInfo.Length / 1048576}MB");

                // ���̃r���h�t�H���_���폜
                Directory.Delete(buildPath, true);
                Debug.Log($"���̃r���h�t�H���_���폜���܂���: {buildPath}");
            }
            else
            {
                Debug.LogError($"ZIP�t�@�C�����쐬����܂���ł���: {zipPath}");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"���k�G���[: {e.Message}");
            Debug.LogError($"�X�^�b�N�g���[�X: {e.StackTrace}");
        }
    }

    /// <summary>
    /// �f�B���N�g����ZIP�ɒǉ�����⏕���\�b�h
    /// </summary>
    private void AddDirectoryToZip(System.IO.Compression.ZipArchive archive, string sourceDirPath, string entryPrefix)
    {
        // �f�B���N�g�����̂��ׂẴt�@�C����ǉ�
        foreach (string filePath in Directory.GetFiles(sourceDirPath))
        {
            string fileName = Path.GetFileName(filePath);
            string entryName = Path.Combine(entryPrefix, fileName).Replace('\\', '/');

            try
            {
                archive.CreateEntryFromFile(filePath, entryName);
                Debug.Log($"ZIP�Ƀt�@�C����ǉ�: {entryName}");
            }
            catch (Exception e)
            {
                Debug.LogError($"�t�@�C���ǉ��G���[ ({entryName}): {e.Message}");
            }
        }

        // �T�u�f�B���N�g�����ċA�I�ɒǉ�
        foreach (string subDirPath in Directory.GetDirectories(sourceDirPath))
        {
            string subDirName = Path.GetFileName(subDirPath);
            string newEntryPrefix = Path.Combine(entryPrefix, subDirName);
            AddDirectoryToZip(archive, subDirPath, newEntryPrefix);
        }
    }

    /// <summary>
    /// ���L�t�H���_�ɃA�b�v���[�h����
    /// </summary>
    private void UploadToGoogleDrive(string zipPath)
    {
        try
        {
            if (string.IsNullOrEmpty(googleDriveFolder))
            {
                Debug.LogError("���L�t�H���_�̃p�X���ݒ肳��Ă��܂���");
                return;
            }

            if (!Directory.Exists(googleDriveFolder))
            {
                Debug.LogError($"���L�t�H���_�����݂��܂���: {googleDriveFolder}");
                return;
            }

            // ZIP�t�@�C�������݂��邩�m�F
            if (!File.Exists(zipPath))
            {
                Debug.LogError($"�A�b�v���[�h�Ώۂ�ZIP�t�@�C�������݂��܂���: {zipPath}");
                return;
            }

            // ���t�t�H���_���쐬 (yyyyMMdd�`��)
            string dateFolder = DateTime.Now.ToString("yyyyMMdd");
            string dateFolderPath = Path.Combine(googleDriveFolder, dateFolder);

            // ���t�t�H���_�����݂��Ȃ��ꍇ�͍쐬
            if (!Directory.Exists(dateFolderPath))
            {
                Directory.CreateDirectory(dateFolderPath);
                Debug.Log($"���t�t�H���_���쐬���܂���: {dateFolderPath}");
            }

            string fileName = Path.GetFileName(zipPath);
            string destination = Path.Combine(dateFolderPath, fileName);

            Debug.Log($"���L�t�H���_�ɃR�s�[��: �\�[�X={zipPath}, ���M��={destination}");

            // �t�@�C���T�C�Y���L�^
            FileInfo sourceInfo = new FileInfo(zipPath);
            long fileSize = sourceInfo.Length;

            // �R�s�[���s
            File.Copy(zipPath, destination, true);

            // �R�s�[�������������m�F
            if (File.Exists(destination))
            {
                FileInfo destInfo = new FileInfo(destination);
                Debug.Log($"�R�s�[����: �t�@�C��={fileName}, �T�C�Y={fileSize / 1048576}MB");

                // �T�C�Y����v���邩�m�F
                if (destInfo.Length != fileSize)
                {
                    Debug.LogWarning($"�R�s�[���ꂽ�t�@�C���̃T�C�Y����v���܂���: ��={fileSize}, �R�s�[��={destInfo.Length}");
                }
            }
            else
            {
                Debug.LogError($"�R�s�[��Ƀt�@�C�����쐬����܂���ł���: {destination}");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"�A�b�v���[�h�G���[: {e.Message}");
            Debug.LogError($"�X�^�b�N�g���[�X: {e.StackTrace}");
        }
    }

    /// <summary>
    /// �Â��r���h���폜����
    /// </summary>
    private void CleanupOldBuilds()
    {
        try
        {
            if (string.IsNullOrEmpty(googleDriveFolder) || !Directory.Exists(googleDriveFolder))
            {
                Debug.LogError("���L�t�H���_�������ł�");
                return;
            }

            Debug.Log($"{buildRetentionDays}�����Â��r���h���폜���܂�");

            DateTime cutoffDate = DateTime.Now.AddDays(-buildRetentionDays);
            int deletedCount = 0;

            // ���t�t�H���_������
            string[] dateFolders = Directory.GetDirectories(googleDriveFolder);

            foreach (string dateFolder in dateFolders)
            {
                try
                {
                    // �t�H���_��������t�𒊏o
                    string folderName = Path.GetFileName(dateFolder);

                    // ���t�`���̃t�H���_�����`�F�b�N (yyyyMMdd�`��)
                    if (DateTime.TryParseExact(
                        folderName,
                        "yyyyMMdd",
                        null,
                        System.Globalization.DateTimeStyles.None,
                        out DateTime folderDate))
                    {
                        // ���t��������Â����`�F�b�N
                        if (folderDate < cutoffDate)
                        {
                            // �t�H���_���̂��ׂẴt�@�C�����폜
                            string[] files = Directory.GetFiles(dateFolder);
                            foreach (string file in files)
                            {
                                File.Delete(file);
                                deletedCount++;
                            }

                            // ��ɂȂ����t�H���_���폜
                            Directory.Delete(dateFolder);
                            Debug.Log($"�Â����t�t�H���_���폜: {folderName}");
                        }
                        else
                        {
                            Debug.Log($"�ێ�������t�t�H���_: {folderName}");
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"�t�H���_�����G���[ ({dateFolder}): {e.Message}");
                }
            }

            Debug.Log($"�N���[���A�b�v����: {deletedCount}�̃t�@�C�����폜���܂���");
        }
        catch (Exception e)
        {
            Debug.LogError($"�N���[���A�b�v�G���[: {e.Message}");
            Debug.LogError($"�X�^�b�N�g���[�X: {e.StackTrace}");
        }
    }

    /// <summary>
    /// �����r���h�����s���邽�߂̃R�}���h���C���֐�
    /// </summary>
    public static void PerformAutoBuild()
    {
        AutoBuildSystem buildSystem = new AutoBuildSystem();
        buildSystem.LoadConfig();
        buildSystem.PerformBuild();
        EditorApplication.Exit(0);
    }
}

/// <summary>
/// �r���h�ݒ��ۑ����邽�߂̃N���X
/// </summary>
[Serializable]
public class BuildConfig
{
    public string googleDriveFolder = "";
    public int buildRetentionDays = 30;
    public bool buildDebug = true;
    public bool buildRelease = true;
}