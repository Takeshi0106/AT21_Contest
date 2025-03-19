@echo off
chcp 65001 > nul
setlocal enabledelayedexpansion

REM カラー出力用の関数を定義
call :DEFINE_COLORS

REM ビルド開始時の情報表示
call :LOG_INFO "=============================================="
call :LOG_INFO "         Unity ビルド自動化ツール 開始         "
call :LOG_INFO "=============================================="
call :LOG_INFO "開始日時: %date% %time%"

REM バッチファイルのディレクトリパスを取得
SET "BATCH_DIR=%~dp0"

REM 設定ファイルのパスを絶対パスで指定
SET "PATH_INI=%BATCH_DIR%path.ini"
SET "GITURL_INI=%BATCH_DIR%giturl.ini"

IF NOT EXIST "!PATH_INI!" (
    call :LOG_ERROR "エラー: %PATH_INI% ファイルが見つかりません。"
    call :LOG_ERROR "以下の形式でファイルを作成してください:"
    call :LOG_ERROR "1行目: Unityエディタのパス"
    call :LOG_ERROR "2行目: プロジェクトのパス"
    pause
    exit /b 1
)

REM ファイルから行を読み込む
SET LINE_NUM=0
REM パスを行ごとに完全に取得するために delims="" を使用
FOR /F "usebackq tokens=* delims=" %%a IN ("%PATH_INI%") DO (
    SET /A LINE_NUM+=1
    IF !LINE_NUM! EQU 1 SET "UNITY_PATH=%%a"
    IF !LINE_NUM! EQU 2 SET "PROJECT_PATH=%%a"
)

IF "%UNITY_PATH%"=="" (
    call :LOG_ERROR "エラー: Unityエディタのパスが path.ini に指定されていません。"
    pause
    exit /b 1
)

IF "%PROJECT_PATH%"=="" (
    call :LOG_ERROR "エラー: プロジェクトのパスが path.ini に指定されていません。"
    pause
    exit /b 1
)

REM 引用符の処理を修正
SET "UNITY_PATH_CLEAN=%UNITY_PATH:"=%"
SET "PROJECT_PATH_CLEAN=%PROJECT_PATH:"=%"
SET "UNITY_PATH_QUOTED="%UNITY_PATH_CLEAN%""
SET "PROJECT_PATH_QUOTED="%PROJECT_PATH_CLEAN%""

REM 情報表示
call :LOG_PATH "Unityエディタのパス:" "%UNITY_PATH_CLEAN%"
call :LOG_PATH "プロジェクトのパス:" "%PROJECT_PATH_CLEAN%"

REM ログファイルのパスを設定
SET "LOG_FILE=%~dp0build_log.txt"
call :LOG_INFO "ログファイル: %LOG_FILE%"
call :LOG_INFO "ビルド処理を開始します..."

REM コマンドライン引数 - 引用符の処理を修正
SET "ARGS=-batchmode -quit -projectPath "!PROJECT_PATH_CLEAN!" -executeMethod AutoBuildSystem.PerformAutoBuild -logFile "!LOG_FILE!""

REM Unityを実行
"!UNITY_PATH_CLEAN!" !ARGS!

IF %ERRORLEVEL% NEQ 0 (
    call :LOG_ERROR "エラー: Unityの実行に失敗しました（エラーコード: %ERRORLEVEL%）"
    call :LOG_PATH "Unity実行パス:" "%UNITY_PATH_CLEAN%"
    call :LOG_INFO "引数: !ARGS!"
    pause
    exit /b 1
)

REM Unity のログファイルでビルド結果を確認
call :LOG_INFO "ビルドプロセスの実行状況を監視しています..."

:CHECK_LOG
timeout /t 2 /nobreak > nul
IF EXIST "%LOG_FILE%" (
    REM ビルドエラーを検出
    type "%LOG_FILE%" | findstr /C:"error CS" /I > nul
    IF !ERRORLEVEL! EQU 0 (
        call :LOG_ERROR "ビルドエラーが発生しました:"
        type "%LOG_FILE%" | findstr /C:"error CS" /I
        pause
        exit /b 1
    )
    
    REM コンパイルエラーを検出
    type "%LOG_FILE%" | findstr /C:"CompilerErrorException" /I > nul
    IF !ERRORLEVEL! EQU 0 (
        call :LOG_ERROR "コンパイルエラーが発生しました:"
        type "%LOG_FILE%" | findstr /B /C:"CompilerErrorException" /I
        pause
        exit /b 1
    )
    
    REM ビルド失敗を検出
    type "%LOG_FILE%" | findstr /C:"Build failed" /I > nul
    IF !ERRORLEVEL! EQU 0 (
        call :LOG_ERROR "ビルドが失敗しました。詳細は %LOG_FILE% を確認してください。"
        pause
        exit /b 1
    )
    
    REM ビルド完了を検出
    type "%LOG_FILE%" | findstr /C:"Build succeeded" /I > nul
    IF !ERRORLEVEL! EQU 0 (
        call :LOG_SUCCESS "ビルドが正常に完了しました。"
        rem アップロードしたROMを削除
        rmdir /s /q Builds
        call :LOG_INFO "アップロード済ROMをローカルから削除しました"
        exit /b 0
    )
    
    REM ビルド完了を検出（代替メッセージ）
    type "%LOG_FILE%" | findstr /C:"Built " /C:"Successfully built" /I > nul
    IF !ERRORLEVEL! EQU 0 (
        call :LOG_SUCCESS "ビルドが正常に完了しました。"
        rem アップロードしたROMを削除
        rmdir /s /q Builds
        call :LOG_INFO "アップロード済ROMをローカルから削除しました"
        exit /b 0
    )
    
    REM プロセスが完了していない場合は再度チェック
    GOTO CHECK_LOG
) ELSE (
    REM ログファイルがまだ作成されていない場合は待機
    GOTO CHECK_LOG
)

REM ========================================
REM カラー出力用の関数定義
REM ========================================
:DEFINE_COLORS
REM ANSI エスケープシーケンスを使用するための設定
for /F "tokens=1,2 delims=#" %%a in ('"prompt #$H#$E# & echo on & for %%b in (1) do rem"') do (
  set "ESC=%%b"
)
exit /b 0

:LOG_INFO
echo %ESC%[94m[情報]%ESC%[0m %~1
exit /b 0

:LOG_ERROR
echo %ESC%[91m[エラー]%ESC%[0m %~1
exit /b 0

:LOG_SUCCESS
echo %ESC%[94m[成功]%ESC%[0m %~1
exit /b 0

:LOG_PATH
echo %ESC%[94m[情報]%ESC%[0m %~1 %~2
exit /b 0