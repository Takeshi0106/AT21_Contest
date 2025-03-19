using System;
using System.IO;
using System.IO.Compression; // CompressionLevel と ZipFile を含む
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

// 名前空間の衝突を避けるためのエイリアス (明示的に使用する場合に備えて)
using IOCompressionLevel = System.IO.Compression.CompressionLevel;

/// <summary>
/// ビルドの自動化、圧縮、アップロード、古いビルドの削除を行うエディタ拡張クラス
/// </summary>
public class AutoBuildSystem : EditorWindow
{
    // 設定
    private string googleDriveFolder = "";
    private int buildRetentionDays = 30; // デフォルト値
    private string configFilePath = "Assets/Editor/BuildConfig.json";

    // ビルド設定
    private bool buildDebug = true;
    private bool buildRelease = true;

    [MenuItem("Tools/Auto Build System")]
    public static void ShowWindow()
    {
        GetWindow<AutoBuildSystem>("Auto Build System");
    }

    private void OnGUI()
    {
        GUILayout.Label("ビルド自動化システム", EditorStyles.boldLabel);

        googleDriveFolder = EditorGUILayout.TextField("Googleドライブフォルダパス", googleDriveFolder);
        buildRetentionDays = EditorGUILayout.IntField("ビルド保持日数", buildRetentionDays);

        EditorGUILayout.Space();

        buildDebug = EditorGUILayout.Toggle("デバッグビルド", buildDebug);
        buildRelease = EditorGUILayout.Toggle("リリースビルド", buildRelease);

        EditorGUILayout.Space();

        if (GUILayout.Button("設定を保存"))
        {
            SaveConfig();
        }

        if (GUILayout.Button("ビルド実行"))
        {
            PerformBuild();
        }

        if (GUILayout.Button("古いビルドを削除"))
        {
            CleanupOldBuilds();
        }
    }

    private void OnEnable()
    {
        LoadConfig();
    }

    /// <summary>
    /// 設定をJSONファイルから読み込む
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

