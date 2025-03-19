@echo off
chcp 65001 > nul
setlocal enabledelayedexpansion

echo Unityプロジェクト自動更新ツール
echo ------------------------------

REM 現在のディレクトリを取得
set CURRENT_DIR=%~dp0
cd %CURRENT_DIR%

echo 現在の作業ディレクトリ: %CURRENT_DIR%

REM giturl.iniファイルの存在を確認（絶対パスを使用）
if not exist "%~dp0giturl.ini" (
    echo エラー: giturl.iniファイルが見つかりません。
    echo giturl.iniファイルを作成して、1行目にGitリポジトリのURLを記述してください。
    exit /b 1
)
REM giturl.iniファイルからリポジトリURLを読み取る（絶対パスを使用）
set /p REPO_URL=<"%~dp0giturl.ini"
echo リポジトリURL: %REPO_URL%

REM Git認証情報キャッシュを設定（必要に応じて）
git config --global credential.helper store

REM Gitコマンドが利用可能か確認
where git >nul 2>&1
if %ERRORLEVEL% neq 0 (
    echo エラー: Gitがインストールされていないか、PATHに含まれていません。
    echo Gitをインストールするか、環境変数PATHにGitの実行ファイルのパスを追加してください。
    exit /b 1
)

REM .gitディレクトリの存在を確認
if exist ".git" (
    echo すでにGitリポジトリが初期化されています。最新版に更新します。
    
    REM リモートURLが正しいか確認し、必要に応じて更新
    for /f "tokens=*" %%a in ('git config --get remote.origin.url') do set CURRENT_URL=%%a
    if not "!CURRENT_URL!"=="%REPO_URL%" (
        echo リモートURLを更新します...
        git remote set-url origin %REPO_URL%
        if %ERRORLEVEL% neq 0 (
            echo エラー: リモートURLの更新に失敗しました。
            exit /b 1
        )
    )
    
    echo 変更をステージングしています...
    git add -A
    
    echo 変更をコミットしています...
    git commit -m "Auto-commit before update: %date% %time%"
    
    echo リモートの変更を取得しています...
    git fetch origin
    if %ERRORLEVEL% neq 0 (
        echo エラー: リモートの変更の取得に失敗しました。
        echo インターネット接続や認証情報を確認してください。
        exit /b 1
    )
    
    echo ローカルの変更をリベースしています...
    git pull --rebase origin main
    if %ERRORLEVEL% neq 0 (
        echo 警告: リベースでコンフリクトが発生しました。
        echo コンフリクトを解決して続行してください。
        exit /b 1
    )
) else (
    echo 新しいリポジトリをクローンします...
    
    REM 親ディレクトリに移動してクローン
    cd ..
    set PARENT_DIR=%CD%
    set PROJECT_DIR_NAME=%CURRENT_DIR:~0,-1%
    for %%i in ("%PROJECT_DIR_NAME%") do set PROJECT_NAME=%%~nxi
    
    echo プロジェクト名: %PROJECT_NAME%
    echo 親ディレクトリ: %PARENT_DIR%
    
    REM 既存のディレクトリをバックアップし、新しくクローン
    if exist "%PROJECT_NAME%" (
        echo 既存のディレクトリをバックアップしています...
        set BACKUP_NAME=%PROJECT_NAME%_backup_%date:~-4,4%%date:~-7,2%%date:~-10,2%_%time:~0,2%%time:~3,2%%time:~6,2%
        set BACKUP_NAME=!BACKUP_NAME: =0!
        ren "%PROJECT_NAME%" "!BACKUP_NAME!"
        if %ERRORLEVEL% neq 0 (
            echo エラー: ディレクトリのバックアップに失敗しました。
            exit /b 1
        )
    )
    
    echo リポジトリをクローンしています...
    git clone %REPO_URL% "%PROJECT_NAME%"
    if %ERRORLEVEL% neq 0 (
        echo エラー: リポジトリのクローンに失敗しました。
        echo URLや認証情報を確認してください。
        exit /b 1
    )
    
    REM クローンしたディレクトリに移動
    cd "%PROJECT_NAME%"
)

echo LFSファイルを取得しています...
git lfs pull
if %ERRORLEVEL% neq 0 (
    echo 警告: Git LFSファイルの取得に失敗しました。
    echo Git LFSがインストールされているか確認してください。
)

echo ------------------------------
echo 更新が完了しました！
echo 現在のバージョン: 
git log -1 --pretty=format:"%%h - %%an, %%ar : %%s"
echo.

REM 終了メッセージを表示して自動終了
echo 処理を完了しました。ウィンドウは3秒後に自動的に閉じます...
timeout /t 3 > nul
exit /b 0