                Debug.Log("設定を読み込みました");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"設定読み込みエラー: {e.Message}");
        }
    }

    /// <summary>
    /// 設定をJSONファイルに保存する
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

            // ディレクトリが存在しない場合は作成
            string directory = Path.GetDirectoryName(configFilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(configFilePath, json);
            Debug.Log("設定を保存しました");
        }
        catch (Exception e)
        {
            Debug.LogError($"設定保存エラー: {e.Message}");
        }
    }

    /// <summary>
    /// ビルドを実行する
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
    /// 指定された設定でビルドを実行する
    /// </summary>
    private void BuildPlayerWithSettings(string timestamp, bool isDebug)
    {
        string buildType = isDebug ? "Debug" : "Release";

        // 絶対パスを使用
        string buildsRoot = Path.Combine(Application.dataPath, "..", "Builds");
        Directory.CreateDirectory(buildsRoot); // Builds フォルダが存在することを確認

        string productName = PlayerSettings.productName;
        string buildFolderName = $"{productName}_{buildType}_{timestamp}";
        string buildFolderPath = Path.Combine(buildsRoot, buildFolderName);

        // .exe ファイル名を設定
        string exeName = $"{productName}.exe";
        string buildPathWithExe = Path.Combine(buildFolderPath, exeName);

        Debug.Log($"ビルド出力先: {buildPathWithExe}");

        // ビルド設定
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = EditorBuildSettings.scenes.Select(s => s.path).ToArray(),
            locationPathName = buildPathWithExe,
            target = BuildTarget.StandaloneWindows64, // Win 64のみ
            options = isDebug
                ? BuildOptions.Development | BuildOptions.AllowDebugging
                : BuildOptions.None
        };

        Debug.Log($"{buildType}ビルドを開始します...");

        // ビルド実行
        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log($"{buildType}ビルド成功: {summary.totalSize / 1048576} MB");

            // ビルドフォルダが本当に存在するか確認
            if (Directory.Exists(buildFolderPath))
            {
                // フォルダ内のファイル一覧をログに出力（デバッグ用）
                string[] files = Directory.GetFiles(buildFolderPath, "*", SearchOption.AllDirectories);
                Debug.Log($"ビルドフォルダ内のファイル数: {files.Length}");
                foreach (string file in files.Take(10)) // 最初の10個だけログ出力
                {
                    Debug.Log($"ファイル: {file}");
                }

                // ZIPに圧縮
                string zipPath = $"{buildFolderPath}.zip";
                CompressBuild(buildFolderPath, zipPath);

                // Googleドライブにアップロード
                UploadToGoogleDrive(zipPath);
            }
            else
            {
                Debug.LogError($"ビルドフォルダが見つかりません: {buildFolderPath}");
            }
        }
        else
        {
            Debug.LogError($"{buildType}ビルド失敗: {summary.result}");
        }
    }

    /// <summary>
    /// ビルドを圧縮する
    /// </summary>
    private void CompressBuild(string buildPath, string zipPath)
    {
        try
        {
            Debug.Log($"ビルドを圧縮しています: 元フォルダ={buildPath}, ZIP={zipPath}");

            // フォルダが存在するか確認
            if (!Directory.Exists(buildPath))
            {
                Debug.LogError($"圧縮対象のフォルダが存在しません: {buildPath}");
                return;
            }

            // ファイルが既に存在する場合は削除
            if (File.Exists(zipPath))
            {
                File.Delete(zipPath);
                Debug.Log($"既存のZIPファイルを削除しました: {zipPath}");
            }

            // ZIPファイル作成を直接処理
            Debug.Log("ZIPファイル作成開始...");
            using (var zipArchive = System.IO.Compression.ZipFile.Open(zipPath, System.IO.Compression.ZipArchiveMode.Create))
            {
                AddDirectoryToZip(zipArchive, buildPath, "");
            }

            // ZIPファイルが作成されたか確認
            if (File.Exists(zipPath))
            {
                FileInfo zipInfo = new FileInfo(zipPath);
                Debug.Log($"圧縮完了: ZIPファイルサイズ={zipInfo.Length / 1048576}MB");

                // 元のビルドフォルダを削除
                Directory.Delete(buildPath, true);
                Debug.Log($"元のビルドフォルダを削除しました: {buildPath}");
            }
            else
            {
                Debug.LogError($"ZIPファイルが作成されませんでした: {zipPath}");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"圧縮エラー: {e.Message}");
            Debug.LogError($"スタックトレース: {e.StackTrace}");
        }
    }

    /// <summary>
    /// ディレクトリをZIPに追加する補助メソッド
    /// </summary>
    private void AddDirectoryToZip(System.IO.Compression.ZipArchive archive, string sourceDirPath, string entryPrefix)
    {
        // ディレクトリ内のすべてのファイルを追加
        foreach (string filePath in Directory.GetFiles(sourceDirPath))
        {
            string fileName = Path.GetFileName(filePath);
            string entryName = Path.Combine(entryPrefix, fileName).Replace('\\', '/');

            try
            {
                archive.CreateEntryFromFile(filePath, entryName);
                Debug.Log($"ZIPにファイルを追加: {entryName}");
            }
            catch (Exception e)
            {
                Debug.LogError($"ファイル追加エラー ({entryName}): {e.Message}");
            }
        }

        // サブディレクトリを再帰的に追加
        foreach (string subDirPath in Directory.GetDirectories(sourceDirPath))
        {
            string subDirName = Path.GetFileName(subDirPath);
            string newEntryPrefix = Path.Combine(entryPrefix, subDirName);
            AddDirectoryToZip(archive, subDirPath, newEntryPrefix);
        }
    }

    /// <summary>
    /// 共有フォルダにアップロードする
    /// </summary>
    private void UploadToGoogleDrive(string zipPath)
    {
        try
        {
            if (string.IsNullOrEmpty(googleDriveFolder))
            {
                Debug.LogError("共有フォルダのパスが設定されていません");
                return;
            }

            if (!Directory.Exists(googleDriveFolder))
            {
                Debug.LogError($"共有フォルダが存在しません: {googleDriveFolder}");
                return;
            }

            // ZIPファイルが存在するか確認
            if (!File.Exists(zipPath))
            {
                Debug.LogError($"アップロード対象のZIPファイルが存在しません: {zipPath}");
                return;
            }

            // 日付フォルダを作成 (yyyyMMdd形式)
            string dateFolder = DateTime.Now.ToString("yyyyMMdd");
            string dateFolderPath = Path.Combine(googleDriveFolder, dateFolder);

            // 日付フォルダが存在しない場合は作成
            if (!Directory.Exists(dateFolderPath))
            {
                Directory.CreateDirectory(dateFolderPath);
                Debug.Log($"日付フォルダを作成しました: {dateFolderPath}");
            }

            string fileName = Path.GetFileName(zipPath);
            string destination = Path.Combine(dateFolderPath, fileName);

            Debug.Log($"共有フォルダにコピー中: ソース={zipPath}, 送信先={destination}");

            // ファイルサイズを記録
            FileInfo sourceInfo = new FileInfo(zipPath);
            long fileSize = sourceInfo.Length;

            // コピー実行
            File.Copy(zipPath, destination, true);

            // コピーが成功したか確認
            if (File.Exists(destination))
            {
                FileInfo destInfo = new FileInfo(destination);
                Debug.Log($"コピー完了: ファイル={fileName}, サイズ={fileSize / 1048576}MB");

                // サイズが一致するか確認
                if (destInfo.Length != fileSize)
                {
                    Debug.LogWarning($"コピーされたファイルのサイズが一致しません: 元={fileSize}, コピー後={destInfo.Length}");
                }
            }
            else
            {
                Debug.LogError($"コピー先にファイルが作成されませんでした: {destination}");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"アップロードエラー: {e.Message}");
            Debug.LogError($"スタックトレース: {e.StackTrace}");
        }
    }

    /// <summary>
    /// 古いビルドを削除する
    /// </summary>
    private void CleanupOldBuilds()
    {
        try
        {
            if (string.IsNullOrEmpty(googleDriveFolder) || !Directory.Exists(googleDriveFolder))
            {
                Debug.LogError("共有フォルダが無効です");
                return;
            }

            Debug.Log($"{buildRetentionDays}日より古いビルドを削除します");

            DateTime cutoffDate = DateTime.Now.AddDays(-buildRetentionDays);
            int deletedCount = 0;

            // 日付フォルダを検索
            string[] dateFolders = Directory.GetDirectories(googleDriveFolder);

            foreach (string dateFolder in dateFolders)
            {
                try
                {
                    // フォルダ名から日付を抽出
                    string folderName = Path.GetFileName(dateFolder);

                    // 日付形式のフォルダ名かチェック (yyyyMMdd形式)
                    if (DateTime.TryParseExact(
                        folderName,
                        "yyyyMMdd",
                        null,
                        System.Globalization.DateTimeStyles.None,
                        out DateTime folderDate))
                    {
                        // 日付が基準日より古いかチェック
                        if (folderDate < cutoffDate)
                        {
                            // フォルダ内のすべてのファイルを削除
                            string[] files = Directory.GetFiles(dateFolder);
                            foreach (string file in files)
                            {
                                File.Delete(file);
                                deletedCount++;
                            }

                            // 空になったフォルダを削除
                            Directory.Delete(dateFolder);
                            Debug.Log($"古い日付フォルダを削除: {folderName}");
                        }
                        else
                        {
                            Debug.Log($"保持する日付フォルダ: {folderName}");
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"フォルダ処理エラー ({dateFolder}): {e.Message}");
                }
            }

            Debug.Log($"クリーンアップ完了: {deletedCount}個のファイルを削除しました");
        }
        catch (Exception e)
        {
            Debug.LogError($"クリーンアップエラー: {e.Message}");
            Debug.LogError($"スタックトレース: {e.StackTrace}");
        }
    }

    /// <summary>
    /// 自動ビルドを実行するためのコマンドライン関数
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
/// ビルド設定を保存するためのクラス
/// </summary>
[Serializable]
public class BuildConfig
{
    public string googleDriveFolder = "";
    public int buildRetentionDays = 30;
    public bool buildDebug = true;
    public bool buildRelease = true;
}